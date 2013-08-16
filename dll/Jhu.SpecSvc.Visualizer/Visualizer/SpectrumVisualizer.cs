#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Visualizer classes are for plotting spectra on
 * the webpage
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Visualizer.cs,v 1.1 2008/01/08 22:54:10 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:54:10 $
 */
#endregion
using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Jhu.SpecSvc.SpectrumLib;
using ZedGraph;

namespace Jhu.SpecSvc.Visualizer
{

    /// <summary>
    /// Summary description for Visualizer.
    /// </summary>
    public class SpectrumVisualizer
    {
        public SpectrumVisualizer()
            : base()
        {
        }

        public Bitmap PlotSpectraGraph(SpectrumPlotParameters par, Jhu.SpecSvc.SpectrumLib.Spectrum spec)
        {
            return PlotSpectraGraph(par, new Jhu.SpecSvc.SpectrumLib.Spectrum[] { spec });
        }

        public Bitmap PlotSpectraGraph(SpectrumPlotParameters par, IEnumerable<Jhu.SpecSvc.SpectrumLib.Spectrum> spectra)
        {
            Bitmap bmp = new Bitmap((int)par.Width, (int)par.Height);
            Graphics g = Graphics.FromImage(bmp);

            GraphPane pane = new GraphPane();
            pane.Rect = new RectangleF(0, 0, par.Width, par.Height);

            pane.Legend.IsVisible = false;
            pane.Border.IsVisible = false;

            AddLines(par, pane);

            double xmin = double.MaxValue;
            double xmax = double.MinValue;

            List<LineItem> lines = new List<LineItem>();
            int q = 0;
            foreach (Jhu.SpecSvc.SpectrumLib.Spectrum spectrum in spectra)
            {
                if (par.Labels)
                {
                    if (q == 0)
                    {
                        pane.Title.Text = spectrum.Target.Name.Value;
                        pane.XAxis.Title.Text = "Wavelength [" + spectrum.Data.SpectralAxis.Value.Unit + "]";
                        pane.YAxis.Title.Text = "Flux [" + spectrum.Data.FluxAxis.Value.Unit + "]";
                    }
                    else
                    {
                        pane.Title.IsVisible = false;   // legend will be added if required
                        pane.Legend.IsVisible = par.Legend;
                        pane.Legend.Border.IsVisible = false;
                    }
                }

                for (int i = 0; i < par.DataArrays.Length; i++)
                {
                    double[] array = (double[])spectrum.GetType().GetField(par.DataArrays[i]).GetValue(spectrum);

                    List<double> x = new List<double>();
                    List<double> y = new List<double>();

                    for (int wl = 0; wl < array.Length; wl++)
                    {
                        if ((spectrum.Flux_Accuracy_Quality[wl] & (long)PointMask.NoData) == 0)
                        {
                            x.Add(spectrum.Spectral_Value[wl]);
                            y.Add(array[wl]);
                        }
                    }

                    LineItem curve = pane.AddCurve(spectrum.Target.Name.Value, spectrum.Spectral_Value, array, GetChartColor(q), SymbolType.None);
                    lines.Add(curve);

                    /*
                    if (par.plotError && par.Arrays[i] == "Flux_Value")
                    {
                        double[] errlow = new double[spectrum.Spectral_Value.Length];
                        double[] errhigh = new double[spectrum.Spectral_Value.Length];
                        for (int wl = 0; wl < spectrum.Spectral_Value.Length; wl++)
                        {
                            errlow[wl] = spectrum.Flux_Value[wl] - spectrum.Flux_Accuracy_StatErrLow[wl];
                            errhigh[wl] = spectrum.Flux_Value[wl] + spectrum.Flux_Accuracy_StatErrHigh[wl];
                        }
                        LineItem errline = pane.AddCurve(string.Empty, spectrum.Spectral_Value, errlow, GetChartColor(q), SymbolType.None);
                        errline.Line.Color = Color.FromArgb(128, errline.Line.Color);
                        lines.Add(errline);

                        errline = pane.AddCurve(string.Empty, spectrum.Spectral_Value, errhigh, GetChartColor(q), SymbolType.None);
                        errline.Line.Color = Color.FromArgb(128, errline.Line.Color);
                        lines.Add(errline);
                    }
                     * */

                    q++;
                }

                xmin = Math.Min(xmin, spectrum.Spectral_Value[0]);
                xmax = Math.Max(xmax, spectrum.Spectral_Value[spectrum.Spectral_Value.Length - 1]);
            }

            /* - reverse order
            pane.CurveList.Clear();
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                pane.CurveList.Add(lines[i]);
            }
             * */

            pane.IsBoundedRanges = false;

            pane.XAxis.Scale.Min = (par.XMin > 0) ? par.XMin : xmin;
            pane.XAxis.Scale.Max = (par.XMax > 0) ? par.XMax : xmax;

            if (par.YMin != -1) pane.YAxis.Scale.Min = par.YMin;
            if (par.YMax != -1) pane.YAxis.Scale.Max = par.YMax;

            if (par.YLogScale) pane.YAxis.Type = AxisType.Log;
            if (par.XLogScale)
            {
                pane.XAxis.Type = AxisType.Log;
                pane.XAxis.Scale.MajorStep = 0.1;
                pane.XAxis.Scale.IsUseTenPower = false;
            }

            pane.XAxis.Scale.Mag = 0;
            pane.XAxis.Scale.MagAuto = false;
            pane.YAxis.Scale.Mag = 0;
            pane.YAxis.Scale.MagAuto = false;

            // Scale axes to the graph and turn off exponential notation
            pane.AxisChange(g);            

            pane.Draw(g);

            return bmp;
        }

        private void AddLines(SpectrumPlotParameters par, GraphPane pane)
        {
            // add lines
            if (par.PlotSpectralLines && par.LineLabels != null && par.LineWavelengths != null)
            {
                for (int i = 0; i < par.LineLabels.Length; i++)
                {
                    LineObj li = new LineObj(par.LineWavelengths[i], 0, par.LineWavelengths[i], 1);
                    li.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                    li.IsClippedToChartRect = true;
                    li.Line.Color = Color.LightGray;
                    li.ZOrder = ZOrder.E_BehindCurves;
                    pane.GraphObjList.Add(li);

                    double top = 0.05 + (i % 4) * 0.025;

                    TextObj to = new TextObj(par.LineLabels[i], par.LineWavelengths[i], top);
                    to.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                    to.Location.AlignV = AlignV.Bottom;
                    to.Location.AlignH = AlignH.Center;
                    to.IsClippedToChartRect = true;
                    to.FontSpec.Border.IsVisible = false;
                    to.FontSpec.Fill.IsVisible = false;
                    to.FontSpec.Size = 8;
                    to.ZOrder = ZOrder.A_InFront;
                    pane.GraphObjList.Add(to);
                }
            }
        }

        public Bitmap PlotColorGraph(ColorPlotParameters par, IEnumerable<Spectrum> spectra)
        {
            Bitmap bmp = new Bitmap((int)par.Width, (int)par.Height);
            Graphics g = Graphics.FromImage(bmp);

            GraphPane pane = new GraphPane();
            pane.Rect = new RectangleF(0, 0, par.Width, par.Height);

            pane.Legend.IsVisible = false;
            pane.Border.IsVisible = false;

            List<LineItem> lines = new List<LineItem>();
            int q = 0;
            foreach (Spectrum spectrum in spectra)
            {
                if (q == 0)
                {
                    if (par.Legend)
                    {
                        pane.Title.Text = spectrum.Target.Name.Value;

                        if (par.XFilter1 != -1 && par.XFilter2 != -1)
                        {
                            pane.XAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.XFilter1] + " - " + spectrum.Magnitudes[0].FilterName[par.XFilter2];
                        }
                        else if (par.XFilter1 != -1)
                        {
                            pane.XAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.XFilter1];
                        }
                        else if (par.XFilter2 != -1)
                        {
                            pane.XAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.XFilter2];
                        }

                        if (par.YFilter1 != -1 && par.YFilter2 != -1)
                        {
                            pane.YAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.YFilter1] + " - " + spectrum.Magnitudes[0].FilterName[par.YFilter2];
                        }
                        else if (par.YFilter1 != -1)
                        {
                            pane.YAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.YFilter1];
                        }
                        else if (par.YFilter2 != -1)
                        {
                            pane.YAxis.Title.Text = spectrum.Magnitudes[0].FilterName[par.YFilter2];
                        }
                    }
                }
                else
                {
                    pane.Title.IsVisible = false;   // legend will be added if required
                    pane.Legend.IsVisible = par.Legend;
                    pane.Legend.Border.IsVisible = false;
                }

                double[] x = null;
                double[] y = null;

                if (par.XFilter1 != -1 && par.XFilter2 != -1)
                {
                    Util.Vector.Subtract(spectrum.Magnitudes[0].Flux[par.XFilter1], spectrum.Magnitudes[0].Flux[par.XFilter2], out x);
                }
                else if (par.XFilter1 != -1)
                {
                    x = spectrum.Magnitudes[0].Flux[par.XFilter1];
                }
                else if (par.XFilter2 != -1)
                {
                    x = spectrum.Magnitudes[0].Flux[par.XFilter2];
                }

                if (par.YFilter1 != -1 && par.YFilter2 != -1)
                {
                    Util.Vector.Subtract(spectrum.Magnitudes[0].Flux[par.YFilter1], spectrum.Magnitudes[0].Flux[par.YFilter2], out y);
                }
                else if (par.YFilter1 != -1)
                {
                    x = spectrum.Magnitudes[0].Flux[par.YFilter1];
                }
                else if (par.YFilter2 != -1)
                {
                    x = spectrum.Magnitudes[0].Flux[par.YFilter2];
                }

                if (x != null && y != null)
                {
                    LineItem curve = pane.AddCurve(spectrum.Target.Name.Value, x, y, GetChartColor(q), SymbolType.None);

                    lines.Add(curve);

                    for (int z = 0; z < spectrum.Magnitudes[0].Redshift.Length; z += 10)
                    {
                        TextObj to = new TextObj(spectrum.Magnitudes[0].Redshift[z].ToString("0.000"), x[z], y[z]);
                        //to.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                        to.Location.AlignV = AlignV.Bottom;
                        to.Location.AlignH = AlignH.Center;
                        to.IsClippedToChartRect = true;
                        to.FontSpec.Border.IsVisible = false;
                        to.FontSpec.Fill.IsVisible = false;
                        to.FontSpec.Size = 8;
                        to.ZOrder = ZOrder.A_InFront;
                        pane.GraphObjList.Add(to);
                    }
                }

                q++;
            }

            pane.IsBoundedRanges = false;

            if (par.YMin != -1) pane.YAxis.Scale.Min = par.YMin;
            if (par.YMax != -1) pane.YAxis.Scale.Max = par.YMax;

            if (par.YLogScale) pane.YAxis.Type = AxisType.Log;
            if (par.XLogScale) pane.XAxis.Type = AxisType.Log;

            /*{
                pane.XAxis.Type = AxisType.Log;
                pane.XAxis.Scale.MajorStep = 0.1;
                pane.XAxis.Scale.IsUseTenPower = false;
            }*/

            pane.AxisChange(g);
            pane.Draw(g);

            return bmp;
        }

        protected System.Drawing.Color GetChartColor(int index)
        {
            switch (index % 8)
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Orange;
                case 4:
                    return Color.Brown;
                case 5:
                    return Color.Purple;
                case 6:
                    return Color.Cyan;
                case 7:
                    return Color.Gray;
            }
            return Color.Blue;
        }
    }
}
#region Revision History
/* Revision History

        $Log: Visualizer.cs,v $
        Revision 1.1  2008/01/08 22:54:10  dobos
        Initial checkin


*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Web.Pipeline.Formats
{
    public partial class SpectrumPlotFormatControl : FileOutputFormatControlBase<SpectrumPlotFormat>
    {
        protected override void OnEnabledChanged()
        {

        }

        protected override void OnUpdateForm(SpectrumPlotFormat format)
        {
            ImageFormat.SelectedValue = format.ImageFormat.ToString();
            Width.Text = format.PlotParameters.Width.ToString();
            Height.Text = format.PlotParameters.Height.ToString();
            XMin.Text = format.PlotParameters.XMin == -1 ? "" : format.PlotParameters.XMin.ToString();
            XMax.Text = format.PlotParameters.XMax == -1 ? "" : format.PlotParameters.XMax.ToString();
            XLogScale.Checked = format.PlotParameters.XLogScale;
            YMin.Text = format.PlotParameters.YMin == -1 ? "" : format.PlotParameters.YMin.ToString();
            YMax.Text = format.PlotParameters.YMax == -1 ? "" : format.PlotParameters.YMax.ToString();
            YLogScale.Checked = format.PlotParameters.YLogScale;
            SpectralLines.Checked = format.PlotParameters.PlotSpectralLines;

            Prefix.Text = format.Prefix;
        }

        protected override void OnSaveForm(SpectrumPlotFormat format)
        {
            format.ImageFormat = (SpectrumPlotFormat.ImageFileFormat)Enum.Parse(typeof(SpectrumPlotFormat.ImageFileFormat), ImageFormat.SelectedValue);
            format.PlotParameters.Width = float.Parse(Width.Text);
            format.PlotParameters.Height = float.Parse(Height.Text);
            format.PlotParameters.XMin = XMin.Text == "" ? -1 : double.Parse(XMin.Text);
            format.PlotParameters.XMax = XMax.Text == "" ? -1 : double.Parse(XMax.Text);
            format.PlotParameters.XLogScale = XLogScale.Checked;
            format.PlotParameters.YMin = YMin.Text == "" ? -1 : double.Parse(YMin.Text);
            format.PlotParameters.YMax = YMax.Text == "" ? -1 : double.Parse(YMax.Text);
            format.PlotParameters.YLogScale = YLogScale.Checked;
            format.PlotParameters.PlotSpectralLines = SpectralLines.Checked;

            format.Prefix = Prefix.Text;
        }



    }
}
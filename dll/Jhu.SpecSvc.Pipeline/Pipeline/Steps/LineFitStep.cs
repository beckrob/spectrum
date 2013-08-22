using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class LineFitStep : PipelineStep
    {
        private class FitTask
        {
            private abstract class LineFitter
            {
                public abstract double OptimizeLines(LineParameters[] lines, double[] wl, double[] fl, double[] w, bool[] mask);
                protected abstract double CalculateChi2(LineParameters[] lines, double[] x, double[] wl, double[] fl, double[] w);

                protected double OptimizeLineParameters(LineParameters[] lines, double[] p, double[] s, double[] pmin, double[] pmax, double[] wl, double[] fl, double[] w)
                {

                    Alglib.Wrappers.ScalarFunctionOptimizationProblem min = new Alglib.Wrappers.ScalarFunctionOptimizationProblem();

                    min.MaxIterations = 500;
                    min.Diffstep = 1e-2;
                    min.Epsg = 1e-7;
                    min.Epsf = min.Epsi = min.Epsx = 1e-9;

                    min.Parameters = p;
                    min.Scale = s;
                    min.MinLimits = pmin;
                    min.MaxLimits = pmax;
                    min.Function = delegate(double[] x)
                    {
                        return CalculateChi2(lines, x, wl, fl, w);
                    };

                    //Alglib.Wrappers.StopCriterium sc = min.Optimize(new Alglib.Wrappers.LevenbergMarquardtMinimizer());
                    Alglib.Wrappers.StopCriterium sc = min.Optimize(new Alglib.Wrappers.BLEICMinimizer());

                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] = min.Parameters[i];
                    }

                    // Calculate reduced and normalized chi2
                    double chi2 = CalculateChi2(lines, p, wl, fl, w);
                    chi2 /= wl.Length;

                    return chi2;
                }
            }

            private class GaussianLineFitter : LineFitter
            {
                public override double OptimizeLines(LineParameters[] lines, double[] wl, double[] fl, double[] w, bool[] mask)
                {
                    // Array of free parameters
                    double[] p = new double[1 + 2 * lines.Length];
                    double[] s = new double[p.Length];
                    double[] pmin = new double[p.Length];
                    double[] pmax = new double[p.Length];

                    p[0] = 0;   // wavelength offset
                    s[0] = 10;
                    pmin[0] = -MaxLambdaOffset;
                    pmax[0] = MaxLambdaOffset;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        p[2 * i + 1] = lines[i].Amplitude;
                        p[2 * i + 2] = lines[i].Sigma;

                        s[2 * i + 1] = 100;
                        s[2 * i + 2] = 1;

                        pmin[2 * i + 1] = 0;
                        pmin[2 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MinVDisp);

                        pmax[2 * i + 1] = double.PositiveInfinity;
                        pmax[2 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MaxVDisp);
                    }

                    double chi2 = 0;
                    chi2 = OptimizeLineParameters(lines, p, s, pmin, pmax, wl, fl, w);

                    // Array of free parameters
                    double offset = p[0];                   // wavelength offset
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i].Model = LineModel.Gaussian;
                        lines[i].Detected = true;
                        lines[i].ExceptionFitting = double.IsNaN(chi2);
                        lines[i].Amplitude = p[2 * i + 1];
                        lines[i].Sigma = p[2 * i + 2];
                        lines[i].Wavelength -= offset;
                        lines[i].LineFitChi2 = chi2;
                        lines[i].LineVDisp = AstroUtil.VDispFromSigma(lines[i].Wavelength, lines[i].Sigma);
                    }

                    return chi2;
                }

                protected override double CalculateChi2(LineParameters[] lines, double[] x, double[] wl, double[] fl, double[] w)
                {
                    // Calculate gaussians
                    double offset = x[0];
                    double[] fy = new double[wl.Length];
                    for (int i = 0; i < lines.Length; i++)
                    {
                        double a = x[2 * i + 1];
                        double s = x[2 * i + 2];
                        double m = lines[i].Wavelength;

                        for (int j = 0; j < wl.Length; j++)
                        {
                            fy[j] += Util.Functions.Gauss(wl[j], a, m - offset, s);
                        }
                    }

                    double chi2 = 0;
                    for (int j = 0; j < wl.Length; j++)
                    {
                        // weight (w) can be used here but isn't robust enough, so removed
                        //chi2 += w[j] * (fl[j] - fy[j]) * (fl[j] - fy[j]);
                        chi2 += (fl[j] - fy[j]) * (fl[j] - fy[j]);
                    }

                    return chi2;
                }
            }

            private class DoubleGaussianLineFitter : LineFitter
            {
                public override double OptimizeLines(LineParameters[] lines, double[] wl, double[] fl, double[] w, bool[] mask)
                {
                    // Array of free parameters
                    double[] p = new double[1 + 4 * lines.Length];
                    double[] s = new double[p.Length];
                    double[] pmin = new double[p.Length];
                    double[] pmax = new double[p.Length];

                    p[0] = 0;   // wavelength offset
                    s[0] = 10;
                    pmin[0] = -MaxLambdaOffset;
                    pmax[0] = MaxLambdaOffset;   // wavelength offset
                    for (int i = 0; i < lines.Length; i++)
                    {
                        p[4 * i + 1] = lines[i].Amplitude * 0.6;
                        p[4 * i + 2] = lines[i].Sigma;
                        p[4 * i + 3] = lines[i].Amplitude * 0.4;
                        p[4 * i + 4] = lines[i].Sigma;

                        s[4 * i + 1] = 100;
                        s[4 * i + 2] = 1;
                        s[4 * i + 3] = 100;
                        s[4 * i + 4] = 1;

                        pmin[4 * i + 1] = 0;
                        pmin[4 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MinVDisp);
                        pmin[4 * i + 3] = 0;
                        pmin[4 * i + 4] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MinVDisp);

                        pmax[4 * i + 1] = double.PositiveInfinity;
                        pmax[4 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MaxVDisp);
                        pmax[4 * i + 3] = double.PositiveInfinity;
                        pmax[4 * i + 4] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MaxVDisp);
                    }

                    double chi2 = 0;
                    chi2 = OptimizeLineParameters(lines, p, s, pmin, pmax, wl, fl, w);

                    // Array of free parameters
                    double offset = p[0];                   // wavelength offset
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i].Model = LineModel.DoubleGaussian;
                        lines[i].Detected = true;
                        lines[i].ExceptionFitting = double.IsNaN(chi2);
                        lines[i].Amplitude = p[4 * i + 1];
                        lines[i].Sigma = p[4 * i + 2];
                        lines[i].Amplitude2 = p[4 * i + 3];
                        lines[i].Sigma2 = p[4 * i + 4];
                        lines[i].Wavelength -= offset;
                        lines[i].Wavelength2 = lines[i].Wavelength;
                        lines[i].LineFitChi2 = chi2;
                        lines[i].LineVDisp = Math.Max(AstroUtil.VDispFromSigma(lines[i].Wavelength, lines[i].Sigma),
                                                      AstroUtil.VDispFromSigma(lines[i].Wavelength2, lines[i].Sigma2));
                    }

                    return chi2;
                }

                protected override double CalculateChi2(LineParameters[] lines, double[] x, double[] wl, double[] fl, double[] w)
                {
                    // Calculate gaussians
                    double offset = x[0];
                    double[] fy = new double[wl.Length];
                    for (int i = 0; i < lines.Length; i++)
                    {
                        double a = x[4 * i + 1];
                        double s = x[4 * i + 2];
                        double a2 = x[4 * i + 3];
                        double s2 = x[4 * i + 4];
                        double m = lines[i].Wavelength;

                        for (int j = 0; j < wl.Length; j++)
                        {
                            fy[j] += Util.Functions.Gauss(wl[j], a, m - offset, s)
                                   + Util.Functions.Gauss(wl[j], a2, m - offset, s2);
                        }
                    }

                    double chi2 = 0;
                    for (int j = 0; j < wl.Length; j++)
                    {
                        // weight (w) can be used here but isn't robust enough, so removed
                        //chi2 += w[j] * (fl[j] - fy[j]) * (fl[j] - fy[j]);
                        chi2 += (fl[j] - fy[j]) * (fl[j] - fy[j]);
                    }

                    return chi2;
                }
            }

            private class DoubleGaussianWithOffsetLineFitter : LineFitter
            {
                public override double OptimizeLines(LineParameters[] lines, double[] wl, double[] fl, double[] w, bool[] mask)
                {
                    // Array of free parameters
                    double[] p = new double[1 + 5 * lines.Length];
                    double[] s = new double[p.Length];
                    double[] pmin = new double[p.Length];
                    double[] pmax = new double[p.Length];

                    p[0] = 0;
                    s[0] = 10;
                    pmin[0] = -MaxLambdaOffset;
                    pmax[0] = MaxLambdaOffset;   // wavelength offset
                    for (int i = 0; i < lines.Length; i++)
                    {
                        p[5 * i + 1] = lines[i].Amplitude * 0.6;
                        p[5 * i + 2] = lines[i].Sigma;
                        p[5 * i + 3] = lines[i].Amplitude * 0.4;
                        p[5 * i + 4] = lines[i].Sigma;
                        p[5 * i + 5] = MaxLambdaDifference / 3;

                        s[5 * i + 1] = 1e2;
                        s[5 * i + 2] = 1;
                        s[5 * i + 3] = 1e2;
                        s[5 * i + 4] = 1;
                        s[5 * i + 5] = 1e1;

                        pmin[5 * i + 1] = 0;
                        pmin[5 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MinVDisp);
                        pmin[5 * i + 3] = 0;
                        pmin[5 * i + 4] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MinVDisp);
                        pmin[5 * i + 5] = -MaxLambdaDifference;

                        pmax[5 * i + 1] = double.PositiveInfinity;
                        pmax[5 * i + 2] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MaxVDisp);
                        pmax[5 * i + 3] = double.PositiveInfinity;
                        pmax[5 * i + 4] = AstroUtil.SigmaFromVDisp(lines[i].Wavelength, MaxVDisp);
                        pmax[5 * i + 5] = MaxLambdaDifference;
                    }

                    double chi2 = 0;
                    chi2 = OptimizeLineParameters(lines, p, s, pmin, pmax, wl, fl, w);

                    // Array of free parameters
                    double offset = p[0];                   // wavelength offset
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i].Model = LineModel.DoubleGaussianWithOffset;
                        lines[i].Detected = true;
                        lines[i].ExceptionFitting = double.IsNaN(chi2);
                        lines[i].Amplitude = p[5 * i + 1];
                        lines[i].Sigma = p[5 * i + 2];
                        lines[i].Amplitude2 = p[5 * i + 3];
                        lines[i].Sigma2 = p[5 * i + 4];
                        lines[i].Wavelength -= offset;
                        lines[i].Wavelength2 = lines[i].Wavelength + p[5 * i + 5];
                        lines[i].LineFitChi2 = chi2;
                        lines[i].LineVDisp = Math.Max(AstroUtil.VDispFromSigma(lines[i].Wavelength, lines[i].Sigma),
                                                      AstroUtil.VDispFromSigma(lines[i].Wavelength2, lines[i].Sigma2));
                        // *** TODO: How to take offset into account for LineVDisp calculation?
                    }

                    return chi2;
                }

                protected override double CalculateChi2(LineParameters[] lines, double[] x, double[] wl, double[] fl, double[] w)
                {
                    // Calculate gaussians
                    double offset = x[0];
                    double[] fy = new double[wl.Length];
                    for (int i = 0; i < lines.Length; i++)
                    {
                        double a = x[5 * i + 1];
                        double s = x[5 * i + 2];
                        double a2 = x[5 * i + 3];
                        double s2 = x[5 * i + 4];
                        double dw = x[5 * i + 5];
                        double m = lines[i].Wavelength;

                        for (int j = 0; j < wl.Length; j++)
                        {
                            fy[j] += Util.Functions.Gauss(wl[j], a, m - offset, s)
                                   + Util.Functions.Gauss(wl[j], a2, m - offset + dw, s2);
                        }
                    }

                    double chi2 = 0;
                    for (int j = 0; j < wl.Length; j++)
                    {
                        // weight (w) can be used here but isn't robust enough, so removed
                        //chi2 += w[j] * (fl[j] - fy[j]) * (fl[j] - fy[j]);
                        chi2 += (fl[j] - fy[j]) * (fl[j] - fy[j]);
                    }

                    return chi2;
                }
            }

            // Overlap sigma limit
            protected const double GroupVDispLimit = 150;
            protected const double GroupSigmaLimit = 15;

            // Used to determine the noise level around lines
            protected const double BackgroundSigmaRange = 25.0;
            // Used to determine fitting width
            protected const double FitSigmaRange = 4;
            // Used to find the peak
            protected const double EstimateSigmaRange = 1;

            // Paramter fit limits
            protected const double MinVDisp = 75;
            protected const double MaxVDisp = 350;
            protected const double MaxLambdaOffset = 5;
            protected const double MaxLambdaDifference = 10;    // in case of double lines

            // Used in Chi2 functions to report out of range derivatives
            protected const double BigNumber = 1e7;

            // Start fitting from a stronger line because it converges better
            protected static readonly double[] AmplitudeStartValues = { 1.0, 1.5, 0.8 };
            //protected static readonly double[] AmplitudeStartValues = { 1.0 };
            // VDisp starting values
            protected static readonly double[] VDispStartValues = { 75, 100, 200 };
            //protected static readonly double[] VDispStartValues = { 150 };

            private LineFitStep step;

            private Spectrum spectrum;
            private double redshift;

            private double[] fback;   // background model of residual
            private double[] fflux;   // 
            private double fnoise;
            private bool[] fmask;
            private double[] fweight;

            public FitTask(LineFitStep step, Spectrum spectrum)
            {
                this.step = step;
                this.spectrum = spectrum;

                // Fitting will be done in this frame
                this.redshift = spectrum.Derived.Redshift.Value.Value;
            }

            private void CreateBackground()
            {
                fflux = spectrum.Flux_Lines;
                fback = new double[fflux.Length];

                // Filter width
                int width = 101;

                // --- Filter residual with a rolling median filter
                double[] temp = new double[width];
                int off = width / 2;

                int pmin, pmax;
                for (int p = 0; p < fflux.Length; p++)
                {
                    pmin = p < off ? 0 : p - off;
                    pmax = p + off > fflux.Length ? fflux.Length : p + off;

                    // get median
                    for (int i = pmin; i < pmax; i++)
                    {
                        temp[i - pmin] = fflux[i];
                    }
                    Array.Sort(temp);
                    double med = temp[(pmax - pmin) / 2];

                    fback[p] = med;
                }

                // DEBUG CODE
                /*
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(@"..\..\output\fline.txt"))
                {
                    for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
                    {
                        outfile.WriteLine("{0}\t{1}\t{2}", spectrum.Spectral_Value[i], fback[i], fflux[i]);
                    }
                }*/

                // Subtract median-filtered background
                for (int i = 0; i < fflux.Length; i++)
                {
                    fflux[i] -= fback[i];
                }

                // Determine overall noise
                double avg;
                Util.Vector.AvgVar(fflux, null, out avg, out fnoise, 3, 5);
            }

            private void CreateMaskAndWeight()
            {
                // --- Create mask and weight vector for fitting

                int maskedpoints = 0;

                // Create mask vector
                fmask = new bool[spectrum.Spectral_Value.Length];
                fweight = new double[spectrum.Spectral_Value.Length];
                for (int i = 0; i < fmask.Length; i++)
                {
                    // Use mask from spectrum
                    bool masked = ((spectrum.Flux_Accuracy_Quality[i] & step.mask) != 0);

                    // Mask invalid values
                    masked |= double.IsNaN(spectrum.Flux_Value[i]) | double.IsInfinity(spectrum.Flux_Value[i]);

                    // if error and flux are equally 0 spectrum point should be masked
                    masked |= (step.maskZeroError && spectrum.Flux_Accuracy_StatError != null && spectrum.Flux_Accuracy_StatError[i] == 0.0);

                    fmask[i] = masked;


                    if (masked)
                    {
                        maskedpoints++;
                    }
                    else
                    {
                        // Calculate weight
                        double w;

                        // add this if everything else is OK
                        if (step.weightWithError && spectrum.Flux_Accuracy_StatError != null && spectrum.Flux_Accuracy_StatError[i] != 0.0)
                        {
                            w = spectrum.Flux_Value[i] / spectrum.Flux_Accuracy_StatError[i];   // relative error   *** TODO: check this
                            w *= w;

                            if (step.errorSoftening != 0.0)
                            {
                                w = Math.Sqrt(w * w + (step.errorSoftening * spectrum.Flux_Value[i] * step.errorSoftening * spectrum.Flux_Value[i]));
                            }

                            fweight[i] = w;
                        }
                        else
                        {
                            fweight[i] = 1.0;
                        }
                    }
                }
            }

            private void CreateLineGroups(out List<LineParameters[]> lineGroups)
            {
                lineGroups = new List<LineParameters[]>();

                // Group lines if they overlap
                double sigma = 0;
                List<LineParameters> linelist = new List<LineParameters>();

                for (int i = 0; i < step.lines.Length; i++)
                {
                    sigma = AstroUtil.SigmaFromVDisp(step.lines[i].Wavelength * (1 + redshift), GroupVDispLimit);

                    if (linelist.Count == 0)
                    {
                        AddLineGroupLine(linelist, i, sigma);
                    }
                    else
                    {
                        // Check if lines should be fitted together
                        if (Math.Abs(step.lines[i].Wavelength * (1 + redshift) - linelist[linelist.Count - 1].Wavelength) < GroupSigmaLimit * sigma)
                        {
                            AddLineGroupLine(linelist, i, sigma);
                        }
                        else
                        {
                            // group done
                            lineGroups.Add(linelist.ToArray());
                            linelist.Clear();

                            AddLineGroupLine(linelist, i, sigma);
                        }
                    }
                }
                if (linelist.Count > 0)
                {
                    lineGroups.Add(linelist.ToArray());
                }
            }

            private void AddLineGroupLine(List<LineParameters> linelist, int i, double sigma)
            {
                linelist.Add(new LineParameters()
                {
                    Name = step.lines[i].Name,
                    LabWavelength = step.lines[i].Wavelength,
                    Wavelength = step.lines[i].Wavelength * (1 + redshift),
                    Sigma = sigma
                });
            }

            private void GetLineGroupRanges(LineParameters[] lines, double sigmaRange, out double[] wl, out double[] fl, out double[] w, out bool[] mask)
            {
                if (lines[0].Wavelength < spectrum.Spectral_Value[0] ||
                    lines[lines.Length - 1].Wavelength > spectrum.Spectral_Value[spectrum.Spectral_Value.Length - 1])
                {
                    wl = fl = w = null;
                    mask = null;
                    return;
                }

                // determine wavelength range
                double minwave = double.MaxValue, maxwave = double.MinValue;
                double minsigma = double.MaxValue, maxsigma = double.MinValue;
                for (int i = 0; i < lines.Length; i++)
                {
                    minwave = Math.Min(lines[i].Wavelength, minwave);
                    maxwave = Math.Max(lines[i].Wavelength, maxwave);
                    minsigma = Math.Min(lines[i].Sigma, minsigma);
                    maxsigma = Math.Max(lines[i].Sigma, maxsigma);
                }

                // --- Get range
                double sr;
                if (maxsigma < 1.0)
                {
                    sr = sigmaRange / maxsigma;
                }
                else
                {
                    sr = sigmaRange;
                }

                // Get range for fitting
                double[][] temp;
                Util.Grid.GetRange(spectrum.Spectral_Value, new double[][] { fflux, fweight }, fmask,
                    minwave - sr * maxsigma,
                    maxwave + sr * maxsigma, out wl, out temp, out mask);


                fl = temp[0];
                w = temp[1];

                // Calculate weight *** TODO: implement this
                /*
                for (int i = 0; i < w.Length; i++)
                {
                    bool inside = false;
                    for (int j = 0; j < lines.Length; j++)
                    {
                        inside |= (Math.Abs(wl[i] - lines[j].Wavelength) <= FitSigmaRange * lines[j].Sigma);
                    }

                    if (inside)
                    {
                        w[i] = 1.0; // w[i];
                    }
                    else
                    {
                        w[i] = 0.0;
                    }
                }
                */

#if debug
            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter("lines.txt"))
            {
                for (int qq = 1; qq < wl.Length; qq++)
                {
                    outfile.WriteLine("{0}\t{1}", wl[qq], fl[qq]);
                }
            }
#endif

            }

            private void EstimateLineParameters(LineParameters[] lines, double[] wl, double[] fl, double vdisp, double amp)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i].Sigma = AstroUtil.SigmaFromVDisp(lines[i].Wavelength * (1 + redshift), vdisp) - 0.1;

                    // find min/max to estimate line height
                    double ymin = double.MaxValue, ymax = double.MinValue;
                    for (int j = 0; j < fl.Length; j++)
                    {
                        if (Math.Abs(wl[j] - lines[i].Wavelength) < EstimateSigmaRange * lines[i].Sigma)
                        {
                            ymin = Math.Min(ymin, fl[j]);
                            ymax = Math.Max(ymax, fl[j]);
                        }
                    }

                    lines[i].Amplitude = 0;
                    if (Math.Abs(ymin) > Math.Abs(ymax))
                    {
                        lines[i].Amplitude = ymin;
                    }
                    else
                    {
                        lines[i].Amplitude = ymax;
                    }
                    lines[i].Amplitude *= Math.Sqrt(2 * Math.PI) * lines[i].Sigma * amp;
                }
            }

            private void FitLineGroup(ref LineParameters[] lines)
            {
                double[] wl, fl, w;
                bool[] mask;

                LineFitter[] linefitters =
                    {
                        new GaussianLineFitter(),
                        new DoubleGaussianLineFitter(),
                        new DoubleGaussianWithOffsetLineFitter()
                    };

                GetLineGroupRanges(lines, FitSigmaRange, out wl, out fl, out w, out mask);

                if (wl != null && wl.Length > 0)
                {
                    int tries = VDispStartValues.Length * AmplitudeStartValues.Length;
                    LineParameters[][][] res = new LineParameters[linefitters.Length][][];
                    double[][] chi2 = new double[linefitters.Length][];
                    double[] bgnoise = new double[linefitters.Length];

                    double bestchi2;
                    int[] besti = new int[linefitters.Length];

                    int maxm = -1;
                    for (int m = 0; m < linefitters.Length; m++)
                    {
                        maxm = m;
                        res[m] = new LineParameters[tries][];
                        chi2[m] = new double[tries];

                        // Run fitting with a large set of initial parameters
                        int iter = 0;
                        bestchi2 = double.MaxValue;
                        besti[m] = -1;
                        for (int ia = 0; ia < AmplitudeStartValues.Length; ia++)
                        {
                            for (int iv = 0; iv < VDispStartValues.Length; iv++)
                            {
                                res[m][iter] = (LineParameters[])lines.Clone();
                                EstimateLineParameters(res[m][iter], wl, fl, VDispStartValues[iv], AmplitudeStartValues[ia]);
                                chi2[m][iter] = linefitters[m].OptimizeLines(res[m][iter], wl, fl, w, mask);
                                chi2[m][iter] /= wl.Length;

                                if (chi2[m][iter] < bestchi2)
                                {
                                    bestchi2 = chi2[m][iter];
                                    besti[m] = iter;
                                }

                                iter++;
                            }
                        }

                        if (besti[m] == -1)
                        {
                            // all fitting failed
                            continue;
                        }
                        else
                        {
                            bgnoise[m] = CalculateLineGroupBackgroundNoiseLevel(res[m][besti[m]]);

                            // If noise level is too high, use more advanced fitting method
                            //if (res[m][besti[m]][0].LineFitChi2 > 2 * bgnoise[m])
                            if (bestchi2 > 2 * bgnoise[m])
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    //Find "bestest" fit of all line models (more complex methods might be worse)
                    int bestm = -1;
                    bestchi2 = double.MaxValue;
                    for (int m = 0; m < maxm + 1; m++)
                    {
                        if (besti[m] != -1 && chi2[m][besti[m]] < bestchi2)
                        {
                            bestchi2 = chi2[m][besti[m]];
                            bestm = m;
                        }
                    }


                    if (bestm == -1)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            lines[i].Detected = false;
                            lines[i].ExceptionFitting = true;
                        }
                    }
                    else
                    {
                        lines = res[bestm][besti[bestm]];
                        FilterLines(lines, bgnoise[bestm]);
                    }
                }
            }

#if false
            private double OptimizeLineGroupParameters(LineParameters[] lines, double[] p, double[] wl, double[] fl, double[] w, Chi2Function chi2function)
            {
                NumericalRecipes.MinimizationOrMaximizationOfFunctions.Dfpmin min = new NumericalRecipes.MinimizationOrMaximizationOfFunctions.Dfpmin();

                try
                {
                    min.ITMAX = 200;
                    min.dfpmin(p, p.Length, 1e-7,
                        delegate(double[] x)
                        {
                            return chi2function(lines, x, wl, fl, w);
                        },
                        delegate(double[] x)
                        {
                            double eps = 1.0e-5;

                            // Calculate gradient

                            double f = chi2function(lines, x, wl, fl, w);

                            double fold = f;

                            double[] df = new double[x.Length];
                            double[] xh = new double[x.Length];
                            for (int i = 0; i < x.Length; i++)
                            {
                                xh[i] = x[i];
                            }

                            for (int i = 0; i < x.Length; i++)
                            {

                                double temp = x[i];
                                double h = eps * temp;
                                if (h == 0.0) h = eps;

                                xh[i] = temp + h;
                                h = xh[i] - temp;

                                double fh = chi2function(lines, xh, wl, fl, w);

                                xh[i] = temp;
                                df[i] = (fh - fold) / h;
                            }

                            return df;
                        });

                    // Extract values

                    // Calculate reduced and normalized chi2
                    double chi2 = chi2function(lines, p, wl, fl, w);
                    chi2 /= wl.Length;
                    return chi2;
                }
                catch (Exception)
                {
                    return double.NaN;
                }
            }
#endif

            private void ReconstructLines(LineParameters[] lines, double[] wl, out double[] fl)
            {
                fl = new double[wl.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Detected)
                    {
                        for (int j = 0; j < wl.Length; j++)
                        {
                            switch (lines[i].Model)
                            {
                                case LineModel.Gaussian:
                                    fl[j] += Util.Functions.Gauss(spectrum.Spectral_Value[j], lines[i].Amplitude, lines[i].Wavelength, lines[i].Sigma);
                                    break;
                                case LineModel.DoubleGaussian:
                                case LineModel.DoubleGaussianWithOffset:
                                    fl[j] += Util.Functions.Gauss(spectrum.Spectral_Value[j], lines[i].Amplitude, lines[i].Wavelength, lines[i].Sigma);
                                    fl[j] += Util.Functions.Gauss(spectrum.Spectral_Value[j], lines[i].Amplitude2, lines[i].Wavelength2, lines[i].Sigma2);
                                    break;
                                case LineModel.SkewGaussian:
                                    fl[j] += Util.Functions.SkewGauss(spectrum.Spectral_Value[j], lines[i].Amplitude, lines[i].Wavelength, lines[i].Sigma, lines[i].Skew);
                                    fl[j] += Util.Functions.Gauss(spectrum.Spectral_Value[j], lines[i].Amplitude2, lines[i].Wavelength, lines[i].Sigma2);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                    }
                }
            }

#if false
            private void ReconstructLines(LineFit res)
            {
                // --- Reconstruct lines vector

                res.LineModel = new double[spectrum.Spectral_Value.Length];
                for (int i = 0; i < res.Lines.Length; i++)
                {
                    if (res.Lines[i].Detected)
                    {
                        for (int j = 0; j < spectrum.Spectral_Value.Length; j++)
                        {

                            switch (res.Lines[i].Model)
                            {
                                case LineModel.Gaussian:
                                    res.LineModel[j] += Gauss(spectrum.Spectral_Value[j], res.Lines[i].Amplitude, res.Lines[i].Wavelength, res.Lines[i].Sigma);
                                    break;
                                case LineModel.DoubleGaussian:
                                    res.LineModel[j] += Gauss(spectrum.Spectral_Value[j], res.Lines[i].Amplitude, res.Lines[i].Wavelength, res.Lines[i].Sigma);
                                    res.LineModel[j] += Gauss(spectrum.Spectral_Value[j], res.Lines[i].Amplitude2, res.Lines[i].Wavelength2, res.Lines[i].Sigma2);
                                    break;
                                case LineModel.SkewGaussian:
                                    res.LineModel[j] += SkewGauss(spectrum.Spectral_Value[j], res.Lines[i].Amplitude, res.Lines[i].Wavelength, res.Lines[i].Sigma, res.Lines[i].Skew);
                                    res.LineModel[j] += Gauss(spectrum.Spectral_Value[j], res.Lines[i].Amplitude2, res.Lines[i].Wavelength, res.Lines[i].Sigma2);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                    }
                }

                res.Chi2 = Util.Vector.ChiSquared(spectrum.Flux_Lines, res.LineModel, fmask, out res.Ndf);
                res.Ndf = spectrum.Flux_Lines.Length;       //**** TODO
            }
#endif

            private double CalculateLineGroupBackgroundNoiseLevel(LineParameters[] lines)
            {
                // Determine the noise outside and inside the range
                double[] wl, fl, w;
                bool[] mask;
                GetLineGroupRanges(lines, BackgroundSigmaRange, out wl, out fl, out w, out mask);

                if (fl != null)
                {
                    // Determine noise level by throwing 3 sigma outliers (lines) away
                    double bgavg, bgstdev;
                    Util.Vector.AvgVar(fl, null, out bgavg, out bgstdev, 3, 5);

                    return bgstdev;
                }
                else
                {
                    return BigNumber;
                }
            }

            private void FilterLines(LineParameters[] lines, double bgnoise)
            {
                // This is the magic: filter out bad lines:

                for (int i = 0; i < lines.Length; i++)
                {
                    // Apply constraints for valid line fits
                    FilterLine(ref lines[i], bgnoise);
                }
            }

            private void FilterLine(ref LineParameters line, double bgnoise)
            {
                // Determine line SNR
                switch (line.Model)
                {
                    case LineModel.Gaussian:
                        line.Snr = Util.Functions.Gauss(0, line.Amplitude, 0, line.Sigma) / bgnoise;
                        line.LineVDisp = AstroUtil.VDispFromSigma(line.Wavelength, line.Sigma);
                        break;
                    case LineModel.DoubleGaussian:
                    case LineModel.DoubleGaussianWithOffset:
                        line.Snr = (Util.Functions.Gauss(0, line.Amplitude, 0, line.Sigma) +
                                    Util.Functions.Gauss(0, line.Amplitude2, 0, line.Sigma2)) / bgnoise;
                        line.LineVDisp = Math.Max(AstroUtil.VDispFromSigma(line.Wavelength, line.Sigma),
                                                  AstroUtil.VDispFromSigma(line.Wavelength, line.Sigma2));
                        // *** TODO: not perfect for the offset version
                        break;
                    default:
                        throw new NotImplementedException();
                }



                // *** TODO work a bit on this
                line.Detected = true

                    && !double.IsNaN(line.LineFitChi2)
                    && !double.IsInfinity(line.Amplitude)
                    && !double.IsNaN(line.Amplitude)
                    && !double.IsInfinity(line.Sigma)
                    && !double.IsNaN(line.Sigma)

                // It's fitted well
                    //&& line.Amplitude != 0
                    //&& line.LineVDisp > 75

                // It has at least a 2 sigma peak
                    && line.Snr > 3;
            }

            private void CalculateEquivalentWidths(LineParameters[] lines)
            {
                for (int i = 0; i < lines.Length; i ++)
                {
                    if (lines[i].Detected)
                    {
                        CalculateEquivalentWidth(ref lines[i]);
                    }
                }
            }

            private void CalculateEquivalentWidth(ref LineParameters line)
            {
                double flux = 0;
                // Calculate line flux
                switch (line.Model)
                {
                    case LineModel.Gaussian:
                        flux = Math.Abs(line.Amplitude);
                        break;
                    case LineModel.DoubleGaussian:
                    case LineModel.DoubleGaussianWithOffset:
                        flux = Math.Abs(line.Amplitude + line.Amplitude2);
                        break;
                    case LineModel.SkewGaussian:
                    default:
                        throw new NotImplementedException();
                }

                double wwdstep = 8;
                double wd, ci;

                while (true)
                {
                    double wdstep = wwdstep;
                    ci = 0;
                    wd = 0;

                    double[] res;

                    for (int st = 0; st < 128; st++)
                    {
                        if (Math.Abs(flux - ci) < 1e-5)
                        {
                            break;
                        }
                        else if (ci < flux)
                        {
                            wd += wdstep;
                        }
                        else
                        {
                            wd -= wdstep;
                        }

                        Util.Integral.Integrate(spectrum.Spectral_Accuracy_BinLow,
                            spectrum.Spectral_Accuracy_BinHigh,
                            new double[][] { spectrum.Flux_Continuum },
                            line.Wavelength - wd, line.Wavelength + wd, out res);

                        ci = res[0];

                        wdstep /= 2.0;
                    }

                    if (Math.Abs(flux - ci) < 1e-5)
                    {
                        break;
                    }
                    else if (double.IsNaN(ci))
                    {
                        wd = double.NaN;
                        break;
                    }
                    else if (wwdstep > 512)
                    {
                        wd = double.NaN;
                        break;
                    }
                    else
                    {
                        wwdstep *= 2;
                    }
                }

                line.EqWidth = 2.0 * wd * Math.Sign(line.Amplitude);    //
            }

            public LineFit Execute()
            {
                List<LineParameters[]> lineGroups;

                CreateBackground();
                CreateMaskAndWeight();
                CreateLineGroups(out lineGroups);

                // Fit lines in groups and count resulting lines
                int lc = 0;
                for (int i = 0; i < lineGroups.Count; i++)
                {
                    LineParameters[] lines = lineGroups[i];

                    lc += lines.Length;
                    FitLineGroup(ref lines);
                    lineGroups[i] = lines;
                }

                // Copy lines from line groups to a common array
                LineFit res = new LineFit();
                res.Lines = new LineParameters[lc];

                int q = 0;
                foreach (LineParameters[] lines in lineGroups)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        res.Lines[q] = lines[i];
                        q++;
                    }
                }

                // Reconstruct lines and calculate residual
                ReconstructLines(res.Lines, spectrum.Spectral_Value, out res.LineModel);

                lock (spectrum)
                {
                    Util.Vector.Subtract(spectrum.Flux_Value, res.LineModel, out spectrum.Flux_Continuum);
                }

                // Calculate equivalent width
                CalculateEquivalentWidths(res.Lines);

                lock (spectrum)
                {
                    spectrum.LineFit = res;
                    spectrum.Model_Lines = res.LineModel;
                }

                return res;
            }
        }

        protected SpectralLineDefinition[] lines;
        //protected DoubleParam vDisp;
        protected long mask;
        protected bool maskZeroError;
        protected bool weightWithError;
        protected double errorSoftening;

        public SpectralLineDefinition[] Lines
        {
            get { return lines; }
            set { lines = value; }
        }

        /* delete
        public DoubleParam VDisp
        {
            get { return vDisp; }
            set { vDisp = value; }
        }*/

        public long Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public bool MaskZeroError
        {
            get { return maskZeroError; }
            set { maskZeroError = value; }
        }

        public bool WeightWithError
        {
            get { return weightWithError; }
            set { weightWithError = value; }
        }

        public double ErrorSoftening
        {
            get { return errorSoftening; }
            set { errorSoftening = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.LineFitTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.LineFitDescription; }
        }

        public LineFitStep()
        {
            InitializeMembers();
        }

        public LineFitStep(LineFitStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.lines = new SpectralLineDefinition[0];
            // delete this.vDisp = new DoubleParam(120.0, "km s-1");

            AddStandardLines();
        }

        private void CopyMembers(LineFitStep old)
        {
            this.lines = old.lines;
            // delete this.vDisp = new DoubleParam(old.vDisp);
        }

        protected override Spectrum OnExecute(Spectrum spectrum)
        {
            Console.Write("l");

            LineFit res = new FitTask(this, spectrum).Execute();
            return spectrum;
        }

        public void AddStandardLines()
        {
            this.lines = Constants.StandardLines;
        }

        public void AddSdssLines()
        {
            this.lines = Constants.SdssLines;
        }

        public void AddSdssEmissionLines()
        {
            this.lines = Constants.SdssEmissionLines;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class ContinuumFitStep : PipelineStep
    {

        protected const double VDispMaxSigma = 5;

        public enum FitMethod
        {
            LeastSquares,
            NonNegativeLeastSquares
        }

        protected class FitTask
        {
            private ContinuumFitStep step;

            private Spectrum spectrum;

            private double redshift;
            private double wlinc;               // exponential increment in log-lambda binning
            private double[] wl, wllo, wlhi;    // wavelength grid for fitting
            private double[] rfwl;              // restframe wavelength (for masking, extinction etc.)
            private double[] wltr;              // caches transformed wl for extinction code

            private double[] spfl;              // Spectrum flux to fit, rebinned to log-lambda
            private double[] sperr;             // Spectrum flux error
            private long[] spmask;              // Spectrum mask, rebinned to log-lambda

            private double[] avgfl;             // Average spectrum flux
            private double[][] tempfl;          // Template fluxes
            private double[] tempage;           // Template ages (req's by intrinsic extinction model)

            private bool[] fmask;               // Fitting mask
            private double[] fweight;           // Fitting weight

            private Dictionary<double, double[][]> vdispcache = new Dictionary<double, double[][]>();

            public FitTask(ContinuumFitStep step, Spectrum spectrum)
            {
                this.step = step;
                this.spectrum = spectrum;

                // Fitting will be done in this frame
                this.redshift = spectrum.Derived.Redshift.Value.Value;

                // Rebin spectrum to log-lambda grid for fitting
                CalculateLogGrid();
                PreprocessSpectrum();
                CreateMaskAndWeight(wl, rfwl, spfl, sperr, spmask);
            }

            public ContinuumFit Execute(double vdisp, double tau, double mu)
            {
                RebinTemplates();

                // Use vdisp value from catalog, if possible
                /*if (spectrum.Derived.VelocityDispersion != null && !step.fitVDisp)
                {
                    vdisp = spectrum.Derived.VelocityDispersion.Value.Value;
                }*/

                ContinuumFit res = FitParameters(vdisp, tau, mu);

                if (res.Coeffs == null)
                {
                    throw new System.Exception();
                }

                // Recalculate fit with original binning
                res = RecalculateFit(res);

                return res;
            }

            private void CalculateLogGrid()
            {
                // Allow for velocity dispersion
                double vdispbuffer = 400;

                double wlmin = spectrum.Spectral_Value[0];
                wlmin -= VDispMaxSigma * AstroUtil.SigmaFromVDisp(wlmin, vdispbuffer);

                double wlmax = spectrum.Spectral_Value[spectrum.Spectral_Value.Length - 1];
                wlmax += VDispMaxSigma * AstroUtil.SigmaFromVDisp(wlmax, vdispbuffer);

                wlmin = Math.Log(wlmin);
                wlmax = Math.Log(wlmax);
                int bins = (int)Math.Ceiling((wlmax - wlmin)) * 2500;   // number of bins to use
                wlinc = (wlmax - wlmin) / bins;

                wl = new double[bins];
                wllo = new double[bins];
                wlhi = new double[bins];
                rfwl = new double[bins];
                for (int i = 0; i < wl.Length; i++)
                {
                    wllo[i] = Math.Exp(wlmin + i * wlinc);
                    wl[i] = Math.Exp(wlmin + (i + 0.5) * wlinc);
                    wlhi[i] = Math.Exp(wlmin + (i + 1.0) * wlinc);

                    rfwl[i] = wl[i] / (1 + redshift);
                }
            }

            private void PreprocessSpectrum()
            {
                // Rebin spectrum to log-grid

                double[][] fl;

                Util.Grid.Rebin(
                    spectrum.Spectral_Accuracy_BinLow,
                    spectrum.Spectral_Accuracy_BinHigh,
                    new double[][] { spectrum.Flux_Value, spectrum.Flux_Accuracy_StatError, spectrum.Counts_Value },
                    spectrum.Flux_Accuracy_Quality,
                    wllo, wlhi,
                    out fl, out spmask);

                spfl = fl[0];

                // Which error vector to use
                sperr = fl[1];    // error in the spectrum
            }

            private void CreateMaskAndWeight(double[] wl, double[] rfwl, double[] spfl, double[] sperr, long[] spmask)
            {
                // --- Create mask and weight vector for fitting

                // mask and weight
                fmask = new bool[wl.Length];
                fweight = new double[wl.Length];
                int maskedpoints = 0;

                for (int l = 0; l < wl.Length; l++)
                {
                    // Use mask from spectrum
                    bool masked = ((spmask[l] & step.mask) != 0);

                    // Mask invalid values
                    masked |= double.IsNaN(spfl[l]) | double.IsInfinity(spfl[l]);


                    // Mask regions defined in fit step
                    for (int q = 0; q < step.maskMin.Value.Length; q++)
                    {
                        masked |= ((rfwl[l] >= step.maskMin.Value[q]) &&
                            (rfwl[l] <= step.maskMax.Value[q]));
                    }

                    // Mask night sky line
                    for (int q = 0; q < step.maskSkyMin.Value.Length; q++)
                    {
                        masked |= (step.maskSkyLines && (wl[l] >= step.maskSkyMin.Value[q]) &&
                                (wl[l] <= step.maskSkyMax.Value[q]));
                    }

                    // if error and flux are equally 0 spectrum point should be masked
                    masked |= (step.maskZeroError && sperr != null && sperr[l] == 0.0);

                    fmask[l] = masked;

                    if (masked)
                    {
                        maskedpoints++;
                    }
                    else
                    {
                        // Calculate weight
                        double w;

                        if (step.weightWithError && sperr != null && sperr[l] != 0.0)
                        {
                            w = spfl[l] / sperr[l];   // relative error   *** TODO: check this
                            w *= w;

                            if (step.errorSoftening != 0.0)
                            {
                                w = Math.Sqrt(w * w + (step.errorSoftening * spfl[l] * step.errorSoftening * spfl[l]));
                            }

                            fweight[l] = w;
                        }
                        else
                        {
                            fweight[l] = 1.0;
                        }
                    }
                }
            }

            private void RebinTemplates()
            {
                RebinTemplates(wllo, wlhi);
            }

            private void RebinTemplates(double[] wllo, double[] wlhi)
            {
                long[] nmask;

                // Rebin average
                if (step.subtractAverage && step.average != null)
                {
                    // Shift to the frame of the spectrum
                    Spectrum t = new Spectrum(step.average);
                    t.Redshift(redshift);

                    Util.Grid.Rebin(t.Spectral_Accuracy_BinLow, t.Spectral_Accuracy_BinHigh, t.Flux_Value, null,
                        wllo, wlhi, out avgfl, out nmask);
                }
                else
                {
                    avgfl = null;
                }

                // Rebin templates
                int tempnum = step.templates.Length;

                tempage = new double[tempnum];
                tempfl = new double[tempnum][];
                for (int i = 0; i < tempnum; i++)
                {
                    // Age of template required for extinction model
                    if (step.templates[i].ModelParameters != null)
                    {
                        tempage[i] = step.templates[i].ModelParameters.T_form.Value;
                    }

                    // Shift to the frame of the spectrum
                    Spectrum t = new Spectrum(step.templates[i]);
                    t.Redshift(redshift);

                    Util.Grid.Rebin(t.Spectral_Accuracy_BinLow, t.Spectral_Accuracy_BinHigh, t.Flux_Value, null,
                        wllo, wlhi, out tempfl[i], out nmask);
                }
            }

            private void ApplyVDispToTemplates(double vdisp, out double[][] ntempfl)
            {
                if (vdispcache.ContainsKey(vdisp))
                {
                    ntempfl = vdispcache[vdisp];
                }
                else
                {

                    // Compute kernel
                    double c = Constants.LightSpeed / 1000.0;   // km s-1

                    // Number of bins within 5 sigma
                    int bins = (int)Math.Ceiling(Math.Log(VDispMaxSigma * vdisp / c + 1) / wlinc) + 1;

                    double[] g = new double[bins];
                    for (int i = 0; i < g.Length; i++)
                    {
                        // TODO: old integrator
                        /*
                        NumericalRecipes.IntegrationOfFunctions.Qsimp intg = new NumericalRecipes.IntegrationOfFunctions.Qsimp();
                        g[i] = intg.qsimp(
                            delegate(double xx)
                            {
                                return Util.Functions.Gauss(xx, 1, 0, vdisp);
                            },
                        c * (Math.Exp(i * wlinc) - 1),
                        c * (Math.Exp((i + 1) * wlinc) - 1));
                         * */

                        // New integrator from alglib
                        var intg = new Alglib.Wrappers.SmoothFunctionIntegrator(
                            delegate(double xx)
                            {
                                return Util.Functions.Gauss(xx, 1, 0, vdisp);
                            },
                            c * (Math.Exp(i * wlinc) - 1),
                            c * (Math.Exp((i + 1) * wlinc) - 1));

                        g[i] = intg.Integrate();
                    }

                    // Convolve
                    Util.Integral.ConvolveSymmetricKernel(tempfl, g, out ntempfl);

                    // Save into the cache
                    vdispcache.Add(vdisp, ntempfl);
                }
            }

            private ContinuumFit FitContinuum(double vdisp, double tau, double mu)
            {
                // Modified templates for current iteration
                double[][] ntempfl;

                // Apply veolocity dispersion to templates
                if ((step.applyVDisp || step.fitVDisp)) // && vdisp > 100)
                {
                    ApplyVDispToTemplates(vdisp, out ntempfl);
                }
                else
                {
                    ntempfl = tempfl;
                }

                // Apply extinction to templates
                if (step.applyExtinction || step.fitExtinction)
                {
                    // Use restframe wavelength!
                    AstroUtil.Extinction_Charlot(rfwl, ntempfl, tempage, out ntempfl, tau, mu, ref wltr);
                }

                ContinuumFit res = new ContinuumFit();

                double[] value;
                if (step.subtractAverage)
                {
                    Util.Vector.Subtract(spfl, avgfl, out value);
                }
                else
                {
                    value = spfl;
                }

                switch (step.method)
                {
                    case FitMethod.LeastSquares:
                        Util.Fit.FitLeastSquares(value, fmask, fweight, ntempfl, out res.Coeffs, out res.Covariance, out res.RegressionCoeff);
                        break;
                    case FitMethod.NonNegativeLeastSquares:
                        Util.Fit.FitNonNegativeLeastSquares(value, fmask, fweight, ntempfl, out res.Coeffs, out res.Covariance, out res.RegressionCoeff);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // Calculate continuum and residual

                Util.Vector.Sum(ntempfl, res.Coeffs, out res.Continuum);
                Util.Vector.Subtract(value, res.Continuum, out res.Residual);

                if (step.subtractAverage)
                {
                    Util.Vector.Add(res.Continuum, avgfl, out res.Continuum);
                }

                res.Chi2 = Util.Vector.ChiSquared(spfl, res.Continuum, fmask, out res.Ndf);

                res.VDisp = vdisp;
                res.Tau_V = tau;
                res.Mu = mu;

                // **** DEBUG CODE
                /*using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(String.Format("chi2_{0}.txt", spectrum.Id), true))
                {
                        outfile.WriteLine("{0}\t{1}", res.VDisp, res.Chi2);
                }*/
                // **** END DEBUG CODE

                return res;
            }

            private ContinuumFit FitParameters(double vdisp, double tau, double mu)
            {
                // --- Fit continuum by fitting vdisp and E(B-V)

                // Array for initial fit parameters
                double[] p = { vdisp, tau };
                double[] pmin = { 50, 0 };
                double[] pmax = { 350, 15 };
                double[] s = { 100, 1 };

                Alglib.Wrappers.ScalarFunctionOptimizationProblem min = new Alglib.Wrappers.ScalarFunctionOptimizationProblem();

                min.Diffstep = 1e-2;
                min.MaxIterations = 2000;
                min.Epsi = min.Epsx = min.Epsf = 1e-7;
                min.Epsg = 1e-5;

                min.Parameters = p;
                min.Scale = s;
                min.MinLimits = pmin;
                min.MaxLimits = pmax;
                min.Function = delegate(double[] x)
                {
                    return FitContinuum(x[0], x[1], mu).Chi2; // / (spfl.Length - 2);
                };

                Alglib.Wrappers.StopCriterium sc = min.Optimize(new Alglib.Wrappers.LevenbergMarquardtMinimizer());

                ContinuumFit res = FitContinuum(min.Parameters[0], min.Parameters[1], mu);
                
                return res;
            }

            private ContinuumFit RecalculateFit(ContinuumFit res)
            {
                // Restframe wavelength grid
                Util.Vector.Multiply(1 / (1 + redshift), spectrum.Spectral_Value, out rfwl);

                // Recalculate mask (needed for chi2)
                CreateMaskAndWeight(spectrum.Spectral_Value, rfwl, spectrum.Flux_Value, spectrum.Flux_Accuracy_StatError, spectrum.Flux_Accuracy_Quality);

                // Rebin average and templates to match spectrum
                RebinTemplates(spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh);

                // Modified templates for current iteration
                double[][] ntempfl;

                // Apply velocity dispersion to templates
                if ((step.applyVDisp || step.fitVDisp) && res.VDisp > 100)
                {
                    double c = Constants.LightSpeed / 1000.0;
                    double sq2piinv = 1 / Math.Sqrt(2 * Math.PI);
                    double a = sq2piinv / res.VDisp;
                    double b = 2 * res.VDisp * res.VDisp;

                    Util.Integral.Convolve(
                        spectrum.Spectral_Value, spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh,
                        tempfl,
                        -20, 20, 5,
                        delegate(double dx, double x)
                        {
                            double v = c * ((x + dx) / x - 1);
                            double q = a * Math.Exp(-v * v / b);
                            return q;
                        },
                        out ntempfl);
                }
                else
                {
                    ntempfl = tempfl;
                }

                // Apply extinction to templates
                if (step.applyExtinction || step.fitExtinction)
                {
                    // Use restframe wavelength!
                    wltr = null;
                    AstroUtil.Extinction_Charlot(rfwl, ntempfl, tempage, out ntempfl, res.Tau_V, res.Mu, ref wltr);
                }

                // Reconstruct model continuum

                Util.Vector.Sum(ntempfl, res.Coeffs, out res.Continuum);

                if (step.subtractAverage)
                {
                    Util.Vector.Add(res.Continuum, avgfl, out res.Continuum);
                }

                Util.Vector.Subtract(spectrum.Flux_Value, res.Continuum, out res.Residual);

                // Calculate statistics
                res.Chi2 = Util.Vector.ChiSquared(spectrum.Flux_Value, res.Continuum, fmask, out res.Ndf);

                res.TemplateNames = new string[step.templates.Length];
                for (int i = 0; i < step.templates.Length; i++)
                {
                    res.TemplateNames[i] = step.templates[i].Target.Name.Value;
                }

                return res;
            }
        }

        protected FitMethod method;
        protected DoubleArrayParam maskMin;
        protected DoubleArrayParam maskMax;
        protected SpectralLineDefinition[] lines;
        protected bool maskLines;
        protected DoubleArrayParam maskSkyMin;
        protected DoubleArrayParam maskSkyMax;
        protected bool maskSkyLines;
        protected long mask;
        protected bool maskZeroError;
        protected bool weightWithError;
        protected double errorSoftening;
        protected bool applyExtinction;
        protected bool fitExtinction;
        protected DoubleParam tau_v;
        protected DoubleParam mu;
        protected bool applyVDisp;
        protected bool fitVDisp;
        protected DoubleParam vDisp;
        protected bool subtractAverage;
        protected string templateSet;
        protected string[] templateList;

        protected Spectrum average;
        protected Spectrum[] templates;

        public FitMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public DoubleArrayParam MaskMin
        {
            get { return maskMin; }
            set { maskMin = value; }
        }

        public DoubleArrayParam MaskMax
        {
            get { return maskMax; }
            set { maskMax = value; }
        }

        public SpectralLineDefinition[] Lines
        {
            get { return lines; }
            set { lines = value; }
        }

        public bool MaskLines
        {
            get { return maskLines; }
            set { maskLines = value; }
        }

        public DoubleArrayParam MaskSkyMin
        {
            get { return maskSkyMin; }
            set { maskSkyMin = value; }
        }

        public DoubleArrayParam MaskSkyMax
        {
            get { return maskSkyMax; }
            set { maskSkyMax = value; }
        }

        public bool MaskSkyLines
        {
            get { return maskSkyLines; }
            set { maskSkyLines = value; }
        }

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

        public bool ApplyExtinction
        {
            get { return applyExtinction; }
            set { applyExtinction = value; }
        }

        public bool FitExtinction
        {
            get { return fitExtinction; }
            set { fitExtinction = value; }
        }

        public DoubleParam Tau_V
        {
            get { return tau_v; }
            set { tau_v = value; }
        }

        public DoubleParam Mu
        {
            get { return mu; }
            set { mu = value; }
        }

        public bool ApplyVDisp
        {
            get { return applyVDisp; }
            set { applyVDisp = value; }
        }

        public bool FitVDisp
        {
            get { return fitVDisp; }
            set { fitVDisp = value; }
        }

        public DoubleParam VDisp
        {
            get { return vDisp; }
            set { vDisp = value; }
        }

        public bool SubtractAverage
        {
            get { return subtractAverage; }
            set { subtractAverage = value; }
        }

        public string TemplateSet
        {
            get { return templateSet; }
            set { templateSet = value; }
        }

        public string[] TemplateList
        {
            get { return templateList; }
            set { templateList = value; }
        }

        public Spectrum Average
        {
            get { return average; }
            set { average = value; }
        }

        public Spectrum[] Templates
        {
            get { return templates; }
            set { templates = value; }
        }


        public override string Title
        {
            get { return StepDescriptions.ContinuumFitTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.ContinuumFitDescription; }
        }

        public ContinuumFitStep()
        {
            InitializeMembers();
        }

        public ContinuumFitStep(ContinuumFitStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = FitMethod.NonNegativeLeastSquares;
            this.maskMin = new DoubleArrayParam(true);
            this.maskMax = new DoubleArrayParam(true);
            this.lines = new SpectralLineDefinition[0];
            this.maskLines = true;
            this.maskSkyMin = new DoubleArrayParam(true);
            this.maskSkyMax = new DoubleArrayParam(true);
            this.maskSkyLines = true;
            this.mask = 0;
            this.maskZeroError = true;
            this.weightWithError = true;
            this.errorSoftening = 0.0;
            this.applyVDisp = false;
            this.fitVDisp = false;
            this.vDisp = new DoubleParam(200.0, "km s-1");
            this.applyExtinction = false;
            this.fitExtinction = false;
            this.tau_v = new DoubleParam(0, "");
            this.mu = new DoubleParam(0.3, "");
            this.subtractAverage = false;
            this.templateSet = null;
            this.templateList = null;

            this.average = null;
            this.templates = null;

            AddSdssLines();
        }

        private void CopyMembers(ContinuumFitStep old)
        {
            this.method = old.method;
            this.maskMin = new DoubleArrayParam(old.maskMin);
            this.maskMax = new DoubleArrayParam(old.maskMax);
            this.lines = old.lines;
            this.maskLines = old.maskLines;
            this.maskSkyMin = new DoubleArrayParam(old.maskSkyMin);
            this.maskSkyMax = new DoubleArrayParam(old.maskSkyMax);
            this.maskSkyLines = old.maskLines;
            this.mask = old.mask;
            this.maskZeroError = old.maskZeroError;
            this.weightWithError = old.weightWithError;
            this.errorSoftening = old.errorSoftening;
            this.applyVDisp = old.applyVDisp;
            this.fitVDisp = old.fitVDisp;
            this.vDisp = new DoubleParam(old.vDisp);
            this.applyExtinction = old.applyExtinction;
            this.fitExtinction = false;
            this.tau_v = new DoubleParam(old.tau_v);
            this.mu = new DoubleParam(old.mu);
            this.subtractAverage = old.subtractAverage;
            this.templateSet = old.templateSet;
            this.templateList = old.templateList;       // ****** TODO do array copy

            this.average = old.average; // ********** TODO do array copy here
            this.templates = old.templates; // ********** TODO do array copy here
        }

        public override void InitializeStep()
        {
            base.InitializeStep();

            if (templates == null || templates.Length == 0)
            {
                throw new NotImplementedException();

                // check and reimplement code below to automatically load templates
                // this needed for the on-line code where templates are loaded by id
                // automatocally, instead of programatically by the calling code

#if false

                // Load templates
                IdSearchParameters idpar = new IdSearchParameters(true);
                idpar.UserGuid = Guid.Empty; //**** LoggedInUserGuid;
                idpar.LoadPoints = true;
                idpar.LoadDetails = true;

                /* **** load default templates - can be removed?
                if (par.TemplateList.Length == 0)
                {
                    // loading templates
                    TemplateSet temp = new TemplateSet(true);
                    Connector.LoadTemplateSet(temp, int.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateSet"]));
                    idpar.Ids = Connector.QueryTemplates(temp.Id);
                }
                else
                {*/
                idpar.Ids = this.templateList;
                /*}*/

                List<Spectrum> tmps = new List<Spectrum>();
                tmps.AddRange(Connector.GetSpectrum(idpar));
                templates = tmps.ToArray();

#endif
            }
        }

        protected override Spectrum OnExecute(Spectrum spectrum)
        {
            Console.Write("c");

#if false
            // Run fitting for each template set
            ContinuumFit[] res = new ContinuumFit[templates.Length];

            double minchi2 = double.MaxValue;
            int minidx = -1;

            for (int i = 0; i < templates.Length; i++)
            {

                // *** DEBUG CODE
                /*this.fitVDisp = false;

                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(String.Format("chi2_{0}.txt", spectrum.Id), true))
                {
                    for (double vd = 100; vd <= 250; vd += 10)
                    {
                        res[i] = new FitTask(this, spectrum).Execute(i, vd, tau_v.Value, mu.Value);

                        outfile.WriteLine("{0}\t{1}\t{2}\t{3}", i, vd, res[i].Chi2, res[i].VDisp);
                    }
                }*/
                //**** END DEBUG CODE

                // Original code line
                res[i] = new FitTask(this, spectrum).Execute(i, vDisp.Value, tau_v.Value, mu.Value);

                if (res[i].Chi2 < minchi2)
                {
                    minchi2 = res[i].Chi2;
                    minidx = i;
                }
            }

            lock (spectrum)
            {
                if (minidx != -1)
                {
                    spectrum.ContinuumFitParameters = res[minidx];
                    spectrum.ContinuumFitBest = minidx;
                    spectrum.Model_Continuum = res[minidx].Continuum;
                    spectrum.Flux_Lines = res[minidx].Residual;
                }
            }
#endif

            lock (spectrum)
            {
                spectrum.ContinuumFits.Add(new FitTask(this, spectrum).Execute(vDisp.Value, tau_v.Value, mu.Value));
            }

            return spectrum;
        }

        public void AddSdssLines()
        {
            this.lines = Constants.SdssLines;

            GenerateLineMask();
        }

        public void AddSdssEmissionLines()
        {
            this.lines = Constants.SdssEmissionLines;

            GenerateLineMask();
        }

        public void GenerateLineMask()
        {
            if (this.maskLines)
            {
                // calculating mask with a given velocity dispersion
                double c = Constants.LightSpeed;		// vacuum, m/s
                double z = 1000 * this.VDisp / c;

                int maskcount = 0;
                if (this.maskLines) maskcount += this.lines.Length;
                this.maskMin.Value = new double[maskcount];
                this.maskMax.Value = new double[maskcount];

                if (this.maskLines)
                    for (int i = 0; i < this.Lines.Length; i++)
                    {
                        this.maskMin.Value[i] = this.lines[i].Wavelength * (1 - z);
                        this.maskMax.Value[i] = this.lines[i].Wavelength * (1 + z);
                    }
            }
            else
            {
                this.maskMin.Value = new double[0];
                this.maskMax.Value = new double[0];
            }

            if (this.maskSkyLines)
            {
                this.maskSkyMin.Value = new double[] { 5574, 4276, 6297, 6364 };
                this.maskSkyMax.Value = new double[] { 5590, 4282, 6305, 6368 };
            }
            else
            {
                this.maskSkyMin.Value = new double[0];
                this.maskSkyMax.Value = new double[0];
            }
        }
    }
}

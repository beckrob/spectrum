#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumFit.cs,v 1.3 2008/10/27 20:17:37 dobos Exp $
 *   Revision:    $Revision: 1.3 $
 *   Date:        $Date: 2008/10/27 20:17:37 $
 */
#endregion
using System;
using Lapack;
using VoServices.Schema.Spectrum;
using VoServices.Spectrum.Lib;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace VoServices.Spectrum.Lib
{
    /// <summary>
    /// Summary description for SpectrumFit.
    /// </summary>
    public class SpectrumFit
    {
        public SpectrumFit()
        {
        }

        /*
        public static VoServices.Spectrum.Lib.Spectrum Combine(Spectrum orig, Spectrum[] templates, double[] coeffs)
        {
            VoServices.Spectrum.Lib.Spectrum sed = new VoServices.Spectrum.Lib.Spectrum();
            sed.BasicInitialize();

            sed.Target.Name.Value = "Best fit";

            int points = templates[0].Spectral_Value.Length;
            sed.Spectral_Value = new double[points];
            sed.Flux_Value = new double[points];
            sed.Flux_Accuracy_StatErrLow = new double[points];
            sed.Flux_Accuracy_StatErrHigh = new double[points];
            sed.Flux_Accuracy_Quality = new long[points];

            for (int i = 0; i < templates[0].Spectral_Value.Length; i++)
            {
                double newflux = 0.0;
                for (int j = 0; j < templates.Length; j++)
                {
                    newflux += templates[j].Flux_Value[i] * coeffs[j];
                }
                sed.Spectral_Value[i] = templates[0].Spectral_Value[i];
                sed.Flux_Value[i] = newflux;
                sed.Flux_Accuracy_StatErrHigh[i] = 0.0;
                sed.Flux_Accuracy_StatErrLow[i] = 0.0;

                if (orig.Flux_Accuracy_Quality != null)
                {
                    sed.Flux_Accuracy_Quality[i] = orig.Flux_Accuracy_Quality[i];
                }
            }

            return sed;
        }*/

        /*
        public static void Combine_Lines(VoServices.Spectrum.Lib.Spectrum spectrum, double[] lines, double[] coeffs, double vdisp)
        {
            double z = vdisp / Constants.LightSpeed;	// required to calculate line width
            //			double[,] limits = new double [lines.Length, 2];
            //			for (int i = 0; i < lines.Length; i ++)
            //			{
            //				limits[i,0] = lines[i] * (1 - z);
            //				limits[i,1] = lines[i] * (1 + z);
            //			}

            spectrum.Model_Lines = new double[spectrum.Spectral_Value.Length];
            for (int wl = 0; wl < spectrum.Spectral_Value.Length; wl++)
            {
                double newflux = 0.0;
                for (int j = 0; j < coeffs.Length; j++)
                {
                    newflux += coeffs[j] * Gauss(spectrum.Spectral_Value[wl] - lines[j], lines[j] * z);
                }
                
                spectrum.Model_Lines[wl] = newflux;
            }
        }
         * */

        /*
        public static VoServices.Spectrum.Lib.Spectrum Difference(VoServices.Spectrum.Lib.Spectrum orig, VoServices.Spectrum.Lib.Spectrum fit)
        {
            VoServices.Spectrum.Lib.Spectrum sed = new VoServices.Spectrum.Lib.Spectrum();
            sed.BasicInitialize();
            sed.Target.Name.Value = "Difference";

            int points = orig.Spectral_Value.Length;
            sed.Spectral_Value = new double[points];
            sed.Flux_Value = new double[points];
            sed.Flux_Accuracy_StatErrLow = new double[points];
            sed.Flux_Accuracy_StatErrHigh = new double[points];
            sed.Flux_Accuracy_Quality = new long[points];

            for (int i = 0; i < orig.Spectral_Value.Length; i++)
            {
                sed.Spectral_Value[i] = orig.Spectral_Value[i];

                if (orig.Flux_Accuracy_Quality != null && (orig.Flux_Accuracy_Quality[i] & (long)PointMask.NoData) > 0)
                {
                    sed.Flux_Value[i] = 0;
                    sed.Flux_Accuracy_Quality[i] = (long)PointMask.NoData;
                }
                else
                {
                    double newflux = orig.Flux_Value[i] - fit.Flux_Value[i];
                    sed.Flux_Value[i] = newflux;
                    sed.Flux_Accuracy_Quality[i] = 0;
                }

                sed.Flux_Accuracy_StatErrHigh[i] = 0.0;
                sed.Flux_Accuracy_StatErrLow[i] = 0.0;
            }

            return sed;
        }
        */

        public static double Gauss(double x, double s)
        {
            return Math.Exp(-x * x / s / s);
        }

        /*
        public static double Fit_Lines_ChiSquare(VoServices.Spectrum.Lib.Spectrum spectrum, double[] lines, double[] coeffs, double vdisp)
        {
            // *** combine this code with line fitting

            double res = 0.0;

            double z = vdisp / Constants.LightSpeed;	// required to calculate line width

            for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
            {
                double newflux = 0.0;
                for (int j = 0; j < coeffs.Length; j++)
                {
                    newflux += coeffs[j] * Gauss(spectrum.Spectral_Value[i] - lines[j], lines[j] * z);
                }
                res += (spectrum.Flux_Lines[i] - newflux) * (spectrum.Flux_Lines[i] - newflux);
            }

            return res;
        }*/

#if false
        public static double[] Fit_Lines_Linear(VoServices.Spectrum.Lib.Spectrum spectrum, double[] lines, double vdisp, out double[] error)
        {
            double z = vdisp / Constants.LightSpeed;	// required to calculate line width
            double[,] limits = new double[lines.Length, 2];
            for (int i = 0; i < lines.Length; i++)
            {
                limits[i, 0] = lines[i] * (1 - z);
                limits[i, 1] = lines[i] * (1 + z);
            }

            Matrix A = new Matrix(spectrum.Spectral_Value.Length, lines.Length);
            Matrix F = new Matrix(spectrum.Spectral_Value.Length, 1);
            for (int wl = 0; wl < A.Rows; wl++)
            {
                for (int i = 0; i < A.Columns; i++)
                {
                    if (spectrum.Flux_Accuracy_Quality[wl] == 0)
                    {
                        A[wl, i] = Gauss(spectrum.Spectral_Value[wl] - lines[i], (limits[i, 1] - limits[i, 0]) / 2);
                        F[i, 0] += spectrum.Flux_Lines[wl] * A[wl, i];
                    }
                    else
                    {
                        A[wl, i] = 0;
                    }
                }
            }

            Matrix M = A.Transpose() * A;

            /* --- old code, remove
            Matrix M = new Matrix(lines.Length);
            Matrix F = new Matrix(lines.Length, 1);

            
            for (int wl = 0; wl < diff.Spectral_Value.Length; wl++)
            {
                if (diff.Flux_Accuracy_Quality[wl] == 0)
                {
                    for (int i = 0; i < M.Rows; i++)
                    {
                        for (int j = 0; j < M.Columns; j++)
                        {
                            M[i, j] +=
                                Gauss(diff.Spectral_Value[wl] - lines[i], (limits[i, 1] - limits[i, 0]) / 2) *
                                Gauss(diff.Spectral_Value[wl] - lines[j], (limits[j, 1] - limits[j, 0]) / 2);

                        }
                        F[i, 0] += diff.Flux_Value[wl] * Gauss(diff.Spectral_Value[wl] - lines[i], (limits[i, 1] - limits[i, 0]) / 2);
                    }
                }
            }
             * */

            /*
            Matrix A = new Matrix(diff.Spectral_Value.Length, lines.Length);		// templates
            Matrix b = new Matrix(diff.Spectral_Value.Length, 1);					// spectrum

            // setting values of matrices
            for (int i = 0; i < diff.Spectral_Value.Length; i++)
            {
                // generating templates
                for (int j = 0; j < lines.Length; j++)
                {
                    if (diff.Spectral_Value[i] >= limits[j, 0] &&
                        diff.Spectral_Value[i] <= limits[j, 1])		// limits
                    {
                        A[i, j] = Gauss(diff.Spectral_Value[i] - lines[j], (limits[j, 1] - limits[j, 0]) / 2);
                    }
                    else	// use 0 values (no fit to noise)
                    {
                        A[i, j] = 0.0;
                    }
                }

                b[i, 0] = diff.Flux_Value[i];
            }*/

            // performing fit
            Matrix a = null;	// solution
            Matrix C = null;	// cov matrix

            Fit_SVD(M, F, out a, out C);

            double[] result = new double[a.Rows];
            error = new double[a.Rows];
            for (int i = 0; i < a.Rows; i++)
            {
                error[i] = Math.Sqrt(Math.Abs(C[i, i]));
                result[i] = a[i, 0];
            }

            return result;
        }
#endif

        /*
        public static double[] Fit_Lines(double[] lines, VoServices.Spectrum.Lib.Spectrum diff, ref double vdisp, out double chiSquare, out double[] error)
        {
            double[] res = Fit_Lines_Linear(lines, diff, vdisp, out error);
            chiSquare = Fit_Lines_ChiSquare(lines, diff, res, vdisp);
            return res;
        }*/


        /*public static double[] Fit_Lines(VoServices.Spectrum.Lib.Spectrum spectrum, double[] lines, ref double vdisp, out double chiSquare, out double[] error)
        {
            const int ITMAX = 100;
            const double CGOLD = 0.3819660;
            const double ZEPS = 1.0e-10;

            double ax = vdisp - 50, bx = vdisp, cx = vdisp + 50;
            const double tol = 0.01;

            double a, b, d = 0.0, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
            double[] cou = new double[lines.Length];
            double[] cov = new double[lines.Length];
            double[] cow = new double[lines.Length];
            double[] cox = new double[lines.Length];
            double[] eru = new double[lines.Length];
            double[] erv = new double[lines.Length];
            double[] erw = new double[lines.Length];
            double[] erx = new double[lines.Length];
            double e = 0.0;				// This will be the distance moved on the step before last.
            a = (ax < cx ? ax : cx);	// a and b must be in ascending order, but input abscissas need not be.
            b = (ax > cx ? ax : cx);
            x = w = v = bx;				//Initializations...


            // fitting
            //fw=fv=fx=(*f)(x);
            Fit_Lines_Linear(spectrum, lines, x, out erx).CopyTo(cox, 0);
            cox.CopyTo(cow, 0);
            cox.CopyTo(cov, 0);
            erx.CopyTo(erw, 0);
            erx.CopyTo(erv, 0);
            fw = fv = fx = Fit_Lines_ChiSquare(spectrum, lines, cox, x);

            for (int iter = 1; iter <= ITMAX; iter++)
            {
                //Main program loop.
                xm = 0.5 * (a + b);
                tol2 = 2.0 * (tol1 = tol * Math.Abs(x) + ZEPS);
                if (Math.Abs(x - xm) <= (tol2 - 0.5 * (b - a)))
                {
                    //Test for done here.
                    vdisp = x;
                    error = erx;
                    chiSquare = fx;
                    return cox;
                }
                if (Math.Abs(e) > tol1)
                {
                    //Construct a trial parabolic fit.
                    r = (x - w) * (fx - fv);
                    q = (x - v) * (fx - fw);
                    p = (x - v) * q - (x - w) * r;
                    q = 2.0 * (q - r);
                    if (q > 0.0) p = -p;
                    q = Math.Abs(q);
                    etemp = e;
                    e = d;
                    if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (a - x) || p >= q * (b - x))
                        d = CGOLD * (e = (x >= xm ? a - x : b - x));
                    //The above conditions determine the acceptability of the parabolic fit. Here we
                    //take the golden section step into the larger of the two segments.
                    else
                    {
                        d = p / q; //Take the parabolic step.
                        u = x + d;
                        if (u - a < tol2 || b - u < tol2)
                            d = ((xm - x) >= 0.0 ? Math.Abs(tol1) : -Math.Abs(tol1));

                    }
                }
                else
                {
                    d = CGOLD * (e = (x >= xm ? a - x : b - x));
                }
                u = Math.Abs(d) >= tol1 ? x + d : x + ((d >= 0.0 ? Math.Abs(tol1) : -Math.Abs(tol1)));


                // fitting
                // fu = (*f)(u);
                // This is the one function evaluation per iteration.
                Fit_Lines_Linear(spectrum, lines, u, out eru).CopyTo(cou, 0);
                fu = Fit_Lines_ChiSquare(spectrum, lines, cou, u);

                if (fu <= fx)
                {
                    //Now decide what to do with our function evaluation.
                    if (u >= x) a = x; else b = x;

                    //Housekeeping follows:
                    v = w; w = x; x = u;
                    fv = fw; fw = fx; fx = fu;
                    cov = cow; cow = cox; cox = cou;	//
                    erv = erw; erw = erx; erx = eru;

                    //SHFT(v,w,x,u) 
                    //SHFT(fv,fw,fx,fu)
                }
                else
                {
                    if (u < x) a = u; else b = u;
                    if (fu <= fw || w == x)
                    {
                        v = w;
                        w = u;
                        fv = fw;
                        cov = cow;	//
                        erv = erw;
                        fw = fu;
                        cow = cou;	//
                        erw = eru;
                    }
                    else if (fu <= fv || v == x || v == w)
                    {
                        v = u;
                        fv = fu;
                        cov = cou;	//
                        erv = eru;
                    }
                } //Done with housekeeping. Back for another iteration. 
            }
            //nrerror("Too many iterations in brent");
            vdisp = x;
            error = erx;
            chiSquare = fx;
            return cox;
        }
        */

#if false
        private static FitResults FitContinuum(Spectrum spectrum, Spectrum[] templates, FitParameters par)
        {
            // --- Initialize results object

            FitResults res = new FitResults();
            res.Spectrum = spectrum;

            // --- Prepare templates

            /*
            // Make a copy of the templates
            Spectrum[] templates_copy;
            templates_copy = new Spectrum[templates.Length];
            for (int i = 0; i < templates.Length; i++)
                templates_copy[i] = new Spectrum(templates[i]);

            // Copy template names (for displaying later)
            res.TemplateNames = new string[templates.Length];
            for (int q = 0; q < templates.Length; q++)
                res.TemplateNames[q] = templates[q].Target.Name.Value;

            // rebin templates to match the spectrum binning
            foreach (VoServices.Spectrum.Lib.Spectrum temp in templates_copy)
            {
                if (spectrum.Spectral_Accuracy_BinLow != null && spectrum.Spectral_Accuracy_BinHigh != null)
                {
                    temp.FindBins();
                    temp.Rebin(spectrum.Spectral_Value, spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh);
                }
                else
                {
                    throw new NotImplementedException();
                    //temp.ResampleTo(spectrum, par.Interpolation);
                }
            }
            */

            double[][] temp = new double[templates.Length][];
            for (int i = 0; i < templates.Length; i++)
            {
                long[] nmask;
                Util.Grid.Rebin(templates[i].Spectral_Accuracy_BinLow, templates[i].Spectral_Accuracy_BinHigh, templates[i].Flux_Value, null,
                    spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, out temp[i], out nmask);
            }

            // --- Build matrices for fitting ---

            Matrix M = new Matrix(temp.Length);
            Matrix F = new Matrix(temp.Length, 1);

            // blue shift sky line
            // *** night sky changed to intervals
            //double skymin = (Constants.NightSkyLine - 5.0) * (1 - spectrum.OriginalRedshift.Value);
            //double skymax = (Constants.NightSkyLine + 5.0) * (1 - spectrum.OriginalRedshift.Value);

            // mask and weight
            bool[] mask = new bool[spectrum.Spectral_Value.Length];
            double[] weight = new double[spectrum.Spectral_Value.Length];
            int maskedpoints = 0;

            for (int wl = 0; wl < spectrum.Spectral_Value.Length; wl++)
            {
                bool masked = (par.MaskFromSpectra && spectrum.Flux_Accuracy_Quality[wl] != 0);
                for (int q = 0; q < par.MaskMin.Value.Length; q++)
                {
                    masked |= ((spectrum.Spectral_Value[wl] >= par.MaskMin.Value[q]) &&
                        (spectrum.Spectral_Value[wl] <= par.MaskMax.Value[q]));
                }

                // night sky line
                for (int q = 0; q < par.MaskSkyMin.Value.Length; q++)
                {
                    masked |= (par.MaskSkyLines && (spectrum.Spectral_Value[wl] >= par.MaskSkyMin.Value[q]) &&
                            (spectrum.Spectral_Value[wl] <= par.MaskSkyMax.Value[q]));
                }

                // if error and flux are equally 0 this point should be masked
                masked |= (par.MaskZeroError && spectrum.Flux_Accuracy_StatErrHigh != null && spectrum.Flux_Accuracy_StatErrHigh[wl] == 0.0);

                mask[wl] = masked;

                if (masked)
                {
                    maskedpoints++;
                }
                else
                {
                    double w;

                    if (par.WeightWithError && spectrum.Flux_Accuracy_StatErrHigh != null && spectrum.Flux_Accuracy_StatErrHigh[wl] != 0.0)
                    {
                        w = spectrum.Flux_Value[wl] / spectrum.Flux_Accuracy_StatErrHigh[wl];
                        w *= w;

                        if (par.ErrorSoftening != 0.0)
                        {
                            w = Math.Sqrt(w * w + (par.ErrorSoftening * spectrum.Flux_Value[wl] * par.ErrorSoftening * spectrum.Flux_Value[wl]));
                        }

                        weight[wl] = w;
                    }
                    else
                    {
                        weight[wl] = 1.0;
                    }
                }
            }

            // sum over lamda
            for (int wl = 0; wl < spectrum.Spectral_Value.Length; wl++)
            {
                if (!mask[wl])
                {

                    for (int i = 0; i < M.Rows; i++)
                    {
                        for (int j = 0; j < M.Columns; j++)
                        {
                            M[i, j] += (weight[wl] * temp[i][wl] * temp[j][wl]);
                            if (double.IsNaN(M[i, j]))
                                return null;
                        }
                        F[i, 0] += (weight[wl] * spectrum.Flux_Value[wl] * temp[i][wl]);
                        if (double.IsNaN(F[i, 0]))
                            return null;
                    }
                }
            }

            // --- Fit continuum ---

            Matrix a = null;	// solution
            Matrix C = null;	// cov matrix

            switch (par.Method)
            {
                case FitMethods.LeastSquare:		// svd
                    Fit_SVD(M, F, out a, out C);
                    break;
                case FitMethods.NonNegativeLeastSquare:		// nnls
                    Fit_NNLS(M, F, out a);
                    break;
            }

            res.ContinuumRegressionCoeff = RCoeff(M, F, a);
            res.TemplateCoeffs = new double[a.Rows];
            int nzcoeff = 0;
            for (int i = 0; i < a.Rows; i++)
            {
                res.TemplateCoeffs[i] = a[i, 0];
                if (a[i, 0] != 0.0) nzcoeff++;
            }

            // --- Calculate continuum and residual

            Util.Transform.Sum(temp, res.TemplateCoeffs, out spectrum.Model_Continuum);
            Util.Transform.Subtract(spectrum.Flux_Value, spectrum.Model_Continuum, out spectrum.Flux_Lines);

            res.ContinuumChiSquare = 0;
            for (int wl = 0; wl < spectrum.Spectral_Value.Length; wl++)
            {
                if (!mask[wl])
                {
                    res.ContinuumChiSquare += spectrum.Flux_Lines[wl] * spectrum.Flux_Lines[wl];
                }
            }

            res.ContinuumNdf = spectrum.Spectral_Value.Length - maskedpoints; // **** goodpoints.Count;
            // **** ? fix it
            switch (par.Method)
            {
                case FitMethods.NonNegativeLeastSquare:
                    res.ContinuumNdf -= nzcoeff;
                    break;
            }

            // --- Fit Lines

            res.VDisp = par.VDisp;

            if (par.FitLines)
            {
                FitLines(spectrum, res, par);
            }

            return res;
        }
#endif
#if false
        private static void FitLines(Spectrum spectrum, FitResults res, FitParameters par)
        {

            // lines
            double cs0;	// required for vdisp error estimation
            res.LineAmplitude = SpectrumFit.Fit_Lines(spectrum, par.Lines, ref res.VDisp, out cs0, out res.LineAmplitudeError);
            //res.Linefit = SpectrumFit.Combine_Lines(spectrum, par.Lines, res.LineAmplitude, res.VDisp);
            res.LineNames = par.LineNames;

            // calculating error of velocity dispersion
            // have to calculate chi-square at to different positions around the minimum and fit a parabola on them
            double csa = cs0 - 10, csb = cs0 + 10;
            double[] erd;	// dummy, we don't need it
            double[] cfd;

            cfd = SpectrumFit.Fit_Lines_Linear(spectrum, par.Lines, res.VDisp - 10, out erd);
            csa = SpectrumFit.Fit_Lines_ChiSquare(spectrum, par.Lines, cfd, res.VDisp - 10);

            cfd = SpectrumFit.Fit_Lines_Linear(spectrum, par.Lines, res.VDisp + 10, out erd);
            csb = SpectrumFit.Fit_Lines_ChiSquare(spectrum, par.Lines, cfd, res.VDisp + 10);

            // chi2 = (x - x0) / sigma2 + c : should be fitted to csa, cs0 and csb
            //Matrix
            Matrix A = new Matrix(3, 3);
            A[0, 0] = (res.VDisp - 10) * (res.VDisp - 10); A[0, 1] = (res.VDisp - 10); A[0, 2] = 1.0;
            A[1, 0] = (res.VDisp) * (res.VDisp); A[1, 1] = (res.VDisp); A[1, 2] = 1.0;
            A[2, 0] = (res.VDisp + 10) * (res.VDisp + 10); A[2, 1] = (res.VDisp + 10); A[2, 2] = 1.0;

            //Matrix
            Matrix b = new Matrix(3, 1);
            b[0, 0] = csa;
            b[1, 0] = cs0;
            b[2, 0] = csb;

            //Lapack.QrDecomposition qrd = new Lapack.QrDecomposition(A);
            //Matrix sol = qrd.Solve(b);

            Lapack.Svd svd = new Lapack.Svd(A, Implementation.Managed);
            svd.Solve(b);
            Matrix sol = svd.x;

            res.VDispError = 1 / Math.Sqrt(Math.Abs(sol[0, 0]));

            //				Response.Write(String.Format("VDisp error calculated in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
            //				start = DateTime.Now;

            Combine_Lines(spectrum, par.Lines, res.LineAmplitude, res.VDisp);
            spectrum.Flux_Continuum = new double[spectrum.Spectral_Value.Length];
            for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
            {
                if (spectrum.Flux_Accuracy_Quality[i] != 0)
                {
                    // Substitute with continuum fit
                    spectrum.Flux_Continuum[i] = spectrum.Model_Continuum[i];
                }
                else
                {
                    spectrum.Flux_Continuum[i] = spectrum.Flux_Value[i] - spectrum.Model_Lines[i];
                }
            }


            // calculating ChiSquare
            double lineChiSquare = 0.0;
            for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
                lineChiSquare += (spectrum.Flux_Lines[i] - spectrum.Model_Lines[i]) * (spectrum.Flux_Lines[i] - spectrum.Model_Lines[i]);
            res.LineChiSquare = lineChiSquare;

            // amplitudes
            res.LineSigma = new double[par.Lines.Value.Length];
            res.LineSigmaError = new double[par.Lines.Value.Length];
            //_amplitudes = new double[_lines.Length];
            double zz = res.VDisp / Constants.LightSpeed;
            double zze = res.VDispError / Constants.LightSpeed;
            double sqrt2pi = Math.Sqrt(2 * Math.PI);
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                res.LineSigma[i] = par.Lines.Value[i] * zz;
                res.LineSigmaError[i] = par.Lines.Value[i] * zze;
                res.LineAmplitude[i] *= res.LineSigma[i] * sqrt2pi;
            }

            //**** check this
            res.LineNdf = spectrum.Spectral_Value.Length - par.Lines.Value.Length - 1;



            // equivalent widths
            res.LineEqWidth = new double[par.Lines.Value.Length];

#if false
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                try
                {
                    // step with the pointer to the closest point in the spectrum
                    int pointer = 0;
                    while (res.Continuum.Spectral_Value[pointer] <= par.Lines.Value[i])
                    {
                        pointer++;
                        if (pointer >= res.Continuum.Spectral_Value.Length) break;
                    }


                    // now pointer is located right after the required wavelength
                    // integrating continuum until the integral exceeds the amplitude (integral of the Gauss-curve)

                    // probably equivalent width is larger than the resolution of grid, so making the first step
                    // won't cause any problem
                    int pleft = pointer - 1;
                    int pright = pointer;
                    double integral = (res.Continuum.Flux_Value[pleft] + res.Continuum.Flux_Value[pright]) / 2 * (res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft]);
                    double nintegral = 0.0;

                    while (integral + nintegral < Math.Abs(res.LineAmplitude[i]))
                    {
                        // suppose that integral is less than the amplitude
                        // make a single step and check whether we exceeded amplitude
                        int npleft = pleft - 1;
                        int npright = pright + 1;

                        nintegral += (res.Continuum.Flux_Value[npleft] + res.Continuum.Flux_Value[pleft]) / 2 * (res.Continuum.Spectral_Value[pleft] - res.Continuum.Spectral_Value[npleft]);
                        nintegral += (res.Continuum.Flux_Value[pright] + res.Continuum.Flux_Value[npright]) / 2 * (res.Continuum.Spectral_Value[npright] - res.Continuum.Spectral_Value[pright]);

                        pleft = npleft;
                        pright = npright;
                    }

                    // now the bracketed area slightly larger than the required
                    // but we may accept it as an assumption and interpolate between this and the previous integral
                    if (nintegral != 0.0)
                        res.LineEqWidth[i] = res.LineAmplitude[i] * (res.Continuum.Spectral_Value[pleft + 1] - res.Continuum.Spectral_Value[pleft] +
                            res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft - 1]) / nintegral;
                    else
                    {

                        res.LineEqWidth[i] = res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft];

                        // *** eq with can be negative?
                        //if (res.LineAmplitude[i] < 0) res.LineEqWidth[i] = - res.LineEqWidth[i];
                    }
                    res.LineEqWidth[i] = Math.Abs(res.LineEqWidth[i]);
                }
                catch (System.Exception)
                {
                    res.LineEqWidth[i] = double.NaN;
                }
            }
#endif
        }
#endif

        /*
        private static FitResults DoFit(Spectrum spectrum, Spectrum[] templates, FitParameters par)
        {
            // Make a copy of the templates
            Spectrum[] templates_copy;
            templates_copy = new Spectrum[templates.Length];
            for (int i = 0; i < templates.Length; i++)
                templates_copy[i] = new Spectrum(templates[i]);

            FitResults res = new FitResults();

            res.Original = spectrum;

            //DateTime start = DateTime.Now;

            res.TemplateNames = new string[templates.Length];
            for (int q = 0; q < templates.Length; q++)
                res.TemplateNames[q] = templates[q].Target.Name.Value;


            // resample templates to the spectrum
            foreach (VoServices.Spectrum.Lib.Spectrum temp in templates_copy)
            {
                temp.ResampleTo(spectrum, par.Interpolation);
            }

            //			Response.Write(String.Format("Templates resampled in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
            //			start = DateTime.Now;

            //
            //fitmask
            //			fitmask = new VoServices.Spectrum.Lib.Spectrum();
            //			fitmask.InitializeMembers();
            //			fitmask.Target.Name.Value = "mask";

            List<int> goodpoints = new List<int>();
            for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
            {
                bool masked = (par.MaskFromSpectra && spectrum.Flux_Accuracy_Quality[i] != 0);
                for (int q = 0; q < par.MaskMin.Value.Length; q++)
                {
                    masked |= ((spectrum.Spectral_Value[i] >= par.MaskMin.Value[q]) &&
                        (spectrum.Spectral_Value[i] <= par.MaskMax.Value[q]));
                }

                // if error and flux are equally 0 this point should be masked
                masked |= (par.MaskZeroError && spectrum.Flux_Accuracy_StatErrHigh != null && spectrum.Flux_Accuracy_StatErrHigh[i] == 0.0);

                if (!masked)
                    goodpoints.Add(i);
            }

            if (goodpoints.Count < templates.Length)
                return null;
            else
            {
                Matrix A = new Matrix(goodpoints.Count, templates_copy.Length);		// templates
                Matrix b = new Matrix(goodpoints.Count, 1);				// spectrum			

                for (int i = 0; i < goodpoints.Count; i++)
                {
                    int s = (int)goodpoints[i];
                    for (int j = 0; j < templates_copy.Length; j++)
                    {
                        A[i, j] = (templates_copy[j]).Flux_Value[s];

                        //if (weightError) A[i, j] /= ((spectrum.Flux_Accuracy_StatErrHigh[s] < 0.001)
                        //					 ? 1.0 : spectrum.Flux_Accuracy_StatErrHigh[s]);

                        if (par.WeightWithError) A[i, j] /= Math.Sqrt(
                            (spectrum.Flux_Accuracy_StatErrHigh[s] * spectrum.Flux_Accuracy_StatErrHigh[s])
                            + (par.ErrorSoftening * spectrum.Flux_Value[s] * par.ErrorSoftening * spectrum.Flux_Value[s]));
                    }
                    b[i, 0] = spectrum.Flux_Value[s];
                    //if (weightError) b[0, i] /= ((spectrum.Flux_Accuracy_StatErrHigh[s] < 0.001)
                    //					 ? 1.0 : spectrum.Flux_Accuracy_StatErrHigh[s]);

                    if (par.WeightWithError) b[i, 0] /= Math.Sqrt(
                        (spectrum.Flux_Accuracy_StatErrHigh[s] * spectrum.Flux_Accuracy_StatErrHigh[s])
                        + (par.ErrorSoftening * spectrum.Flux_Value[s] * par.ErrorSoftening * spectrum.Flux_Value[s]));
                }

                //			Response.Write(String.Format("Matrices built in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                //			start = DateTime.Now;

                Matrix a = null;	// solution
                Matrix C = null;	// cov matrix

                // dump matrix
                //			System.IO.StreamWriter outfile = new System.IO.StreamWriter("c:\\temp\\mdump.txt");
                //			outfile.Write(A.ToString());
                //			outfile.Close();

                switch (par.Method)
                {
                    case FitMethods.LeastSquare:		// svd
                        Fit_SVD(A, b, out a, out C);
                        break;
                    case FitMethods.NonNegativeLeastSquare:		// nnls
                        Fit_NNLS(A, b, out a);
                        break;
                }

                //			Response.Write(String.Format("Fit done in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                //			start = DateTime.Now;

                res.ContinuumRegressionCoeff = RCoeff(A, b, a);
                res.TemplateCoeffs = new double[a.Rows];
                int nzcoeff = 0;
                for (int i = 0; i < a.Rows; i++)
                {
                    res.TemplateCoeffs[i] = a[i, 0];
                    if (a[i, 0] != 0.0) nzcoeff++;
                }

                res.ContinuumNdf = goodpoints.Count;

                if (par.Method == FitMethods.NonNegativeLeastSquare)
                    res.ContinuumNdf -= nzcoeff;

                //
                // recombine
                res.Continuum = SpectrumFit.Combine(templates_copy, res.TemplateCoeffs);

                // difference
                res.Difference = SpectrumFit.Difference(spectrum, res.Continuum);

                //			Response.Write(String.Format("Difference in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                //			start = DateTime.Now;

                // calculating ChiSquare
                double fitChiSquare = 0.0;
                for (int i = 0; i < res.Difference.Spectral_Value.Length; i++)
                    fitChiSquare += res.Difference.Flux_Value[i] * res.Difference.Flux_Value[i];
                res.ContinuumChiSquare = fitChiSquare;

                res.VDisp = par.VDisp;
#if false
                if (par.FitLines)
                {
                    // lines
                    double cs0;	// required for vdisp error estimation
                    res.LineAmplitude = SpectrumFit.Fit_Lines(par.Lines, res.Difference, ref res.VDisp, out cs0, out res.LineAmplitudeError);
                    res.Linefit = SpectrumFit.Combine_Lines(spectrum, par.Lines, res.LineAmplitude, res.VDisp);

                    //				Response.Write(String.Format("Lines fitted in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                    //				start = DateTime.Now;

                    // calculating error of velocity dispersion
                    // have to calculate chi-square at to different positions around the minimum and fit a parabola on them
                    double csa = cs0 - 10, csb = cs0 + 10;
                    double[] erd;	// dummy, we don't need it
                    double[] cfd;
                    cfd = SpectrumFit.Fit_Lines_Linear(par.Lines, res.Difference, res.VDisp - 10, out erd);
                    csa = SpectrumFit.Fit_Lines_ChiSquare(par.Lines, res.Difference, cfd, res.VDisp - 10);

                    cfd = SpectrumFit.Fit_Lines_Linear(par.Lines, res.Difference, res.VDisp + 10, out erd);
                    csb = SpectrumFit.Fit_Lines_ChiSquare(par.Lines, res.Difference, cfd, res.VDisp + 10);

                    // chi2 = (x - x0) / sigma2 + c : should be fitted to csa, cs0 and csb
                    //Matrix
                    A = new Matrix(3, 3);
                    A[0, 0] = (res.VDisp - 10) * (res.VDisp - 10); A[0, 1] = (res.VDisp - 10); A[0, 2] = 1.0;
                    A[1, 0] = (res.VDisp) * (res.VDisp); A[1, 1] = (res.VDisp); A[1, 2] = 1.0;
                    A[2, 0] = (res.VDisp + 10) * (res.VDisp + 10); A[2, 1] = (res.VDisp + 10); A[2, 2] = 1.0;

                    //Matrix
                    b = new Matrix(3, 1);
                    b[0, 0] = csa;
                    b[1, 0] = cs0;
                    b[2, 0] = csb;

                    Lapack.QrDecomposition qrd = new Lapack.QrDecomposition(A);
                    Matrix sol = qrd.Solve(b);

                    res.VDispError = 1 / Math.Sqrt(Math.Abs(sol[0, 0]));

                    //				Response.Write(String.Format("VDisp error calculated in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                    //				start = DateTime.Now;

                    // calculating ChiSquare
                    double lineChiSquare = 0.0;
                    for (int i = 0; i < res.Linefit.Spectral_Value.Length; i++)
                        lineChiSquare += (res.Difference.Flux_Value[i] - res.Linefit.Flux_Value[i]) *
                            (res.Difference.Flux_Value[i] - res.Linefit.Flux_Value[i]);
                    res.LineChiSquare = lineChiSquare;

                    // amplitudes
                    res.LineSigma = new double[par.Lines.Value.Length];
                    res.LineSigmaError = new double[par.Lines.Value.Length];
                    //_amplitudes = new double[_lines.Length];
                    double zz = res.VDisp / SpectrumFit.LightSpeed;
                    double zze = res.VDispError / SpectrumFit.LightSpeed;
                    double sqrt2pi = Math.Sqrt(2 * Math.PI);
                    for (int i = 0; i < par.Lines.Value.Length; i++)
                    {
                        res.LineSigma[i] = par.Lines.Value[i] * zz;
                        res.LineSigmaError[i] = par.Lines.Value[i] * zze;
                        res.LineAmplitude[i] *= res.LineSigma[i] * sqrt2pi;
                    }

                    res.LineNdf = res.Difference.Spectral_Value.Length - par.Lines.Value.Length - 1;

                    // equivalent widths
                    res.LineEqWidth = new double[par.Lines.Value.Length];
                    for (int i = 0; i < par.Lines.Value.Length; i++)
                    {
                        try
                        {
                            // step with the pointer to the closest point in the spectrum
                            int pointer = 0;
                            while (res.Continuum.Spectral_Value[pointer] <= par.Lines.Value[i])
                            {
                                pointer++;
                                if (pointer >= res.Continuum.Spectral_Value.Length) break;
                            }


                            // now pointer is located right after the required wavelength
                            // integrating continuum until the integral exceeds the amplitude (integral of the Gauss-curve)

                            // probably equivalent width is larger than the resolution of grid, so making the first step
                            // won't cause any problem
                            int pleft = pointer - 1;
                            int pright = pointer;
                            double integral = (res.Continuum.Flux_Value[pleft] + res.Continuum.Flux_Value[pright]) / 2 * (res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft]);
                            double nintegral = 0.0;

                            while (integral + nintegral < Math.Abs(res.LineAmplitude[i]))
                            {
                                // suppose that integral is less than the amplitude
                                // make a single step and check whether we exceeded amplitude
                                int npleft = pleft - 1;
                                int npright = pright + 1;

                                nintegral += (res.Continuum.Flux_Value[npleft] + res.Continuum.Flux_Value[pleft]) / 2 * (res.Continuum.Spectral_Value[pleft] - res.Continuum.Spectral_Value[npleft]);
                                nintegral += (res.Continuum.Flux_Value[pright] + res.Continuum.Flux_Value[npright]) / 2 * (res.Continuum.Spectral_Value[npright] - res.Continuum.Spectral_Value[pright]);

                                pleft = npleft;
                                pright = npright;
                            }

                            // now the bracketed area slightly larger than the required
                            // but we may accept it as an assumption and interpolate between this and the previous integral
                            if (nintegral != 0.0)
                                res.LineEqWidth[i] = res.LineAmplitude[i] * (res.Continuum.Spectral_Value[pleft + 1] - res.Continuum.Spectral_Value[pleft] +
                                    res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft - 1]) / nintegral;
                            else
                            {

                                res.LineEqWidth[i] = res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft];

                                // *** eq with can be negative?
                                //if (res.LineAmplitude[i] < 0) res.LineEqWidth[i] = - res.LineEqWidth[i];
                            }
                            res.LineEqWidth[i] = Math.Abs(res.LineEqWidth[i]);
                        }
                        catch (System.Exception)
                        {
                            res.LineEqWidth[i] = double.NaN;
                        }
                    }

                    //				Response.Write(String.Format("Eq widths calculated in {0} msec<br>", ((TimeSpan) (DateTime.Now - start)).TotalMilliseconds));
                    //				start = DateTime.Now;
                }
#endif
                return res;
            }
        }
        */

        /*
        public static FitResults Fit(Spectrum spectrum, Spectrum[] templates, FitParameters par)
        {
            return FitContinuum(spectrum, templates, par);
        }

        public static IEnumerable<FitResults> Fit(IEnumerable<Spectrum> spectra, Spectrum[] templates, FitParameters par)
        {
            foreach (Spectrum spectrum in spectra)
            {
                yield return FitContinuum(spectrum, templates, par);
            }
        }
         * */

        /*
        protected static void Fit_SVD(Matrix A, Matrix b, out Matrix a, out Matrix C)
        {
            //********
            //a = new Matrix(A.Columns, 1);				// fitting parameters (solution)
            //C = new Matrix(A.Columns, A.Columns);		// covariance matrix

            Svd svd = new Svd(A, Implementation.Managed);
            svd.Solve(b);

            a = svd.x;
            C = svd.C;
        }
         * */

#if false
        /// <summary>
        /// Non-negative least-square algorithm
        /// </summary>
        /// <param name="E">Input matrix with the vectors to fit in the columns</param>
        /// <param name="f">Function to fit to</param>
        /// <param name="x">Solution</param>
        protected static void Fit_NNLS(Matrix E, Matrix f, out Matrix x)
        {
            // Step 1.

            x = new Matrix(E.Columns, 1);		// solution matrix
            bool[] Z = new bool[E.Columns];		// Z contains indexes held at zero, if true, element is in Z
            for (int i = 0; i < x.Rows; i++)
            {
                x[i, 0] = 0;					//  all elements to 0
                Z[i] = true;					// all elements are in Z, P is Z's complementary set!
            }

            // Step 2. -- main loop
            int l1count = 0;
            while (l1count < 1000)
            {
                l1count++;
                //optimized loop implementation below
                //Matrix w = E.Transpose() * (f - (E * x));		// negative gradient vector

                double[] ED = E.Data;
                double[] EjkXk = new double[E.Rows];
                double[] xd = x.Data;
                for (int j = 0; j < E.Rows; j++)
                    for (int k = 0; k < E.Columns; k++)
                        EjkXk[j] += ED[j + k * E.Rows] * xd[k];


                Matrix w = new Matrix(E.Columns, 1);
                double[] wd = w.Data;
                double[] fd = f.Data;
                for (int i = 0; i < w.Rows; i++)
                {
                    int ri = i * E.Rows;
                    for (int j = 0; j < E.Rows; j++)
                    {
                        wd[i] += ED[j + ri] * (fd[j] - EjkXk[j]);
                    }
                }

                // Step 3.

                // if Z is empty or all elements of w with indices in Z are <= 0 we have a solution
                bool empty = true;
                bool allwneg = true;
                double wmax = 0.0;
                int wmaxidx = -1;
                for (int i = 0; i < w.Rows; i++)
                {
                    if (Z[i]) empty = false;
                    if ((w.Data[i] > 0) && Z[i]) allwneg = false;
                    if (w.Data[i] > wmax)	// finds max elem of w to be used @ step 4.
                    {
                        wmax = w.Data[i];
                        wmaxidx = i;
                    }
                }

                if (empty || allwneg)
                    return;			// x is always the solution

                // Step 4.

                // wmax stores maximum element in w, it is positive, move its index from Z to P
                Z[wmaxidx] = false;		// Z -> P since they're complementary sets

                // Step 5. -- possible inner loop
                int l2count = 0;
                while (l2count < 100)
                {
                    l2count++;

#if FITDEBUG
					Console.WriteLine(l1count.ToString() + " " + l2count.ToString());
					Console.WriteLine(x.Transpose().ToString());
#endif
                    // #of indexes not in Z
                    int notinz = 0;
                    for (int j = 0; j < Z.Length; j++) if (!Z[j]) notinz++;

                    Matrix Epos = new Matrix(E.Rows, notinz);
                    int newcol = 0;
                    for (int j = 0; j < E.Columns; j++)
                        if (!Z[j])
                        {
                            for (int i = 0; i < E.Rows; i++)
                                Epos.Data[i + newcol * Epos.Rows] = E.Data[i + j * E.Rows];
                            newcol++;
                        }

                    // solving unconstraint least square Epos * z = f;
                    Matrix zpos;	// solution of uncorrelated problem
                    Matrix C;	// correlation matrix (not used)

                    Fit_SVD(Epos, f, out zpos, out C);
                    //QrDecomposition qr = new QrDecomposition(Epos);
                    //zpos = qr.Solve(f);

                    // set all elements of z to 0, whose index is in Z
                    // check whether all other elements of z are >= 0,
                    // if it is true, z is a good solution, set x = z and countinue to main loop
                    Matrix z = new Matrix(x.Rows, 1);
                    newcol = 0;
                    bool allpos = true;
                    for (int j = 0; j < Z.Length; j++)
                        if (Z[j])
                            z.Data[j] = 0.0;
                        else
                        {
                            z.Data[j] = zpos.Data[newcol];
                            if (z.Data[j] <= 0.0) allpos = false;
                            newcol++;
                        }

#if FITDEBUG
					Console.WriteLine(allpos.ToString());
#endif
                    // Step 6.
                    if (allpos)
                    {
                        x = new Matrix(z);
                        break;
                    }

                    // Step 7.
                    // find the minimum of alpha, and it's index;
                    int q = -1;		//
                    double alpha = double.PositiveInfinity;
                    for (int i = 0; i < z.Rows; i++)
                    {
                        if (z.Data[i] >= 0) continue;
                        double a = x.Data[i] / (x.Data[i] - z.Data[i]);
                        if (a < alpha)
                        {
                            q = i;
                            alpha = a;
                        }
                    }

                    // Step 8.
                    //x = x + (alpha * (z - x));
                    for (int i = 0; i < x.Rows; i++)
                        x.Data[i] += alpha * (z.Data[i] - x.Data[i]);

                    // Step 9.
                    // move from P to Z (ie set Z[i]=true) where x[i]=0.0
                    for (int i = 0; i < x.Rows; i++)
                        Z[i] |= (x.Data[i] == 0.0);

                } // end inner loop

            }	// end outer loop


        }
#endif
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumFit.cs,v $
        Revision 1.3  2008/10/27 20:17:37  dobos
        *** empty log message ***

        Revision 1.2  2008/10/25 18:26:22  dobos
        *** empty log message ***

        Revision 1.1  2008/01/08 21:36:57  dobos
        Initial checkin


*/
#endregion
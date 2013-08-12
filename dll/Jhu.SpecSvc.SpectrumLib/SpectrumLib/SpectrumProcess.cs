#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumProcess.cs,v 1.5 2008/10/27 20:17:38 dobos Exp $
 *   Revision:    $Revision: 1.5 $
 *   Date:        $Date: 2008/10/27 20:17:38 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using Edu.Jhu.FilterProfileLib;
using System.Collections.Generic;

namespace VoServices.Spectrum.Lib
{
    /// <summary>
    /// Summary description for SpectrumProcess.
    /// </summary>
    public class SpectrumProcess
    {
        public SpectrumProcess()
        {
        }

        public static IEnumerable<Spectrum> Preprocess(SqlConnection cn, SqlTransaction tn, PreprocessParameters par, IEnumerable<Spectrum> specs)
        {
            return Preprocess(cn, tn, par, specs, true);
        }

        public static IEnumerable<Spectrum> Preprocess(SqlConnection cn, SqlTransaction tn, PreprocessParameters par, IEnumerable<Spectrum> specs, bool skipExceptions)
        {
            foreach (Spectrum s in specs)
            {
                Spectrum ss = null;
                try
                {
                    ss = Preprocess(cn, tn, par, s);
                }
                catch (System.Exception ex)
                {
                    if (!skipExceptions)
                        throw ex;
                }

                if (ss != null)
                {
                    yield return ss;
                }
            }
        }

        public static Spectrum Preprocess(SqlConnection cn, SqlTransaction tn, PreprocessParameters par, Spectrum s)
        {
            if (par.WavelengthConversion == WavelengthConversion.Vac2Air)
                s.Vac2Air();
            else if (par.WavelengthConversion == WavelengthConversion.Air2Vac)
                s.Air2Vac();

            if (par.Deredden)
            {
                if (s.Target.GalacticExtinction != null)
                    s.Deredden(s.Target.GalacticExtinction.Value);
                else
                    s.Deredden(GetExtinction(cn, tn,
                        s.Data.SpatialAxis.Coverage.Location.Value.Value.Ra,
                        s.Data.SpatialAxis.Coverage.Location.Value.Value.Dec));
            }

            if (par.Restframe) s.Restframe();

            switch (par.NormalizeMethod)
            {
                case NormalizeMethods.FluxMedianInRanges:
                    s.Normalize_Median(par.NormalizeLimitsStart.Value, par.NormalizeLimitsEnd.Value, par.NormalizeFlux.Value, par.NormalizePower, par.NormalizeFactor);
                    break;
                case NormalizeMethods.FluxAtWavelength:
                    s.Normalize(par.NormalizeWavelength.Value, par.NormalizeFlux.Value);
                    break;
                case NormalizeMethods.FluxIntegralInRanges:
                    s.Normalize_Integral(par.NormalizeLimitsStart.Value, par.NormalizeLimitsEnd.Value, par.NormalizeFlux.Value);
                    break;
            }

            //rebinning

            if (par.Rebin)
            {
                s.Rebin(par.RebinValue.Value, par.RebinBinLow.Value, par.RebinBinHigh.Value);
            }

            if (par.Convolve)
            {
                double vdisp = par.ConvolveVelocityDispersion.Value;
                double vdpc = vdisp / Constants.LightSpeed;
                double sq2piinv = 1 / Math.Sqrt(2 * Math.PI);
                s.Convolve(-150, 150, 4,
                    delegate(double x, double ss)
                    {
                        double invsigma = 1 / (ss * vdpc);
                        double xpersigma = x * invsigma;
                        return sq2piinv * Math.Exp(-xpersigma * xpersigma / 2) * invsigma;
                    });
            }

            return s;
        }

        public static SynthMagResults CalculateSynthMag(SqlConnection cn, SqlTransaction tn, SynthMagParameters convpar, Spectrum[] spectra)
        {
            SynthMagResults res = new SynthMagResults();

            FilterCollection filters = new FilterCollection();

            res.FilterNames = new string[convpar.FilterIds.Length];
            res.FilterIds = new int[convpar.FilterIds.Length];
            for (int i = 0; i < convpar.FilterIds.Length; i++)
            {
                Filter filter = new Filter(cn, tn);
                filter.Load(convpar.FilterIds[i]);
                filter.LoadResponses();

                filters.Add(filter);

                res.FilterNames[i] = filter.Name;
                res.FilterIds[i] = filter.Id;
            }

            // preforming convolution
            res.Magnitudes = new double[spectra.Length][];
            res.SpectrumNames = new string[spectra.Length];
            res.SpectrumIds = new string[spectra.Length];
            for (int i = 0; i < spectra.Length; i++)
            {
                res.Magnitudes[i] = new double[filters.Count];
                res.SpectrumIds[i] = spectra[i].PublisherId;
                res.SpectrumNames[i] = spectra[i].Target.Name.Value;
                for (int j = 0; j < filters.Count; j++)
                {
                    res.Magnitudes[i][j] = SynthMag.Magnitude(filters[j], spectra[i]);
                }
            }

            return res;
        }

        public static ColorDiagResults CalculateColorDiag(SqlConnection cn, SqlTransaction tn, ColorDiagParameters par, Spectrum[] spectra)
        {
            ColorDiagResults res = new ColorDiagResults();

            FilterCollection xfilters = new FilterCollection();
            res.XFilters = new int[par.XFilters.Length];
            for (int i = 0; i < par.XFilters.Length; i++)
            {
                Filter filter = new Filter(cn, tn);
                filter.Load(par.XFilters[i]);
                filter.LoadResponses();

                xfilters.Add(filter);

                res.XFilters[i] = filter.Id;
            }
            res.XFilterNames = xfilters[0].Name + " - " + xfilters[1].Name;

            FilterCollection yfilters = new FilterCollection();
            res.YFilters = new int[par.YFilters.Length];
            for (int i = 0; i < par.YFilters.Length; i++)
            {
                Filter filter = new Filter(cn, tn);
                filter.Load(par.YFilters[i]);
                filter.LoadResponses();

                yfilters.Add(filter);

                res.YFilters[i] = filter.Id;
            }
            res.YFilterNames = yfilters[0].Name + " - " + yfilters[1].Name;

            // performing convolution
            res.Magnitudes = new double[spectra.Length][][];
            res.SpectrumNames = new string[spectra.Length];
            res.SpectrumIds = new string[spectra.Length];
            for (int i = 0; i < spectra.Length; i++)
            {
                //*** add z resolution to parameter class
                res.Magnitudes[i] = new double[2][];
                res.Magnitudes[i][0] = new double[par.Steps];
                res.Magnitudes[i][1] = new double[par.Steps];
                res.Redshift = new double[par.Steps];
                res.SpectrumNames[i] = spectra[i].Target.Name.Value;
                res.SpectrumIds[i] = spectra[i].Curation.PublisherDID.Value;
                for (int z = 0; z < par.Steps; z++)
                {
                    res.Redshift[z] = z * (par.Redshift.Max.Value - par.Redshift.Min.Value) / 100;
                    Spectrum s = new Spectrum(spectra[i]);
                    s.Redshift(res.Redshift[z]);

                    res.Magnitudes[i][0][z] =
                        SynthMag.Magnitude(xfilters[0], s) - SynthMag.Magnitude(xfilters[1], s);

                    res.Magnitudes[i][1][z] =
                        SynthMag.Magnitude(yfilters[0], s) - SynthMag.Magnitude(yfilters[1], s);
                }
            }

            return res;
        }

        public static void CalculateCommonSampling(Spectrum[] spectra, PreprocessParameters par)
        {
            /*
            //*** validity of par should be checked

            double start = 0.0;
            double inc = 0.0;
            double minwave = double.MaxValue, maxwave = double.MinValue;
            int points = 0;

            foreach (Spectrum spectrum in spectra)
            {
                minwave = Math.Min(spectrum.Spectral_Value[0], minwave);
                maxwave = Math.Max(spectrum.Spectral_Value[spectrum.Spectral_Value.Length - 1], maxwave);
                points = Math.Max(spectrum.Spectral_Value.Length, points);
            }
            par.ResamplingPoints = points;

            if (par.ResamplingMethod == ResamplingMethods.Linear)
            {
                start = minwave;
                inc = (maxwave - minwave) / (par.ResamplingPoints - 1);
            }
            else if (par.ResamplingMethod == ResamplingMethods.Logarithmic)
            {
                start = Math.Log10(minwave);
                inc = (Math.Log10(maxwave) - start) / (par.ResamplingPoints - 1);
            }

            par.ResamplingStart.Value = start;
            par.ResamplingInc.Value = inc;
             * */

        }

        public static Spectrum Composite(PreprocessParameters preprocpar, CompositeMethods method, IEnumerable<Spectrum> spectra)
        {
            Spectrum comp = null;
            int points = 0;
            int q = 0;

            foreach (Spectrum s in spectra)
            {
                if (q == 0)
                {
                    // Creating the composite
                    comp = new Spectrum();
                    comp.BasicInitialize();

                    points = s.Flux_Value.Length;

                    comp.Data = new VoServices.Schema.Spectrum.Data(s.Data);

                    comp.Spectral_Value = new double[points];
                    comp.Spectral_Accuracy_BinLow = new double[points];
                    comp.Spectral_Accuracy_BinHigh = new double[points];
                    comp.Flux_Value = new double[points];
                    comp.Flux_Accuracy_StatErrLow = new double[points];
                    comp.Flux_Accuracy_StatErrHigh = new double[points];
                    comp.Flux_Accuracy_Quality = new long[points];

                    switch (method)
                    {
                        case CompositeMethods.Average:
                            comp.Target.Name.Value = "Average composite spectrum";
                            comp.Data.FluxAxis.Value.Unit = "ADU";
                            break;
                        case CompositeMethods.Sum:
                            comp.Target.Name.Value = "Sum spectrum";
                            //comp.Data.FluxAxis.Value.Unit = "ADU";
                            break;
                    }
                    comp.DataId.CreationType.Value = VoServices.Schema.Spectrum.DataId.COMPOSITE;

                    s.Spectral_Value.CopyTo(comp.Spectral_Value, 0);
                    s.Spectral_Accuracy_BinLow.CopyTo(comp.Spectral_Accuracy_BinLow, 0);
                    s.Spectral_Accuracy_BinHigh.CopyTo(comp.Spectral_Accuracy_BinHigh, 0);
                }

                switch (method)
                {
                    case CompositeMethods.Average:
                        CompositeAverageCumulate(comp, s);
                        break;
                    case CompositeMethods.Sum:
                        CompositeSumCumulate(comp, s);
                        break;
                }

                q++;
            }

            if (comp != null)
            {
                switch (method)
                {
                    case CompositeMethods.Average:
                        CompositeAverageFinalize(comp, q);
                        break;
                    case CompositeMethods.Sum:
                        CompositeSumFinalize(comp, q);
                        break;
                }
            }

            return comp;
        }

        private static void CompositeAverageCumulate(Spectrum comp, Spectrum s)
        {
            for (int i = 0; i < comp.Flux_Value.Length; i++)
            {
                comp.Flux_Value[i] += s.Flux_Value[i];
                comp.Flux_Accuracy_StatErrLow[i] += s.Flux_Accuracy_StatErrLow[i] * s.Flux_Accuracy_StatErrLow[i];
                comp.Flux_Accuracy_StatErrHigh[i] += s.Flux_Accuracy_StatErrHigh[i] * s.Flux_Accuracy_StatErrHigh[i];
                comp.Flux_Accuracy_Quality[i] |= s.Flux_Accuracy_Quality[i];
            }
        }

        private static void CompositeAverageFinalize(Spectrum comp, int count)
        {
            for (int i = 0; i < comp.Flux_Value.Length; i++)
            {
                comp.Flux_Value[i] /= count;
                comp.Flux_Accuracy_StatErrLow[i] = Math.Sqrt(comp.Flux_Accuracy_StatErrLow[i]) / count;
                comp.Flux_Accuracy_StatErrHigh[i] = Math.Sqrt(comp.Flux_Accuracy_StatErrHigh[i]) / count;
            }
        }

        private static void CompositeSumCumulate(Spectrum comp, Spectrum s)
        {
            for (int i = 0; i < comp.Flux_Value.Length; i++)
            {
                comp.Flux_Value[i] += s.Flux_Value[i];
                comp.Flux_Accuracy_StatErrLow[i] += s.Flux_Accuracy_StatErrLow[i] * s.Flux_Accuracy_StatErrLow[i];
                comp.Flux_Accuracy_StatErrHigh[i] += s.Flux_Accuracy_StatErrHigh[i] * s.Flux_Accuracy_StatErrHigh[i];
                comp.Flux_Accuracy_Quality[i] |= s.Flux_Accuracy_Quality[i];
            }
        }

        private static void CompositeSumFinalize(Spectrum comp, int count)
        {
            for (int i = 0; i < comp.Flux_Value.Length; i++)
            {
                //comp.Flux_Value[i] /= count;
                comp.Flux_Accuracy_StatErrLow[i] = Math.Sqrt(comp.Flux_Accuracy_StatErrLow[i]) / count;
                comp.Flux_Accuracy_StatErrHigh[i] = Math.Sqrt(comp.Flux_Accuracy_StatErrHigh[i]) / count;
            }
        }

        public static Spectrum Compose_Median(IEnumerable<Spectrum> spectra)
        {
            /*
            // assuming resampled and normalized spectra
            Spectrum comp = CompositeBase(spectra);
            comp.Target.Name.Value = "Median composite spectrum";

            int points = spectra[0].Flux_Value.Length;
            comp.Spectral_Value = new double[points];
            comp.Flux_Value = new double[points];
            comp.Flux_Accuracy_StatErrLow = new double[points];
            comp.Flux_Accuracy_StatErrHigh = new double[points];
            comp.Flux_Accuracy_Quality = new long[points];

            // Calculating median at each wavelength
            for (int i = 0; i < spectra[0].Flux_Value.Length; i++)
            {
                // populating a new array with the values of spectra at the given wavelength
                float[] val = new float[spectra.Length];
                float[] err = new float[spectra.Length];
                int numvals = 0;
                for (int j = 0; j < spectra.Length; j++)
                {
                    if (spectra[j].Flux_Value.Length > 0)
                    {
                        if ((((PointMask)spectra[j].Flux_Accuracy_Quality[i]) & PointMask.NoData) == 0)
                        {
                            val[j] = (float)spectra[j].Flux_Value[i];
                            err[j] = (float)spectra[j].Flux_Accuracy_StatErrHigh[i];
                            numvals++;
                        }
                    }
                }

                // standard deviation of values and square average of errors
                float av = 0, std = 0, sqav = 0;
                for (int j = 0; j < numvals; j++)
                {
                    av += val[j];
                    std += val[j] * val[j];

                    sqav += err[j] * err[j];
                }
                float sd = (float)Math.Sqrt((std - av * av / numvals) / numvals);
                sqav = (float)Math.Sqrt(sqav);


                // calculating median
                Array.Sort(val, 0, numvals);

                comp.Spectral_Value[i] = spectra[0].Spectral_Value[i];
                comp.Flux_Value[i] = val[numvals / 2];		// median of values
                comp.Flux_Accuracy_StatErrHigh[i] = sd > sqav ? sd : sqav;	// error is the larger of sd and sqav
                comp.Flux_Accuracy_StatErrLow[i] = sd > sqav ? sd : sqav;	// error is the larger of sd and sqav

                comp.Flux_Accuracy_Quality[i] = (long)PointMask.Ok;
            }

            // Done
            return comp;
             * */

            return null;
        }

        private static double GetExtinction(SqlConnection cn, SqlTransaction tn, double ra, double dec)
        {
            lock (cn)
            {
                double sr = 10;

                while (true)
                {
                    string sql = "SELECT Dust.ext FROM Dust WHERE " + GetHtmRanges(ra, dec, sr);

                    SqlCommand cmd = new SqlCommand(sql, cn, tn);

                    object ext = cmd.ExecuteScalar();

                    if (ext != null)
                        return (double)(float)ext;

                    sr += 10;
                }
            }
        }

        private static string GetHtmRanges(double ra, double dec, double sr)
        {
            string where = "";

            Spherical.Region r = new Spherical.Region(new Spherical.Halfspace(ra, dec, sr));
            r.Simplify();
            Spherical.Htm.Cover c = new Spherical.Htm.Cover(r);
            c.SetTunables(20, 40, 15);
            c.Run();

            foreach (Spherical.Htm.Int64Pair pair in c.GetPairs(Spherical.Htm.Markup.Outer))
            {
                where += " OR HTMID BETWEEN " + pair.lo.ToString() + " AND " + pair.hi.ToString();
            }

            if (where != "")
                return where.Substring(4);  // strip of the first OR
            else
                return "";
        }
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumProcess.cs,v $
        Revision 1.5  2008/10/27 20:17:38  dobos
        *** empty log message ***

        Revision 1.4  2008/10/25 18:26:23  dobos
        *** empty log message ***

        Revision 1.3  2008/10/15 20:05:40  dobos
        *** empty log message ***

        Revision 1.2  2008/09/11 10:45:38  dobos
        Bugfixes to rebinning code

        Revision 1.1  2008/01/08 21:36:58  dobos
        Initial checkin


*/
#endregion
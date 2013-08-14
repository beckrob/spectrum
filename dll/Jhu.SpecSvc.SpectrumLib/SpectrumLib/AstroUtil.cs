#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: AstroUtil.cs,v 1.1 2008/01/08 21:36:53 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:53 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.SpectrumLib
{
    /// <summary>
    /// Summary description for AstroUtil.
    /// </summary>
    public static class AstroUtil
    {
        public static bool TryParseRaDec(string radec, out double ra, out double dec)
        {
            // split into multiple parts
            var parts = radec.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                ra = dec = 0;
                return false;
            }

            // Try parse as decimals
            if (double.TryParse(parts[0], out ra) &&
                double.TryParse(parts[1], out dec))
            {
                return true;
            }

            // Try as sexagesimals
            ra = hms2deg(parts[0]);
            dec = dms2deg(parts[1]);
            return true;
        }

        /// Converts ra into decimal degrees.
        /// </summary>
        public static double hms2deg(String s)
        {
            string[] a = s.Split(':');
            double v = 15.0 * Convert.ToDouble(a[0]) + Convert.ToDouble(a[1]) / 4.0 + Convert.ToDouble(a[2]) / 240.0;

            return v;
        }

        public static double rastring2deg(string s)
        {
            if (s.IndexOf(":") < 0)
                return double.Parse(s);
            else
                return hms2deg(s);
        }

        public static string deg2hms(double deg)
        {
            double hour = deg / 15.0;
            double min = (deg - 15.0 * Math.Floor(hour)) * 4.0;
            double sec = (min - Math.Floor(min)) * 60.0;

            return Math.Floor(hour).ToString("00") + ":" +
                Math.Floor(min).ToString("00") + ":" +
                sec.ToString("00.00");
        }

        /// <summary>
        /// Converts dec into decimal degrees.
        /// </summary>
        public static double dms2deg(String s)
        {
            string[] a = s.Split(':');
            double v;

            if (s.LastIndexOf("-") == 0)
                v = -(-1.0 * Convert.ToDouble(a[0]) +
                    Convert.ToDouble(a[1]) / 60.0 +
                    Convert.ToDouble(a[2]) / 3600.0);
            else
                v = (Convert.ToDouble(a[0]) +
                    Convert.ToDouble(a[1]) / 60.0 +
                    Convert.ToDouble(a[2]) / 3600.0);
            return v;
        }

        public static double decstring2deg(string s)
        {
            if (s.IndexOf(":") < 0)
                return double.Parse(s);
            else
                return dms2deg(s);
        }

        public static string deg2dms(double deg)
        {
            string res;

            res = (deg < 0) ? "-" : "+";
            deg = Math.Abs(deg);
            res += Math.Floor(deg).ToString("00") + ":";
            double tmp = (deg - Math.Floor(deg)) * 3600.0;
            double min = Math.Floor(tmp / 60.0);
            double sec = tmp - (min * 60.0);
            res += Math.Floor(min).ToString("00") + ":";
            res += sec.ToString("00.00");

            return res;
        }

        public static double Flux2ABMagnitude(double flux)
        {
            return -2.5 * Math.Log10(flux) - 48.6;
        }

        public static double SigmaFromVDisp(double wavelength, double vdisp)
        {
            return wavelength * (vdisp / Constants.LightSpeed * 1000);
        }

        public static double VDispFromSigma(double wavelength, double sigma)
        {
            return Constants.LightSpeed / 1000 * sigma / wavelength;
        }

        public static void Extinction_Charlot(double[] wl, double[][] fl, double[] age, out double[][] nflux, double tau, double mu, ref double[] wltr)
        {
            // Extinction by dust using the extinction law of Bruzual & Charlot (2003)

            // Create transformed wl vector if necessary
            if (wltr == null)
            {
                wltr = new double[wl.Length];
                for (int i = 0; i < wltr.Length; i++)
                {
                    wltr[i] = Math.Pow((wl[i] / 5500), -0.7);
                }
            }

            // calculate extinction weight function
            double[] fm = new double[wl.Length];
            double[] f1 = new double[wl.Length];
            for (int i = 0; i < fm.Length; i++)
            {
                fm[i] = Math.Exp(-mu * tau * wltr[i]);
                f1[i] = Math.Exp(-tau * wltr[i]);
            }

            nflux = new double[fl.Length][];
            for (int i = 0; i < fl.Length; i++)
            {
                if (age[i] <= 0.01)     // Age in Gyr
                {
                    Util.Vector.Weight(fl[i], f1, out nflux[i]);
                }
                else
                {
                    Util.Vector.Weight(fl[i], fm, out nflux[i]);
                }
            }
        }

    }
}
#region Revision History
/* Revision History

        $Log: AstroUtil.cs,v $
        Revision 1.1  2008/01/08 21:36:53  dobos
        Initial checkin


*/
#endregion
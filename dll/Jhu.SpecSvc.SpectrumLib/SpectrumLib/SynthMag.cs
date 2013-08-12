#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SynthMag.cs,v 1.1 2008/01/08 21:36:58 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:58 $
 */
#endregion
using System;
using VoServices.Schema.Spectrum;
using Edu.Jhu.FilterProfileLib;
using System.Collections;

namespace VoServices.SpecSvc.Lib
{

	/// <summary>
	/// Summary description for PhotoZ.
	/// </summary>
	public class SynthMag
	{
		public SynthMag()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public const double LightSpeed = 2.9979e+8;

		public static double Flux(Filter filter, Spectrum spectrum)
		{
			double errorflag;
			if (filter.WavelengthMin < spectrum.Data.SpectralAxis.Coverage.Bounds.Start.Value ||
                filter.WavelengthMax > spectrum.Data.SpectralAxis.Coverage.Bounds.Stop.Value)
			{
				///******errorflag = -1.0;
                errorflag = 1.0;
			}
			else
				errorflag = 1;

			//VoServices.Schema.Spectrum.PointCollection points = spectrum.Points;
			ResponseCollection resps = filter.Responses;

			double flux = 0.0;
			double filt = 0.0;
			int fp = 0;		// filter pointer
			int sp = 0;		// spectrum pointer

			int fpa = -1, fpb = -1;
			
			double fla = 0.0, flb = 0.0;
			double wla = 0.0, wlb = 0.0;
			double ra = 0.0, rb = 0.0;
			
			while (sp < spectrum.Flux_Value.Length)
			{

				// it runs only in the first iteration and steps the spectrum pointer right after the first existing
				// filter point
				if (resps[fp].Wavelength > spectrum.Spectral_Value[sp] && fpa == -1)
				{
					while (resps[fp].Wavelength > spectrum.Spectral_Value[sp])
					{
						sp++;
					}
				}

				// 
				if (resps[fp].Wavelength < spectrum.Spectral_Value[sp])
				{
					while (resps[fp].Wavelength < spectrum.Spectral_Value[sp])
					{
						fp++;
						if (fp == resps.Count)
							return errorflag * flux / filt / LightSpeed * 1e-27;
					}
				}

				if (fpa == -1)
				{
					fpa = (fp == 0) ? 1 : fp;	//if filter first point exactly matches a spectrum point
					fla = spectrum.Flux_Value[sp];
					wla = spectrum.Spectral_Value[sp];
					ra = resps[fpa-1].Value + (resps[fpa].Value - resps[fpa-1].Value) / (resps[fpa].Wavelength - resps[fpa-1].Wavelength) * (wla - resps[fpa-1].Wavelength);
					sp ++;
					continue;
				}

				fpb = fp;
				flb = spectrum.Flux_Value[sp];
				wlb = spectrum.Spectral_Value[sp];
				rb = resps[fpb-1].Value + (resps[fpb].Value - resps[fpb-1].Value) / (resps[fpb].Wavelength - resps[fpb-1].Wavelength) * (wlb - resps[fpb-1].Wavelength);
				

				//flux += (fla * resps[fpa].Value * wla + flb * resps[fpb].Value * wlb) / 2 * (wlb - wla);
				flux += (fla * ra * wla + flb * rb * wlb) / 2 * (wlb - wla);
				filt += (ra / wla + rb / wlb) / 2 * (wlb - wla);

				fla = flb;
				fpa = fpb;
				wla = wlb;
				ra = rb;

				sp++;
			}

			return errorflag * flux / filt / LightSpeed * 1e-27;

		}

		/// <summary>
		/// Calculates AB magnitude according to Fukugita et al 1995
		/// </summary>
		/// <param name="filter">Optical filter to use</param>
		/// <param name="spectrum">Spectrum of the object</param>
		/// <returns></returns>
		public static double Magnitude(Filter filter, Spectrum spectrum)
		{
			double flux = Flux(filter, spectrum);
			if (flux >= 0)
				return -2.5 * Math.Log10(flux) - 48.6;
			else
				return (-2.5 * Math.Log10(Math.Abs(flux)) - 48.6) + 1000;	// means overscan in wl range
		}

		/*public static double[] Magnitude(Filter filter, List<Spectrum> spectra)
		{
			return null;
		}

		public static double[] Magnitude(FilterCollection filters, Spectrum spectrum)
		{
			return null;
		}*/

		public static double[,] Magnitude(FilterCollection filters, ArrayList spectra)
		{

			double[,] results = new double[filters.Count, spectra.Count];
			for (int i = 0; i < filters.Count; i++)
			{
				for (int j = 0; j < spectra.Count; j++)
				{
					results[i,j] = Magnitude(filters[i], (Spectrum) spectra[j]);
				}
			}

			return results;
		}
	}
}
#region Revision History
/* Revision History

        $Log: SynthMag.cs,v $
        Revision 1.1  2008/01/08 21:36:58  dobos
        Initial checkin


*/
#endregion
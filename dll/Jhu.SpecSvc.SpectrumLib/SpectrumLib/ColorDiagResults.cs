#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ColorDiagResults.cs,v 1.1 2008/01/08 21:36:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:54 $
 */
#endregion
using System;

namespace VoServices.SpecSvc.Lib
{
	/// <summary>
	/// Summary description for ConvolutionResults.
	/// </summary>
	public class ColorDiagResults
	{
        public double[] Redshift;
		public double[][][] Magnitudes;	// spectra in the first, redshifts, filters in the second and third dimensions
		public string XFilterNames;  // for easy displaying
        public string YFilterNames;  // for easy displaying
		public int[] XFilters;
        public int[] YFilters;
		public string[] SpectrumNames;
		public string[] SpectrumIds;
	}
}
#region Revision History
/* Revision History

        $Log: ColorDiagResults.cs,v $
        Revision 1.1  2008/01/08 21:36:54  dobos
        Initial checkin


*/
#endregion
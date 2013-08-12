#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SynthMagResults.cs,v 1.1 2008/01/08 21:37:00 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:37:00 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.SpectrumLib
{
	/// <summary>
	/// Summary description for ConvolutionResults.
	/// </summary>
	public class SynthMagResults
	{
		public double[][] Magnitudes;	// spectra in the first, filters in the second dimension
		public string[]   FilterNames;  // for easy displaying
		public int[]	  FilterIds;
		public string[]   SpectrumNames;
		public string[]   SpectrumIds;
	}
}
#region Revision History
/* Revision History

        $Log: SynthMagResults.cs,v $
        Revision 1.1  2008/01/08 21:37:00  dobos
        Initial checkin


*/
#endregion
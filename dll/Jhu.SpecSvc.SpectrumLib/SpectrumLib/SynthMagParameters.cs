#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SynthMagParameters.cs,v 1.1 2008/01/08 21:36:59 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:59 $
 */
#endregion
using System;

namespace VoServices.SpecSvc.Lib
{
	/// <summary>
	/// Summary description for SynthMagParameters.
	/// </summary>
    [Serializable]
	public class SynthMagParameters
	{
        //***
		public int[] FilterIds = new int[0];
		public MagnitudeSystems MagnitudeSystem = MagnitudeSystems.Flux;
	}
}
#region Revision History
/* Revision History

        $Log: SynthMagParameters.cs,v $
        Revision 1.1  2008/01/08 21:36:59  dobos
        Initial checkin


*/
#endregion
#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumException.cs,v 1.1 2008/01/08 21:36:57 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:57 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.SpectrumLib
{
	/// <summary>
	/// Summary description for SpectrumException.
	/// </summary>
	public class SpectrumException : System.Exception
	{
		public SpectrumException()
		{
		}

		public SpectrumException(string message) : base(message)
		{
		}
	}
}
#region Revision History
/* Revision History

        $Log: SpectrumException.cs,v $
        Revision 1.1  2008/01/08 21:36:57  dobos
        Initial checkin


*/
#endregion
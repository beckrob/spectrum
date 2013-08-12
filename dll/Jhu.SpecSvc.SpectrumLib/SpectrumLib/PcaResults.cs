#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: PcaResults.cs,v 1.1 2008/01/08 21:36:56 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:56 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public class PcaResults
    {
        public Spectrum Average;
        public Spectrum[] Eigenspectra;
        public double[] Eigenvalues;

        public IEnumerable<Spectrum> GetSpectra()
        {
            yield return Average;

            for (int i = 0; i < Eigenspectra.Length; i++)
                yield return Eigenspectra[i];
        }
    }
}
#region Revision History
/* Revision History

        $Log: PcaResults.cs,v $
        Revision 1.1  2008/01/08 21:36:56  dobos
        Initial checkin


*/
#endregion
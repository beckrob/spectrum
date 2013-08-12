#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: PcaParameters.cs,v 1.1 2008/01/08 21:36:56 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:56 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    [Serializable]
    public class PcaParameters
    {
        public int Dimensions;
        public bool Gappy;

        public PcaParameters(bool initialize)
        {
            Dimensions = 5;
            Gappy = false;
        }
    }
}

#region Revision History
/* Revision History

        $Log: PcaParameters.cs,v $
        Revision 1.1  2008/01/08 21:36:56  dobos
        Initial checkin


*/
#endregion
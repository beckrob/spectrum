#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Delegates.cs,v 1.1 2008/01/08 21:36:47 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:47 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace VoServices.Spectrum.Lib
{
    public delegate double DoubleFunction(double x, double s);
}
#region Revision History
/* Revision History

        $Log: Delegates.cs,v $
        Revision 1.1  2008/01/08 21:36:47  dobos
        Initial checkin


*/
#endregion
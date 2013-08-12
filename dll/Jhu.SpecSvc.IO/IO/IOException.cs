#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: IOException.cs,v 1.1 2008/01/08 22:00:49 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:00:49 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class IOException : System.Exception
    {
        public IOException()
            : base()
        {
        }

        public IOException(string message)
            : base(message)
        {
        }

        public IOException(string message, System.Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
#region Revision History
/* Revision History

        $Log: IOException.cs,v $
        Revision 1.1  2008/01/08 22:00:49  dobos
        Initial checkin


*/
#endregion
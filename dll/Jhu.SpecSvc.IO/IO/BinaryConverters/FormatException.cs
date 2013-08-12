#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: FormatException.cs,v 1.1 2008/01/08 22:01:32 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:32 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    class FormatException : Exception
    {
        private int position;

        public FormatException(int position, string message)
            : base(message)
        {
            this.position = position;
        }

        public int Position
        {
            get { return this.position; }
        }
    }
}
#region Revision History
/* Revision History

        $Log: FormatException.cs,v $
        Revision 1.1  2008/01/08 22:01:32  dobos
        Initial checkin


*/
#endregion
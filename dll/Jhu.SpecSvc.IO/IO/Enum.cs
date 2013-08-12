#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Enum.cs,v 1.1 2008/01/08 22:00:49 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:00:49 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public enum CollectionTypes
    {
        Sql = 1,
        WebService = 2,
        Ssa = 3,
        Ssap = 4
    }

    public enum SearchMethods
    {
        Unknown = 0,
        Id = 1,
        All = 2,
        Cone = 3,
        HtmRange = 4,
        Object = 5,
        Redshift = 6,
        Advanced = 7,
        Model = 8,
        Similar = 9,
        Sql = 10,
        SkyServer = 11,
        Folder = 12
    }
}
#region Revision History
/* Revision History

        $Log: Enum.cs,v $
        Revision 1.1  2008/01/08 22:00:49  dobos
        Initial checkin


*/
#endregion
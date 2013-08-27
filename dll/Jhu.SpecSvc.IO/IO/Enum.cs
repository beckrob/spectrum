using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public enum CollectionType
    {
        Sql = 1,
        WebService = 2,
        Ssa = 3,
        Ssap = 4
    }

    public enum CollectionStatus
    {
        Unknown,
        Ok,
        Error
    }

    public enum SearchMethod
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jhu.SpecSvc.Web
{
    public enum ItemFormRequestMethod
    {
        Create,
        Modify,
        Delete
    }

    public enum SpectrumListView
    {
        List,
        Graph,
        Image
    }

    public enum DegreeFormat
    {
        Decimal,
        Sexagesimal
    }
}
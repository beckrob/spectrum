using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline
{
    interface IFileOutputFormatControl
    {
        bool Enabled { get; set; }
        string Title { get; }
        FileOutputFormat Format { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline
{
    interface IPipelineStepControl
    {
        bool Enabled { get; set; }
        string Title { get; }
        PipelineStep Step { get; set; }
    }
}

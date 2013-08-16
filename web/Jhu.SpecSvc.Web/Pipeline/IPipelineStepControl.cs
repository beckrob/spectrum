using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public interface IPipelineStepControl
    {
        bool Enabled { get; set; }

        PipelineStep GetValue();
        void SetValue(PipelineStep value);
        string GetTitle();
    }
}
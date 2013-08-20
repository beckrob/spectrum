using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;
using Jhu.SpecSvc.Web.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
    public partial class LineFitStepControl : PipelineStepControlBase<LineFitStep>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnUpdateForm(LineFitStep step)
        {

        }

        protected override void OnSaveForm(LineFitStep step)
        {

        }
    }
}
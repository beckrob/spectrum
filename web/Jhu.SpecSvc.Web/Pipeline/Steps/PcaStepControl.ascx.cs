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
    public partial class PcaStepControl : PipelineStepControlBase<PcaStep>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnUpdateForm(PcaStep step)
        {
            Components.Text = step.Components.ToString();
        }

        protected override void OnSaveForm(PcaStep step)
        {
            step.Components = int.Parse(Components.Text);
        }
    }
}
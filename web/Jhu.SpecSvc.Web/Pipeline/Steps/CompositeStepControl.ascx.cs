using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
    public partial class CompositeStepControl : PipelineStepControlBase<CompositeStep>
    {
        protected override void OnEnabledChanged()
        {
        }

        protected override void OnUpdateForm(CompositeStep step)
        {
            Method.SelectedValue = step.Method.ToString();
        }

        protected override void OnSaveForm(CompositeStep step)
        {
            step.Method = (CompositeStep.CompositeMethod)Enum.Parse(typeof(CompositeStep.CompositeMethod), Method.SelectedValue);
        }
    }
}
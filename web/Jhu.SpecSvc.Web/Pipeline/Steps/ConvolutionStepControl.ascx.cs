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
    public partial class ConvolutionStepControl : PipelineStepControlBase<ConvolutionStep>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnUpdateForm(ConvolutionStep step)
        {
            VelocityDispersion.Text = step.VelocityDispersion.Value.ToString();
        }

        protected override void OnSaveForm(ConvolutionStep step)
        {
            step.VelocityDispersion.Value = double.Parse(VelocityDispersion.Text);
        }
    }
}
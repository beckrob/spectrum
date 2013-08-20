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
    public partial class RebinStepControl : PipelineStepControlBase<RebinStep>
    {
        protected override void OnEnabledChanged()
        {
            RebinLimitMin.Enabled =
                            RebinLimitMax.Enabled =
                            RebinBinsize.Enabled = Enabled;
        }

        protected override void OnUpdateForm(RebinStep step)
        {
            RebinLimitMin.Text = step.RebinLimits.Min.Value.ToString();
            RebinLimitMax.Text = step.RebinLimits.Max.Value.ToString();
            RebinBinsize.Text = step.RebinBinSize.Value.ToString();
        }

        protected override void OnSaveForm(RebinStep step)
        {
            step.RebinLimits.Min.Value = double.Parse(RebinLimitMin.Text);
            step.RebinLimits.Max.Value = double.Parse(RebinLimitMax.Text);
            step.RebinBinSize.Value = double.Parse(RebinBinsize.Text);
        }
    }
}
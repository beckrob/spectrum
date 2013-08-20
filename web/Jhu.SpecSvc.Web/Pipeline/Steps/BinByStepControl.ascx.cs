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
    public partial class BinByStepControl : PipelineStepControlBase<BinByStep>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnSaveForm(BinByStep step)
        {
            step.Parameter = (BinByStep.BinByParameter)Enum.Parse(typeof(BinByStep.BinByParameter), Parameter.SelectedValue);
            step.BinLimits.Min.Value = double.Parse(BinLimitMin.Text);
            step.BinLimits.Max.Value = double.Parse(BinLimitMax.Text);
            step.BinSize.Value = double.Parse(Binsize.Text);
        }

        protected override void OnUpdateForm(BinByStep step)
        {
            Parameter.SelectedValue = step.Parameter.ToString();
            BinLimitMin.Text = step.BinLimits.Min.Value.ToString();
            BinLimitMax.Text = step.BinLimits.Max.Value.ToString();
            Binsize.Text = step.BinSize.Value.ToString();
        }
    }
}
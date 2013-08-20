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
    public partial class RedshiftStepControl : PipelineStepControlBase<RedshiftStep>
    {
        protected override void OnEnabledChanged()
        {
            Redshift.Enabled = Enabled;
        }

        protected override void OnSaveForm(RedshiftStep step)
        {
            step.Method = (RedshiftStep.RedshiftMethod)Enum.Parse(typeof(RedshiftStep.RedshiftMethod), Method.SelectedValue);
            step.Redshift.Value = double.Parse(Redshift.Text);
        }

        protected override void OnUpdateForm(RedshiftStep step)
        {
            Method.SelectedValue = step.Method.ToString();
            Redshift.Text = step.Redshift.Value.ToString();
            RedshiftRow.Visible = Method.SelectedValue == "Custom";
        }

        protected void Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            RedshiftRow.Visible = Method.SelectedValue == "Custom";
        }
    }
}
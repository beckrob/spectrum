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
    public partial class RedshiftStepControl : System.Web.UI.UserControl, Jhu.SpecSvc.Web.Pipeline.IPipelineStepControl
    {
        private bool enabled;
        private RedshiftStep step;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                Redshift.Enabled = value;
            }
        }

        public RedshiftStep Step
        {
            get
            {
                SaveForm();
                return step;
            }
            set
            {
                step = value;
                UpdateForm();
            }
        }

        public RedshiftStepControl()
        {
            enabled = true;
            step = new RedshiftStep();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            Method.SelectedValue = step.Method.ToString();
            Redshift.Text = step.Redshift.Value.ToString();
            RedshiftRow.Visible = Method.SelectedValue == "Custom";
        }

        private void SaveForm()
        {
            step.Method = (RedshiftStep.RedshiftMethod)Enum.Parse(typeof(RedshiftStep.RedshiftMethod), Method.SelectedValue);
            step.Redshift.Value = double.Parse(Redshift.Text);
        }

        public PipelineStep GetValue()
        {
            SaveForm();
            return step;
        }

        public void SetValue(PipelineStep value)
        {
            step = (RedshiftStep)value;
            UpdateForm();
        }

        public string GetTitle()
        {
            return step.Title;
        }

        protected void Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            RedshiftRow.Visible = Method.SelectedValue == "Custom";
        }
    }
}
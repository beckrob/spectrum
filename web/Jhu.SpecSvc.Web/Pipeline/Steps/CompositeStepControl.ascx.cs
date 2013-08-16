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
    public partial class CompositeStepControl : System.Web.UI.UserControl, IPipelineStepControl
    {
        private bool enabled;
        private CompositeStep step;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                UpdateForm();
            }
        }

        public CompositeStep Step
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

        public CompositeStepControl()
        {
            enabled = true;
            step = new CompositeStep();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                SaveForm();
            }
            else
            {
                UpdateForm();
            }
        }

        private void UpdateForm()
        {
            Method.SelectedValue = step.Method.ToString();
            GroupByRedshift.Checked = step.GroupByRedshift;
            RedshiftMin.Text = step.RedshiftLimits.Min.Value.ToString();
            RedshiftMax.Text = step.RedshiftLimits.Max.Value.ToString();
            RedshiftBin.Text = step.RedshiftBinSize.Value.ToString();

            RedshiftRow.Visible = RedshiftBinRow.Visible = step.GroupByRedshift;

            Method.Enabled = GroupByRedshift.Enabled =
                RedshiftMin.Enabled = RedshiftMax.Enabled = RedshiftBin.Enabled = step.Active;
        }

        private void SaveForm()
        {
            step.Method = (CompositeStep.CompositeMethod)Enum.Parse(typeof(CompositeStep.CompositeMethod), Method.SelectedValue);
            step.GroupByRedshift = GroupByRedshift.Checked;
            step.RedshiftLimits.Min.Value = double.Parse(RedshiftMin.Text);
            step.RedshiftLimits.Max.Value = double.Parse(RedshiftMax.Text);
            step.RedshiftBinSize.Value = double.Parse(RedshiftBin.Text);
        }

        protected void GroupByRedshift_OnCheckedChanged(object sender, EventArgs e)
        {
            step.GroupByRedshift = GroupByRedshift.Checked;
            UpdateForm();
        }

        #region IProcessStepControl Members

        public PipelineStep GetValue()
        {
            SaveForm();
            return step;
        }

        public void SetValue(PipelineStep value)
        {
            step = (CompositeStep)value;
            UpdateForm();
        }

        public string GetTitle()
        {
            return step.Title;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Web.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
public partial class PcaStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private PcaStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            Components.Enabled =
                Iterations.Enabled =
                Gappy.Enabled = value;
        }
    }

    public PcaStep Step
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

    public processStepControls_PcaStep()
    {
        enabled = true;
        step = new PcaStep();
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
        Components.Text = step.Components.ToString();
        Iterations.Text = step.Iterations.ToString();
        Gappy.Checked = step.Gappy;
    }

    private void SaveForm()
    {
        step.Components = int.Parse(Components.Text);
        step.Iterations = int.Parse(Iterations.Text);
        step.Gappy = Gappy.Checked;
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (PcaStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion
}
}
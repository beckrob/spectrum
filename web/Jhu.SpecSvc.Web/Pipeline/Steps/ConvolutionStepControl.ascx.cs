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
public partial class ConvolutionStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private ConvolutionStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            UpdateForm();
        }
    }

    public ConvolutionStep Step
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

    public processStepControls_ConvolutionStep()
    {
        enabled = true;
        step = new ConvolutionStep();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateForm();
    }

    private void UpdateForm()
    {
        VelocityDispersion.Text = step.VelocityDispersion.Value.ToString();
    }

    private void SaveForm()
    {
        step.VelocityDispersion.Value = double.Parse(VelocityDispersion.Text);
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (ConvolutionStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion
}
}
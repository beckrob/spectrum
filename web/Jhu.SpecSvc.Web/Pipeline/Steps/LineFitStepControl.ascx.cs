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
public partial class LineFitStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private LineFitStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
        }
    }

    public LineFitStep Step
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

    public processStepControls_LineFitStep()
    {
        enabled = true;
        step = new LineFitStep();
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
    }

    private void SaveForm()
    {
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (LineFitStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion
}
}
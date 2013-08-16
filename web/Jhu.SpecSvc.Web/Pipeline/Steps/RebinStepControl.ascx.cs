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
public partial class RebinStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private RebinStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            RebinLimitMin.Enabled =
                RebinLimitMax.Enabled =
                RebinBinsize.Enabled = value;
        }
    }

    public RebinStep Step
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

    public processStepControls_RebinStep()
    {
        enabled = true;
        step = new RebinStep();
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
        RebinLimitMin.Text = step.RebinLimits.Min.Value.ToString();
        RebinLimitMax.Text = step.RebinLimits.Max.Value.ToString();
        RebinBinsize.Text = step.RebinBinSize.Value.ToString();
    }

    private void SaveForm()
    {
        step.RebinLimits.Min.Value = double.Parse(RebinLimitMin.Text);
        step.RebinLimits.Max.Value = double.Parse(RebinLimitMax.Text);
        step.RebinBinSize.Value = double.Parse(RebinBinsize.Text);
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (RebinStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion
}
}
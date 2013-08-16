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
public partial class WavelengthConversionStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private WavelengthConversionStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
        }
    }

    public WavelengthConversionStep Step
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

    public processStepControls_WavelengthConversionStep()
    {
        enabled = true;
        step = new WavelengthConversionStep();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateForm();
    }

    private void UpdateForm()
    {
        Method.SelectedValue = step.Method.ToString();
    }

    private void SaveForm()
    {
        step.Method = (WavelengthConversionStep.WavelengthConversionMethod)Enum.Parse(typeof(WavelengthConversionStep.WavelengthConversionMethod), Method.SelectedValue);
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (WavelengthConversionStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion
}
}
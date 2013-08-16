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
public partial class NormalizeStep : System.Web.UI.UserControl, IProcessStepControl
{
    private bool enabled;
    private NormalizeStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            UpdateForm();
        }
    }

    public NormalizeStep Step
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

    public processStepControls_NormalizeStep()
    {
        enabled = true;
        step = new NormalizeStep();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateForm();
    }

    private void UpdateForm()
    {
        Method.SelectedValue = step.Method.ToString();
        Wavelength.Text = step.Wavelength.Value.ToString();
        Flux.Text = step.Flux.Value.ToString();
        Template.SelectedValue = step.Template.ToString();

        WavelengthRow.Visible = Method.SelectedValue == "FluxAtWavelength";
        TemplateRow.Visible = Method.SelectedValue != "FluxAtWavelength";

        Method.Enabled =
            Wavelength.Enabled = Flux.Enabled = Template.Enabled = enabled;
    }

    private void SaveForm()
    {
        step.Method = (NormalizeStep.NormalizeMethod)Enum.Parse(typeof(NormalizeStep.NormalizeMethod), Method.SelectedValue);
        step.Wavelength.Value = double.Parse(Wavelength.Text);
        step.Flux.Value = double.Parse(Flux.Text);
        step.Template = (NormalizeStep.NormalizeTemplate)Enum.Parse(typeof(NormalizeStep.NormalizeTemplate), Template.SelectedValue);
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (NormalizeStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion

    protected void Method_SelectedIndexChanged(object sender, EventArgs e)
    {
        WavelengthRow.Visible = Method.SelectedValue == "FluxAtWavelength";
        TemplateRow.Visible = Method.SelectedValue != "FluxAtWavelength";
    }
}
}
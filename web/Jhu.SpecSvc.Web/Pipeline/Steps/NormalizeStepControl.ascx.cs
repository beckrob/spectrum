using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;
using Jhu.SpecSvc.Web.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
    public partial class NormalizeStepControl : PipelineStepControlBase<NormalizeStep>
    {
        protected override void OnEnabledChanged()
        {
            Method.Enabled = Wavelength.Enabled = Flux.Enabled = Template.Enabled = Enabled;
        }

        protected override void OnUpdateForm(NormalizeStep step)
        {
            Method.SelectedValue = step.Method.ToString();
            Wavelength.Text = step.Wavelength.Value.ToString();
            Flux.Text = step.Flux.Value.ToString();
            Template.SelectedValue = step.Template.ToString();

            WavelengthRow.Visible = Method.SelectedValue == "FluxAtWavelength";
            TemplateRow.Visible = Method.SelectedValue != "FluxAtWavelength";
        }

        protected override void OnSaveForm(NormalizeStep step)
        {
            step.Method = (NormalizeStep.NormalizeMethod)Enum.Parse(typeof(NormalizeStep.NormalizeMethod), Method.SelectedValue);
            step.Wavelength.Value = double.Parse(Wavelength.Text);
            step.Flux.Value = double.Parse(Flux.Text);
            step.Template = (NormalizeStep.NormalizeTemplate)Enum.Parse(typeof(NormalizeStep.NormalizeTemplate), Template.SelectedValue);
        }

        protected void Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            WavelengthRow.Visible = Method.SelectedValue == "FluxAtWavelength";
            TemplateRow.Visible = Method.SelectedValue != "FluxAtWavelength";
        }
    }
}
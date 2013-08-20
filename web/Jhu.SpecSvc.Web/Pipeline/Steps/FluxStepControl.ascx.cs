using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;
using Jhu.SpecSvc.Web.Pipeline;
using Jhu.SpecSvc.FilterLib;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
public partial class FluxStepControl : PipelineStepControlBase<FluxStep>
{
    protected override void OnEnabledChanged()
    {
        
    }

    protected override void OnUpdateForm(FluxStep step)
    {
        RefreshFilterList();

        foreach (int id in step.FilterIds)
        {
            ListItem li = Filters.Items.FindByValue(id.ToString());
            if (li != null)
            {
                li.Selected = true;
            }
        }

        Redshift.SelectedValue = step.Redshift.ToString();
        RedshiftMin.Text = step.RedshiftLimits.Min.Value.ToString();
        RedshiftMax.Text = step.RedshiftLimits.Max.Value.ToString();
        RedshiftBin.Text = step.RedshiftBinSize.Value.ToString();

        RedshiftRow1.Visible = RedshiftRow2.Visible = RedshiftRow3.Visible =
            (Redshift.SelectedValue == FluxStep.RedshiftMode.Variable.ToString());
    }

    protected override void OnSaveForm(FluxStep step)
    {
        var selected = new List<int>();
        foreach (ListItem li in Filters.Items)
        {
            if (li.Selected)
            {
                selected.Add(int.Parse(li.Value));
            }
        }
        step.FilterIds = selected.ToArray();

        step.Redshift = (FluxStep.RedshiftMode)Enum.Parse(typeof(FluxStep.RedshiftMode), Redshift.SelectedValue);

        step.RedshiftLimits.Min.Value = double.Parse(RedshiftMin.Text);
        step.RedshiftLimits.Max.Value = double.Parse(RedshiftMax.Text);
        step.RedshiftBinSize.Value = double.Parse(RedshiftBin.Text);        
    }

    protected void RefreshFilterList()
    {
        if (Filters.Items.Count == 0)
        {
            FilterProfiles app = new FilterProfiles();
            app.DatabaseConnection = Page.DatabaseConnection;
            app.DatabaseTransaction = Page.DatabaseTransaction;

            foreach (Filter f in app.QueryFilters(false))
            {
                ListItem li = new ListItem(f.Name, f.Id.ToString());
                Filters.Items.Add(li);
            }
        }
    }

    protected void Redshift_SelectedIndexChanged(object sender, EventArgs e)
    {
        RedshiftRow1.Visible = RedshiftRow2.Visible = RedshiftRow3.Visible =
            (Redshift.SelectedValue == FluxStep.RedshiftMode.Variable.ToString());
    }
}
}
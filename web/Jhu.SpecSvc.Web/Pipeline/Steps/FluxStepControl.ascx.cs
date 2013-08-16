using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using VoServices.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Edu.Jhu.FilterProfileLib;
using Jhu.SpecSvc.Web.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
public partial class MagnitudeStep : VoServices.SpecSvc.Web.BaseControl, IProcessStepControl
{
    private bool enabled;
    private FluxStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            UpdateForm();
        }
    }

    public FluxStep Step
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

    public processStepControls_MagnitudeStep()
    {
        enabled = true;
        step = new FluxStep();
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

        RedshiftTable.Visible = (Redshift.SelectedValue == FluxStep.RedshiftMode.Variable.ToString());
    }

    private void SaveForm()
    {
        List<int> selected = new List<int>();
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

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (FluxStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion

    protected void RefreshFilterList()
    {
        if (Filters.Items.Count == 0)
        {
            FilterProfiles app = new FilterProfiles();
            app.DatabaseConnection = DatabaseConnection;
            app.DatabaseTransaction = DatabaseTransaction;

            foreach (Filter f in app.QueryFilters(false))
            {
                ListItem li = new ListItem(f.Name, f.Id.ToString());
                Filters.Items.Add(li);
            }
        }
    }
    protected void Redshift_SelectedIndexChanged(object sender, EventArgs e)
    {
        RedshiftTable.Visible = (Redshift.SelectedValue == FluxStep.RedshiftMode.Variable.ToString());
    }
}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Web.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
public partial class ContinuumFitStep : VoServices.SpecSvc.Web.BaseControl, IProcessStepControl
{
    private bool enabled;
    private ContinuumFitStep step;

    public bool Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            UpdateForm();
        }
    }

    public ContinuumFitStep Step
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

    public processStepControls_ContinuumFitStep()
    {
        enabled = true;
        step = new ContinuumFitStep();
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
        RefreshTemplateSetList();

        Method.SelectedValue = step.Method.ToString();

        WeightWithError.Checked = step.WeightWithError;
        MaskLines.Checked = step.MaskLines;
        MaskSkyLines.Checked = step.MaskSkyLines;
        MaskFromSpectra.Checked = step.MaskFromSpectra;

        try
        {
            if (!string.IsNullOrEmpty(step.TemplateSet))
            {
                TemplateSet.SelectedValue = step.TemplateSet;
            }

            RefreshTemplateList();

            foreach (string temp in step.TemplateList)
            {
                ListItem li = Templates.Items.FindByValue(temp);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }
        catch (System.Exception)
        {
        }
    }

    private void SaveForm()
    {
        step.Method = (ContinuumFitStep.FitMethod)Enum.Parse(typeof(ContinuumFitStep.FitMethod), Method.SelectedValue);

        step.WeightWithError = WeightWithError.Checked;
        step.MaskLines = MaskLines.Checked;
        step.MaskSkyLines = MaskSkyLines.Checked;
        step.MaskFromSpectra = MaskFromSpectra.Checked;
        step.TemplateSet = TemplateSet.SelectedValue;

        List<string> selected = new List<string>();
        foreach (ListItem li in Templates.Items)
        {
            if (li.Selected)
            {
                selected.Add(li.Value);
            }
        }
        step.TemplateList = selected.ToArray();
    }

    #region IProcessStepControl Members

    public ProcessStep GetValue()
    {
        SaveForm();
        return step;
    }

    public void SetValue(ProcessStep value)
    {
        step = (ContinuumFitStep)value;
        UpdateForm();
    }

    public string GetTitle()
    {
        return step.Title;
    }

    #endregion

    protected void RefreshTemplateSetList()
    {
        if (TemplateSet.Items.Count == 0)
        {
            // Predefined template sets
            VoServices.SpecSvc.IO.TemplateSet[] temps = Connector.QueryTemplateSets();
            foreach (VoServices.SpecSvc.IO.TemplateSet ts in temps)
                TemplateSet.Items.Add(new ListItem(ts.Name, "TS|" + ts.Id.ToString()));

            // MySpectrum folders
            if ((Guid)LoggedInUserGuid != Guid.Empty)
            {
                WsConnector conn = new WsConnector((string)Session["MySpectrumSearchUrl"]);
                UserFolder[] folders = conn.QueryUserFolders(LoggedInUserGuid);

                foreach (UserFolder folder in folders)
                {
                    ListItem li = new ListItem("MySpectrum folder: " + folder.Name, "MY|" + folder.Id.ToString());
                    li.Attributes["style"] = "background-color:lightblue";
                    TemplateSet.Items.Add(li);
                }
            }
        }
    }

    protected void RefreshTemplateList()
    {
        List<Spectrum> templates = new List<Spectrum>();

        // Load template headers
        if (TemplateSet.SelectedValue.StartsWith("TS"))
        {
            TemplateSet temp = new TemplateSet(true);
            Connector.LoadTemplateSet(temp, int.Parse(TemplateSet.SelectedValue.Substring(3)));

            // loading templates
            IdSearchParameters idpar = new IdSearchParameters(true);
            idpar.Collections = new string[] { "ivo://elte/templates" }; //*****
            idpar.Ids = Connector.QueryTemplates(temp.Id);
            idpar.LoadDetails = false;
            idpar.LoadPoints = false;
            idpar.UserGuid = LoggedInUserGuid;

            templates.AddRange(Connector.GetSpectrum(idpar));
        }
        else if (TemplateSet.SelectedValue.StartsWith("MY"))
        {
            FolderSearchParameters fsp = new FolderSearchParameters(true);
            fsp.FolderId = int.Parse(TemplateSet.SelectedValue.Substring(3));
            fsp.LoadDetails = false;
            fsp.LoadPoints = false;
            fsp.UserGuid = LoggedInUserGuid;

            WsConnector conn = new WsConnector((string)Session["MySpectrumSearchUrl"]);
            templates.AddRange(conn.FindSpectrum(fsp).Take<Spectrum>(50));  // Sets a limit on the number of displayed templates
        }

        Templates.Items.Clear();

        foreach (Spectrum s in templates.OrderBy<Spectrum, string>(s => s.Target.Name.Value))
        {
            ListItem li = new ListItem(s.Target.Name.Value, s.Curation.PublisherDID.Value);
            Templates.Items.Add(li);
        }
    }

    protected void TemplateSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshTemplateList();
    }
}
}
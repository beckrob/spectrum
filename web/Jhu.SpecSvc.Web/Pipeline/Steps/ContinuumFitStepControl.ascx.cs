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

namespace Jhu.SpecSvc.Web.Pipeline.Steps
{
    public partial class ContinuumFitStepControl : PipelineStepControlBase<ContinuumFitStep>
    {
        protected override void OnEnabledChanged()
        {

        }

        protected override void OnUpdateForm(ContinuumFitStep step)
        {
            RefreshTemplateSetList();

            Method.SelectedValue = step.Method.ToString();

            WeightWithError.Checked = step.WeightWithError;
            MaskLines.Checked = step.MaskLines;
            MaskSkyLines.Checked = step.MaskSkyLines;
            MaskFromSpectra.Checked = step.Mask != 0;

            if (!string.IsNullOrEmpty(step.TemplateSet))
            {
                TemplateSet.SelectedValue = step.TemplateSet;


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
        }

        protected override void OnSaveForm(ContinuumFitStep step)
        {
            step.Method = (ContinuumFitStep.FitMethod)Enum.Parse(typeof(ContinuumFitStep.FitMethod), Method.SelectedValue);

            step.WeightWithError = WeightWithError.Checked;
            step.MaskLines = MaskLines.Checked;
            step.MaskSkyLines = MaskSkyLines.Checked;
            step.Mask = (long)(MaskFromSpectra.Checked ? PointMask.SDSSBadValue : 0);

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

        protected void RefreshTemplateSetList()
        {
            if (TemplateSet.Items.Count == 0)
            {
                // Predefined template sets
                var templates = Page.PortalConnector.QueryTemplateSets();
                foreach (var ts in templates)
                    TemplateSet.Items.Add(new ListItem(ts.Name, "TS|" + ts.Id.ToString()));

                // MySpectrum folders
#if false
            if (Page.UserGuid != Guid.Empty)
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
#endif
            }
        }

        protected void RefreshTemplateList()
        {
            var templates = new List<Spectrum>();

            // Load template headers
            if (TemplateSet.SelectedValue.StartsWith("TS"))
            {
                TemplateSet temp = new TemplateSet(true);
                Page.PortalConnector.LoadTemplateSet(temp, int.Parse(TemplateSet.SelectedValue.Substring(3)));

                // loading templates
                IdSearchParameters idpar = new IdSearchParameters(true);
                idpar.Collections = new string[] { "ivo://elte/templates" }; //*****
                idpar.Ids = Page.PortalConnector.QueryTemplates(temp.Id);
                idpar.LoadDetails = false;
                idpar.LoadPoints = false;
                idpar.UserGuid = Page.UserGuid;

                templates.AddRange(Page.PortalConnector.FindSpectrum(idpar));
            }
            else if (TemplateSet.SelectedValue.StartsWith("MY"))
            {
#if false
            FolderSearchParameters fsp = new FolderSearchParameters(true);
            fsp.FolderId = int.Parse(TemplateSet.SelectedValue.Substring(3));
            fsp.LoadDetails = false;
            fsp.LoadPoints = false;
            fsp.UserGuid = LoggedInUserGuid;

            WsConnector conn = new WsConnector((string)Session["MySpectrumSearchUrl"]);
            templates.AddRange(conn.FindSpectrum(fsp).Take<Spectrum>(50));  // Sets a limit on the number of displayed templates
#endif
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
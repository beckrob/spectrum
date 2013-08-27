using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class Pipeline : PageBase
    {
        public static string GetUrl()
        {
            return "~/Pipeline/Pipeline.aspx";
        }

        protected override void OnInit(EventArgs e)
        {
            PipelineStepList.DataSource = Pipeline.Steps;
            PipelineStepList.DataBind();

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                SavePipeline.Enabled = LoadPipeline.Enabled = ManagePipelines.Enabled = (UserGuid != Guid.Empty);

                InitializeStepTypeList();
            }
        }

        private void InitializeStepTypeList()
        {
            StepType.Items.Add(new ListItem("(select from list)", ""));

            foreach (var steptype in PipelineStepFactory.GetStepTypes())
            {
                var ps = (PipelineStep)steptype.GetConstructor(Type.EmptyTypes).Invoke(null);
                StepType.Items.Add(new ListItem(ps.Title, steptype.Name));
            }
        }

        protected void StepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StepType.SelectedValue != "")
            {
                var pst = typeof(PipelineStep);
                var t = pst.Assembly.GetType(String.Format("{0}.Steps.{1}", pst.Namespace, StepType.SelectedValue));
                var step = (PipelineStep)t.GetConstructor(Type.EmptyTypes).Invoke(null);

                Pipeline.Steps.Add(step);
            }

            StepType.SelectedValue = "";
        }

        protected void PipelineStepList_Load(object sender, EventArgs e)
        {
            var lw = (ListView)sender;

            if (Page.IsPostBack && lw.Visible)
            {
                foreach (ListViewDataItem li in lw.Items)
                {
                    var ph = (PlaceHolder)li.FindControl("controlPlaceholder");
                    var control = (IPipelineStepControl)li.FindControl("stepControl");
                    Pipeline.Steps[li.DataItemIndex] = control.Step;
                }
            }
        }

        protected void PipelineStepList_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var di = (ListViewDataItem)e.Item;

                if (di.DataItem != null)
                {
                    var step = (PipelineStep)di.DataItem;

                    var title = (Label)di.FindControl("Title");
                    var activate = (LinkButton)di.FindControl("Active");
                    activate.Text = step.Active ? "disable" : "enable";

                    var placeholder = (PlaceHolder)di.FindControl("controlPlaceholder");

                    IPipelineStepControl control = null;

                    string stepcontrol = step.GetType().Name;
                    control = (IPipelineStepControl)LoadControl(String.Format("Steps/{0}Control.ascx", stepcontrol));
                    ((UserControl)control).ID = "stepControl";
                    control.Step = step;
                    control.Enabled = step.Active;

                    title.Text = control.Title;
                    placeholder.Controls.Add((UserControl)control);
                }
            }
        }

        protected void PipelineStepList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var di = (ListViewDataItem)e.Item;
            var step = Pipeline.Steps[di.DataItemIndex];

            switch (e.CommandName)
            {
                case "RemoveStep":
                    Pipeline.Steps.RemoveAt(di.DataItemIndex);
                    break;
                case "MoveUpStep":
                    if (di.DataItemIndex > 0)
                    {
                        Pipeline.Steps.RemoveAt(di.DataItemIndex);
                        Pipeline.Steps.Insert(di.DataItemIndex - 1, step);
                    }
                    break;
                case "MoveDownStep":
                    if (di.DataItemIndex < Pipeline.Steps.Count - 1)
                    {
                        Pipeline.Steps.RemoveAt(di.DataItemIndex);
                        Pipeline.Steps.Insert(di.DataItemIndex + 1, step);
                    }
                    break;
                case "ActivateStep":
                    step.Active = !step.Active;
                    break;
            }


            PipelineStepList.DataBind();
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ok":
                    Validate();
                    if (IsValid)
                    {
                        Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Target.GetUrl());
                    }
                    break;
                case "back":
                case "results":
                    Response.Redirect(Jhu.SpecSvc.Web.Search.List.GetUrl());
                    break;
                case "finish":
                default:
                    throw new NotImplementedException();
            }
        }

       

        protected void ResetPipeline_Click(object sender, EventArgs e)
        {
            Pipeline = new SpectrumPipeline();
        }

        protected void SavePipeline_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.SpecSvc.Web.Pipeline.PipelineDetails.GetUrl());
        }

        protected void LoadPipeline_Click(object sender, EventArgs e)
        {
            Response.Redirect(PipelineList.GetUrl(PipelineList.RequestMethod.Load));
        }

        protected void ManagePipelines_Click(object sender, EventArgs e)
        {
            Response.Redirect(PipelineList.GetUrl(PipelineList.RequestMethod.Manage));
        }
    }
}
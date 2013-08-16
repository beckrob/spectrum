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
            base.OnInit(e);
            
            PipelineStepList.DataSource = Pipeline;
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
            Type[] steptypes = { typeof(RebinStep), typeof(RedshiftStep), typeof(DereddenStep),
                           typeof(WavelengthConversionStep), typeof(NormalizeStep),
                           typeof(ConvolutionStep), typeof(CompositeStep),
                           typeof(PcaStep), typeof(ContinuumFitStep), typeof(LineFitStep),
                           typeof(FluxStep)};

            foreach (var steptype in steptypes)
            {
                var ps = (PipelineStep)steptype.GetConstructor(Type.EmptyTypes).Invoke(null);
                StepType.Items.Add(new ListItem(ps.Title, steptype.Name));
            }
        }

        /*
        protected void PipelineStepList_Load(object sender, EventArgs e)
        {
            var lw = (ListView)sender;

            if (Page.IsPostBack && lw.Visible)
            {
                foreach (ListViewDataItem li in lw.Items)
                {
                    PlaceHolder ph = (PlaceHolder)li.FindControl("controlPlaceholder");
                    IProcessStepControl control = (IProcessStepControl)li.FindControl("stepControl");
                    steps[li.DataItemIndex] = control.GetValue();
                }
            }

            Session["ProcessSteps"] = steps;
        }*/

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
                    control.SetValue(step);
                    control.Enabled = step.Active;

                    title.Text = control.GetTitle();
                    placeholder.Controls.Add((UserControl)control);
                }
            }
        }

        protected void PipelineStepList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            /*ListViewDataItem di = (ListViewDataItem)e.Item;
            ProcessStep step = steps[di.DataItemIndex];

            switch (e.CommandName)
            {
                case "RemoveStep":
                    steps.RemoveAt(di.DataItemIndex);
                    break;
                case "MoveUpStep":
                    if (di.DataItemIndex > 0)
                    {
                        steps.RemoveAt(di.DataItemIndex);
                        steps.Insert(di.DataItemIndex - 1, step);
                    }
                    break;
                case "MoveDownStep":
                    if (di.DataItemIndex < steps.Count - 1)
                    {
                        steps.RemoveAt(di.DataItemIndex);
                        steps.Insert(di.DataItemIndex + 1, step);
                    }
                    break;
                case "ActivateStep":
                    step.Active = !step.Active;
                    break;
            }


            ProcessStepList.DataBind();*/
        }

        protected void AddStep_OnClick(object sender, EventArgs e)
        {
            var pst = typeof(PipelineStep);
            var t = pst.Assembly.GetType(String.Format("{0}.Steps.{1}", pst.Namespace, StepType.SelectedValue));
            var step = (PipelineStep)t.GetConstructor(Type.EmptyTypes).Invoke(null);

            Pipeline.Add(step);
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ok":
                    Validate();
                    if (IsValid)
                    {
                        Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Output.GetUrl());
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

        protected void ResetWorkflow_Click(object sender, EventArgs e)
        {
            Pipeline.Clear();
        }

        protected void SaveWorkflow_Click(object sender, EventArgs e)
        {
            Response.Redirect("workflow_form_save.aspx");   // *** TODO
        }

        protected void LoadWorkflow_Click(object sender, EventArgs e)
        {
            Response.Redirect("workflow_form_manage.aspx"); // *** TODO
        }

        protected void ManageWorkflows_Click(object sender, EventArgs e)
        {
            Response.Redirect("workflow_form_manage.aspx"); // *** TODO
        }
    }
}
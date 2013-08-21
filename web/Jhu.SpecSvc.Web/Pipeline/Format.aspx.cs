using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Targets;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class Format : PageBase
    {
        public static string GetUrl()
        {
            return "~/Pipeline/Format.aspx";
        }

        public FileTarget FileTarget
        {
            get { return (FileTarget)OutputTarget; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (!(OutputTarget is FileTarget))
            {
                throw new InvalidOperationException("File target must be selected.");
            }

            FormatList.DataSource = ((FileTarget)OutputTarget).Formats;
            FormatList.DataBind();

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //SavePipeline.Enabled = LoadPipeline.Enabled = ManagePipelines.Enabled = (UserGuid != Guid.Empty);

                InitializeFormatTypeList();
            }
        }

        private void InitializeFormatTypeList()
        {
            FormatType.Items.Add(new ListItem("(select from list)", ""));

            foreach (var steptype in FileOutputFormatFactory.GetFormatTypes())
            {
                var ps = (FileOutputFormat)steptype.GetConstructor(Type.EmptyTypes).Invoke(null);
                FormatType.Items.Add(new ListItem(ps.Title, steptype.Name));
            }
        }

        protected void FormatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormatType.SelectedValue != "")
            {
                var pst = typeof(FileOutputFormat);
                var t = pst.Assembly.GetType(String.Format("{0}.Formats.{1}", pst.Namespace, FormatType.SelectedValue));
                var format = (FileOutputFormat)t.GetConstructor(Type.EmptyTypes).Invoke(null);

                FileTarget.Formats.Add(format);
            }

            FormatType.SelectedValue = "";
        }

        protected void FormatList_Load(object sender, EventArgs e)
        {
            var lw = (ListView)sender;

            if (Page.IsPostBack && lw.Visible)
            {
                foreach (ListViewDataItem li in lw.Items)
                {
                    var ph = (PlaceHolder)li.FindControl("controlPlaceholder");
                    var control = (IFileOutputFormatControl)li.FindControl("formatControl");
                    FileTarget.Formats[li.DataItemIndex] = control.Format;
                }
            }
        }

        protected void FormatList_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var di = (ListViewDataItem)e.Item;

                if (di.DataItem != null)
                {
                    var format = (FileOutputFormat)di.DataItem;

                    var title = (Label)di.FindControl("Title");
                    var activate = (LinkButton)di.FindControl("Active");
                    activate.Text = format.Active ? "disable" : "enable";

                    var placeholder = (PlaceHolder)di.FindControl("controlPlaceholder");

                    var name = format.GetType().Name;
                    var control = (IFileOutputFormatControl)LoadControl(String.Format("Formats/{0}Control.ascx", name));
                    ((UserControl)control).ID = "formatControl";
                    control.Format = format;
                    control.Enabled = format.Active;

                    title.Text = control.Title;
                    placeholder.Controls.Add((UserControl)control);
                }
            }
        }

        protected void FormatList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var di = (ListViewDataItem)e.Item;
            var format = FileTarget.Formats[di.DataItemIndex];

            switch (e.CommandName)
            {
                case "RemoveFormat":
                    FileTarget.Formats.RemoveAt(di.DataItemIndex);
                    break;
                case "ActivateFormat":
                    format.Active = !format.Active;
                    break;
            }

            FormatList.DataBind();
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
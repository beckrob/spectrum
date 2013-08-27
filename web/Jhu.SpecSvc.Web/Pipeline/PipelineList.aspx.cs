using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class PipelineList : PageBase
    {
        public enum RequestMethod
        {
            Load,
            Manage
        }

        public static string GetUrl(RequestMethod method)
        {
            return String.Format("~/Pipeline/PipelineList.aspx?method={0}", method);
        }

        public RequestMethod Method
        {
            get { return (RequestMethod)Enum.Parse(typeof(RequestMethod), Request.QueryString["method"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException("Operation requires user logged in.");
            }

            List.DataSource = PipelineConnector.QueryPipelines(UserGuid);

            switch (Method)
            {
                case RequestMethod.Load:
                    PipelineListForm.Text = "Load pipeline";
                    Ok.Visible = true;
                    Rename.Visible = false;
                    Delete.Visible = false;
                    break;
                case RequestMethod.Manage:
                    PipelineListForm.Text = "Manage pipelines";
                    Ok.Visible = false;
                    Rename.Visible = true;
                    Delete.Visible = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                switch (Method)
                {
                    case RequestMethod.Load:
                        LoadPipeline();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        protected void Rename_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                RenamePipeline();
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                DeletePipeline();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Pipeline.GetUrl());
        }

        protected void ListSelectedValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = List.SelectedDataKeys.Count == 1;
        }

        private void LoadPipeline()
        {
            int id = int.Parse(List.SelectedDataKeys.First());
            Pipeline = PipelineConnector.LoadPipeline(id, UserGuid);

            Response.Redirect(OriginalReferer);
        }

        private void RenamePipeline()
        {
            int id = int.Parse(List.SelectedDataKeys.First());
            Response.Redirect(PipelineDetails.GetUrl(PipelineDetails.RequestMethod.Rename, id));
        }

        private void DeletePipeline()
        {
            int id = int.Parse(List.SelectedDataKeys.First());
            PipelineConnector.DeletePipeline(id, UserGuid);
        }
    }
}
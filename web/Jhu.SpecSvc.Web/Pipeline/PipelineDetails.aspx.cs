using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class PipelineDetails : PageBase
    {
        public enum RequestMethod
        {
            Save,
            Rename
        }

        public static string GetUrl()
        {
            return String.Format("~/Pipeline/PipelineDetails.aspx?method={0}", RequestMethod.Save);
        }

        public static string GetUrl(RequestMethod method, int id)
        {
            return String.Format("~/Pipeline/PipelineDetails.aspx?method={0}&id={1}", method, id);
        }

        public RequestMethod Method
        {
            get { return (RequestMethod)Enum.Parse(typeof(RequestMethod), Request.QueryString["method"]); }
        }

        public int PipelineId
        {
            get { return int.Parse(Request.QueryString["id"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException("Operation requires user logged in.");
            }

            if (Method == RequestMethod.Save && Pipeline == null)
            {
                throw new InvalidOperationException("Pipeline is null.");
            }

            switch (Method)
            {
                case RequestMethod.Rename:
                    PipelineDetailsForm.Text = "Rename pipeline";
                    break;
                case RequestMethod.Save:
                    PipelineDetailsForm.Text = "Save pipeline";
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!IsPostBack)
            {
                Name.Text = Pipeline.Name;
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                switch (Method)
                {
                    case RequestMethod.Save:
                        SavePipeline();
                        break;
                    case RequestMethod.Rename:
                        RenamePipeline();
                        break;
                }

                Response.Redirect(OriginalReferer);
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(OriginalReferer);
        }

        private void SavePipeline()
        {
            Pipeline.Name = Name.Text;
            PipelineConnector.SavePipeline(Pipeline, UserGuid);
        }

        private void RenamePipeline()
        {
            var p = PipelineConnector.LoadPipeline(PipelineId, UserGuid);
            p.Name = Name.Text;
            PipelineConnector.SavePipeline(p, UserGuid);
        }
    }
}
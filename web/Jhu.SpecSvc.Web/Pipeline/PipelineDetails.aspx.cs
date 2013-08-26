using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class SavePipeline : PageBase
    {
        public static string GetUrl()
        {
            return "~/Pipeline/SavePipeline.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Pipeline == null)
            {
                throw new InvalidOperationException("Pipeline is null.");
            }

            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException("Operation requires user logged in.");
            }

            if (!IsPostBack)
            {
                Name.Text = Pipeline.Name;
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                Pipeline.Name = Name.Text;
                PipelineConnector.SavePipeline(Pipeline, UserGuid);

                Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Pipeline.GetUrl());
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Pipeline.GetUrl());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class Default : System.Web.UI.Page
    {
        public static string GetUrl()
        {
            return "~/Pipeline/Default.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PipelineMode.Items[2].Enabled = Request.IsAuthenticated;    // load
            }

            PipelineTemplatesDiv.Visible = PipelineMode.SelectedValue == "template";
        }

        protected void Button_Command(object serder, CommandEventArgs args)
        {
            switch (args.CommandName)
            {
                case "results":
                    Response.Redirect(Jhu.SpecSvc.Web.Search.List.GetUrl());
                    break;
                case "ok":
                    Validate();
                    if (IsValid)
                    {
                        InitializePipeline();
                        Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Pipeline.GetUrl());
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void InitializePipeline()
        {
            switch (PipelineMode.SelectedValue)
            {
                case "session":
                    // do nothing
                    break;
                case "load":
                    LoadPipeline();
                    break;
                case "template":
                    switch (PipelineTemplates.SelectedValue)
                    {
                        case "fit":
                            InitializeFit();
                            break;
                        case "lick":
                            InitializeLick();
                            break;
                        case "pca":
                            InitializePca();
                            break;
                        case "composite":
                            InitializeComposite();
                            break;
                        case "magnitudes":
                            InitializeMagnitudes();
                            break;
                        case "color":
                            InitializeColor();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
            }
        }

        private void LoadPipeline()
        {
            Response.Redirect(Jhu.SpecSvc.Web.Pipeline.PipelineList.GetUrl(PipelineList.RequestMethod.Load));
        }

        private void InitializeFit()
        {
        }

        private void InitializeLick()
        {
        }

        private void InitializePca()
        {
        }

        private void InitializeComposite()
        {
        }

        private void InitializeMagnitudes()
        {
        }

        private void InitializeColor()
        {
        }
    }
}
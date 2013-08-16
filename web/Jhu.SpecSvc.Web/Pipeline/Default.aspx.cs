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
            switch (Template.SelectedValue)
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
                case "plot":
                    InitializePlot();
                    break;
                case "myspectra":
                    InitializeMySpectra();
                    break;
                case "download":
                    InitializeDownload();
                    break;
                default:
                    throw new NotImplementedException();
            }
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

        private void InitializePlot()
        {
        }

        private void InitializeMySpectra()
        {
        }

        private void InitializeDownload()
        {
        }
    }
}
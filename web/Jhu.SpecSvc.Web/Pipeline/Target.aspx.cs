using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Targets;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class Target : PageBase
    {
        public static string GetUrl()
        {
            return "~/Pipeline/Target.aspx";
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
                        InitializeOutput();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void InitializeOutput()
        {
            switch (Template.SelectedValue)
            {
                case "screen":
                    throw new NotImplementedException();
                case "myspectrum":
                    throw new NotImplementedException();
                case "download":
                    InitializeDownload();
                    Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Format.GetUrl());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void InitializeDownload()
        {
            var target = new FileTarget();
            target.Destination = FileTarget.FileDestination.Download;

            Pipeline.Target = target;
        }
    }
}
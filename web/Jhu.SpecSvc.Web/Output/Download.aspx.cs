using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.Pipeline.Targets;

namespace Jhu.SpecSvc.Web.Output
{
    public partial class Download : PageBase
    {
        public static string GetUrl()
        {
            return "~/Output/Download.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var spectra = Connector.LoadSelectedResults(ResultsetId, UserGuid, true, true);

            var target = (FileTarget)Pipeline.Target;
            var filename = String.IsNullOrEmpty(target.Uri) ? "spectra.tar.gz" : target.Uri;

            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/octet-stream";

            target.OutputStream = Response.OutputStream;

            Pipeline.InitializePipeline();
            
            foreach (var s in Pipeline.Execute(spectra))
            {
            }

            Pipeline.DeinitializePipeline();

            Response.Flush();
            Response.End();

            
        }
    }
}
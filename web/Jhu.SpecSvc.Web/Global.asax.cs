using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Jhu.Graywulf.Web;
using Jhu.Graywulf.Registry;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web
{
    public class Global : ApplicationBase
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            using (var context = Jhu.Graywulf.Registry.ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.DirtyRead))
            {
                var ef = new EntityFactory(context);
                var federation = ef.LoadEntity<Federation>(Federation.AppSettings.FederationName);

                Application[Jhu.Graywulf.Web.Constants.ApplicationShortTitle] = federation.ShortTitle;
                Application[Jhu.Graywulf.Web.Constants.ApplicatonLongTitle] = federation.LongTitle;
            }
        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            base.Session_Start(sender, e);

            Session[Constants.SessionPipeline] = new SpectrumPipeline();
            Session[Constants.SessionMySpectrumSearchUrl] = AppSettings.DefaultMySpectrumBaseUrl.Replace("[$Hostname]", Request.Url.Host) + "/Search.asmx";
            Session[Constants.SessionMySpectrumAdminUrl] = AppSettings.DefaultMySpectrumBaseUrl.Replace("[$Hostname]", Request.Url.Host) + "/Admin.asmx";
            Session[Constants.SessionMySpectrumGraphUrl] = AppSettings.DefaultMySpectrumBaseUrl.Replace("[$Hostname]", Request.Url.Host) + "/Graph.asmx";
        }
    }
}
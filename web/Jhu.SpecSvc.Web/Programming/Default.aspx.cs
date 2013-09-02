using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Programming
{
    public partial class Default : PageBase
    {
        public static string GetUrl()
        {
            return "~/Programming/Default.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SearchHome.NavigateUrl = GetWebServiceUrl("Search", false);
            SearchWsdl.NavigateUrl = GetWebServiceUrl("Search", true);
            JobsHome.NavigateUrl = GetWebServiceUrl("Jobs", false);
            JobsWsdl.NavigateUrl = GetWebServiceUrl("Jobs", true);

            MySpectrumSearchHome.NavigateUrl = GetMySpectrumUrl("Search", false);
            MySpectrumSearchWsdl.NavigateUrl = GetMySpectrumUrl("Search", true);
            MySpectrumAdminHome.NavigateUrl = GetMySpectrumUrl("Admin", false);
            MySpectrumAdminWsdl.NavigateUrl = GetMySpectrumUrl("Admin", true);

            Ssa.NavigateUrl = GetSsaUrl();
        }

        private string GetWebServiceUrl(string service, bool wsdl)
        {
            return String.Format(
                "{0}/{1}.asmx{2}",
                AppSettings.WebServiceBaseUrl.Replace("[$Hostname]", Request.Url.Host),
                service,
                wsdl ? "?wsdl" : "");
        }

        private string GetSsaUrl()
        {
            return AppSettings.SsaBaseUrl.Replace("[$Hostname]", Request.Url.Host);
        }

        private string GetMySpectrumUrl(string service, bool wsdl)
        {

            return String.Format(
                "{0}/{1}.asmx{2}",
                AppSettings.DefaultMySpectrumBaseUrl.Replace("[$Hostname]", Request.Url.Host),
                service,
                wsdl ? "?wsdl" : "");
        }
    }
}
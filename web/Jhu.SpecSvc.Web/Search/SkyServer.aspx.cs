using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class SkyServer : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/SkyServer.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var par = new SkyServerSearchParameters(true);
                par.Query = Query.Text;
                par.Target = Target.SelectedValue;
                par.WsUrl = AppSettings.SkyServerWsUrl;
                par.WsId = AppSettings.SkyServerWsId;

                par.Collections = Collections.SelectedKeys;

                SearchParameters = par;
                Execute();
            }
        }
    }
}
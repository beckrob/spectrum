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
    public partial class Sql : PageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Sql.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var par = new SqlSearchParameters(true);
                par.Query = Query.Text;
                par.Collections = PortalConnector.LoadCollections(Collections.SelectedKeys, UserGuid);

                SearchParameters = par;
                ExecuteSearch();
            }
        }
    }
}
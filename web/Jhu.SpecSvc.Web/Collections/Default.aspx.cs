using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.Collections
{
    public partial class Default : PageBase
    {
        public static string GetUrl()
        {
            return "~/Collections/Default.aspx";
        }

        public static string GetUrl(bool testStatus)
        {
            if (testStatus)
            {
                return "~/Collections/Default.aspx?status=true";
            }
            else
            {
                return GetUrl();
            }
        }

        private Collection[] collections;

        protected void Page_Load(object sender, EventArgs e)
        {
            collections = PortalConnector.QueryCollections(UserGuid, IO.SearchMethod.Unknown).ToArray();
            List.DataSource = collections;
        }

        protected void Create_Click(object sender, EventArgs e)
        {
        }

        protected void Modify_Click(object sender, EventArgs e)
        {
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
        }

        protected void Test_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < collections.Length; i++)
            {
                collections[i].TestStatus();
            }
        }

        protected void ListSelectedValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
        }
    }
}
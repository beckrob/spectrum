using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.Graywulf.Web.Controls;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class Tabs : System.Web.UI.UserControl
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FreeSearch.NavigateUrl = Default.GetUrl();
            ConeSearch.NavigateUrl = Cone.GetUrl();
            RedshiftSearch.NavigateUrl = Redshift.GetUrl();
            RegionSearch.NavigateUrl = Region.GetUrl();
            AdvancedSearch.NavigateUrl = Advanced.GetUrl();
            ModelSearch.NavigateUrl = Model.GetUrl();
            IDSearch.NavigateUrl = Search.ID.GetUrl();
            ObjectSearch.NavigateUrl = Search.Object.GetUrl();
            SqlSearch.NavigateUrl = Sql.GetUrl();
            SkyServerSearch.NavigateUrl = SkyServer.GetUrl();

            TabHeader.SelectedTab = (Tab)TabHeader.FindControl(SelectedTab);
        }
    }
}
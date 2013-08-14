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

            TabHeader.SelectedTab = (Tab)TabHeader.FindControl(SelectedTab);
        }
    }
}
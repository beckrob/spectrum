using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class Output : System.Web.UI.Page
    {
        public static string GetUrl()
        {
            return "~/Pipeline/Output.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.MySpectrum
{
    public partial class ChangeUrl : PageBase
    {
        public static string GetUrl()
        {
            return "~/MySpectrum/ChangeUrl.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException();
            }

            if (!IsPostBack)
            {
                AdminUrl.Text = MySpectrumAdminUrl;
                SearchUrl.Text = MySpectrumSearchUrl;
                GraphUrl.Text = MySpectrumGraphUrl;
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                MySpectrumAdminUrl = AdminUrl.Text;
                MySpectrumSearchUrl = SearchUrl.Text;
                MySpectrumGraphUrl = GraphUrl.Text;
            }

            Response.Redirect(Default.GetUrl());
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Default.GetUrl());
        }
    }
}
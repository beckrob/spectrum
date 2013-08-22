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
    public partial class Region : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Region.aspx";
        }

        public string HtmRanges
        {
            get { return (string)(ViewState["HtmRanges"]); }
            set { ViewState["HtmRanges"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["HtmRanges"] != null)
            {
                HtmRanges = Request.Form["HtmRanges"];
            }

            RedirectForm.Visible = HtmRanges == null;
            RegionSearchForm.Visible = HtmRanges != null;
        }

        protected void Redirect_Click(object sender, EventArgs e)
        {
            Response.Redirect(AppSettings.RegionSearchUrl.Replace("[$url]", Page.Request.Url.ToString()));
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var par = new HtmRangeSearchParameters(true);
                par.SetRanges(HtmRanges);
                par.Collections = Collections.SelectedKeys;

                SearchParameters = par;
                Execute();
            }
        }
    }
}
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
    public partial class Redshift : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Redshift.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var par = new RedshiftSearchParameters(true);
                par.Redshift.Min.Value = double.Parse(RedshiftFrom.Text);
                par.Redshift.Max.Value = double.Parse(RedshiftTo.Text);
                par.Collections = Collections.SelectedKeys;

                SearchParameters = par;
                Execute();
            }
        }
    }
}
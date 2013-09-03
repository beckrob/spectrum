using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.IO.Remote;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class Default : PageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Default.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                double ra, dec;

                if (!AstroUtil.TryParseRaDec(Query.Text, out ra, out dec))
                {
                    throw new NotImplementedException("NED web service missing...");

                    // *** TODO
                    var ned = new NED();
                    var obj = ned.ObjByName(Query.Text);
                    ra = obj.ra;
                    dec = obj.dec;
                }

                var par = new ConeSearchParameters(true);
                par.Pos.Value = new Jhu.SpecSvc.Schema.Position(ra, dec);
                par.Sr.Value = double.Parse(Radius.Text);
                par.Collections = PortalConnector.LoadCollections(Collections.SelectedKeys, UserGuid);

                SearchParameters = par;
                ExecuteSearch();
            }
        }
    }
}
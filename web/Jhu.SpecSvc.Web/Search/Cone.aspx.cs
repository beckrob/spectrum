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
    public partial class Cone : PageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Cone.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                double ra, dec;
                AstroUtil.TryParseRaDec(Coordinates.Text, out ra, out dec);

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
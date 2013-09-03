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
    public partial class ID : PageBase
    {
        public static string GetUrl()
        {
            return "~/Search/ID.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var rawids = ObjectList.Text.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                var ids = new List<string>(rawids.Length);

                for (int i = 0; i < rawids.Length; i++)
                {
                    if (!String.IsNullOrWhiteSpace(rawids[i]))
                    {
                        ids.Add(rawids[i]);

                        // TODO: append # if necessary, but
                    }
                }

                // Add # for compatibility
                /*for (int i = 0; i < par.Ids.Length; i++)
                {
                    par.Ids[i] = par.Ids[i].Trim();
                    if (par.Ids[i].IndexOf("#") == -1 && par.Ids[i].Trim() != "")
                    {
                        par.Ids[i] = "#" + par.Ids[i];
                    }
                }*/

                var par = new IdSearchParameters(true);
                par.Ids = ids.ToArray();

                SearchParameters = par;
                ExecuteSearch();
            }
        }
    }
}
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
    public partial class Object : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Object.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var lines = ObjectList.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var ids = new List<string>(lines.Length);
                var objs = new List<string>(lines.Length);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (!String.IsNullOrWhiteSpace(lines[i]))
                    {
                        string id;
                        string obj;

                        if (Ids.Checked)
                        {
                            // Find first column and use
                            int l = lines[i].IndexOfAny(new char[] { ' ', '\t', ',', ';' });
                            if (l >= 0)
                            {
                                id = lines[i].Substring(0, l);
                                obj = lines[i].Substring(l + 1);
                            }
                            else
                            {
                                id = "";
                                obj = lines[i];
                            }
                        }
                        else
                        {
                            id = "";
                            obj = lines[i];
                        }

                        ids.Add(id);
                        objs.Add(obj);
                    }
                }
                
                var par = new ObjectSearchParameters(true);

                par.Objects = objs.ToArray();
                par.Ids = ids.ToArray();
                par.Sr.Value = double.Parse(Sr.Text) / 60.0; // arcsec
                par.Collections = Collections.SelectedKeys;

                SearchParameters = par;
                Execute();
            }
        }
    }
}
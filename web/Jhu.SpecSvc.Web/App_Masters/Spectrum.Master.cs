using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.App_Masters
{
    public partial class Spectrum : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.theScriptManager.Scripts.Add(new ScriptReference(Jhu.Graywulf.Web.Util.JQuery.JQueryUrl));
            this.theScriptManager.Scripts.Add(new ScriptReference("Jhu.Graywulf.Web.Controls.DockingPanel.js", "Jhu.Graywulf.Web"));

            this.Page.Title = (string)Page.Application[Jhu.Graywulf.Web.Constants.ApplicatonLongTitle];
            this.Caption.Text = (string)Page.Application[Jhu.Graywulf.Web.Constants.ApplicationShortTitle];
        }
    }
}
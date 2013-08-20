using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Controls
{
    public partial class WizardButtons : System.Web.UI.UserControl
    {
        public event CommandEventHandler Command;

        public bool OkEnabled
        {
            get { return Ok.Enabled; }
            set { Ok.Enabled = value; }
        }

        public bool ResultsEnabled
        {
            get { return Results.Enabled; }
            set { Results.Enabled = value; }
        }

        public bool FinishEnabled
        {
            get { return Finish.Enabled; }
            set { Finish.Enabled = value; }
        }

        public bool BackEnabled
        {
            get { return Back.Enabled; }
            set { Back.Enabled = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            if (Command != null)
            {
                Command(sender, e);
            }
        }
    }
}
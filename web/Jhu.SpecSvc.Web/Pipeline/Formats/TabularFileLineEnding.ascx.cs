using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline.Formats
{
    public partial class TabularFileLineEnding : System.Web.UI.UserControl
    {
        public TabularFile.LineEnding LineEnding
        {
            get { return (TabularFile.LineEnding)Enum.Parse(typeof(TabularFile.LineEnding), lineEnding.SelectedValue); }
            set { lineEnding.SelectedValue = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
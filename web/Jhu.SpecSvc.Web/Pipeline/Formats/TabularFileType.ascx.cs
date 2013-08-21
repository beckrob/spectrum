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
    public partial class TabularFileType : System.Web.UI.UserControl
    {
        public TabularFile.FileType FileType
        {
            get { return (TabularFile.FileType)Enum.Parse(typeof(TabularFile.FileType), fileType.SelectedValue); }
            set { fileType.SelectedValue = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jhu.SpecSvc.Web
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        public new PageBase Page
        {
            get { return (PageBase)base.Page; }
        }
    }
}
﻿using System;

namespace Jhu.SpecSvc.Web.Controls
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Home.NavigateUrl = Jhu.SpecSvc.Web.Default.GetUrl();
            Search.NavigateUrl = Jhu.SpecSvc.Web.Search.Default.GetUrl();

            MySpectrum.NavigateUrl = Jhu.SpecSvc.Web.MySpectrum.Default.GetUrl();
            Collections.NavigateUrl = Jhu.SpecSvc.Web.Collections.Default.GetUrl();
            Programming.NavigateUrl = Jhu.SpecSvc.Web.Programming.Default.GetUrl();
            Docs.NavigateUrl = Jhu.SpecSvc.Web.Docs.Default.GetUrl();
        }
    }
}
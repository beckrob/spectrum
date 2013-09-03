using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jhu.SpecSvc.IO
{
    public static class AppSettings
    {
        public static string PortalConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Jhu.SpecSvc.Portal"].ConnectionString; }
        }
    }
}

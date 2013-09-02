using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Jhu.SpecSvc.MySpectrum
{
    public static class AppSettings
    {
        private static string GetValue(string key)
        {
            return (string)((NameValueCollection)ConfigurationManager.GetSection("Jhu.SpecSvc.MySpectrum/Settings"))[key];
        }

        public static string SqlConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Jhu.SpecSvc.MySpectrum"].ConnectionString; }
        }

        public static Guid UserWebServiceGuid
        {
            get { return new Guid(GetValue("UserWebServiceGuid")); }
        }

        public static string DefaultPublisherID
        {
            get { return GetValue("DefaultPublisherID"); }
        }
    }
}
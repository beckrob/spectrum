﻿using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace Jhu.SpecSvc.Web
{
    public class AppSettings
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Jhu.SpecSvc"].ConnectionString; }
        }

        private static string GetValue(string key)
        {
            return (string)((NameValueCollection)ConfigurationManager.GetSection("Jhu.SpecSvc.Web"))[key];
        }

        public static NameValueCollection ImageCutOuts
        {
            get
            {
                return (NameValueCollection)ConfigurationManager.GetSection("Jhu.SpecSvc.Web/ImageCutOuts");
            }
        }

        /*public static string ExportDir
        {
            get { return GetValue("ExportDir"); }       // *** TODO: move this to the federation config?
        }*/
    }
}
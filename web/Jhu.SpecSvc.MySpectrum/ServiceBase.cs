using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.MySpectrum
{
    public class ServiceBase : System.Web.Services.WebService
    {
        public SqlConnector CreateSqlConnector()
        {
            var cn = new SqlConnector();
            cn.ConnectionString = AppSettings.SqlConnectionString;
            cn.Open();

            return cn;
        }

        public string BaseUrl
        {
            get
            {
                var url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                return String.Format("{0}://{1}{2}/", url.Scheme, url.Authority, HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
            }
        }
    }
}
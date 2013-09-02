using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.WS
{
    public class ServiceBase : System.Web.Services.WebService
    {
        public PortalConnector CreatePortalConnector()
        {
            var cn = new PortalConnector();
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
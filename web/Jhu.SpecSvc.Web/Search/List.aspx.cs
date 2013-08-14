using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class List : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/List.aspx";
        }

        protected void SpectrumDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {
            var sds = (ResultsDataSource)e.ObjectInstance;

            sds.Connector = Connector;
            sds.ResultsetId = ResultsetId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}
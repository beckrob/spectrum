using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.Controls
{
    public partial class CollectionList : UserControlBase
    {
        protected List<Collection> collections;

        public SearchMethods SearchMethod
        {
            get { return (SearchMethods)(ViewState["SearchMethod"] ?? SearchMethods.Unknown); }
            set { ViewState["SearchMethod"] = value; }
        }

        public string[] SelectedKeys
        {
            get { return collectionList.SelectedDataKeys.ToArray(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            collectionList.DataSource = new List<Collection>(Page.PortalConnector.QueryCollections(Page.UserGuid, SearchMethod));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.Collections
{
    public partial class CollectionDetails : PageBase
    {
        public enum RequestMethod
        {
            Create,
            Modify
        }

        public static string GetUrl()
        {
            return String.Format("~/Collections/CollectionDetails.aspx?method={0}", RequestMethod.Create);
        }

        public static string GetUrl(RequestMethod method, string id)
        {
            return String.Format("~/Collections/CollectionDetails.aspx?method={0}&id={1}", method, HttpContext.Current.Server.UrlEncode(id));
        }

        private Collection collection;

        public RequestMethod Method
        {
            get { return (RequestMethod)Enum.Parse(typeof(RequestMethod), Request.QueryString["method"]); }
        }

        public string ID
        {
            get { return Request.QueryString["id"]; }
        }

        public CollectionDetails()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.collection = new Collection();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException();
            }

            if (Method == RequestMethod.Modify)
            {
                collection.Id = ID;
                PortalConnector.LoadCollection(collection);
                PortalConnector.QueryCollectionSearchMethods(collection);

                if (collection.UserGuid != UserGuid)
                {
                    throw new InvalidOperationException();
                }
            }

            if (!IsPostBack)
            {
                RefreshSearchMethodsList();
                UpdateForm();
            }
        }

        protected void IdValidator_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            // check format
            if (CollectionID.Text.EndsWith("/"))
            {
                IdValidator.Text = "Cannot end with /";
                return;
            }

            // check duplicates

            if (!PortalConnector.CheckDuplicateCollectionId(Request.QueryString["id"], CollectionID.Text))
            {
                IdValidator.Text = "Duplicate id";
                return;
            }

            args.IsValid = true;
        }

        protected void ConnectionStringValidator_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            // check availability
            // *** TODO
            try
            {
                /*
                WsConnector conn = new WsConnector();
                conn.Url = ConnectionString.Text;

                conn.Timeout = 10000;
                string[] rev = conn.Revisions();
                args.IsValid = true;
                 * 
                 * */
                return;
            }
            catch (System.Exception ex)
            {
                ConnectionStringValidator.Text = "Remote service not available: " + ex.Message;
                args.IsValid = false;
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                SaveForm();

                switch (Method)
                {
                    case RequestMethod.Create:
                        PortalConnector.SaveCollection(collection, "", UserGuid);
                        break;
                    case RequestMethod.Modify:
                        PortalConnector.SaveCollection(collection, ID, UserGuid);
                        break;
                }

                Response.Redirect(Default.GetUrl());
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Default.GetUrl());
        }

        private void UpdateForm()
        {
            switch (Method)
            {
                case RequestMethod.Create:
                    ColectionDetailsForm.Text = "Register new collection";
                    break;
                case RequestMethod.Modify:
                    ColectionDetailsForm.Text = "Modify collection";

                    CollectionID.Text = collection.Id;
                    Name.Text = collection.Name;
                    Description.Text = collection.Description;
                    CollectionType.SelectedValue = ((int)collection.Type).ToString();
                    Location.Text = collection.Location;
                    ConnectionString.Text = collection.ConnectionString;
                    GraphUrl.Text = collection.GraphUrl;
                    Public.Checked = (collection.Public > 0);

                    foreach (var s in collection.SearchMethods)
                    {
                        SearchMethods.Items.FindByValue(((int)s).ToString()).Selected = true;
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void SaveForm()
        {
            collection.Id = CollectionID.Text;
            collection.Name = Name.Text;
            collection.Description = Description.Text;
            collection.Type = (CollectionType)(int.Parse(CollectionType.SelectedValue));
            collection.Location = Location.Text;
            collection.ConnectionString = ConnectionString.Text;
            collection.GraphUrl = GraphUrl.Text;
            collection.Public = Public.Checked ? 1 : 0;

            collection.UserGuid = UserGuid;

            collection.SearchMethods.Clear();
            foreach (ListItem li in SearchMethods.Items)
            {
                if (li.Selected)
                {
                    collection.SearchMethods.Add((SearchMethod)int.Parse(li.Value));
                }
            }
        }

        private void RefreshSearchMethodsList()
        {
            // Load search methods
            if (!IsPostBack)
            {
                var sm = PortalConnector.QuerySearchMethods();
                foreach (int key in sm.Keys)
                {
                    SearchMethods.Items.Add(new ListItem(sm[key], key.ToString()));
                }
            }
        }
    }
}
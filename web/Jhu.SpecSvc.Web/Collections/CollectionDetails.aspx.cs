using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.Collections
{
    public partial class CollectionDetails : ItemFormBase<Collection>
    {
        public static string GetUrl()
        {
            return String.Format("~/Collections/CollectionDetails.aspx?method={0}", ItemFormRequestMethod.Create);
        }

        public static string GetUrl(ItemFormRequestMethod method, string id)
        {
            return String.Format("~/Collections/CollectionDetails.aspx?method={0}&id={1}", method, HttpContext.Current.Server.UrlEncode(id));
        }

        protected override void OnUpdateForm()
        {
            RefreshSearchMethodsList();

            switch (Method)
            {
                case ItemFormRequestMethod.Create:
                    ColectionDetailsForm.Text = "Register new collection";
                    break;
                case ItemFormRequestMethod.Modify:
                    ColectionDetailsForm.Text = "Modify collection";

                    CollectionID.Text = Item.Id;
                    Name.Text = Item.Name;
                    Description.Text = Item.Description;
                    CollectionType.SelectedValue = ((int)Item.Type).ToString();
                    Location.Text = Item.Location;
                    ConnectionString.Text = Item.ConnectionString;
                    GraphUrl.Text = Item.GraphUrl;
                    Public.Checked = (Item.Public > 0);

                    foreach (var s in Item.SearchMethods)
                    {
                        SearchMethods.Items.FindByValue(((int)s).ToString()).Selected = true;
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnSaveForm()
        {
            Item.Id = CollectionID.Text;
            Item.Name = Name.Text;
            Item.Description = Description.Text;
            Item.Type = (CollectionType)(int.Parse(CollectionType.SelectedValue));
            Item.Location = Location.Text;
            Item.ConnectionString = ConnectionString.Text;
            Item.GraphUrl = GraphUrl.Text;
            Item.Public = Public.Checked ? 1 : 0;

            Item.UserGuid = UserGuid;

            Item.SearchMethods.Clear();
            foreach (ListItem li in SearchMethods.Items)
            {
                if (li.Selected)
                {
                    Item.SearchMethods.Add((SearchMethod)int.Parse(li.Value));
                }
            }
        }

        protected override Collection OnLoadItem()
        {
            return PortalConnector.LoadCollection(Ids[0]);
        }

        protected override void OnCreateItem()
        {
            PortalConnector.SaveCollection(Item, "", UserGuid);
        }

        protected override void OnModifyItem()
        {
            PortalConnector.SaveCollection(Item, Ids[0], UserGuid);
        }

        protected override void OnDeleteItem()
        {
            PortalConnector.DeleteCollection(Item, UserGuid);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException();
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
    }
}
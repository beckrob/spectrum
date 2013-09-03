using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.MySpectrum
{
    public partial class Folder : ItemFormBase<UserFolder>
    {
        public static string GetUrl()
        {
            return String.Format("~/MySpectrum/Folder.aspx?method={0}", ItemFormRequestMethod.Create);
        }

        public static string GetUrl(ItemFormRequestMethod method, string id)
        {
            return String.Format("~/MySpectrum/Folder.aspx?method={0}&id={1}", method, id);
        }

        protected override UserFolder OnLoadItem()
        {
            return MySpectrumConnector.GetUserFolder(UserGuid, int.Parse(Ids[0]));
        }

        protected override void OnCreateItem()
        {
            MySpectrumConnector.SaveUserFolder(Item, UserGuid);
        }

        protected override void OnModifyItem()
        {
            MySpectrumConnector.SaveUserFolder(Item, UserGuid);
        }

        protected override void OnDeleteItem()
        {
            MySpectrumConnector.DeleteUserFolder(Item, UserGuid);
        }

        protected override void OnUpdateForm()
        {
            switch (Method)
            {
                case ItemFormRequestMethod.Create:
                    FolderForm.Text = "Create user folder";
                    break;
                case ItemFormRequestMethod.Modify:
                    FolderForm.Text = "Rename user folder";
                    break;
                case ItemFormRequestMethod.Delete:
                    FolderForm.Text = "Delete user folder";
                    break;
                default:
                    throw new NotImplementedException();
            }

            Name.Text = Item.Name;
            Name.ReadOnly = Method == ItemFormRequestMethod.Delete;
        }

        protected override void OnSaveForm()
        {
            Item.Name = Name.Text;
        }
    }
}
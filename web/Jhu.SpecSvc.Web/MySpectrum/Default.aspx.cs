using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.MySpectrum
{
    public partial class Default : PageBase
    {
        public static string GetUrl()
        {
            return "~/MySpectrum/Default.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                throw new InvalidOperationException();
            }

            AdminUrl.Text = AdminUrl.NavigateUrl = MySpectrumAdminUrl;
            SearchUrl.Text = SearchUrl.NavigateUrl = MySpectrumSearchUrl;
            GraphUrl.Text = GraphUrl.NavigateUrl = MySpectrumGraphUrl;

            FolderList.DataSource = MySpectrumConnector.QueryUserFolders(UserGuid);
        }

        protected void FolderListSelectedValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = FolderList.SelectedDataKeys.Count == 1;
        }

        protected void ChangeUrl_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.SpecSvc.Web.MySpectrum.ChangeUrl.GetUrl());
        }

        protected void Create_Click(object sender, EventArgs e)
        {
            Response.Redirect(Jhu.SpecSvc.Web.MySpectrum.Folder.GetUrl());
        }

        protected void Rename_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Response.Redirect(Jhu.SpecSvc.Web.MySpectrum.Folder.GetUrl(ItemFormRequestMethod.Modify, FolderList.SelectedDataKeys.First()));
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Response.Redirect(Jhu.SpecSvc.Web.MySpectrum.Folder.GetUrl(ItemFormRequestMethod.Delete, FolderList.SelectedDataKeys.First()));
            }
        }

        protected void List_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var par = new FolderSearchParameters(true);
                par.FolderId = int.Parse(FolderList.SelectedDataKeys.First());
                par.UserGuid = UserGuid;

                var coll = new Collection()
                {
                    Type = CollectionType.WebService,
                    ConnectionString = MySpectrumSearchUrl,
                    GraphUrl = MySpectrumGraphUrl,
                    UserGuid = UserGuid,
                };

                par.Collections = new Collection[] { coll };


                SearchParameters = par;
                ExecuteSearch();
            }
        }
    }
}
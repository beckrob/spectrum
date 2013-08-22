using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class List : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/List.aspx";
        }

        public SpectrumListView SpectrumListView
        {
            get { return (SpectrumListView)(ViewState["SpectrumListView"] ?? SpectrumListView.List); }
            set { ViewState["SpectrumListView"] = value; }
        }

        public int GraphWidth
        {
            get { return 300; }
        }

        public int GraphHeight
        {
            get { return 160; }
        }

        protected void SpectrumSelected_ServerValidate(object serder, ServerValidateEventArgs args)
        {
            switch (SpectrumListView)
            {
                case Web.SpectrumListView.List:
                    args.IsValid = SpectrumList.SelectedDataKeys.Count > 0;
                    break;
                case Web.SpectrumListView.Graph:
                case Web.SpectrumListView.Image:
                    args.IsValid = SpectrumCards.SelectedDataKeys.Count > 0;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ok":
                    Validate();
                    if (IsValid)
                    {
                        HashSet<string> selected;

                        switch (SpectrumListView)
                        {
                            case Web.SpectrumListView.List:
                                selected = SpectrumList.SelectedDataKeys;
                                break;
                            case Web.SpectrumListView.Graph:
                            case Web.SpectrumListView.Image:
                                selected = SpectrumCards.SelectedDataKeys;
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        // *** TODO: this can be further optimized
                        Connector.DeselectAllInResultset(ResultsetId);

                        foreach (var id in selected)
                        {
                            Connector.ChangeSelectionInResultset(ResultsetId, long.Parse(id), true);
                        }

                        Response.Redirect(Jhu.SpecSvc.Web.Pipeline.Default.GetUrl());
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void SpectrumDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {
            var sds = (ResultsDataSource)e.ObjectInstance;

            sds.Connector = Connector;
            sds.ResultsetId = ResultsetId;
        }

        protected void SpectrumList_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            var ph = e.Item.FindControl("detailsPlaceholder");

            var sd = (SpectrumRow)LoadControl("SpectrumRow.ascx");
            sd.ID = "spectrumItem";
            ph.Parent.Controls.Add(sd);

            // Set item data
            var spectrum = (Spectrum)e.Item.DataItem;

            if (spectrum != null)
            {
                sd.Index = e.Item.DataItemIndex;
                sd.Item = spectrum;
            }
        }

        protected void SpectrumCards_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            var ph = e.Item.FindControl("detailsPlaceholder");

            var sd = (SpectrumCard)LoadControl("SpectrumCard.ascx");
            sd.ID = "spectrumItem";
            ph.Parent.Controls.Add(sd);

            // Set item data
            var spectrum = (Spectrum)e.Item.DataItem;

            if (spectrum != null)
            {
                sd.Index = e.Item.DataItemIndex;
                sd.Item = spectrum;

                switch (SpectrumListView)
                {
                    case Web.SpectrumListView.Graph:
                        sd.GraphUrl = Graph.GetUrl(spectrum.Curation.PublisherDID.Value, GraphWidth, GraphHeight);
                        break;
                    case Web.SpectrumListView.Image:
                        sd.GraphUrl = GetImageUrl((Spectrum)e.Item.DataItem);
                        break;
                }
            }
        }

        protected void DegreeFormatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DegreeFormat = (DegreeFormat)Enum.Parse(typeof(DegreeFormat), DegreeFormatList.SelectedValue);
        }

        protected void SpectrumListViewList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var slv = (SpectrumListView)Enum.Parse(typeof(SpectrumListView), SpectrumListViewList.SelectedValue);

            // Check if view changed and copy selected items if necessary
            if (SpectrumListView == Web.SpectrumListView.List &&
                (slv == Web.SpectrumListView.Graph || slv == Web.SpectrumListView.Image))
            {
                SpectrumCards.SelectedDataKeys.Clear();
                SpectrumCards.SelectedDataKeys.UnionWith(SpectrumList.SelectedDataKeys);
            }
            else if ((SpectrumListView == Web.SpectrumListView.Graph || SpectrumListView == Web.SpectrumListView.Image) &&
                slv == Web.SpectrumListView.List)
            {
                SpectrumList.SelectedDataKeys.Clear();
                SpectrumList.SelectedDataKeys.UnionWith(SpectrumCards.SelectedDataKeys);
            }

            SpectrumListView = slv;
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            switch (SpectrumListView)
            {
                case Web.SpectrumListView.List:
                    SpectrumCards.Visible = false;
                    SpectrumList.Visible = true;
                    SpectrumListPager.PagedControlID = SpectrumList.ID;
                    break;
                case Web.SpectrumListView.Image:
                case Web.SpectrumListView.Graph:
                    SpectrumCards.Visible = true;
                    SpectrumList.Visible = false;
                    SpectrumListPager.PagedControlID = SpectrumCards.ID;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            TotalResultsCount.Text = SpectrumListPager.TotalRowCount.ToString();
            DisplayedResultsRange.Text = String.Format("{0} - {1}",
                SpectrumListPager.StartRowIndex + 1,
                Math.Min(SpectrumListPager.StartRowIndex + SpectrumListPager.PageSize, SpectrumListPager.TotalRowCount));
        }

        private string GetImageUrl(Spectrum spectrum)
        {
            if (spectrum != null)
            {
                var url = AppSettings.ImageCutOuts["SDSSDR10"];

                url = url.Replace("[$ID]", Server.UrlEncode(spectrum.Curation.PublisherDID.Value));
                url = url.Replace("[$Width]", GraphWidth.ToString());
                url = url.Replace("[$Height]", GraphHeight.ToString());
                url = url.Replace("[$Ra]", spectrum.Target.Pos.Value.Ra.ToString("0.000000"));
                url = url.Replace("[$Dec]", spectrum.Target.Pos.Value.Dec.ToString("0.000000"));

                return url;
            }
            else
            {
                return null;
            }
        }
    }
}
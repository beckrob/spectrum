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

        public int GraphWidth
        {
            get { return 300; }
        }

        public int GraphHeight
        {
            get { return 160; }
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

            var sd = (SpectrumCard)LoadControl("SpectrumCard.ascx");
            sd.ID = "spectrumItem";
            ph.Parent.Controls.Add(sd);

            // Set item data
            var spectrum = (Spectrum)e.Item.DataItem;

            sd.Index = e.Item.DataItemIndex;
            sd.Item = spectrum;
            //sd.GraphUrl = GetImageUrl((Spectrum)e.Item.DataItem);
            sd.GraphUrl = Graph.GetUrl(spectrum.Curation.PublisherDID.Value, GraphWidth, GraphHeight);
        }

        protected void DegreeFormatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DegreeFormat = (DegreeFormat)Enum.Parse(typeof(DegreeFormat), DegreeFormatList.SelectedValue);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
namespace Jhu.SpecSvc.Web.Search
{
    public abstract class SpectrumDetailsBase : System.Web.UI.UserControl
    {
        public int Index { get; set; }
        public Spectrum Item { get; set; }

        protected global::System.Web.UI.WebControls.CheckBox SelectionCheckbox;
        protected global::System.Web.UI.WebControls.Label Number;
        protected global::System.Web.UI.WebControls.Label Name;
        protected global::System.Web.UI.WebControls.Label Class;
        protected global::System.Web.UI.WebControls.Label Redshift;
        protected global::System.Web.UI.WebControls.Label Pos;
        protected global::System.Web.UI.WebControls.HyperLink PublisherID;
        protected global::System.Web.UI.WebControls.HyperLink CreatorID;
        protected global::System.Web.UI.WebControls.HyperLink Details;
        protected global::System.Web.UI.WebControls.HyperLink Modify;
        protected global::System.Web.UI.WebControls.HyperLink Delete;

        protected virtual void UpdateControl()
        {
            if (Item != null)
            {
                SelectionCheckbox.Checked = Item.ResultSelected;
                Number.Text = (Index + 1).ToString();
                Name.Text = Item.Target.Name.Value;
                PublisherID.NavigateUrl = Item.Curation.PublisherDID.Value;
                CreatorID.NavigateUrl = Item.DataId.CreatorDID.Value;
                Class.Text = Item.Target.SpectralClass.Value;

                if (Item.Target.Redshift.Value <= -1)
                {
                    Redshift.Text = "n/a";
                }
                else
                {
                    Redshift.Text = Item.Target.Redshift.Value.ToString("0.0000");
                }

                // Out of range values mean no values
                if (Item.Target.Pos.Value.Dec < -90 || (Item.Target.Pos.Value.Dec == 0 && Item.Target.Pos.Value.Ra == 0))
                {
                    Pos.Text = "n/a";
                }

                switch (((PageBase)Page).DegreeFormat)
                {
                    case DegreeFormat.Sexagesimal:
                        Pos.Text = AstroUtil.deg2hms(Item.Target.Pos.Value.Ra) + "&nbsp;" + AstroUtil.deg2dms(Item.Target.Pos.Value.Dec);
                        break;
                    case DegreeFormat.Decimal:
                        Pos.Text = Item.Target.Pos.Value.Ra.ToString("0.000000") + ", " + Item.Target.Pos.Value.Dec.ToString("0.000000");
                        break;
                }

                //Details.NavigateUrl = "search_details.aspx?id=" + Server.UrlEncode(Item.Curation.PublisherDID.Value);               //***
                //Delete.NavigateUrl = "myspectrum/delete_spectrum.aspx?id=" + Server.UrlEncode(Item.Curation.PublisherDID.Value);    //***
            }
        }
    }
}
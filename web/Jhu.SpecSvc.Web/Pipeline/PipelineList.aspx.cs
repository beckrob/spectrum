using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public partial class List : PageBase
    {
        public static string GetUrl()
        {
            return "~/Pipeline/List.aspx";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PipelineList.DataSource = PipelineConnector.QueryPipelines(UserGuid);
        }

        /*
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
        }*/
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public class SearchPageBase : PageBase
    {
        public SearchParametersBase SearchParameters
        {
            get { return (SearchParametersBase)Session[Constants.SessionSearchParameters]; }
            set { Session[Constants.SessionSearchParameters] = value; }
        }

        public void Execute()
        {
            DeleteExistingResultset();  // TODO: what if saved as job?

            // Create new resultset
            var resultsetId = ResultsetId = Connector.CreateResultset();

            var results = Connector.FindSpectrumDispatch(SearchParameters);
            Connector.SaveResultsetSpectra(resultsetId, results);

            Response.Redirect(List.GetUrl());
        }

        public void DeleteExistingResultset()
        {
            // if a previous resultset exists it should be deleted now
            if (ResultsetId != -1)
            {
                Connector.DeleteResultset(ResultsetId);
                Session["ResultsetId"] = -1;
            }
        }
    }
}
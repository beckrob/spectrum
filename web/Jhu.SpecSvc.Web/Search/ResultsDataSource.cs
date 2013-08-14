using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Web.Search
{
    public class ResultsDataSource
    {
        public PortalConnector Connector { get; set; }
        public int ResultsetId { get; set; }

        public ResultsDataSource()
        {
        }

        public IEnumerable<Spectrum> GetResultsSpectra(int from, int max)
        {
            var res = new List<Spectrum>(Connector.QueryResults(ResultsetId, from, max, false));
            return res;
        }

        public int CountResultsSpectra()
        {
            var res = Connector.GetResultsCount(ResultsetId);
            return res;
        }
    }
}
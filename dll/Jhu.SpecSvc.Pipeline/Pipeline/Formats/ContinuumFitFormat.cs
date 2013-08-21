using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class ContinuumFitFormat : TabularFileOutputFormat
    {
        public override string Title
        {
            get { return FormatDescriptions.ContinuumFitTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.ContinuumFitDescription; }
        }
    }
}

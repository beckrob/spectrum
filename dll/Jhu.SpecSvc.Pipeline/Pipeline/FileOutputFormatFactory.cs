using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Pipeline
{
    public static class FileOutputFormatFactory
    {
        public static Type[] GetFormatTypes()
        {
            return new Type[] {
                    typeof(ContinuumFitFormat),
                    typeof(LineFitFormat),
                    typeof(MagnitudeFormat),
                    typeof(SpectrumAsciiFormat),
                    typeof(SpectrumPlotFormat),
                    typeof(SpectrumVoTableFormat),
                    typeof(SpectrumXmlFormat)};
        }
    }
}

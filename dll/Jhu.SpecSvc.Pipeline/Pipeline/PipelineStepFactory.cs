using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Pipeline.Steps;

namespace Jhu.SpecSvc.Pipeline
{
    public static class PipelineStepFactory
    {
        public static Type[] GetStepTypes()
        {
            return new Type[] {
                    typeof(BinByStep),
                    typeof(RebinStep),
                    typeof(RedshiftStep),
                    typeof(DereddenStep),
                    typeof(WavelengthConversionStep),
                    typeof(NormalizeStep),
                    typeof(ConvolutionStep),
                    typeof(CompositeStep),
                    typeof(PcaStep),
                    typeof(ContinuumFitStep),
                    typeof(SelectBestFitStep),
                    typeof(LineFitStep),
                    typeof(FluxStep),
                    typeof(SpectralIndexStep)};
        }
    }
}

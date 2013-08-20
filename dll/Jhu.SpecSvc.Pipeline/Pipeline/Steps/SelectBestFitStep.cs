using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class SelectBestFitStep : PipelineStep
    {
        public override string Title
        {
            get { return StepDescriptions.SelectBestFitTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.SelectBestFitDescription; }
        }

        public SelectBestFitStep()
        {
            InitializeMembers();
        }

        public SelectBestFitStep(SelectBestFitStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(SelectBestFitStep old)
        {
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            int min = -1;
            for (int i = 0; i < spectrum.ContinuumFits.Count; i++)
            {
                if (min == -1)
                {
                    min = i;
                    continue;
                }

                if (spectrum.ContinuumFits[i].Chi2 / spectrum.ContinuumFits[i].Ndf
                    < spectrum.ContinuumFits[min].Chi2 / spectrum.ContinuumFits[min].Ndf)
                {
                    min = i;
                    continue;
                }
            }

            spectrum.ContinuumFitBest = min;

            if (min != -1)
            {
                spectrum.Model_Continuum = spectrum.ContinuumFits[min].Continuum;
                spectrum.Flux_Lines = spectrum.ContinuumFits[min].Residual;
            }

            return spectrum;
        }
    }
}

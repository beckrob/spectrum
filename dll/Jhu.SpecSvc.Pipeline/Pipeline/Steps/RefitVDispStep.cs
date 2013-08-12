using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline
{
    public class RefitVDispStep : ContinuumFitStep
    {


        public RefitVDispStep()
        {
            InitializeMembers();
        }

        public RefitVDispStep(RefitVDispStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {

        }

        private void CopyMembers(ContinuumFitStep old)
        {

        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            if (Math.Abs(spectrum.ContinuumFitParameters.VDisp - 150) < 2)
            {

                // Put original spectrum aside, and use line subtracted spectrum
                double[] flux = spectrum.Flux_Value;
                spectrum.Flux_Value = spectrum.Flux_Continuum;

                // Get starting values from best fit
                // Run fitting with the best template set
                ContinuumFit res = new FitTask(this, spectrum).Execute(spectrum.ContinuumFitBest, spectrum.ContinuumFitParameters.VDisp, spectrum.ContinuumFitParameters.Tau_V, spectrum.ContinuumFitParameters.Mu);

                lock (spectrum)
                {
                    spectrum.ContinuumFitParameters = res;
                    spectrum.Model_Continuum = res.Continuum;
                    spectrum.Flux_Lines = res.Residual;
                }

                // restore measured flux vector
                spectrum.Flux_Value = flux;
            }

            return spectrum;
        }

    }
}

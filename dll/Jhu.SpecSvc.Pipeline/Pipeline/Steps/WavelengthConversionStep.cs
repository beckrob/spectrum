using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class WavelengthConversionStep : PipelineStep
    {
        public enum WavelengthConversionMethod
        {
            None,
            AirToVacuum,
            VacuumToAir
        }

        private WavelengthConversionMethod method;

        public WavelengthConversionMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.WavelengthConversionTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.WavelengthConversionDescription; }
        }

        public WavelengthConversionStep()
        {
            InitializeMembers();
        }

        public WavelengthConversionStep(WavelengthConversionStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = WavelengthConversionMethod.AirToVacuum;
        }

        private void CopyMembers(WavelengthConversionStep old)
        {
            this.method = old.method;
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            switch (method)
            {
                case WavelengthConversionMethod.None:
                    break;
                case WavelengthConversionMethod.VacuumToAir:
                    spectrum.Vac2Air();
                    break;
                case WavelengthConversionMethod.AirToVacuum:
                    spectrum.Air2Vac();
                    break;
            }

            return spectrum;
        }
    }
}

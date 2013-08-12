using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class CalculateStep : ProcessStep
    {
        public enum CalculateMethod
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        private CalculateMethod method;
        private Spectrum other;

        public CalculateMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        public Spectrum Other
        {
            get { return other; }
            set { other = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.CalculateTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.CalculateDescription; }
        }

        public CalculateStep()
        {
            InitializeMembers();
        }

        public CalculateStep(CalculateStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = CalculateMethod.Add;
            this.other = null;
        }

        private void CopyMembers(CalculateStep old)
        {
            this.method = old.method;
            this.other = old.other;
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            // --- Rebin templates to match spectrum
            double[] temp;
            long[] nmask;

            // rebin template to the spectrum's grid
            Util.Grid.Rebin(other.Spectral_Accuracy_BinLow, other.Spectral_Accuracy_BinHigh, other.Flux_Value, null,
                spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, out temp, out nmask);
            

            switch (method)
            {
                case CalculateMethod.Subtract:
                    Util.Vector.Subtract(spectrum.Flux_Value, temp, out spectrum.Flux_Value);
                    break;
                case CalculateMethod.Add:
                    Util.Vector.Add(spectrum.Flux_Value, temp, out spectrum.Flux_Value);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return spectrum;
        }
    }
}

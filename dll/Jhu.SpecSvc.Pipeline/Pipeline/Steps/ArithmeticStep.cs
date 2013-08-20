using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class ArithmeticStep : PipelineStep
    {
        public enum AritmeticOperator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        private AritmeticOperator @operator;
        private Spectrum other;

        public AritmeticOperator Operator
        {
            get { return @operator; }
            set { @operator = value; }
        }

        public Spectrum Other
        {
            get { return other; }
            set { other = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.ArithmeticTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.ArithmeticDescription; }
        }

        public ArithmeticStep()
        {
            InitializeMembers();
        }

        public ArithmeticStep(ArithmeticStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.@operator = AritmeticOperator.Add;
            this.other = null;
        }

        private void CopyMembers(ArithmeticStep old)
        {
            this.@operator = old.@operator;
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
            

            switch (@operator)
            {
                case AritmeticOperator.Subtract:
                    Util.Vector.Subtract(spectrum.Flux_Value, temp, out spectrum.Flux_Value);
                    break;
                case AritmeticOperator.Add:
                    Util.Vector.Add(spectrum.Flux_Value, temp, out spectrum.Flux_Value);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return spectrum;
        }
    }
}

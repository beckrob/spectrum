using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class BinByStep : PipelineStep
    {
        public enum BinByParameter
        {
            Redshift
        }

        private BinByParameter parameter;
        private DoubleInterval binLimits;
        private DoubleParam binSize;

        public BinByParameter Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        public DoubleInterval BinLimits
        {
            get { return binLimits; }
            set { binLimits = value; }
        }

        public DoubleParam BinSize
        {
            get { return binSize; }
            set { binSize = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.BinByTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.BinByDescription; }
        }

        public BinByStep()
        {
            InitializeMembers();
        }

        public BinByStep(BinByStep old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.parameter = BinByParameter.Redshift;
            this.binLimits = new DoubleInterval(ParamRequired.Optional);
            this.binSize = new DoubleParam(true);
        }

        private void CopyMembers(BinByStep old)
        {
            this.parameter = old.parameter;
            this.binLimits = new DoubleInterval(old.binLimits);
            this.binSize = new DoubleParam(old.binSize);
        }

        protected override Spectrum OnExecute(Spectrum spectrum)
        {
            // Calculate bin hash

            switch (parameter)
            {
                case BinByParameter.Redshift:
                    break;
                default:
                    throw new NotImplementedException();
            }

            return spectrum;
        }

        private void CalculateRedshiftHash(Spectrum spectrum)
        {
            var z = spectrum.Target.Redshift.Value;
            long bin = -1;

            if (binLimits.Min <= z && z <= binLimits.Max)
            {
                bin = (long)Math.Floor((z - binLimits.Min.Value) / binSize.Value);
            }

            spectrum.BinHash = bin;
            spectrum.BinLimits.Min.Value = binLimits.Min.Value + bin * binSize.Value;
            spectrum.BinLimits.Max.Value = binLimits.Min.Value + (1 + bin) * binSize.Value;
            spectrum.BinValue.Value = 0.5 * (spectrum.BinLimits.Min.Value + spectrum.BinLimits.Max.Value);
        }
    }
}

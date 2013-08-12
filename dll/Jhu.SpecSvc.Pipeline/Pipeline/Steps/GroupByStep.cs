using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class GroupByStep : ProcessStep
    {
        private DoubleInterval redshiftLimits;
        private DoubleParam redshiftBinSize;
        private DoubleArrayParam redshiftValue;
        private DoubleArrayParam redshiftBinLow;
        private DoubleArrayParam redshiftBinHigh;

        public DoubleInterval RedshiftLimits
        {
            get { return this.redshiftLimits; }
            set { this.redshiftLimits = value; }
        }

        public DoubleArrayParam RedshiftValue
        {
            get { return this.redshiftValue; }
            set { this.redshiftValue = value; }
        }

        public DoubleParam RedshiftBinSize
        {
            get { return this.redshiftBinSize; }
            set { this.redshiftBinSize = value; }
        }

        public DoubleArrayParam RedshiftBinLow
        {
            get { return this.redshiftBinLow; }
            set { this.redshiftBinLow = value; }
        }

        public DoubleArrayParam RedshiftBinHigh
        {
            get { return this.redshiftBinHigh; }
            set { this.redshiftBinHigh = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.CompositeTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.CompositeDescription; }
        }

        public GroupByStep()
        {
            InitializeMembers();
        }

        public GroupByStep(GroupByStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.redshiftLimits = new DoubleInterval(ParamRequired.Optional);
            this.redshiftLimits.Min.Value = 0;
            this.redshiftLimits.Max.Value = 0.1;
            this.redshiftBinSize = new DoubleParam(true);
            this.redshiftBinSize.Value = 0.01;
            this.redshiftValue = new DoubleArrayParam(true);
            this.redshiftBinLow = new DoubleArrayParam(true);
            this.redshiftBinHigh = new DoubleArrayParam(true);
        }

        private void CopyMembers(GroupByStep old)
        {
            this.redshiftLimits = new DoubleInterval(old.redshiftLimits);
            this.redshiftBinSize = new DoubleParam(old.redshiftBinSize);
            this.redshiftValue = new DoubleArrayParam(old.redshiftValue);
            this.redshiftBinLow = new DoubleArrayParam(old.redshiftBinLow);
            this.redshiftBinHigh = new DoubleArrayParam(old.redshiftBinHigh);
        }

        public override void InitializeStep(int count)
        {
            base.InitializeStep(count);

            CalculateRedshiftGrid();
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            return null;
        }

        public void CalculateRedshiftGrid()
        {
            int points = (int)Math.Floor((redshiftLimits.Max.Value - redshiftLimits.Min.Value) / redshiftBinSize) + 1;

            redshiftValue = new DoubleArrayParam(new double[points], "");
            redshiftBinLow = new DoubleArrayParam(new double[points], "");
            redshiftBinHigh = new DoubleArrayParam(new double[points], "");

            for (int i = 0; i < points; i++)
            {
                redshiftValue.Value[i] = redshiftLimits.Min.Value + i * redshiftBinSize;
                redshiftBinLow.Value[i] = redshiftValue.Value[i] - redshiftBinSize / 2;
                redshiftBinHigh.Value[i] = redshiftValue.Value[i] + redshiftBinSize / 2;
            }
        }
    }
}

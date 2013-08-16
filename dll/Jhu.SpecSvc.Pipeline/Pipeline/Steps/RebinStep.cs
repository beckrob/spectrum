using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class RebinStep : PipelineStep
    {
        private DoubleInterval rebinLimits;
        private DoubleParam rebinBinSize;
        private DoubleArrayParam rebinValue;
        private DoubleArrayParam rebinBinLow;
        private DoubleArrayParam rebinBinHigh;

        public DoubleInterval RebinLimits
        {
            get { return rebinLimits; }
            set { rebinLimits = value; }
        }

        public DoubleParam RebinBinSize
        {
            get { return rebinBinSize; }
            set { rebinBinSize = value; }
        }

        public DoubleArrayParam RebinValue
        {
            get { return rebinValue; }
            set { rebinValue = value; }
        }

        public DoubleArrayParam RebinBinLow
        {
            get { return rebinBinLow; }
            set { rebinBinLow = value; }
        }

        public DoubleArrayParam RebinBinHigh
        {
            get { return rebinBinHigh; }
            set { rebinBinHigh = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.RebinTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.RebinDescription; }
        }

        public RebinStep()
        {
            InitializeMembers();
        }

        public RebinStep(RebinStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.rebinLimits = new DoubleInterval(ParamRequired.Optional);
            this.rebinLimits.Min.Value = 3000;
            this.rebinLimits.Max.Value = 9000;
            this.rebinBinSize = new DoubleParam(true);
            this.rebinBinSize.Value = 1;
            this.rebinValue = new DoubleArrayParam(true);
            this.rebinBinLow = new DoubleArrayParam(true);
            this.rebinBinHigh = new DoubleArrayParam(true);
        }

        private void CopyMembers(RebinStep old)
        {
            this.rebinLimits = new DoubleInterval(old.rebinLimits);
            this.rebinBinSize = new DoubleParam(old.rebinBinSize);
            this.rebinValue = new DoubleArrayParam(old.rebinValue);
            this.rebinBinLow = new DoubleArrayParam(old.rebinBinLow);
            this.rebinBinHigh = new DoubleArrayParam(old.rebinBinHigh);
        }

        public override void InitializeStep(int count)
        {
            base.InitializeStep(count);

            CalculateRebinGrid();
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            spectrum.Rebin(rebinValue.Value, rebinBinLow.Value, rebinBinHigh.Value);

            return spectrum;
        }

        public void CalculateRebinGrid()
        {
            int points = (int)Math.Floor((rebinLimits.Max.Value - rebinLimits.Min.Value) / rebinBinSize);

            rebinValue = new DoubleArrayParam(new double[points], "");
            rebinBinLow = new DoubleArrayParam(new double[points], "");
            rebinBinHigh = new DoubleArrayParam(new double[points], "");

            for (int i = 0; i < points; i++)
            {
                rebinBinLow.Value[i] = rebinLimits.Min.Value + i * rebinBinSize;
                rebinValue.Value[i] = rebinBinLow.Value[i] + rebinBinSize / 2;
                rebinBinHigh.Value[i] = rebinLimits.Min.Value + (i + 1) * rebinBinSize;
            }
        }
    }
}

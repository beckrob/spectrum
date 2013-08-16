using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class ConvolutionStep : PipelineStep
    {


        private DoubleParam velocityDispersion;

        public DoubleParam VelocityDispersion
        {
            get { return velocityDispersion; }
            set { velocityDispersion = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.ConvolutionTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.ConvolutionDescription; }
        }

        public ConvolutionStep()
        {
            InitializeMembers();
        }

        public ConvolutionStep(ConvolutionStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.velocityDispersion = new DoubleParam(150.0, "");
        }

        private void CopyMembers(ConvolutionStep old)
        {
            this.velocityDispersion = new DoubleParam(old.velocityDispersion);
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            double vdisp = velocityDispersion.Value;
            double vdpc = vdisp / Constants.LightSpeed;
            double sq2piinv = 1 / Math.Sqrt(2 * Math.PI);
            spectrum.Convolve(-velocityDispersion.Value, velocityDispersion.Value,
                delegate(double x, double ss)
                {
                    double invsigma = 1 / (ss * vdpc);
                    double xpersigma = x * invsigma;
                    return sq2piinv * Math.Exp(-xpersigma * xpersigma / 2) * invsigma;
                });

            return spectrum;
        }
    }
}

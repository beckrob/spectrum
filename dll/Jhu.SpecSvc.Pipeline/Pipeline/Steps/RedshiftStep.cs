using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class RedshiftStep : PipelineStep
    {
        public enum RedshiftMethod
        {
            RestFrame,
            ObservationFrame,
            Custom
        }

        private DoubleParam redshift;
        private RedshiftMethod method;

        public DoubleParam Redshift
        {
            get { return redshift; }
            set { redshift = value; }
        }

        public RedshiftMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.RedshiftTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.RedshiftDescription; }
        }

        public RedshiftStep()
        {
            InitializeMembers();
        }

        public RedshiftStep(RedshiftStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.redshift = new DoubleParam(true);
            this.method = RedshiftMethod.RestFrame;
        }

        private void CopyMembers(RedshiftStep old)
        {
            this.redshift = new DoubleParam(old.redshift);
            this.method = old.method;
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            switch (method)
            {
                case RedshiftMethod.RestFrame:
                    spectrum.Redshift(0);
                    break;
                case RedshiftMethod.ObservationFrame:
                    spectrum.Redshift(spectrum.Target.Redshift.Value);
                    break;
                case RedshiftMethod.Custom:
                    spectrum.Redshift(this.redshift.Value);
                    break;
            }

            return spectrum;
        }
    }
}

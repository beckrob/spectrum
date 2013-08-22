using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class CustomStep : PipelineStep
    {
        public delegate Spectrum CustomStepFunction(Spectrum spectrum);

        private CustomStepFunction function;

        public CustomStepFunction Function
        {
            get { return function; }
            set { function = value; }
        }

        public override string Title
        {
            get { return "Custom step"; }
        }

        public override string Description
        {
            get { return ""; }
        }

        public CustomStep()
        {
            InitializeMembers();
        }

        public CustomStep(CustomStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(CustomStep old)
        {
        }

        protected override Spectrum OnExecute(Spectrum spectrum)
        {
            return function(spectrum);
        }
    }
}

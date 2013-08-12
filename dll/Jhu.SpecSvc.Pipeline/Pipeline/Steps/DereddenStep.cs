using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class DereddenStep : ProcessStep
    {
        public override string Title
        {
            get { return StepDescriptions.DereddenTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.DereddenDescription; }
        }

        public DereddenStep()
        {
            InitializeMembers();
        }

        public DereddenStep(DereddenStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(DereddenStep old)
        {
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            if (spectrum.Target.GalacticExtinction != null &&
                spectrum.Target.GalacticExtinction.Value != 0.0)
                spectrum.Deredden();
            else
                spectrum.Deredden(connector.GetExtinction(
                    spectrum.Data.SpatialAxis.Coverage.Location.Value.Value.Ra,
                    spectrum.Data.SpatialAxis.Coverage.Location.Value.Value.Dec));

            return spectrum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    [Serializable]
    [XmlInclude(typeof(Targets.FileTarget))]
    //[XmlInclude(typeof(MySpectrumTarget))]
    public abstract class OutputTarget : PipelineObjectBase<Spectrum>
    {
        public OutputTarget()
        {
            InitializeMembers();
        }

        public OutputTarget(OutputTarget old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(OutputTarget old)
        {
        }

        public virtual void InitializeTarget()
        {
        }

        public abstract IEnumerable<Spectrum> Execute(IEnumerable<Spectrum> spectra);
    }
}

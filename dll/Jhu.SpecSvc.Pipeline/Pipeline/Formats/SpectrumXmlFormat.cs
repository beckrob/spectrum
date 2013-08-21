using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class SpectrumXmlFormat : FileOutputFormat
    {
        public override string Title
        {
            get { return FormatDescriptions.SpectrumXmlTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.SpectrumXmlDescription; }
        }

        public SpectrumXmlFormat()
        {
            InitializeMembers();
        }

        public SpectrumXmlFormat(SpectrumVoTableFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(SpectrumVoTableFormat old)
        {
        }

        protected override void Execute(Jhu.SpecSvc.SpectrumLib.Spectrum spectrum, Stream output, out string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Jhu.SpecSvc.SpectrumLib.Spectrum));
            ser.Serialize(output, spectrum);

            filename = GetFilenameFromId(spectrum, true, ".xml");
        }
    }
}

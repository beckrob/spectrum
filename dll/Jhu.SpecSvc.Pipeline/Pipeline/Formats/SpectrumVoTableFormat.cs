using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class SpectrumVoTableFormat : FileOutputFormat
    {
        public override string Title
        {
            get { return FormatDescriptions.SpectrumVoTableTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.SpectrumVoTableDescription; }
        }

        public SpectrumVoTableFormat()
        {
            InitializeMembers();
        }

        public SpectrumVoTableFormat(SpectrumVoTableFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(SpectrumVoTableFormat old)
        {
        }

        protected override void OnExecute(SpectrumLib.Spectrum spectrum, Stream outputStream, out string filename)
        {
            var ser = new XmlSerializer(typeof(VOTABLE.VOTABLE));

            var vt = Jhu.SpecSvc.IO.Mappers.Spectrum2VoTable.MapSpectrum2VoTable(spectrum);
            ser.Serialize(outputStream, vt);

            filename = GetFilenameFromId(spectrum, true, ".votable.xml");
        }
    }
}

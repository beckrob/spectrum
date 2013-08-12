using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
    public class SpectrumVoTableFormat : FileOutputFormat
    {
        public override string Title
        {
            get { return "Spectrum in VOTable"; }
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

        protected override void Execute(Jhu.SpecSvc.SpectrumLib.Spectrum spectrum, Stream output, out string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(VOTABLE.VOTABLE));

            VOTABLE.VOTABLE vt = Jhu.SpecSvc.IO.Mappers.Spectrum2VoTable.MapSpectrum2VoTable(spectrum);
            ser.Serialize(output, vt);

            filename = GetFilenameFromId(spectrum, true, ".votable.xml");
        }
    }
}

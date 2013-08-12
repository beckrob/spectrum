using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Jhu.SpecSvc.IO
{
    [Serializable]
    public class SpectrumAsciiFormat : FileOutputFormat
    {
        private AsciiConnector.AsciiFormat format;
        private List<string> columns;
        private bool writeFields;

        public AsciiConnector.AsciiFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        public List<string> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public bool WriteFields
        {
            get { return writeFields; }
            set { writeFields = value; }
        }

        public override string Title
        {
            get { return "Spectrum in ASCII File"; }
        }

        public SpectrumAsciiFormat()
        {
            InitializeMembers();
        }

        public SpectrumAsciiFormat(SpectrumAsciiFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.format = AsciiConnector.AsciiFormat.Tabular;
            this.columns = new List<string>(){ "Spectral_Value", "Flux_Value" };
        }

        private void CopyMembers(SpectrumAsciiFormat old)
        {
        }

        protected override void Execute(Jhu.SpecSvc.SpectrumLib.Spectrum spectrum, Stream output, out string filename)
        {
            AsciiConnector asc = new AsciiConnector();

            asc.OutputStream = new StreamWriter(output);
            asc.Columns = this.columns.ToArray();
            asc.Format = this.format;
            asc.WriteFields = this.writeFields;

            asc.SaveSpectrum(spectrum, Guid.Empty);


            string extension = null;

            switch (format)
            {
                case AsciiConnector.AsciiFormat.Tabular:
                    extension = ".dat";
                    break;
                case AsciiConnector.AsciiFormat.CommaSeparated:
                    extension = ".csv";
                    break;
            }

            filename = GetFilenameFromId(spectrum, true, extension);
        }
    }
}

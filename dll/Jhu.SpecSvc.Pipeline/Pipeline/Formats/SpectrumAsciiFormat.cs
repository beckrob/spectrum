using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    [Serializable]
    public class SpectrumAsciiFormat : FileOutputFormat
    {
        private AsciiConnector.AsciiFileType fileType;
        private List<string> columns;
        private bool writeFields;

        public AsciiConnector.AsciiFileType FileType
        {
            get { return fileType; }
            set { fileType = value; }
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
            get { return FormatDescriptions.SpectrumAsciiTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.SpectrumAsciiDescription; }
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
            this.fileType = AsciiConnector.AsciiFileType.Tabular;
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
            asc.Format = this.fileType;
            asc.WriteFields = this.writeFields;

            asc.SaveSpectrum(spectrum, Guid.Empty);


            string extension = null;

            switch (fileType)
            {
                case AsciiConnector.AsciiFileType.Tabular:
                    extension = ".dat";
                    break;
                case AsciiConnector.AsciiFileType.CommaSeparated:
                    extension = ".csv";
                    break;
            }

            filename = GetFilenameFromId(spectrum, true, extension);
        }
    }
}

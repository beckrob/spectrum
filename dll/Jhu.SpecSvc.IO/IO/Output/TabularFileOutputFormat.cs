using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public abstract class TabularFileOutputFormat : FileOutputFormat
    {
        private TabularFile.FileType fileType;
        private TabularFile.LineEnding lineEnding;

        public TabularFile.FileType FileType
        {
            get { return this.fileType; }
            set { this.fileType = value; }
        }

        public TabularFile.LineEnding LineEnding
        {
            get { return this.lineEnding; }
            set { this.lineEnding = value; }
        }

        public TabularFileOutputFormat()
        {
            InitializeMembers();
        }

        public TabularFileOutputFormat(TabularFileOutputFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.fileType = TabularFile.FileType.AsciiTabular;
            this.lineEnding = TabularFile.LineEnding.Unix;
        }

        private void CopyMembers(TabularFileOutputFormat old)
        {
            this.fileType = old.fileType;
            this.lineEnding = old.lineEnding;
        }
    }
}

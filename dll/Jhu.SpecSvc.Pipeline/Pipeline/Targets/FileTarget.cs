using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jhu.SpecSvc.SpectrumLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Jhu.SpecSvc.Pipeline.Targets
{
    [Serializable]
    public class FileTarget : OutputTarget
    {
        public enum FileDestination
        {
            Download,
            Ftp,
            VoSpace
        }

        private FileDestination destination;
        private string uri;
        private List<FileOutputFormat> formats;

        public FileDestination Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public List<FileOutputFormat> Formats
        {
            get { return formats; }
            set { formats = value; }
        }

        public FileTarget()
        {
            InitializeMembers();
        }

        public FileTarget(FileTarget old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.destination = FileDestination.Download;
            this.uri = string.Empty;
            this.formats = new List<FileOutputFormat>();
        }

        private void CopyMembers(FileTarget old)
        {
            this.destination = old.destination;
            this.uri = old.uri;
            this.formats = old.formats; //**** list copy?
        }

        public void Execute(IEnumerable<Jhu.SpecSvc.SpectrumLib.Spectrum> spectra, Stream outputStream)
        {
            iteration = 0;
            
            using (Stream zipstream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(outputStream))
            {
                using (
                ICSharpCode.SharpZipLib.Tar.TarOutputStream tar = new ICSharpCode.SharpZipLib.Tar.TarOutputStream(zipstream))
                {
                    foreach (var format in formats)
                    {
                        spectra = format.Execute(tar, spectra, skipExceptions);
                    }

                    foreach (Spectrum spectrum in spectra)
                    {
                        iteration++;
                    }

                    tar.Close();
                }
            }
        }
    }
}

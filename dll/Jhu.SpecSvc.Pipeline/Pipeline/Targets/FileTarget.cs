using System;
using System.Linq;
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
        private Stream outputStream;
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

        public Stream OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
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

        private void OpenOutputStream()
        {
            switch (destination)
            {
                case FileDestination.Download:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void CloseOutputStream()
        {
            switch (destination)
            {
                case FileDestination.Download:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override IEnumerable<Spectrum> Execute(IEnumerable<Spectrum> spectra)
        {
            OpenOutputStream();

            using (Stream zipstream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(outputStream))
            {
                using (var tar = new ICSharpCode.SharpZipLib.Tar.TarOutputStream(zipstream))
                {
                    foreach (var format in formats)
                    {
                        spectra = format.Execute(tar, spectra, SkipExceptions);
                    }

                    foreach (var spectrum in spectra)
                    {
                        Iteration++;
                        yield return spectrum;
                    }

                    tar.Close();
                }
            }

            CloseOutputStream();
        }
    }
}

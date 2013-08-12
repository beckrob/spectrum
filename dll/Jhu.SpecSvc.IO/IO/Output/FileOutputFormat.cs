using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jhu.SpecSvc.SpectrumLib;
using ICSharpCode.SharpZipLib.Tar;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
    [Serializable]
    [XmlInclude(typeof(MagnitudeFormat))]
    [XmlInclude(typeof(SpectrumAsciiFormat))]
    [XmlInclude(typeof(SpectrumPlotFormat))]
    [XmlInclude(typeof(SpectrumVoTableFormat))]
    [XmlInclude(typeof(SpectrumXmlFormat))]
    public abstract class FileOutputFormat
    {
        protected bool active;
        protected List<Exception> exceptions;
        protected string prefix;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        [XmlIgnore]
        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        public abstract string Title
        {
            get;
        }

        public FileOutputFormat()
        {
            InitializeMembers();
        }

        public FileOutputFormat(FileOutputFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.active = true;
            this.exceptions = null;
        }

        private void CopyMembers(FileOutputFormat old)
        {
            this.active = old.active;
            this.exceptions = old.exceptions;
        }

        public virtual void Init(int count, int dim)
        {
        }

        public virtual IEnumerable<Spectrum> Execute(ICSharpCode.SharpZipLib.Tar.TarOutputStream tar, IEnumerable<Spectrum> spectra, bool skipExceptions)
        {
            if (skipExceptions) this.exceptions = new List<Exception>();

            foreach (Spectrum spectrum in spectra)
            {
                try
                {
                    MemoryStream buffer = new MemoryStream();
                    string filename;
                    
                    Execute(spectrum, buffer, out filename);

                    TarEntry entry = TarEntry.CreateTarEntry(filename);

                    entry.ModTime = DateTime.Now;
                    entry.Size = buffer.Length;

                    lock (tar)
                    {
                        tar.PutNextEntry(entry);
                        tar.Write(buffer.ToArray(), 0, (int)buffer.Length);
                        tar.CloseEntry();
                    }
                }
                catch (System.Exception ex)
                {
                    if (skipExceptions)
                    {
                        exceptions.Add(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }

                yield return spectrum;
            }
        }

        protected virtual void Execute(Spectrum spectrum, Stream outputStream, out string filename)
        {
            throw new NotImplementedException();
        }

        protected string GetFilenameFromId(Jhu.SpecSvc.SpectrumLib.Spectrum s, bool includePath, string extension)
        {

            string res = "";

            if (!string.IsNullOrEmpty(prefix))
            {
                res = prefix + "_";
            }

            if (s.Id <= 0)
            {
                res += s.Target.Name.Value + extension;
            }
            else
            {
                res += "spec" + s.Id.ToString();
                res += extension;

                if (includePath)
                {
                    // 6 means ivo://
                    // # trim is a hack here, make sure # is not in the publisher id
                    res = s.PublisherId.Substring(6, s.PublisherId.Length - 6).Trim('#') + "/" + res;
                }
            }

            return res;
        }
    }
}

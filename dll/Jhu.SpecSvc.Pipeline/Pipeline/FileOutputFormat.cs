using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Jhu.SpecSvc.SpectrumLib;
using ICSharpCode.SharpZipLib.Tar;

namespace Jhu.SpecSvc.Pipeline
{
    [Serializable]
    [XmlInclude(typeof(Formats.ContinuumFitFormat))]
    [XmlInclude(typeof(Formats.LineFitFormat))]
    [XmlInclude(typeof(Formats.MagnitudeFormat))]
    [XmlInclude(typeof(Formats.SpectrumAsciiFormat))]
    [XmlInclude(typeof(Formats.SpectrumPlotFormat))]
    [XmlInclude(typeof(Formats.SpectrumVoTableFormat))]
    [XmlInclude(typeof(Formats.SpectrumXmlFormat))]
    public abstract class FileOutputFormat : PipelineObjectBase<Spectrum>
    {
        protected bool active;
        protected string prefix;

        public bool Active
        {
            get { return active; }
            set { active = value; }
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

        public abstract string Description
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
        }

        private void CopyMembers(FileOutputFormat old)
        {
            this.active = old.active;
        }

        public virtual void Init(int count, int dim)
        {
        }

        public virtual IEnumerable<Spectrum> Execute(ICSharpCode.SharpZipLib.Tar.TarOutputStream tar, IEnumerable<Spectrum> spectra, bool skipExceptions)
        {
            foreach (Spectrum spectrum in spectra)
            {
                try
                {
                    var buffer = new MemoryStream();
                    string filename;
                    
                    OnExecute(spectrum, buffer, out filename);

                    var entry = TarEntry.CreateTarEntry(filename);

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
                        LogException(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }

                yield return spectrum;
            }
        }

        protected abstract void OnExecute(Spectrum spectrum, Stream outputStream, out string filename);

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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class MagnitudeFormat : TabularFileOutputFormat
    {
        public enum MagnitudeSystem
        {
            Flux,
            ABMagnitude
        }

        private MagnitudeSystem system;

        public override string Title
        {
            get { return "Synthetic magnitudes"; }
        }

        public MagnitudeSystem System
        {
            get { return system; }
            set { system = value; }
        }

        public MagnitudeFormat()
            : base()
        {
            InitializeMembers();
        }

        public MagnitudeFormat(MagnitudeFormat old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.system = MagnitudeSystem.ABMagnitude;
        }

        private void CopyMembers(MagnitudeFormat old)
        {
            this.system = old.system;
        }

        public override IEnumerable<Spectrum> Execute(ICSharpCode.SharpZipLib.Tar.TarOutputStream tar, IEnumerable<Jhu.SpecSvc.SpectrumLib.Spectrum> spectra, bool skipExceptions)
        {
            MemoryStream[] ms = null;
            TabularFile[] tf = null;

            bool first = true;
            foreach (Spectrum spectrum in spectra)
            {
                if (first)
                {
                    first = false;

                    ms = new MemoryStream[spectrum.Magnitudes.Count];
                    tf = new TabularFile[spectrum.Magnitudes.Count];

                    for (int i = 0; i < spectrum.Magnitudes.Count; i++)
                    {
                        ms[i] = new MemoryStream();
                        tf[i] = new TabularFile(FileType, LineEnding, ms[i]);
                        tf[i].Open();

                        // Write file header
                        List<string> headers = new List<string>() { "SpectrumID", "Redshift" };

                        for (int j = 0; j < spectrum.Magnitudes[i].FilterName.Length; j++)
                        {
                            headers.Add(spectrum.Magnitudes[i].FilterName[j].Replace(' ', '_'));
                        }

                        tf[i].WriteHeader(headers);
                    }
                }

                for (int i = 0; i < spectrum.Magnitudes.Count; i++)
                {
                    for (int z = 0; z < spectrum.Magnitudes[i].Redshift.Length; z++)
                    {
                        // Write data line
                        List<string> data = new List<string>()
                        {
                            spectrum.Curation.PublisherDID.Value, 
                            spectrum.Magnitudes[i].Redshift[z].ToString()
                        };

                        for (int j = 0; j < spectrum.Magnitudes[i].FilterName.Length; j++)
                        {
                            string flux;
                            if (spectrum.Magnitudes[i].Error[j][z] ||
                                double.IsNaN(spectrum.Magnitudes[i].Flux[j][z]) ||
                                double.IsInfinity(spectrum.Magnitudes[i].Flux[j][z]))
                            {
                                flux = (-9999).ToString();
                            }
                            else
                            {
                                switch (system)
                                {
                                    case MagnitudeSystem.Flux:
                                        flux = spectrum.Magnitudes[i].Flux[j][z].ToString();
                                        break;
                                    case MagnitudeSystem.ABMagnitude:
                                        flux = AstroUtil.Flux2ABMagnitude(spectrum.Magnitudes[i].Flux[j][z]).ToString();
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }
                            }
                            data.Add(flux.ToString());
                        }

                        tf[i].WriteData(data);
                    }
                }

                yield return spectrum;
            }

            for (int i = 0; i < ms.Length; i++)
            {
                string filename = "";

                if (!string.IsNullOrEmpty(prefix))
                {
                    filename = prefix + "_";
                }

                filename += string.Format("flux_{0}.dat", i);

                TarEntry entry = TarEntry.CreateTarEntry(filename);

                tf[i].Close();
                ms[i].Flush();
                byte[] buffer = ms[i].ToArray();

                entry.ModTime = DateTime.Now;
                entry.Size = buffer.Length;

                lock (tar)
                {
                    tar.PutNextEntry(entry);
                    tar.Write(buffer, 0, buffer.Length);
                    tar.CloseEntry();
                }
            }

            ms = null;
            tf = null;
        }
    }
}

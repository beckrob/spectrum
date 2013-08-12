#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumOutput.cs,v 1.2 2008/09/11 10:45:00 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/09/11 10:45:00 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using VoServices.SpecSvc.Visualizer;

namespace VoServices.SpecSvc.IO
{
    public class SpectrumOutput
    {
        #region Member variables

        private Stream outputStream;
        private OutputParameters par;
        private SpectrumGraphParameters gpar;

        private XmlSerializer xmlSerializer;

        #endregion
        #region Properties

        public Stream OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
        }

        public OutputParameters Parameters
        {
            get { return par; }
            set { par = value; }
        }

        public SpectrumGraphParameters GraphParameters
        {
            get { return gpar; }
            set { gpar = value; }
        }

        #endregion
        #region Constructors

        public SpectrumOutput()
        {
            InitializeMembers();
        }

        public SpectrumOutput(SpectrumOutput old)
        {
            CopyMembers(old);
        }

        public SpectrumOutput(Stream outputStream, OutputParameters par)
        {
            InitializeMembers();
            this.outputStream = outputStream;
            this.par = par;
        }

        #endregion
        #region Initializer functions

        private void InitializeMembers()
        {
            outputStream = null;
            par = null;
            xmlSerializer = null;
        }

        private void CopyMembers(SpectrumOutput old)
        {
            this.outputStream = old.outputStream;
        }

        #endregion

        public string GetContentType()
        {
            if (par.Zip)
                return OutputParameters.ZipMimeType;
            else if (par.Tar)
                return OutputParameters.TarMimeType;
            else
                return (OutputParameters.FormatMimeTypes[(int)par.Format]);
        }

        public string GetSingleFilename(VoServices.SpecSvc.Lib.Spectrum s, bool includePath)
        {
            if (s.Id <= 0)
            {
                return s.Target.Name.Value + OutputParameters.FormatExtensions[(int)par.Format];
            }
            {
                string res = "spec" + s.Id.ToString();
                res += OutputParameters.FormatExtensions[(int)par.Format];
                //if (par.Tar)
                //    res += OutputParameters.TarExtension;
                //if (par.Zip)
                //    res += OutputParameters.ZipExtension;

                if (includePath)
                {
                    // 6 means ivo://
                    // # trim is a hack here, make sure # is not in the publisher id
                    res = s.PublisherId.Substring(6, s.PublisherId.Length - 6).Trim('#') + "/" + res;
                }

                return res;
            }
        }

        public string GetArchiveFilename()
        {
            string res = "speclist" + DateTime.Now.ToString("yyMMddhhmm");
            if (par.Tar)
                res += OutputParameters.TarExtension;
            if (par.Zip)
                res += OutputParameters.ZipExtension;

            return res;
        }

        private void SerializeSpectrum(VoServices.SpecSvc.Lib.Spectrum s, Stream os)
        {
            switch (par.Format)
            {
                case OutputFormat.Ascii:
                case OutputFormat.Csv:
                    AsciiConnector asc = new AsciiConnector();
                    switch (par.Format)
                    {
                        case OutputFormat.Ascii:
                            asc.Format = AsciiConnector.AsciiFormat.Tabular; break;
                        case OutputFormat.Csv:
                            asc.Format = AsciiConnector.AsciiFormat.CommaSeparated; break;
                    }
                    asc.Columns = par.Columns;
                    asc.OutputStream = new System.IO.StreamWriter(os);
                    asc.SaveSpectrum(s, Guid.Empty);
                    break;
                case OutputFormat.Fits:
                    break;
                case OutputFormat.GraphGif:
                case OutputFormat.GraphJpeg:
                case OutputFormat.GraphPng:
                case OutputFormat.GraphPs:
                    VoServices.SpecSvc.Visualizer.Visualizer vis = new VoServices.SpecSvc.Visualizer.Visualizer();
                    vis.PlotSpectraGraph(gpar, s).Save(os, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case OutputFormat.Html:
                    break;
                case OutputFormat.VoTable:
                    VOTABLE.VOTABLE vt = VoServices.SpecSvc.IO.Mappers.Spectrum2VoTable.MapSpectrum2VoTable(s);
                    lock (xmlSerializer)
                    {
                        xmlSerializer.Serialize(os, vt);
                    }
                    break;
                case OutputFormat.Xml:
                    lock (xmlSerializer)
                    {
                        xmlSerializer.Serialize(os, s);
                    }
                    break;
            }
        }

        public int SerializeSpectra(IEnumerable<VoServices.SpecSvc.Lib.Spectrum> spectra)
        {
            if (!par.Tar) throw new IOException("Serializing multiple spectra requires tar.");

            int count = 0;

            // initialize xml serializer for XML and VOTable output
            switch (par.Format)
            {
                case OutputFormat.Xml:
                    xmlSerializer = new XmlSerializer(typeof(VoServices.SpecSvc.Lib.Spectrum));
                    break;
                case OutputFormat.VoTable:
                    xmlSerializer = new XmlSerializer(typeof(VoServices.VOTABLE.VOTABLE));
                    break;
            }

            // if the results are zipped, pipe through a zip stream
            Stream os = null;
            if (!par.Zip)
                os = outputStream;
            else
                os = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(outputStream);

            using (ICSharpCode.SharpZipLib.Tar.TarOutputStream tar =
                new ICSharpCode.SharpZipLib.Tar.TarOutputStream(os))
            {
                foreach (VoServices.SpecSvc.Lib.Spectrum s in spectra)
                {
                    MemoryStream buffer = new MemoryStream();
                    SerializeSpectrum(s, buffer);

                    ICSharpCode.SharpZipLib.Tar.TarEntry entry =
                        ICSharpCode.SharpZipLib.Tar.TarEntry.CreateTarEntry(GetSingleFilename(s, true));

                    //entry.IsDirectory = false;
                    entry.ModTime = DateTime.Now;
                    entry.Size = buffer.Length;

                    tar.PutNextEntry(entry);
                    tar.Write(buffer.ToArray(), 0, (int)buffer.Length);
                    tar.CloseEntry();

                    count++;
                }

                tar.Finish();
                tar.Flush();
            }

            os.Flush();

            // dispose output stream if it's a zip
            if (par.Zip)
                os.Dispose();

            xmlSerializer = null;

            return count;
        }
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumOutput.cs,v $
        Revision 1.2  2008/09/11 10:45:00  dobos
        Bugfixes and parallel execution added to PortalConnector

        Revision 1.1  2008/01/08 22:01:40  dobos
        Initial checkin


*/
#endregion
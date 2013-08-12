#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumInput.cs,v 1.1 2008/01/08 22:01:37 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:37 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Jhu.SpecSvc.Visualizer;

namespace Jhu.SpecSvc.IO
{
    public class SpectrumInput
    {
        #region Member variables

        private Stream inputStream;
        private InputParameters par;

        private XmlSerializer xmlSerializer;

        #endregion
        #region Properties

        public Stream InputStream
        {
            get { return inputStream; }
            set { inputStream = value; }
        }

        public InputParameters Parameters
        {
            get { return par; }
            set { par = value; }
        }

        #endregion
        #region Constructors

        public SpectrumInput()
        {
            InitializeMembers();
        }

        public SpectrumInput(SpectrumInput old)
        {
            CopyMembers(old);
        }

        public SpectrumInput(Stream inputStream, InputParameters par)
        {
            InitializeMembers();
            this.inputStream = inputStream;
            this.par = par;
        }

        #endregion
        #region Initializer functions

        private void InitializeMembers()
        {
            inputStream = null;
            par = null;
            xmlSerializer = null;
        }

        private void CopyMembers(SpectrumInput old)
        {
            this.inputStream = old.inputStream;
            this.par = new InputParameters(old.par);
        }

        #endregion

        private Jhu.SpecSvc.SpectrumLib.Spectrum DeserializeSpectrum(Stream os)
        {
            Jhu.SpecSvc.SpectrumLib.Spectrum s = null;

            switch (par.Format)
            {
                case InputFormat.Ascii:
                case InputFormat.Csv:
                    AsciiConnector asc = new AsciiConnector();
                    switch (par.Format)
                    {
                        case InputFormat.Ascii:
                            asc.Format = AsciiConnector.AsciiFormat.Tabular; break;
                        case InputFormat.Csv:
                            asc.Format = AsciiConnector.AsciiFormat.CommaSeparated; break;
                    }
                    asc.Columns = par.Columns;
                    asc.InputStream = new System.IO.StreamReader(os);

                    s = asc.GetSpectrum(par.UserGuid, "#0");
                    break;
                case InputFormat.Fits:
                    break;
                case InputFormat.VoTable:
                    VOTABLE.VOTABLE vt = null;
                    lock (xmlSerializer)
                    {
                        vt = (VOTABLE.VOTABLE)xmlSerializer.Deserialize(os);
                    }
                    s = Jhu.SpecSvc.IO.Mappers.VoTable2Spectrum.MapVoTable2Spectrum(vt);
                    break;
                case InputFormat.Xml:
                    lock (xmlSerializer)
                    {
                        s = (Jhu.SpecSvc.SpectrumLib.Spectrum)xmlSerializer.Deserialize(os);
                    }
                    break;
            }

            return s;
        }

        public IEnumerable<Jhu.SpecSvc.SpectrumLib.Spectrum> DeserializeSpectra()
        {
            int count = 0;

            // if the results are zipped, pipe through a zip stream
            Stream os = null;
            if (!par.Zip)
                os = inputStream;
            else
                os = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(inputStream);

            // initialize deserializer

            if (par.Format == InputFormat.VoTable)
            {
                xmlSerializer = new XmlSerializer(typeof(VOTABLE.VOTABLE));
            }
            else if (par.Format == InputFormat.Xml)
            {
                xmlSerializer = new XmlSerializer(typeof(Jhu.SpecSvc.SpectrumLib.Spectrum));
            }

            if (!par.Tar)
            {
                yield return DeserializeSpectrum(os);
            }
            else
            {
                /*
                using (ICSharpCode.SharpZipLib.Tar.TarInputStream tar =
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
                 * */

                yield break;
            }

            // dispose output stream if it's a zip
            if (par.Zip)
                os.Dispose();

            xmlSerializer = null;
        }
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumInput.cs,v $
        Revision 1.1  2008/01/08 22:01:37  dobos
        Initial checkin


*/
#endregion
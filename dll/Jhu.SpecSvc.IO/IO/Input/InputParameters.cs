#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: InputParameters.cs,v 1.1 2008/01/08 22:01:37 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:37 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    [Serializable]
    public class InputParameters
    {
        public static readonly string[] FormatDescptions =
            { "ASCII (tabular)", "CSV", "XML (v3.1)", "VOTable (v1.1)", "FITS" };

        public static readonly string[] FormatExtensions =
            { ".txt", ".csv", ".xml", ".votable.xml", ".fits" };

        public static readonly string[] FormatMimeTypes =
            { "text/plain", "text/plain", "text/xml", "text/xml", "application/octet-stream" };

        public static readonly string[] ColumnNames =
            {
         "Spectral_Value",
         "Spectral_Accuracy_StatError",
         "Spectral_Accuracy_StatErrLow",
         "Spectral_Accuracy_StatErrHigh",
         "Spectral_Accuracy_BinSize",
         "Spectral_Accuracy_BinLow",
         "Spectral_Accuracy_BinHigh",
         "Flux_Value",
         "Flux_Accuracy_StatError",
         "Flux_Accuracy_StatErrLow",
         "Flux_Accuracy_StatErrHigh",
         "Flux_Accuracy_Quality",
         "BackgroundModel_Value",
         "BackgroundModel_Accuracy_StatError",
         "BackgroundModel_Accuracy_StatErrLow",
         "BackgroundModel_Accuracy_StatErrHigh",
         "BackgroundModel_Accuracy_Quality",
         "Time_Value",
         "Time_Accuracy_StatError",
         "Time_Accuracy_BinSize",
         "Time_Accuracy_BinLow",
         "Time_Accuracy_BinHigh"
            };

        public const string ZipMimeType = "application/x-gzip";
        public const string TarMimeType = "application/x-tar";
        public const string ZipExtension = ".gz";
        public const string TarExtension = ".tar";

        private bool loadPoints;
        private bool loadDetails;

        private string[] columns;
        private InputFormat format;
        private bool tar;
        private bool zip;

        private Guid userGuid;

        public bool LoadPoints
        {
            get { return loadPoints; }
            set { loadPoints = value; }
        }

        public bool LoadDetails
        {
            get { return loadDetails; }
            set { loadDetails = value; }
        }

        public string[] Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public InputFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        public bool Tar
        {
            get { return tar; }
            set { tar = value; }
        }

        public bool Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        //---

        public bool IsColumnSelectRequired
        {
            get
            {
                return (this.format == InputFormat.Ascii ||
                    this.format == InputFormat.Csv);
            }
        }

        public InputParameters()
        {
            InitializeMembers();
        }

        public InputParameters(InputParameters old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.loadPoints = true;
            this.loadDetails = true;

            this.columns = new string[] { "Spectral_Value", "Flux_Value", "Flux_Accuracy_StatErrLow", "Flux_Accuracy_StatErrHigh", "Flux_Accuracy_Quality" };
            this.format = InputFormat.Xml;
            this.tar = false;
            this.zip = false;

            this.userGuid = Guid.Empty;
        }

        private void CopyMembers(InputParameters old)
        {
            this.loadPoints = old.loadPoints;
            this.loadDetails = old.loadDetails;

            this.columns = new string[old.columns.Length];
            old.columns.CopyTo(this.columns, 0);
            this.format = old.format;
            this.tar = old.tar;
            this.zip = old.zip;

            this.userGuid = old.userGuid;
        }
    }

    public enum InputFormat
    {
        Ascii,
        Csv,
        Xml,
        VoTable,
        Fits,
        All
    }
}
#region Revision History
/* Revision History

        $Log: InputParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:37  dobos
        Initial checkin


*/
#endregion
#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: OutputParameters.cs,v 1.1 2008/01/08 22:01:39 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:39 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace VoServices.SpecSvc.IO
{
    [Serializable]
    public class OutputParameters
    {
        public static readonly string[] FormatDescptions =
            { "Graph (gif)", "Graph (jpeg)", "Graph (png)", "Graph (ps)",
              "ASCII (tabular)", "CSV", "HTML", "XML (v3.1)", "VOTable (v1.1)", "FITS" };

        public static readonly string[] FormatExtensions =
            { ".gif", ".jpg", ".png", ".ps",
              ".txt", ".csv", ".htm", ".xml", ".votable.xml", ".fits" };

        public static readonly string[] FormatMimeTypes =
            { "image/gif", "image/jpeg", "image/png", "application/postscript",
              "text/plain", "text/plain", "text/html", "text/xml", "text/xml", "application/octet-stream" };

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
        private OutputTarget target;
        private OutputFormat format;
        private OutputType type;
        private bool tar;
        private bool zip;

        private string connectionString; // url for ws admin.asmx
        private string userGuid;         // web service id
        private string password;         // not used
        private int folderId;
        private string namePrefix;
        private int @public;

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

        public OutputTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        public OutputFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        public OutputType Type
        {
            get { return type; }
            set { type = value; }
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

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public string UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int FolderId
        {
            get { return folderId; }
            set { folderId = value; }
        }

        public string NamePrefix
        {
            get { return namePrefix; }
            set { namePrefix = value; }
        }

        public int Public
        {
            get { return @public; }
            set { @public = value; }
        }

        //---

        public bool IsGraphicOutput
        {
            get
            {
                return (this.target == OutputTarget.Display ||
                        this.format == OutputFormat.GraphGif ||
                        this.format == OutputFormat.GraphJpeg ||
                        this.format == OutputFormat.GraphPng ||
                        this.format == OutputFormat.GraphPs);
            }
        }

        public bool IsColumnSelectRequired
        {
            get
            {
                return (this.format == OutputFormat.Ascii ||
                    this.format == OutputFormat.Html ||
                    this.format == OutputFormat.Csv);
            }
        }

        public OutputParameters()
        {
            InitializeMembers();
        }

        public OutputParameters(OutputParameters old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.loadPoints = true;
            this.loadDetails = true;

            this.columns = new string[] { "Spectral_Value", "Flux_Value", "Flux_Accuracy_StatErrLow", "Flux_Accuracy_StatErrHigh", "Flux_Accuracy_Quality" };
            this.target = OutputTarget.Download;
            this.format = OutputFormat.Xml;
            this.type = OutputType.Spectra;
            this.tar = true;
            this.zip = true;

            this.connectionString = string.Empty;
            this.userGuid = string.Empty;
            this.password = string.Empty;
            this.folderId = 0;
            this.namePrefix = string.Empty;
            this.@public = 0;
        }

        private void CopyMembers(OutputParameters old)
        {
            this.loadPoints = old.loadPoints;
            this.loadDetails = old.loadDetails;

            this.columns = new string[old.columns.Length];
            old.columns.CopyTo(this.columns, 0);
            this.target = old.target;
            this.format = old.format;
            this.type = old.type;
            this.tar = old.tar;
            this.zip = old.zip;

            this.connectionString = old.connectionString;
            this.userGuid = old.userGuid;
            this.password = old.password;
            this.folderId = old.folderId;
            this.namePrefix = old.namePrefix;
            this.@public = old.@public;
        }

        public void InitializeFor(string op)
        {
            switch (op)
            {
                case "download":
                case "ws":
                case "graph":
                case "composite":
                case "pca":
                    this.target = OutputTarget.Download;
                    this.format = OutputFormat.Xml;
                    this.type = OutputType.Spectra;
                    break;
                case "convolve":
                    break;
                case "fit":
                    break;
            }
        }
    }

    public enum OutputType
    {
        Graph,
        Spectra,
        FitResults,
        Magnitudes
    }

    public enum OutputTarget
    {
        Display,
        Download,
        MySpectrum,
        VoSpace
    }

    public enum OutputFormat
    {
        GraphGif = 0,
        GraphJpeg,
        GraphPng,
        GraphPs,
        Ascii,
        Csv,
        Html,
        Xml,
        VoTable,
        Fits,
        All
    }
}
#region Revision History
/* Revision History

        $Log: OutputParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:39  dobos
        Initial checkin


*/
#endregion
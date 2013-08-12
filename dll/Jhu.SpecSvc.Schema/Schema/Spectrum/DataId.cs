#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * Jhu.SpecSvc.Schema classes support the implementation
 * of Virtual Observatory Data Models.
 * Jhu.SpecSvc.Schema.Spectrum implements the spectrum data model
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: DataId.cs,v 1.1 2008/01/08 22:26:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:45 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class DataId : Group, ICloneable
    {
        public const string ARCHIVAL = "ARCHIVAL";
        public const string CUTOUT = "CUTOUT";
        public const string DERIVED = "DERIVED";

        public const string UNKNOWN = "UNKNOWN";
        public const string SURVEY = "SURVEY";
        public const string POINTED = "POINTED";
        public const string THEORY = "THEORY";
        public const string ARTIFICAL = "ARTIFICAL";
        public const string COMPOSITE = "COMPOSITE";

        private TextParam title;
        private TextParam creator;
        private TextParam collection;
        private TextParam datasetId;
        private TextParam creatorDID;
        private TimeParam date;
        private TextParam version;
        private TextParam instrument;
        private TextParam bandpass;
        private TextParam creationType;
        private TextParam logo;
        private TextParam contributor;
        private TextParam dataSource;

        private TextParam matchDID;

        /// <summary>
        /// Dataset title
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.title;meta.dataset", RefMember = "Title", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// IVOA Creator ID
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, RefMember = "Creator", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// Collection's (dataset's) name
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, RefMember = "Collection", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        /// <summary>
        /// IVOA Dataset ID
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.id;meta.dataset", RefMember = "DatasetID", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam DatasetId
        {
            get { return datasetId; }
            set { datasetId = value; }
        }

        /// <summary>
        /// Creator's ID for the spectrum
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.id", RefMember = "CreatorDID", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam CreatorDID
        {
            get { return creatorDID; }
            set { creatorDID = value; }
        }

        /// <summary>
        /// Data processing / creation time
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional | ParamRequired.SpecService, Ucd = "time;meta.dataset", RefMember = "Date", ReferenceMode = ReferenceMode.ArrayItem)]
        public TimeParam Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Version of dataset
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional | ParamRequired.SpecService, Ucd = "meta.version;meta.dataset", RefMember = "Version", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// Instrument ID
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.id;instr", RefMember = "Instrument", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Instrument
        {
            get { return instrument; }
            set { instrument = value; }
        }

        /// <summary>
        /// Band, consistent with RSM Coverage.Spectral
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "instr.bandpass", RefMember = "Bandpass", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Bandpass
        {
            get { return bandpass; }
            set { bandpass = value; }
        }

        /// <summary>
        /// Dataset creation type: ARCHIVAL, CUTOUT, DERIVED 
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional | ParamRequired.SpecService, DefaultValue = DataId.ARCHIVAL, RefMember = "CreationType", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam CreationType
        {
            get { return creationType; }
            set { creationType = value; }
        }

        /// <summary>
        /// URL for creator logo
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.ref.url")]
        public TextParam Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        /// <summary>
        /// Contributor (may be many)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional)]
        public TextParam Contributor
        {
            get { return contributor; }
            set { contributor = value; }
        }

        /// <summary>
        /// Original data type: UNKNOWN,SURVEY, POINTED, THEORY, ARTIFICAL, COMPOSITE
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, DefaultValue = DataId.UNKNOWN, RefMember = "DataSource", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// ID assigned by user when running queries
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "meta.ref.url;meta.curation", RefMember = "MatchDID", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam MatchDID
        {
            get { return matchDID; }
            set { matchDID = value; }
        }

        #region Constructors
        public DataId()
        {
        }

        public DataId(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public DataId(DataId old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static DataId Clone(DataId old)
        {
            if (old != null)
            {
                return new DataId(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: DataId.cs,v $
        Revision 1.1  2008/01/08 22:26:45  dobos
        Initial checkin


*/
#endregion
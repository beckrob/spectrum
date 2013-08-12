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
 *   ID:          $Id: Curation.cs,v 1.1 2008/01/08 22:26:16 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:16 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class Curation : Group, ICloneable
    {
        public const string PUBLIC = "PUBLIC";
        public const string PROPRIETARY = "PROPRIETARY";
        public const string MIXED = "MIXED";

        /// <summary>
        /// Name of the publisher
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Mandatory, Ucd = "meta.curation", RefMember = "Publisher", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Publisher;

        /// <summary>
        /// IVOA ID of publisher
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.ref.url;meta.curation")]
        public TextParam PublisherID;

        /// <summary>
        /// Date curated dataset last modified
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, RefMember = "PublisherDate", ReferenceMode = ReferenceMode.ArrayItem)]
        public TimeParam Date;

        /// <summary>
        /// Version info
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.version;meta.curation", RefMember = "PublisherVersion", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Version;

        /// <summary>
        /// Restrictions: PUBLIC, PROPRIETARY, MIXED
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Recommended, DefaultValue = Curation.PUBLIC, RefMember = "Rights", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Rights;

        /// <summary>
        /// URL or Bibcode for documentation
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Recommended, Ucd = "meta.bib.bibcode", RefMember = "Reference", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam Reference;

        [XmlElement]
        [Field(Required = ParamRequired.Optional)]
        public Contact Contact;

        /// <summary>
        /// Publishers internal database ID (primary key)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "meta.ref.url;meta.curation", RefMember = "PublisherDID", ReferenceMode = ReferenceMode.ArrayItem)]
        public TextParam PublisherDID;

        #region Constructors
        public Curation()
        {
        }

        public Curation(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Curation(Curation old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Curation Clone(Curation old)
        {
            if (old != null)
            {
                return new Curation(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Curation.cs,v $
        Revision 1.1  2008/01/08 22:26:16  dobos
        Initial checkin


*/
#endregion
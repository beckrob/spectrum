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
 *   ID:          $Id: Contact.cs,v 1.1 2008/01/08 22:26:16 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:16 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    public class Contact: Group, ICloneable
    {
        /// <summary>
        /// Contact name
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.bib.author;meta.curation")]
        public TextParam ContactName;

        /// <summary>
        /// Contact email
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.ref.url;meta.email")]
        public TextParam ContactEmail;

#region Constructors
        public Contact()
        {
        }

        public Contact(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Contact(Contact old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Contact Clone(Contact old)
        {
            if (old != null)
            {
                return new Contact(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Contact.cs,v $
        Revision 1.1  2008/01/08 22:26:16  dobos
        Initial checkin


*/
#endregion
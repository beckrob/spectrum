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
 *   ID:          $Id: Redshift.cs,v 1.1 2008/01/08 22:26:46 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:46 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class Redshift : Group, ICloneable
	{
		private DoubleParam value;
		private DoubleParam statError;
		private DoubleParam confidence;

        /// <summary>
        /// Measured redshift for the spectrum
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService)]
		public DoubleParam Value
		{
			get { return value; }
			set { this.value = value; }
		}

        /// <summary>
        /// Error on measured redshift
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService, Ucd = "stat.error;src.redshift")]
		public DoubleParam StatError
		{
			get { return statError; }
			set { statError = value; }
		}

        /// <summary>
        /// Confidence value for redshift
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService)]
		public DoubleParam Confidence
		{
			get { return confidence; }
			set { confidence = value; }
		}

#region Constructors
        public Redshift()
        {
        }

        public Redshift(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Redshift(Redshift old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Redshift Clone(Redshift old)
        {
            if (old != null)
            {
                return new Redshift(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Redshift.cs,v $
        Revision 1.1  2008/01/08 22:26:46  dobos
        Initial checkin


*/
#endregion
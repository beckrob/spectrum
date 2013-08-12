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
    public class VelocityDispersion : Group, ICloneable
	{
		private DoubleParam value;
		private DoubleParam statError;
		private DoubleParam confidence;

        /// <summary>
        /// Measured velocity dispersion for the spectrum
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService)]
		public DoubleParam Value
		{
			get { return value; }
			set { this.value = value; }
		}

        /// <summary>
        /// Error on measured velocity dispersion
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService, Ucd = "stat.error")]
		public DoubleParam StatError
		{
			get { return statError; }
			set { statError = value; }
		}

        /// <summary>
        /// Confidence value for velocity dispersion
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional | ParamRequired.SpecService)]
		public DoubleParam Confidence
		{
			get { return confidence; }
			set { confidence = value; }
		}

#region Constructors
        public VelocityDispersion()
        {
        }

        public VelocityDispersion(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public VelocityDispersion(VelocityDispersion old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static VelocityDispersion Clone(VelocityDispersion old)
        {
            if (old != null)
            {
                return new VelocityDispersion(old);
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
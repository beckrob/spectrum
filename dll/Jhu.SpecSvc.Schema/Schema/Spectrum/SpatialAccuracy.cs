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
 *   ID:          $Id: SpatialAccuracy.cs,v 1.1 2008/01/08 22:26:47 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:47 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpatialAccuracy : Group, ICloneable, IAccuracy
	{
		private DoubleParam statError;
		private DoubleParam sysError;

        // ----- Characterization

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.BinSize
        {
            get
            {
                return null;
            }
            set { }
        }

        // -----------------------

        /// <summary>
        /// Astrometric statistical error
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "pos.eq;stat.error", DefaultUnit = SpatialAxis.COMMONUNIT)]
		public DoubleParam StatError
		{
			get { return statError; }
			set { statError = value; }
		}

        /// <summary>
        /// Astrometric systematic error
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "pos.eq;stat.error.sys", DefaultUnit = SpatialAxis.COMMONUNIT)]
		public DoubleParam SysError
		{
			get { return sysError; }
			set { sysError = value; }
		}

		#region Constructors
        public SpatialAccuracy()
        {
        }

        public SpatialAccuracy(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpatialAccuracy(SpatialAccuracy old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpatialAccuracy Clone(SpatialAccuracy old)
        {
            if (old != null)
            {
                return new SpatialAccuracy(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: SpatialAccuracy.cs,v $
        Revision 1.1  2008/01/08 22:26:47  dobos
        Initial checkin


*/
#endregion
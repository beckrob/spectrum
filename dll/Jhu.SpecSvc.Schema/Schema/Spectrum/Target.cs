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
 *   ID:          $Id: Target.cs,v 1.1 2008/01/08 22:26:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:54 $
 */
#endregion
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public partial class Target : Group, ICloneable
	{
        public const string UNKNOWN = "UNKNOWN";
        public const string GALAXY = "GALAXY";
        public const string STAR = "STAR";
        public const string QSO = "QSO";
        public const string SKY = "SKY";

		private TextParam name;
		private TextParam description;
		private TextParam @class;
		private TextParam spectralClass;
		private DoubleParam redshift;
		private PositionParam pos;
		private DoubleParam varAmpl;

        /// <summary>
        /// Target name
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.Mandatory, Ucd = "meta.id;src", RefMember = "TargetName", ReferenceMode = ReferenceMode.ArrayItem)]
		public TextParam Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// Target descriptive text
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "meta.note;src")]
		public TextParam Description
		{
			get { return description; }
			set { description = value; }
		}
		
        /// <summary>
        /// Target object class
        /// </summary>
		// Have to be modified to comply with Java Axis2
        [XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "src.class", RefMember = "TargetClass", ReferenceMode = ReferenceMode.ArrayItem)]
		public TextParam Class
		{
			get { return @class; }
			set { @class = value; }
		}

        /// <summary>
        /// Object spectral class
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.SpecService, Ucd = "src.spType")]
		public TextParam SpectralClass
		{
			get { return spectralClass; }
			set { spectralClass = value; }
		}
		
        /// <summary>
        /// Target redshift (keep as is if rest-frame!)
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "src.redshift", RefMember = "TargetRedshift", ReferenceMode = ReferenceMode.ArrayItem)]
		public DoubleParam Redshift
		{
			get { return redshift; }
			set { redshift = value; }
		}
		
        /// <summary>
        /// Target RA and Dec
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "pos.eq;src", RefMember = "TargetPos", ReferenceMode = ReferenceMode.ArrayItem)]
		public PositionParam Pos
		{
			get { return pos; }
			set { pos = value; }
		}
		
        /// <summary>
        /// Target variability amplitude, typical
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "src.var.amplitude", RefMember = "TargetVarAmpl", ReferenceMode = ReferenceMode.ArrayItem)]
		public DoubleParam VarAmpl
		{
			get { return varAmpl; }
			set { varAmpl = value; }
		}

		#region Constructors
        public Target()
        {
        }

        public Target(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Target(Target old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Target Clone(Target old)
        {
            if (old != null)
            {
                return new Target(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Target.cs,v $
        Revision 1.1  2008/01/08 22:26:54  dobos
        Initial checkin


*/
#endregion
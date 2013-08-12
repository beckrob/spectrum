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
 *   ID:          $Id: Environment.cs,v 1.1 2008/10/14 13:29:22 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/10/14 13:29:22 $
 */
#endregion
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public partial class Environment : Group, ICloneable
	{
        private DoubleParam airmass;
        private DoubleParam seeing20;
        private DoubleParam seeing50;
        private DoubleParam seeing80;
        private DoubleParam airTemperature;
        private DoubleParam airPressure;
        private DoubleParam airHumidity;

		[XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
		public DoubleParam Airmass
		{
			get { return airmass; }
			set { airmass = value; }
		}

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam Seeing20
        {
            get { return seeing20; }
            set { seeing20 = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam Seeing50
        {
            get { return seeing50; }
            set { seeing50 = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam Seeing80
        {
            get { return seeing80; }
            set { seeing80 = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam AirTemperature
        {
            get { return airTemperature; }
            set { airTemperature = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam AirPressure
        {
            get { return airPressure; }
            set { airPressure = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "")]
        public DoubleParam AirHumidity
        {
            get { return airHumidity; }
            set { airHumidity = value; }
        }

		#region Constructors
        public Environment()
        {
        }

        public Environment(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Environment(Environment old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Environment Clone(Environment old)
        {
            if (old != null)
            {
                return new Environment(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Environment.cs,v $
        Revision 1.1  2008/10/14 13:29:22  dobos
        Environment data added to schema.

        Revision 1.1  2008/01/08 22:26:54  dobos
        Initial checkin


*/
#endregion
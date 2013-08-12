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
 *   ID:          $Id: SpaceFrame.cs,v 1.1 2008/01/08 22:26:21 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:21 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class SpaceFrame : Group, ICloneable
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string CUSTOM = "CUSTOM";
        public const string AZ_EL = "AZ_EL";
        public const string BODY = "BODY";
        public const string ICRS = "ICRS";
        public const string FK4 = "FK4";
        public const string FK5 = "FK5";
        public const string ECLIPTIC = "ECLIPTIC";
        public const string GALACTIC_I = "GALACTIC_I";
        public const string GALACTIC_II = "GALACTIC_II";
        public const string SUPER_GALACTIC = "SUPER_GALACTIC";
        public const string MAG = "MAG";
        public const string GSE = "GSE";
        public const string GSM = "GSM";
        public const string SM = "SM";
        public const string HGC = "HGC";
        public const string HEE = "HEE";
        public const string HEEQ = "HEEQ";
        public const string HCI = "HCI";
        public const string HCD = "HCD";
        public const string GEO_C = "GEO_C";
        public const string GEO_D = "GEO_D";
        public const string MERCURY_C = "MERCURY_C";
        public const string VENUS_C = "VENUS_C";
        public const string LUNA_C = "LUNA_C";
        public const string MARS_C = "MARS_C";
        public const string JUPITER_C_III = "JUPITER_C_III";
        public const string SATURN_C_III = "SATURN_C_III";
        public const string URANUS_C_III = "URANUS_C_III";
        public const string NEPTUNE_C_III = "NEPTUNE_C_III";
        public const string PLUTO_C = "PLUTO_C";
        public const string MERCURY_G = "MERCURY_G";
        public const string VENUS_G = "VENUS_G";
        public const string LUNA_G = "LUNA_G";
        public const string MARS_G = "MARS_G";
        public const string JUPITER_G_III = "JUPITER_G_III";
        public const string SATURN_G_III = "SATURN_G_III";
        public const string URANUS_G_III = "URANUS_G_III";
        public const string NEPTUNE_G_III = "NEPTUNE_G_III";
        public const string PLUTO_G = "PLUTO_G";

        /// <summary>
        /// Name of space frame, ICRS or FK5
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Recommended, DefaultValue = SpaceFrame.ICRS, RefMember="SpaceFrameName", ReferenceMode=ReferenceMode.ArrayItem)]
        public TextParam Name;

        /// <summary>
        /// Space frame UCD
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional)]
        public TextParam UCD;

        /// <summary>
        /// Origin of SpaceFrame
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, DefaultValue = SpaceFrame.UNKNOWN)]
        public TextParam RefPos;

        /// <summary>
        /// Equinox
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time.equinox;pos.frame", DefaultValue = 2000.0, RefMember="SpaceFrameEquinox", ReferenceMode=ReferenceMode.ArrayItem)]
        public DoubleParam Equinox;

#region Constructors
        public SpaceFrame()
        {
        }

        public SpaceFrame(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpaceFrame(SpaceFrame old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpaceFrame Clone(SpaceFrame old)
        {
            if (old != null)
            {
                return new SpaceFrame(old);
            }
            else
                return null;
        }
        #endregion

    }
}
#region Revision History
/* Revision History

        $Log: SpaceFrame.cs,v $
        Revision 1.1  2008/01/08 22:26:21  dobos
        Initial checkin


*/
#endregion
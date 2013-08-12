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
 *   ID:          $Id: CoordSys.cs,v 1.1 2008/01/08 22:26:44 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:44 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class CoordSys : Group, ICloneable
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string RELOCATABLE = "RELOCATABLE";
        public const string CUSTOM = "CUSTOM";
        public const string TOPOCENTER = "TOPOCENTER";
        public const string BARYCENTER = "BARYCENTER";
        public const string HELIOCENTER = "HELIOCENTER";
        public const string GEOCENTER = "GEOCENTER";
        public const string EMBARYCENTER = "EMBARYCENTER";
        public const string MOON = "MOON";
        public const string MERCURY = "MERCURY";
        public const string VENUS = "VENUS";
        public const string MARS = "MARS";
        public const string JUPITER = "JUPITER";
        public const string SATURN = "SATURN";
        public const string URANUS = "URANUS";
        public const string NEPTUNE = "NEPTUNE";
        public const string PLUTO = "PLUTO";
        public const string LSRK = "LSRK";
        public const string LSRD = "LSRD";
        public const string GALACTIC_CENTER = "GALACTIC_CENTER";
        public const string LOCAL_GROUP_CENTER = "LOCAL_GROUP_CENTER";

		private TextParam id;
		private SpaceFrame spaceFrame;
		private TimeFrame timeFrame;
		private SpectralFrame spectralFrame;
		private RedshiftFrame redshiftFrame;

        /// <summary>
        /// ID String for coordinate system
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TextParam ID
		{
			get { return id; }
			set { id = value; }
		}

		[Field(Required = ParamRequired.Recommended)]
		public SpaceFrame SpaceFrame
		{
			get { return spaceFrame; }
			set { spaceFrame = value; }
		}

		[Field(Required = ParamRequired.Optional)]
		public TimeFrame TimeFrame
		{
			get { return timeFrame; }
			set { timeFrame = value; }
		}

		[Field(Required = ParamRequired.Optional)]
		public SpectralFrame SpectralFrame
		{
			get { return spectralFrame; }
			set { spectralFrame = value; }
		}

		[Field(Required = ParamRequired.Optional)]
		public RedshiftFrame RedshiftFrame
		{
			get { return redshiftFrame; }
			set { redshiftFrame = value; }
		}

		#region Constructors
        public CoordSys()
        {
        }

        public CoordSys(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public CoordSys(CoordSys old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static CoordSys Clone(CoordSys old)
        {
            if (old != null)
            {
                return new CoordSys(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: CoordSys.cs,v $
        Revision 1.1  2008/01/08 22:26:44  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: TimeFrame.cs,v 1.1 2008/01/08 22:26:56 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:56 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    [Serializable]
    public class TimeFrame : Group, ICloneable
    {
        public const string LOCAL = "LOCAL";
        public const string TT = "TT";
        public const string UTC = "UTC";
        public const string ET = "ET";
        public const string TDB = "TDB";
        public const string TCG = "TCG";
        public const string TCB = "TCB";
        public const string TAI = "TAI";
        public const string LST = "LST";

		private TextParam name;
		private TextParam ucd;
		private DoubleParam zero;
		private TextParam refPos;

        /// <summary>
        /// Timescale
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time.scale", DefaultValue = TimeFrame.TT, RefMember = "TimeFrameName", ReferenceMode = ReferenceMode.ArrayItem)]
		public TextParam Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// Time frame UCD
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, DefaultValue = "time")]
		public TextParam UCD
		{
			get { return ucd; }
			set { ucd = value; }
		}

        /// <summary>
        /// Zero point of timescale in MJD
        /// </summary>
		[XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;arith.zp", DefaultValue = 0.0, RefMember = "TimeFrameZero", ReferenceMode = ReferenceMode.ArrayItem)]
		public DoubleParam Zero
		{
			get { return zero; }
			set { zero = value; }
		}

        /// <summary>
        /// Times of photon arrival are at this location
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "time.scale", DefaultValue = CoordSys.TOPOCENTER)]
		public TextParam RefPos
		{
			get { return refPos; }
			set { refPos = value; }
		}

#region Constructors
        public TimeFrame()
        {
        }

        public TimeFrame(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeFrame(TimeFrame old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeFrame Clone(TimeFrame old)
        {
            if (old != null)
            {
                return new TimeFrame(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeFrame.cs,v $
        Revision 1.1  2008/01/08 22:26:56  dobos
        Initial checkin


*/
#endregion
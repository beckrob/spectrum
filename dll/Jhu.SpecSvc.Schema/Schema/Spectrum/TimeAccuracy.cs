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
 *   ID:          $Id: TimeAccuracy.cs,v 1.1 2008/01/08 22:26:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:54 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class TimeAccuracy : Group, ICloneable, IAccuracy
    {
        private DoubleParam binSize;
        private DoubleParam binLow;
        private DoubleParam binHigh;
        private DoubleParam statError;
        private DoubleParam statErrLow;
        private DoubleParam statErrHigh;
        private DoubleParam sysError;

        // ----- Characterization

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.BinSize
        {
            get { return binSize; }
            set { binSize = value; }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.StatError
        {
            get { return statError; }
            set { statError = value; }
        }
        // -----------------------

        /// <summary>
        /// Time bin size
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time.interval", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_BinSize", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinSize
        {
            get { return binSize; }
            set { binSize = value; }
        }

        /// <summary>
        /// Time coord bin lower end
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.min", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_BinLow", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinLow
        {
            get { return binLow; }
            set { binLow = value; }
        }

        /// <summary>
        /// Time coord bin upper end
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.max", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_BinHigh", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinHigh
        {
            get { return binHigh; }
            set { binHigh = value; }
        }

        /// <summary>
        /// Time coord measurement error (symetric, plus/minus)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.error", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_StatError", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatError
        {
            get { return statError; }
            set { statError = value; }
        }

        /// <summary>
        /// Time coord measurement error (lower bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.error;stat.min", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_StatErrLow", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrLow
        {
            get { return statErrLow; }
            set { statErrLow = value; }
        }

        /// <summary>
        /// Time coord measurement error (upper bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.error;stat.max", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Accuracy_StatErrHigh", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrHigh
        {
            get { return statErrHigh; }
            set { statErrHigh = value; }
        }

        /// <summary>
        /// Time coord measurement systematic error
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time;stat.error.sys", DefaultUnit = TimeAxis.COMMONUNIT)]
        public DoubleParam SysError
        {
            get { return sysError; }
            set { sysError = value; }
        }

        #region Constructors
        public TimeAccuracy()
        {
        }

        public TimeAccuracy(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeAccuracy(TimeAccuracy old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeAccuracy Clone(TimeAccuracy old)
        {
            if (old != null)
            {
                return new TimeAccuracy(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeAccuracy.cs,v $
        Revision 1.1  2008/01/08 22:26:54  dobos
        Initial checkin


*/
#endregion
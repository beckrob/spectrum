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
 *   ID:          $Id: TimeAxis.cs,v 1.1 2008/01/08 22:26:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:54 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class TimeAxis : Axis, ICloneable, IAxis
    {
        public const string COMMONUNIT = "s";
        public const string SIUNIT = "s";

        private DoubleParam value;
        private TimeAccuracy accuracy;
        private TimeCoverage coverage;
        private DoubleParam resolution;
        private TextParam calibration;
        private TimeSamplingPrecision samplingPrecision;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "TimeAxis";
                res.Ucd = "meta.id";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        public TextParam Ucd
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = value.Ucd;
                res.Ucd = "meta.ucd";

                return res;
            }
            set { }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        public TextParam Unit
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = value.Unit;
                res.Ucd = "meta.unit";
                return res;
            }
            set { }
        }

        /// <summary>
        /// Temporal resolution FWHM
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time.resolution")]
        public DoubleParam Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        [XmlIgnore]
        DoubleParam IAxis.ResPower
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Type of spectra coord calibration
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.code.qual", DefaultValue = CALIBRATED, SerializationMode = SerializationMode.Characterization)]
        public TextParam Calibration
        {
            get { return calibration; }
            set { calibration = value; }
        }

        /// <summary>
        /// Time coord for points
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "time", DefaultUnit = TimeAxis.COMMONUNIT, RefMember = "Time_Value", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional)]
        public TimeAccuracy Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        IAccuracy IAxis.Accuracy
        {
            get { return accuracy; }
            set { accuracy = (TimeAccuracy)value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode = SerializationMode.Characterization)]
        public TimeCoverage Coverage
        {
            get { return coverage; }
            set { coverage = value; }
        }

        ICoverage IAxis.Coverage
        {
            get { return coverage; }
            set { coverage = (TimeCoverage)value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode = SerializationMode.Characterization)]
        public TimeSamplingPrecision SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = value; }
        }

        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = (TimeSamplingPrecision)value; }
        }

        #region Constructors
        public TimeAxis()
        {
        }

        public TimeAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeAxis(TimeAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeAxis Clone(TimeAxis old)
        {
            if (old != null)
            {
                return new TimeAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:54  dobos
        Initial checkin


*/
#endregion
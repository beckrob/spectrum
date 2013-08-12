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
 *   ID:          $Id: SpatialAxis.cs,v 1.1 2008/01/08 22:26:48 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:48 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpatialAxis : Axis, ICloneable, IAxis
	{
        public const string COMMONUNIT = "deg";
        public const string SIUNIT = "deg";

        private DoubleParam resolution;
        private TextParam calibration;
		private SpatialAccuracy accuracy;
		private SpatialCoverage coverage;
        private SpatialSamplingPrecision samplingPrecision;

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "SpatialAxis";
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
                res.Value = coverage.Location.Value.Ucd;
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
                res.Value = coverage.Location.Value.Unit;
                res.Ucd = "meta.unit";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        DoubleParam IAxis.ResPower
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Type of astrometric calibration
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.code.qual", DefaultValue = CALIBRATED, SerializationMode = SerializationMode.Characterization)]
        public TextParam Calibration
        {
            get { return calibration; }
            set { calibration = value; }
        }

		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public SpatialAccuracy Accuracy
		{
			get { return accuracy; }
			set { accuracy = value; }
		}

		IAccuracy IAxis.Accuracy
		{
			get { return accuracy; }
			set { accuracy = (SpatialAccuracy)value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.SpecService, SerializationMode=SerializationMode.Characterization)]
		public SpatialCoverage Coverage
		{
			get { return coverage; }
			set { coverage = value; }
		}

		ICoverage IAxis.Coverage
		{
			get { return coverage; }
			set { coverage = (SpatialCoverage)value; }
		}

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode=SerializationMode.Characterization)]
        public SpatialSamplingPrecision SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = value; }
        }

        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = (SpatialSamplingPrecision)value; }
        }

		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "pos.angResolution")]
		public DoubleParam Resolution
		{
			get { return resolution; }
			set { resolution = value; }
		}

#region Constructors
        public SpatialAxis()
        {
        }

        public SpatialAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpatialAxis(SpatialAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpatialAxis Clone(SpatialAxis old)
        {
            if (old != null)
            {
                return new SpatialAxis(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: SpatialAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:48  dobos
        Initial checkin


*/
#endregion
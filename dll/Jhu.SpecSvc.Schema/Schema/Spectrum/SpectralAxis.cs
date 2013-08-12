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
 *   ID:          $Id: SpectralAxis.cs,v 1.1 2008/01/08 22:26:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:50 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpectralAxis : Axis, IAxis, ICloneable
    {
        public const string COMMONUNIT = "A";
        public const string SIUNIT = "m";

        private DoubleParam resolution;
        private DoubleParam resPower;
        private TextParam calibration;
        private DoubleParam value;
        private SpectralAccuracy accuracy;
        private SpectralCoverage coverage;
        private SpectralSamplingPrecision samplingPrecision;

        #region Characterization
        // --------------------------------------------------------------------------

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization)]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "SpectralAxis";
                res.Ucd = "meta.id";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization | SerializationMode.Data)]
        TextParam IAxis.Ucd
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
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization | SerializationMode.Data)]
        TextParam IAxis.Unit
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
        /// Type of spectra coord calibration
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd="meta.code.qual", DefaultValue = CALIBRATED, SerializationMode = SerializationMode.Characterization)]
        public TextParam Calibration
        {
            get { return calibration; }
            set { calibration = value; }
        }

        /// <summary>
        /// Spectral resolution FWHM
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "spect.resolution;em.wl", SerializationMode = SerializationMode.Characterization | SerializationMode.Data)]
        public DoubleParam Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        /// <summary>
        /// Spectral resolving power
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.SpecService, Ucd = "spect.resolution", SerializationMode = SerializationMode.Characterization)]
        public DoubleParam ResPower
        {
            get { return resPower; }
            set { resPower = value; }
        }

        [XmlIgnore]
        IAccuracy IAxis.Accuracy
        {
            get { return accuracy; }
            set { accuracy = (SpectralAccuracy)value; }
        }

        [XmlIgnore]
        ICoverage IAxis.Coverage
        {
            get { return coverage; }
            set { coverage = (SpectralCoverage)value; }
        }

        [XmlIgnore]
        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = (SpectralSamplingPrecision)value; }
        }

        // -------------------------------------------------------------------------------------
        #endregion

        /// <summary>
        /// Spectral coord for points
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Mandatory, Ucd = "em.wl", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Value", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Recommended)]
        public SpectralAccuracy Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Mandatory, SerializationMode = SerializationMode.Characterization)]
        public SpectralCoverage Coverage
        {
            get { return coverage; }
            set { coverage = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode = SerializationMode.Characterization)]
        public SpectralSamplingPrecision SamplingPrecision
        {
            get { return samplingPrecision; }
            set { samplingPrecision = value; }
        }

        #region Constructors
        public SpectralAxis()
        {
        }

        public SpectralAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralAxis(SpectralAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralAxis Clone(SpectralAxis old)
        {
            if (old != null)
            {
                return new SpectralAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:50  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: FluxAxis.cs,v 1.1 2008/01/08 22:26:46 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:46 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class FluxAxis : Axis, ICloneable, IAxis
    {
        public const string COMMONUNIT = "10e-17 erg s-1 cm-2 A-1";
        public const string SIUNIT = "W m-2 m-1";

        private TextParam calibration;
        private DoubleParam value;
        private FluxAccuracy accuracy;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "FluxAxis";
                res.Ucd = "meta.id";
                return res;
            }
            set { }
        }

        [XmlIgnore]
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

        [XmlIgnore]
        DoubleParam IAxis.Resolution
        {
            get { return null; }
            set { }
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
        [Field(Required = ParamRequired.SpecService, Ucd = "meta.code.qual", DefaultValue = CALIBRATED, SerializationMode = SerializationMode.Characterization)]
        public TextParam Calibration
        {
            get { return calibration; }
            set { calibration = value; }
        }

        /// <summary>
        /// Flux values for points
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Mandatory, Ucd = "phot.flux.density;em.wl", DefaultUnit = FluxAxis.COMMONUNIT, RefMember = "Flux_Value", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        public TextParam Ucd
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = value.Ucd;
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
                return res;
            }
            set { }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Recommended)]
        public FluxAccuracy Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        IAccuracy IAxis.Accuracy
        {
            get { return accuracy; }
            set { accuracy = (FluxAccuracy)value; }
        }

        ICoverage IAxis.Coverage
        {
            get { return null; }
            set { }
        }

        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return null; }
            set { }
        }

        #region Constructors
        public FluxAxis()
        {
        }

        public FluxAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public FluxAxis(FluxAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static FluxAxis Clone(FluxAxis old)
        {
            if (old != null)
            {
                return new FluxAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: FluxAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:46  dobos
        Initial checkin


*/
#endregion
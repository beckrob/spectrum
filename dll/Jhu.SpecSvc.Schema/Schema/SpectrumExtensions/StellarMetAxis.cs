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
 *   ID:          $Id: StellarMetAxis.cs,v 1.1 2008/01/08 22:26:41 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:41 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class StellarMetAxis : Axis, ICloneable, IAxis
    {
        public const string COMMONUNIT = "";
        public const string SIUNIT = "";

        private DoubleParam value;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "StellarMetAxis";
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

        [XmlIgnore]
        TextParam IAxis.Calibration
        {
            get { return null; }
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
        /// Star formation rate
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Custom, DefaultUnit = StellarMetAxis.COMMONUNIT, RefMember = "StellarMet_Value", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam Value
        {
            get { return value; }
            set { this.value = value; }
        }

        IAccuracy IAxis.Accuracy
        {
            get { return null; }
            set {  }
        }

        ICoverage IAxis.Coverage
        {
            get { return null; }
            set {  }
        }

        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return null; }
            set {  }
        }

        #region Constructors
        public StellarMetAxis()
        {
        }

        public StellarMetAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public StellarMetAxis(StellarMetAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static StellarMetAxis Clone(StellarMetAxis old)
        {
            if (old != null)
            {
                return new StellarMetAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: StellarMetAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:41  dobos
        Initial checkin


*/
#endregion
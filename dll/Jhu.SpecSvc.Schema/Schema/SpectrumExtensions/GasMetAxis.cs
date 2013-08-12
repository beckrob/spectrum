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
 *   ID:          $Id: GasMetAxis.cs,v 1.1 2008/01/08 22:26:40 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:40 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class GasMetAxis : Axis, ICloneable, IAxis
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
                res.Value = "GasMetAxis";
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
        [Field(Required = ParamRequired.Custom, DefaultUnit = GasMetAxis.COMMONUNIT, RefMember = "GasMet_Value", ReferenceMode = ReferenceMode.Item)]
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
        public GasMetAxis()
        {
        }

        public GasMetAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public GasMetAxis(GasMetAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static GasMetAxis Clone(GasMetAxis old)
        {
            if (old != null)
            {
                return new GasMetAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: GasMetAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:40  dobos
        Initial checkin


*/
#endregion
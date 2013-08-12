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
 *   ID:          $Id: SfrAxis.cs,v 1.1 2008/01/08 22:26:41 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:41 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SfrAxis : Axis, ICloneable, IAxis
    {
        public const string COMMONUNIT = "M_sol";
        public const string SIUNIT = "M_sol";

        private DoubleParam value;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "SfrAxis";
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
        [Field(Required = ParamRequired.Custom, DefaultUnit = SfrAxis.COMMONUNIT, RefMember = "Sfr_Value", ReferenceMode = ReferenceMode.Item)]
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
        public SfrAxis()
        {
        }

        public SfrAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SfrAxis(SfrAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SfrAxis Clone(SfrAxis old)
        {
            if (old != null)
            {
                return new SfrAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SfrAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:41  dobos
        Initial checkin


*/
#endregion
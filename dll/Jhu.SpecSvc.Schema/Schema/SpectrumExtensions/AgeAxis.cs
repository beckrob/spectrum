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
 *   ID:          $Id: AgeAxis.cs,v 1.1 2008/01/08 22:26:39 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:39 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class AgeAxis : Axis, ICloneable, IAxis
    {
        public const string COMMONUNIT = "Gyr";
        public const string SIUNIT = "Gyr";

        private DoubleParam value;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "AgeAxis";
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
        [Field(Required = ParamRequired.Custom, DefaultUnit = AgeAxis.COMMONUNIT, RefMember = "Age_Value", ReferenceMode = ReferenceMode.Item)]
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
        public AgeAxis()
        {
        }

        public AgeAxis(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public AgeAxis(AgeAxis old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static AgeAxis Clone(AgeAxis old)
        {
            if (old != null)
            {
                return new AgeAxis(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: AgeAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:39  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: DoubleParam.cs,v 1.1 2008/01/08 22:26:17 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:17 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class DoubleParam : ParamBase
    {
        private double value;
        private bool valueSet;

        #region Constructors

        public DoubleParam()
        {
        }

        public DoubleParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public DoubleParam(DoubleParam old)
        {
            CopyMembers(old);
        }

        public DoubleParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        public DoubleParam(double value, string unit)
        {
            InitializeMembers();

            this.value = value;
            this.valueSet = true;
            this.Unit = unit;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = 0.0;
            this.valueSet = false;
        }

        protected void CopyMembers(DoubleParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
            this.valueSet = old.valueSet;
        }

        #endregion
        #region Properties


        [XmlAttribute("value")]
        public double Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.valueSet = true;
            }
        }

        [XmlIgnore]
        public override bool IsNull
        {
            get { return (!this.valueSet); }
        }

        #endregion
        #region Member functions

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override void Parse(string value)
        {
            this.value = double.Parse(value);
            this.valueSet = true;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (double)value;
            this.valueSet = true;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@double;
        }

        #endregion

        public override object Clone()
        {
            return new DoubleParam(this);
        }

        public static implicit operator double(DoubleParam param)
        {
            return param.Value;
        }

        public static explicit operator DoubleParam(double value)
        {
            return new DoubleParam(value, "");
        }
    }
}
#region Revision History
/* Revision History

        $Log: DoubleParam.cs,v $
        Revision 1.1  2008/01/08 22:26:17  dobos
        Initial checkin


*/
#endregion
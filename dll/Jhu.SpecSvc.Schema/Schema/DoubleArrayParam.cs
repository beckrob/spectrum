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
 *   ID:          $Id: DoubleArrayParam.cs,v 1.1 2008/01/08 22:26:17 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:17 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class DoubleArrayParam : ParamBase
    {
        private double[] value;

        #region Constructors

        public DoubleArrayParam()
        {
        }

        public DoubleArrayParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public DoubleArrayParam(DoubleArrayParam old)
        {
            CopyMembers(old);
        }

        public DoubleArrayParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        public DoubleArrayParam(double[] value, string unit)
        {
            InitializeMembers();

            this.value = value;
            this.Unit = unit;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = null;
        }

        protected void CopyMembers(DoubleArrayParam old)
        {
            base.CopyMembers(old);

            if (old.value != null)
            {
                this.value = new double[old.value.Length];
                Array.Copy(old.value, this.value, old.value.Length);
            }
            else
                this.value = null;
        }

        #endregion
        #region Properties


        //[XmlIgnore]
        public double[] Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        [XmlIgnore]
        public override bool IsNull
        {
            get { return (this.value == null); }
        }

        #endregion
        #region Member functions

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < this.value.Length; i++)
                res += " " + this.value[i].ToString();

            if (res != "")
                return res.Substring(1);
            else
                return "";
        }

        public override void Parse(string value)
        {
            List<double> res = new List<double>();
            string[] parts = value.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                double d;
                if (double.TryParse(parts[i], out d))
                    res.Add(d);
            }

            this.value = res.ToArray();
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (double[])value;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@char;
        }

        #endregion

        public override object Clone()
        {
            return new DoubleArrayParam(this);
        }

        public static implicit operator double[](DoubleArrayParam param)
        {
            return param.Value;
        }

        public static explicit operator DoubleArrayParam(double[] value)
        {
            return new DoubleArrayParam(value, "");
		}
    }
}
#region Revision History
/* Revision History

        $Log: DoubleArrayParam.cs,v $
        Revision 1.1  2008/01/08 22:26:17  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: IntArrayParam.cs,v 1.1 2008/01/08 22:26:18 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:18 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class IntArrayParam : ParamBase
    {
        private long[] value;

        #region Constructors

        public IntArrayParam()
        {
        }

        public IntArrayParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public IntArrayParam(IntArrayParam old)
        {
            CopyMembers(old);
        }

        public IntArrayParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

		public IntArrayParam(long[] value, string unit)
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

		protected void CopyMembers(IntArrayParam old)
        {
            base.CopyMembers(old);

            if (old.value != null)
            {
                this.value = new long[old.value.Length];
                Array.Copy(old.value, this.value, old.value.Length);
            }
            else
                this.value = null;
        }

        #endregion
        #region Properties


        [XmlIgnore]
        public long[] Value
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
            List<long> res = new List<long>();
            string[] parts = value.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                long d;
                if (long.TryParse(parts[i], out d))
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
            this.value = (long[])value;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@char;
        }

        #endregion

        public override object Clone()
        {
			return new IntArrayParam(this);
        }

		public static implicit operator long[](IntArrayParam param)
        {
            return param.Value;
        }

		public static explicit operator IntArrayParam(long[] value)
        {
			return new IntArrayParam(value, "");
        }
    }
}
#region Revision History
/* Revision History

        $Log: IntArrayParam.cs,v $
        Revision 1.1  2008/01/08 22:26:18  dobos
        Initial checkin


*/
#endregion
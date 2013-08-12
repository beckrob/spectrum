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
 *   ID:          $Id: IntParam.cs,v 1.1 2008/01/08 22:26:19 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:19 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class IntParam : ParamBase
    {
        private long value;
        private bool valueSet;

        #region Constructors

        public IntParam()
        {
        }

        public IntParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public IntParam(IntParam old)
        {
            CopyMembers(old);
        }

        public IntParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        public IntParam(long value)
        {
            InitializeMembers();

            this.value = value;
            this.valueSet = true;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = 0;
            this.valueSet = false;
        }

        protected void CopyMembers(IntParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
            this.valueSet = old.valueSet;
        }

        #endregion
        #region Properties


        [XmlAttribute("value")]
        public long Value
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
            this.value = long.Parse(value);
            this.valueSet = true;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (long)value;
            this.valueSet = true;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@long;
        }

        #endregion

        public override object Clone()
        {
            return new IntParam(this);
        }

        public static implicit operator long(IntParam param)
        {
            return param.Value;
        }

        public static explicit operator IntParam(long value)
        {
            return new IntParam(value);
        }
    }
}
#region Revision History
/* Revision History

        $Log: IntParam.cs,v $
        Revision 1.1  2008/01/08 22:26:19  dobos
        Initial checkin


*/
#endregion
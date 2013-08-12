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
 *   ID:          $Id: BoolParam.cs,v 1.1 2008/01/08 22:26:16 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:16 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class BoolParam : ParamBase
    {
        private bool value;
        private bool valueSet;

        #region Constructors

        public BoolParam()
        {
        }

        public BoolParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public BoolParam(BoolParam old)
        {
            CopyMembers(old);
        }

        public BoolParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        //public BoolParam(bool value)
        //{
        //    InitializeMembers();

        //    this.value = value;
        //    this.valueSet = true;
        //}

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = false;
            this.valueSet = false;
        }

        protected void CopyMembers(BoolParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
            this.valueSet = old.valueSet;
        }

        #endregion
        #region Properties


        [XmlAttribute("value")]
        public bool Value
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
            get { return (!valueSet); }
        }

        #endregion
        #region Member functions

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override void Parse(string value)
        {
            this.value = bool.Parse(value);
            this.valueSet = true;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (bool)value;
            this.valueSet = true;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.boolean;
        }

        #endregion

        public override object Clone()
        {
            return new BoolParam(this);
        }

        public static implicit operator bool(BoolParam param)
        {
            return param.Value;
        }

        public static explicit operator BoolParam(bool value)
        {
            return new BoolParam(value);
        }
    }
}
#region Revision History
/* Revision History

        $Log: BoolParam.cs,v $
        Revision 1.1  2008/01/08 22:26:16  dobos
        Initial checkin


*/
#endregion
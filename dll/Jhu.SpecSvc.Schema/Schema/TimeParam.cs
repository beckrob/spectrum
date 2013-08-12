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
 *   ID:          $Id: TimeParam.cs,v 1.1 2008/01/08 22:26:23 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:23 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class TimeParam : ParamBase
    {
        private DateTime value;
        private bool valueSet;

        #region Constructors

        public TimeParam()
        {
        }

        public TimeParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public TimeParam(TimeParam old)
        {
            CopyMembers(old);
        }

        public TimeParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        public TimeParam(DateTime value)
        {
            InitializeMembers();

            this.value = value;
            this.valueSet = true;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = DateTime.FromOADate(0);
            this.valueSet = false;
        }

        protected void CopyMembers(TimeParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
            this.valueSet = old.valueSet;
        }

        #endregion
        #region Properties


        [XmlAttribute("value")]
        public DateTime Value
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
            return this.value.ToString("u");
        }

        public override void Parse(string value)
        {
            this.value = DateTime.Parse(value);
            this.valueSet = true;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (DateTime)value;
            this.valueSet = true;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@char;
        }

        #endregion

        public override object Clone()
        {
            return new TimeParam(this);
        }

        public static implicit operator DateTime(TimeParam param)
        {
            return param.Value;
        }

        public static explicit operator TimeParam(DateTime value)
        {
            return new TimeParam(value);
        }
    }
}
#region Revision History
/* Revision History

        $Log: TimeParam.cs,v $
        Revision 1.1  2008/01/08 22:26:23  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: TextParam.cs,v 1.1 2008/01/08 22:26:22 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:22 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class TextParam : ParamBase
    {
        private string value;

        #region Constructors

        public TextParam()
        {
        }

        public TextParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public TextParam(TextParam old)
        {
            CopyMembers(old);
        }

        public TextParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = null;
        }

        protected void CopyMembers(TextParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
        }

        #endregion
        #region Properties


        [XmlAttribute("value")]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        [XmlIgnore]
        public override bool IsNull
        {
            get { return (value == null); }
        }

        #endregion
        #region Member functions

        public override string ToString()
        {
            return this.value;
        }

        public override void Parse(string value)
        {
            this.value = value;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (string)value;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            //return VOTABLE.dataType.unicodeChar;
            return VOTABLE.dataType.@char;
        }

        #endregion

        public override object Clone()
        {
            return Clone(this);
        }

        public static TextParam Clone(TextParam old)
        {
            if (old != null)
            {
                return new TextParam(old);
            }
            else
                return null;
        }

        public static implicit operator string(TextParam param)
        {
            return param.Value;
        }

        public static explicit operator TextParam(string value)
        {
            TextParam tp = new TextParam(true);
            tp.Value = value;
            return tp;
        }
    }
}
#region Revision History
/* Revision History

        $Log: TextParam.cs,v $
        Revision 1.1  2008/01/08 22:26:22  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: PositionParam.cs,v 1.1 2008/01/08 22:26:21 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:21 $
 */
#endregion
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class PositionParam : ParamBase
    {
        private Position value;
        private bool valueSet;

        #region Constructors

        public PositionParam()
        {
        }

        public PositionParam(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public PositionParam(PositionParam old)
        {
            CopyMembers(old);
        }

        public PositionParam(string ucd)
        {
            InitializeMembers();

            this.Ucd = ucd;
        }

        public PositionParam(Position value, string unit)
        {
            InitializeMembers();

            this.value = value;
            this.valueSet = true;
            this.Unit = unit;
        }

        protected override void InitializeMembers()
        {
            base.InitializeMembers();

            this.value = new Position(0, 0);
            this.valueSet = false;
        }

        protected void CopyMembers(PositionParam old)
        {
            base.CopyMembers(old);

            this.value = old.value;
            this.valueSet = old.valueSet;
        }

        #endregion
        #region Properties


        [XmlIgnore]
        public Position Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.valueSet = true;
            }
        }

        [XmlAttribute("value")]
        public string Value_ForXml
        {
            get { return this.value.ToString(); }
            set
            {
                this.value = Position.Parse(value);
                this.valueSet = true;
            }
        }

        [XmlIgnore]
        public override bool IsNull
        {
            get
            {
                return !this.valueSet; 
            }
        }

        #endregion
        #region Member functions

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override void Parse(string value)
        {
            this.value = Position.Parse(value);
            this.valueSet = true;
        }

        public override object GetValue()
        {
            return this.value;
        }

        public override void SetValue(object value)
        {
            this.value = (Position)value;
            this.valueSet = true;
        }

        public override VOTABLE.dataType GetVOTableType()
        {
            return VOTABLE.dataType.@char;
        }

        #endregion

        public override object Clone()
        {
            return new PositionParam(this);
        }

        public static implicit operator Position(PositionParam param)
        {
            return param.Value;
        }

        public static explicit operator PositionParam(Position value)
        {
            return new PositionParam(value, "");
        }
    }
}
#region Revision History
/* Revision History

        $Log: PositionParam.cs,v $
        Revision 1.1  2008/01/08 22:26:21  dobos
        Initial checkin


*/
#endregion
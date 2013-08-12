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
 *   ID:          $Id: ParamBase.cs,v 1.1 2008/01/08 22:26:20 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:20 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class ParamBase : ICloneable
    {
        private string name;
        private string unit;
        private string ucd;
        private string key;
        private bool system;

        public ParamBase()
        {
        }

        [XmlIgnore()]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [XmlAttribute("ucd")]
        public string Ucd
        {
            get { return this.ucd; }
            set { this.ucd = value; }
        }

        [XmlAttribute("unit")]
        public string Unit
        {
            get { return this.unit; }
            set { this.unit = value; }
        }

        [XmlAttribute("key")]
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        [XmlIgnore]
        public bool System
        {
            get { return this.system; }
            set { this.system = value; }
        }

        [XmlIgnore]
        public virtual bool IsNull
        {
            get { throw new System.NotImplementedException(); }
        }

        protected virtual void InitializeMembers()
        {
            this.name = null;
            this.unit = null;
            this.ucd = null;
            this.key = null;
            this.system = false;
        }

        protected void CopyMembers(ParamBase old)
        {
            this.name = old.name;
            this.unit = old.unit;
            this.ucd = old.ucd;
            this.key = old.key;
            this.system = old.system;
        }

        public virtual void Parse(string value)
        {
            throw new System.NotImplementedException();
        }

        public virtual object GetValue()
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        public virtual VOTABLE.dataType GetVOTableType()
        {
            throw new System.NotImplementedException();
        }

        public virtual object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
#region Revision History
/* Revision History

        $Log: ParamBase.cs,v $
        Revision 1.1  2008/01/08 22:26:20  dobos
        Initial checkin


*/
#endregion
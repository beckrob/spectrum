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
 *   ID:          $Id: FieldAttribute.cs,v 1.1 2008/01/08 22:26:14 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:14 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.Schema
{
    /// <summary>
    /// Summary description for ParamRefAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FieldAttribute : System.Attribute
    {
        private string ucd;
        private ParamRequired required;
        private object defaultValue;
        private string defaultUnit;
        private string refMember;
        private ReferenceMode referenceMode;
        private SerializationMode serializationMode;

        public string Ucd
        {
            get { return this.ucd; }
            set { this.ucd = value; }
        }

        public ParamRequired Required
        {
            get { return this.required; }
            set { this.required = value; }
        }

        public object DefaultValue
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

        public string DefaultUnit
        {
            get { return this.defaultUnit; }
            set { this.defaultUnit = value; }
        }

        public string RefMember
        {
            get { return this.refMember; }
            set { this.refMember = value; }
        }

		public ReferenceMode ReferenceMode
		{
			get { return referenceMode; }
            set { referenceMode = value; }
		}

        public SerializationMode SerializationMode
        {
            get { return serializationMode; }
            set { serializationMode = value; }
        }

        public FieldAttribute()
        {
            InitializeMembers();
        }
        
        private void InitializeMembers()
        {
            this.ucd = null;
            this.required = ParamRequired.Optional;
            this.defaultValue = null;
            this.defaultUnit = null;
            this.refMember = null;
			this.referenceMode = ReferenceMode.Inline;
            this.serializationMode = SerializationMode.Data;
        }
    }

    [Flags]
    public enum ParamRequired : int
    {
        SpecService = 1,
        Mandatory = 1,      // must
        Recommended = 3,    // should
        Optional = 7,       // may
        Custom = 15,
        Dummy = 32,
        Derived = 64,
    }

    [Flags]
    public enum ReferenceMode
    {
        Inline = 0,
        Item = 1,
        ArrayItem = 2,
    }

    [Flags]
    public enum SerializationMode : int
    {
        None = 0,
        Data = 1,
        Characterization = 2
    }
}
#region Revision History
/* Revision History

        $Log: FieldAttribute.cs,v $
        Revision 1.1  2008/01/08 22:26:14  dobos
        Initial checkin


*/
#endregion
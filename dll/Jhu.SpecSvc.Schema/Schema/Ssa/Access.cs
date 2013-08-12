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
 *   ID:          $Id: Access.cs,v 1.1 2008/01/08 22:26:57 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:57 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.Schema.Ssa
{
	/// <summary>
	/// Summary description for Access.
	/// </summary>
	public class Access : Group, ICloneable
	{
		public TextParam reference;
		public TextParam format;
		public IntParam size;

        [Field(Required = ParamRequired.Mandatory, RefMember = "AcRef", ReferenceMode = ReferenceMode.Item)]
        public TextParam Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "Format", ReferenceMode = ReferenceMode.Item)]
        public TextParam Format
        {
            get { return format; }
            set { format = value; }
        }

        [Field(Required = ParamRequired.Recommended)]
        public IntParam Size
        {
            get { return size; }
            set { size = value; }
        }

        #region Constructors
        public Access()
        {
        }

        public Access(bool initialize)
        {
            if (initialize)
            {
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
            }
        }

        public Access(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Access(Access old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Access Clone(Access old)
        {
            if (old != null)
            {
                return new Access(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Access.cs,v $
        Revision 1.1  2008/01/08 22:26:57  dobos
        Initial checkin


*/
#endregion
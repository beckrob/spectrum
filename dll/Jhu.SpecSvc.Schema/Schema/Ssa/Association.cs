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
 *   ID:          $Id: Association.cs,v 1.1 2008/01/08 22:26:58 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:58 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.Schema.Ssa
{
    /// <summary>
    /// Summary description for Association.
    /// </summary>
    public class Association : Group, ICloneable
    {
        public TextParam type;
        public TextParam id;
        public TextParam key;

        [Field(Required = ParamRequired.Optional, RefMember = "AssocType", ReferenceMode = ReferenceMode.Item)]
        public TextParam Type
        {
            get { return type; }
            set { type = value; }
        }

        [Field(Required = ParamRequired.Optional, RefMember = "AssocId", ReferenceMode = ReferenceMode.Item)]
        public TextParam Id
        {
            get { return id; }
            set { id = value; }
        }

        [Field(Required = ParamRequired.Optional, RefMember = "AssocKey", ReferenceMode = ReferenceMode.Item)]
        public TextParam Key
        {
            get { return key; }
            set { key = value; }
        }

        #region Constructors
        public Association()
        {
        }

        public Association(bool initialize)
        {
            if (initialize)
            {
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
            }
        }

        public Association(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Association(Association old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Association Clone(Association old)
        {
            if (old != null)
            {
                return new Association(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Association.cs,v $
        Revision 1.1  2008/01/08 22:26:58  dobos
        Initial checkin


*/
#endregion
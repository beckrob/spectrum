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
 *   ID:          $Id: Query.cs,v 1.1 2008/01/08 22:26:58 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:58 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.Schema.Ssa
{
    /// <summary>
    /// Summary description for Query.
    /// </summary>
    public class Query : Group, ICloneable
    {
        public DoubleParam score;

        [Field(Required = ParamRequired.Recommended, RefMember = "Score", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam Score
        {
            get { return score; }
            set { score = value; }
        }

        #region Constructors
        public Query()
        {
        }

        public Query(bool initialize)
        {
            if (initialize)
            {
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
            }
        }

        public Query(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Query(Query old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Query Clone(Query old)
        {
            if (old != null)
            {
                return new Query(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Query.cs,v $
        Revision 1.1  2008/01/08 22:26:58  dobos
        Initial checkin


*/
#endregion
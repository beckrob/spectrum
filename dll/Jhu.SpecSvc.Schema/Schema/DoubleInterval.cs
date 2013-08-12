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
 *   ID:          $Id: DoubleInterval.cs,v 1.1 2008/01/08 22:26:17 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:17 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class DoubleInterval : Group, ICloneable
    {
        [Field(Required=ParamRequired.Optional)]
        public DoubleParam Min;
        [Field(Required = ParamRequired.Optional)]
        public DoubleParam Max;

        #region Constructors
        public DoubleInterval()
        {
        }

        public DoubleInterval(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public DoubleInterval(DoubleInterval old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static DoubleInterval Clone(DoubleInterval old)
        {
            if (old != null)
            {
                return new DoubleInterval(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: DoubleInterval.cs,v $
        Revision 1.1  2008/01/08 22:26:17  dobos
        Initial checkin


*/
#endregion
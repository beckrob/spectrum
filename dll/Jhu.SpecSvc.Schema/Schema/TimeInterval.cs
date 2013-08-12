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
 *   ID:          $Id: TimeInterval.cs,v 1.1 2008/01/08 22:26:22 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:22 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class TimeInterval : Group, ICloneable
	{
        [Field(Required = ParamRequired.Optional)]
		public TimeParam Start;
        [Field(Required = ParamRequired.Optional)]
		public TimeParam Stop;



        #region Constructors
        public TimeInterval()
        {
        }

        public TimeInterval(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeInterval(TimeInterval old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeInterval Clone(TimeInterval old)
        {
            if (old != null)
            {
                return new TimeInterval(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: TimeInterval.cs,v $
        Revision 1.1  2008/01/08 22:26:22  dobos
        Initial checkin


*/
#endregion
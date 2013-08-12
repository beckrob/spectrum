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
 *   ID:          $Id: ParamCollection.cs,v 1.1 2008/01/08 22:26:20 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:20 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public class ParamCollection : System.Collections.Generic.List<ParamBase>, ICloneable
    {
        public ParamCollection()
            : base()
        {
        }

        public ParamCollection(ParamCollection old)
            : base()
        {
            foreach (ParamBase param in old)
                this.Add((ParamBase)param.Clone());
        }

        public void Add(string key, ParamBase param)
        {
            param.Key = key;
            base.Add(param);
        }

        public ParamBase this[string key]
        {
            get
            {
                return this.Find(delegate(ParamBase p)
                            {
                                return p.Key == key;
                            });
            }
            set
            {
                this[key] = value;
            }
        }

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static ParamCollection Clone(ParamCollection old)
        {
            if (old != null)
            {
                return new ParamCollection(old);
            }
            else
                return null;
        }
        #endregion

        }
}
#region Revision History
/* Revision History

        $Log: ParamCollection.cs,v $
        Revision 1.1  2008/01/08 22:26:20  dobos
        Initial checkin


*/
#endregion
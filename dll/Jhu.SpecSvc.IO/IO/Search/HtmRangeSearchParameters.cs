#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: HtmRangeSearchParameters.cs,v 1.1 2008/01/08 22:01:44 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:44 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;

namespace Jhu.SpecSvc.IO
{
    public class HtmRangeSearchParameters : SearchParametersBase
    {

        public struct HtmRange
        {
            public long Lo;
            public long Hi;

            public HtmRange(long lo, long hi)
            {
                Lo = lo;
                Hi = hi;
            }
        }

        #region Member variables

        private HtmRange[] ranges;
        
        #endregion
        #region Properties

        public HtmRange[] Ranges
        {
            get { return this.ranges; }
            set { this.ranges = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.HtmRange; }
        }

        #endregion
        #region Constructors

        public HtmRangeSearchParameters()
        {
        }

        public HtmRangeSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public HtmRangeSearchParameters(HtmRangeSearchParameters old)
        {
            CopyMembers(old);
        }

        public HtmRangeSearchParameters(SearchParametersBase old)
            : base(old)
        {
            InitializeMembers();
        }

        public HtmRangeSearchParameters(HtmRange[] ranges)
        {
            InitializeMembers();

            this.ranges = ranges;
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.ranges = null;
        }

        private void CopyMembers(HtmRangeSearchParameters old)
        {
            base.CopyMembers(old);

            if (old.ranges != null)
            {
                this.ranges = new HtmRange[old.ranges.Length];
                Array.Copy(old.ranges, this.ranges, old.ranges.Length);
            }
        }

        public HtmRangeSearchParameters GetStandardUnits()
        {
            return this;
        }

        public void SetRanges(string ranges)
        {
            List<HtmRange> pairs = new List<HtmRange>();

            string[] lines = ranges.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                pairs.Add(new HtmRange(long.Parse(parts[0]), long.Parse(parts[1])));
            }

            this.ranges = pairs.ToArray();
        }

        public override string ToString()
        {
            string res = "";
            foreach (HtmRange pair in ranges)
            {
                res += String.Format("{0},{1}\r\n", pair.Lo, pair.Hi);
            }

            return res.Trim();
        }

        public void GetRanges(out string lo, out string hi)
        {
            lo = "";
            hi = "";

            foreach (HtmRange pair in ranges)
            {
                lo += String.Format(",{0}", lo);
                hi += String.Format(",{0}", hi);
            }

            if (lo != "") lo = lo.Substring(1);
            if (hi != "") hi = hi.Substring(1);
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: HtmRangeSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:44  dobos
        Initial checkin


*/
#endregion
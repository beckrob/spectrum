#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: RedshiftSearchParameters.cs,v 1.1 2008/01/08 22:01:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:45 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;

namespace Jhu.SpecSvc.IO
{
    public class RedshiftSearchParameters : SearchParametersBase
    {
        private DoubleInterval redshift;

        public DoubleInterval Redshift
        {
            get { return this.redshift; }
            set { this.redshift = value; }
        }

        public override SearchMethods Type
        {
            get { return SearchMethods.Redshift; }
        }

        #region Constructors

        public RedshiftSearchParameters()
        {
        }

        public RedshiftSearchParameters(bool initialize)
            :base (initialize)
        {
            if (initialize) InitializeMembers();
        }

        public RedshiftSearchParameters(RedshiftSearchParameters old)
        {
            CopyMembers(old);
        }

        public RedshiftSearchParameters(double min, double max)
        {
            InitializeMembers();

            this.redshift.Min = new DoubleParam(min, "");
            this.redshift.Min.Ucd = "src.redshift";
            
            this.redshift.Max = new DoubleParam(max, "");
            this.redshift.Max.Ucd = "src.redshift";
        }

        public RedshiftSearchParameters(DoubleParam min, DoubleParam max)
        {
            InitializeMembers();

            this.redshift.Min = new DoubleParam(min);
            this.redshift.Max = new DoubleParam(max);
        }

        public RedshiftSearchParameters(DoubleInterval z)
        {
            InitializeMembers();

            this.redshift = new DoubleInterval(z);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.redshift = new DoubleInterval(ParamRequired.Custom);
            
            this.redshift.Min.Ucd = "src.redshift";
            this.redshift.Min.Unit = "";

            this.redshift.Max.Ucd = "src.redshift";
            this.redshift.Max.Unit = "";
        }

        private void CopyMembers(RedshiftSearchParameters old)
        {
            base.CopyMembers(old);

            this.redshift = old.redshift == null ? null : new DoubleInterval(old.redshift);
        }

        public RedshiftSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: RedshiftSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:45  dobos
        Initial checkin


*/
#endregion
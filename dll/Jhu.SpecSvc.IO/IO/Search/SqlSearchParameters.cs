#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SqlSearchParameters.cs,v 1.1 2008/01/08 22:01:46 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:46 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class SqlSearchParameters : SearchParametersBase
    {
        private string query;

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public SqlSearchParameters()
        {
            InitializeMembers();
        }

        public SqlSearchParameters(bool initialize)
            : base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public SqlSearchParameters(SqlSearchParameters old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.query = "";
        }

        private void CopyMembers(SqlSearchParameters old)
        {
            this.query = old.query;
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Sql; }
        }

        public SqlSearchParameters GetStandardUnits()
        {
            return this;
        }

    }
}
#region Revision History
/* Revision History

        $Log: SqlSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:46  dobos
        Initial checkin


*/
#endregion
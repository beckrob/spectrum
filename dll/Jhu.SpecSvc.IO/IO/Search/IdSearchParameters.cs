#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: IdSearchParameters.cs,v 1.1 2008/01/08 22:01:44 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:44 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;

namespace Jhu.SpecSvc.IO
{
	public class IdSearchParameters : SearchParametersBase
    {
        #region Member variables

        private string[] ids;

        #endregion

        public string[] Ids
        {
            get { return this.ids; }
            set { this.ids = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Id; }
        }

        #region Constructors

        public IdSearchParameters()
        {
        }

        public IdSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public IdSearchParameters(IdSearchParameters old)
            : base(old)
        {
            CopyMembers(old);
        }

        public IdSearchParameters(string id)
        {
            InitializeMembers();

            this.ids = new string[] { id };
        }

        public IdSearchParameters(string[] ids)
        {
            InitializeMembers();

            this.ids = ids;
        }

        private void InitializeMembers()
        {
            this.ids = null;
        }

        private void CopyMembers(IdSearchParameters old)
        {
            if (old.ids != null)
            {
                this.ids = new string[old.ids.Length];
                Array.Copy(old.ids, this.ids, old.ids.Length);
            }
            else
            {
                this.ids = null;
            }
        }

        #endregion
        #region Member functions

        public IdSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: IdSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:44  dobos
        Initial checkin


*/
#endregion
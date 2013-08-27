#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: FolderSearchParameters.cs,v 1.1 2008/01/08 22:01:43 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:43 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;

namespace Jhu.SpecSvc.IO
{
	public class FolderSearchParameters : SearchParametersBase
    {
        private int folderId;

        public int FolderId
        {
            get { return this.folderId; }
            set { this.folderId = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Folder; }
        }

        #region Constructors

        public FolderSearchParameters()
        {
        }

        public FolderSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public FolderSearchParameters(FolderSearchParameters old)
        {
            CopyMembers(old);
        }

        public FolderSearchParameters(int folderId)
        {
            InitializeMembers();

            this.folderId = folderId;
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.folderId = 0;
        }

        private void CopyMembers(FolderSearchParameters old)
        {
            base.CopyMembers(old);

            this.folderId = old.folderId;
        }

        public FolderSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: FolderSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:43  dobos
        Initial checkin


*/
#endregion
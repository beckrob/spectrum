#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SearchParametersBase.cs,v 1.1 2008/01/08 22:00:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:00:50 $
 */
#endregion
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace Jhu.SpecSvc.IO
{
    public abstract class SearchParametersBase
    {
        private Guid userGuid;
        private string[] collections;
        private bool loadPoints;
        private string[] pointsMask;
        private bool loadDetails;

        public virtual SearchMethod Type
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Guid UserGuid
        {
            get { return this.userGuid; }
            set { this.userGuid = value; }
        }

        public string[] Collections
        {
            get { return collections; }
            set { collections = value; }
        }

        public bool LoadPoints
        {
            get { return this.loadPoints; }
            set { this.loadPoints = value; }
        }

        public string[] PointsMask
        {
            get { return this.pointsMask; }
            set { this.pointsMask = value; }
        }

        public bool LoadDetails
        {
            get { return this.loadDetails; }
            set { this.loadDetails = value; }
        }

        public SearchParametersBase()
        {
        }

        public SearchParametersBase(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public SearchParametersBase(SearchParametersBase old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.userGuid = Guid.Empty;
            this.collections = null;
            this.loadPoints = false;
            this.pointsMask = null;
            this.loadDetails = false;
        }

        protected void CopyMembers(SearchParametersBase old)
        {
            this.userGuid = old.userGuid;

            if (old.collections != null)
            {
                this.collections = new string[old.collections.Length];
                Array.Copy(old.collections, this.collections, old.collections.Length);
            }
            else
            {
                this.collections = null;
            }

            this.loadPoints = old.loadPoints;
            this.loadDetails = old.loadDetails;
        }
    }
}
#region Revision History
/* Revision History

        $Log: SearchParametersBase.cs,v $
        Revision 1.1  2008/01/08 22:00:50  dobos
        Initial checkin


*/
#endregion
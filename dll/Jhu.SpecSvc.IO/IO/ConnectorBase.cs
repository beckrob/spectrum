#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ConnectorBase.cs,v 1.2 2008/09/11 10:44:59 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/09/11 10:44:59 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
	public abstract class ConnectorBase : IDisposable
	{
        protected string collectionId;
        protected List<Exception> exceptions;

        public ConnectorBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.exceptions = new List<Exception>();
        }

        /*protected IEnumerable<T> ToIEnumerable<T>(IEnumerable<T> spectra)
        {
            return spectra.AsParallel();
        }*/

        public Spectrum GetSpectrum(Guid userGuid, string spectrumId)
        {
            return GetSpectrum(userGuid, spectrumId, true, null, true);
        }

        public virtual Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par)
        {
            throw new System.NotImplementedException();
        }

        //

        public virtual void LoadSpectrum(Spectrum spec, Guid userGuid, long id, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            throw new System.NotImplementedException();
        }

        public virtual void LoadSpectrumFields(Spectrum spec, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void LoadSpectrumData(Spectrum spec, Guid userGuid, string[] pointsMask)
        {
            throw new System.NotImplementedException();
        }

        public virtual long SaveSpectrum(Spectrum spec, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SaveFields(Spectrum spec, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SaveData(Spectrum spec, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DeleteSpectrum(Spectrum spec, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        //

        public virtual UserFolder[] QueryUserFolders(Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual UserFolder GetUserFolder(Guid userGuid, int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SaveUserFolder(UserFolder folder, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DeleteUserFolder(UserFolder folder, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }



        //

        public virtual IEnumerable<Collection> QueryCollections(Guid userGuid, SearchMethods searchMethod)
        {
            throw new System.NotImplementedException();
        }

        public virtual int LoadCollection(Collection collection, Guid userGuid, int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SaveCollection(Collection collection, string newId, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DeleteCollection(Collection collection, Guid userGuid)
        {
            throw new System.NotImplementedException();
        }

        protected void PrefixCollectionId(Spectrum[] spectra)
        {
            for (int i = 0; i < spectra.Length; i++)
                PrefixCollectionId(spectra[i]);
        }

        protected void PrefixCollectionId(Spectrum spec)
        {
            string[] idparts = spec.PublisherId.Split('#');
            if (idparts.Length < 2)
                spec.PublisherId = collectionId + "#" + spec.PublisherId;
            else
                spec.PublisherId = collectionId + "#" + idparts[1];

        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: ConnectorBase.cs,v $
        Revision 1.2  2008/09/11 10:44:59  dobos
        Bugfixes and parallel execution added to PortalConnector

        Revision 1.1  2008/01/08 22:00:48  dobos
        Initial checkin


*/
#endregion

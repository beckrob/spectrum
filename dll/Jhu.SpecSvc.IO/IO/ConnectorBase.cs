using System;
using System.Collections.Generic;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
	public abstract class ConnectorBase : IDisposable
	{
        protected string collectionId;
        private List<Exception> exceptions;

        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        #region Constructors and initializers

        public ConnectorBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.exceptions = new List<Exception>();
        }

        public virtual void Dispose()
        {
        }

        #endregion

        public Spectrum GetSpectrum(Guid userGuid, string spectrumId)
        {
            return GetSpectrum(userGuid, spectrumId, true, null, true);
        }

        public abstract Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails);

        public abstract IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par);

        public abstract IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par);

        //

        public virtual void LoadSpectrum(Spectrum spec, Guid userGuid, long id, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadSpectrumFields(Spectrum spec, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadSpectrumData(Spectrum spec, Guid userGuid, string[] pointsMask)
        {
            throw new NotImplementedException();
        }

        public virtual long SaveSpectrum(Spectrum spec, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveSpectrumFields(Spectrum spec, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveSpectrumData(Spectrum spec, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteSpectrum(Spectrum spec, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        //

        public virtual UserFolder[] QueryUserFolders(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual UserFolder GetUserFolder(Guid userGuid, int id)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveUserFolder(UserFolder folder, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteUserFolder(UserFolder folder, Guid userGuid)
        {
            throw new NotImplementedException();
        }



        //

        public virtual IEnumerable<Collection> QueryCollections(Guid userGuid, SearchMethod searchMethod)
        {
            throw new NotImplementedException();
        }

        public virtual int LoadCollection(Collection collection, Guid userGuid, int id)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveCollection(Collection collection, string newId, Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteCollection(Collection collection, Guid userGuid)
        {
            throw new NotImplementedException();
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
    }
}


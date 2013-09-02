#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: WsConnector.cs,v 1.1 2008/01/08 22:01:36 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:36 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    public class WsConnector : ConnectorBase
    {
        private Remote.Search search;
        private Remote.Admin admin;

        public WsConnector()
        {
            InitializeMembers();
        }

		public WsConnector(Collection collection)
		{
			if (collection.Type != CollectionType.WebService)
				throw new System.Exception("Not valid collection type");

            InitializeMembers();

			this.search.Url = collection.ConnectionString;
		}

        public WsConnector(string url)
        {
            InitializeMembers();

            search.Url = url;
        }

        public string Url
        {
            get { return search.Url; }
            set { search.Url = value; }
        }

        public string AdminUrl
        {
            get { return admin.Url; }
            set { admin.Url = value; }
        }

        public int Timeout
        {
            get { return search.Timeout; }
            set { search.Timeout = value; }
        }

        private void InitializeMembers()
        {
            search = new Remote.Search();
            admin = new Remote.Admin();
        }

        public override Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            return search.GetSpectrum_Details(userGuid.ToString(), spectrumId, loadPoints, pointsMask, loadDetails);
        }

        public override IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par)
        {
            return search.FindSpectrum_Id(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par)
        {
            return search.FindSpectrum_Redshift(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par)
        {
            return search.FindSpectrum_Cone(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par)
        {
            return search.FindSpectrum_Folder(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par)
        {
            return search.FindSpectrum_Advanced(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            return search.FindSpectrum_Similar(par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par)
        {
            throw new NotImplementedException();
        }

        //

        public override long SaveSpectrum(Spectrum spec, Guid userGuid)
        {
            spec.UserGuid = userGuid;
            if (spec.Id <= 0)
                return admin.CreateSpectrum(userGuid.ToString(), spec.UserFolderId, spec.Public, spec);
            else
                return admin.ModifySpectrum(userGuid.ToString(), spec.UserFolderId, spec.Public, spec) ? spec.Id : 0;
        }

        public override void DeleteSpectrum(Spectrum spec, Guid userGuid)
        {
            spec.UserGuid = userGuid;
            admin.DeleteSpectrum(userGuid.ToString(), spec.Curation.PublisherDID.Value);
        }

        //

        public override UserFolder[] QueryUserFolders(Guid userGuid)
        {
            return search.QueryUserFolders(userGuid.ToString());
        }

        public override UserFolder GetUserFolder(Guid userGuid, int id)
        {
            return admin.GetUserFolder(userGuid.ToString(), id);
        }

        public override void SaveUserFolder(UserFolder folder, Guid userGuid)
        {
            folder.UserGuid = userGuid;
            if (folder.Id <= 0)
                admin.CreateUserFolder(userGuid.ToString(), folder);
            else
                admin.ModifyUserFolder(userGuid.ToString(), folder);
        }

        public override void DeleteUserFolder(UserFolder folder, Guid userGuid)
        {
            folder.UserGuid = userGuid;
            admin.DeleteUserFolder(userGuid.ToString(), folder);
        }

        public string[] Revisions()
        {
            return search.Revisions();
        }
    }
}
#region Revision History
/* Revision History

        $Log: WsConnector.cs,v $
        Revision 1.1  2008/01/08 22:01:36  dobos
        Initial checkin


*/
#endregion
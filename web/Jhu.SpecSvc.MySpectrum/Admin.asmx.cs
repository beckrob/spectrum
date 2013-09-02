using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.MySpectrum
{
    [WebService(Namespace = "http://voservices.net/spectrum")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Admin : ServiceBase
    {

        public Admin()
        {

        }

        [WebMethod]
        public long CreateSpectrum(string userGuid, int userFolderId, int publicFlag, Spectrum spectrum)
        {
            using (var cn = CreateSqlConnector())
            {
                spectrum.Id = 0;
                spectrum.UserGuid = new Guid(userGuid);
                spectrum.UserFolderId = userFolderId;
                spectrum.Public = publicFlag;

                cn.SaveSpectrum(spectrum, new Guid(userGuid));

                return spectrum.Id;
            }
        }

        [WebMethod]
        public long[] CreateSpectrumMultiple(string userGuid, int userFolderId, int publicFlag, Spectrum[] spectra)
        {
            using (var cn = CreateSqlConnector())
            {
                long[] res = new long[spectra.Length];
                for (int i = 0; i < spectra.Length; i++)
                {
                    Spectrum spectrum = spectra[i];
                    try
                    {
                        spectrum.Id = 0;
                        spectrum.UserGuid = new Guid(userGuid);
                        spectrum.UserFolderId = userFolderId;
                        spectrum.Public = publicFlag;

                        cn.SaveSpectrum(spectrum, new Guid(userGuid));

                        res[i] = spectrum.Id;
                    }
                    catch (System.Exception)
                    {
                        res[i] = -1;
                    }
                }

                return res;
            }

        }

        [WebMethod]
        public bool ModifySpectrum(string userGuid, int userFolderId, int publicFlag, Spectrum spectrum)
        {
            using (var cn = CreateSqlConnector())
            {
                spectrum.UserGuid = new Guid(userGuid);
                spectrum.UserFolderId = userFolderId;
                spectrum.Public = publicFlag;

                cn.SaveSpectrum(spectrum, new Guid(userGuid));

                return true;
            }
        }

        [WebMethod]
        public bool DeleteSpectrum(string userGuid, string id)
        {
            using (var cn = CreateSqlConnector())
            {
                Spectrum spectrum = new Spectrum();
                spectrum.BasicInitialize();
                spectrum.Curation.PublisherDID.Value = id;

                cn.DeleteSpectrum(spectrum, new Guid(userGuid));

                return true;
            }
        }

        [WebMethod]
        public Spectrum GetEmptySpectrum()
        {
            Spectrum sp = new Spectrum();
            sp.BasicInitialize();
            return sp;
        }

        //

        [WebMethod]
        public int CreateUserFolder(string userGuid, UserFolder folder)
        {
            using (var cn = CreateSqlConnector())
            {
                folder.Id = 0;
                folder.UserGuid = new Guid(userGuid);

                cn.SaveUserFolder(folder, folder.UserGuid);

                return folder.Id;
            }
        }

        [WebMethod]
        public bool ModifyUserFolder(string userGuid, UserFolder folder)
        {
            using (var cn = CreateSqlConnector())
            {
                folder.UserGuid = new Guid(userGuid);

                cn.SaveUserFolder(folder, folder.UserGuid);

                return true;
            }
        }

        [WebMethod]
        public bool DeleteUserFolder(string userGuid, UserFolder folder)
        {
            using (var cn = CreateSqlConnector())
            {
                folder.UserGuid = new Guid(userGuid);

                cn.DeleteUserFolder(folder, folder.UserGuid);

                return true;
            }
        }

        [WebMethod]
        public UserFolder GetUserFolder(string userGuid, int id)
        {
            using (var cn = CreateSqlConnector())
            {
                UserFolder res = cn.GetUserFolder(new Guid(userGuid), id);

                return res;
            }
        }

        [WebMethod]
        public UserFolder[] QueryUserFolders(string userGuid)
        {
            var app = new Search();
            UserFolder[] res = app.QueryUserFolders(userGuid);

            return res;
        }

        [WebMethod]
        public string[] Revisions()
        {
            return null;

            /*return new string[] {"VoServices.Spectrum.MySpectrum: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
								 "VoServices.Schema.Spectrum: " + System.Reflection.Assembly.GetAssembly(typeof(VoServices.Schema.Spectrum.Spectrum)).GetName().Version.ToString(),
								 "Spherical.Htm: " + System.Reflection.Assembly.GetAssembly(typeof(Spherical.Htm.Cover)).GetName().Version.ToString()};*/
        }


    }
}

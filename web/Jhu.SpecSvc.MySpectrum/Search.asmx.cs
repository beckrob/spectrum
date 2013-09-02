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
    public class Search : ServiceBase
    {
        public Search()
        {
        }

        [WebMethod]
        public Spectrum GetSpectrum(string userGuid, string spectrumId)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.GetSpectrum(
                    (userGuid == "") ? Guid.Empty : new Guid(userGuid),
                    spectrumId);
            }
        }

        [WebMethod]
        public Spectrum GetSpectrum_Details(string userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.GetSpectrum(
                    (userGuid == "") ? Guid.Empty : new Guid(userGuid),
                    spectrumId,
                    loadPoints,
                    pointsMask,
                    loadDetails);
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Id(IdSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_All(AllSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Folder(FolderSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Cone(ConeSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Redshift(RedshiftSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Advanced(AdvancedSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Object(ObjectSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_HtmRange(HtmRangeSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Similar(SimilarSearchParameters par)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public UserFolder[] QueryUserFolders(string userGuid)
        {
            using (var cn = CreateSqlConnector())
            {
                return cn.QueryUserFolders(new Guid(userGuid));
            }
        }

        //
        [WebMethod]
        public Collection[] QueryCollections(string userGuid)
        {
            var coll = new Collection();
            coll.Id = AppSettings.DefaultPublisherID;

            return new Collection[] { coll };
        }

        //
        [WebMethod]
        public string[] Revisions()
        {
            return null;

            /*return new string[] {"VoServices.Spectrum.MySpectrum: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
									"VoServices.Schema.Spectrum: " + System.Reflection.Assembly.GetAssembly(typeof(VoServices.Schema.Spectrum.Spectrum)).GetName().Version.ToString(),
                                    "VoServices.Spectrum.Lib: " + System.Reflection.Assembly.GetAssembly(typeof(VoServices.Schema.Spectrum.Spectrum)).GetName().Version.ToString(),
									"Spherical.Htm: " + System.Reflection.Assembly.GetAssembly(typeof(Spherical.Htm.Cover)).GetName().Version.ToString()};*/
        }

    }
}
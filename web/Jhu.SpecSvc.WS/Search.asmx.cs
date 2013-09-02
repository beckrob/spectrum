using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.WS
{
    /// <summary>
    /// Summary description for Search
    /// </summary>
    [WebService(Namespace = "http://voservices.net/spectrum")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Search : ServiceBase
    {
        public Search()
        {
        }

        [WebMethod]
        public Spectrum GetSpectrum(string userGuid, string spectrumId)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.GetSpectrum(userGuid == string.Empty ? Guid.Empty : new Guid(userGuid), spectrumId, true, null, true);
            }
        }

        [WebMethod]
        public Spectrum GetSpectrum_Details(string userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.GetSpectrum(userGuid == string.Empty ? Guid.Empty : new Guid(userGuid), spectrumId, loadPoints, pointsMask, loadDetails);
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Id(IdSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_All(AllSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Folder(FolderSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Cone(ConeSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Redshift(RedshiftSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Advanced(AdvancedSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Object(ObjectSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_HtmRange(HtmRangeSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Similar(SimilarSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_SkyServer(SkyServerSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Spectrum[] FindSpectrum_Sql(SqlSearchParameters par)
        {
            using (var cn = CreatePortalConnector())
            {
                return cn.FindSpectrum(par).ToArray();
            }
        }

        [WebMethod]
        public Collection[] QueryCollections(string userGuid)
        {
            using (var cn = CreatePortalConnector())
            {
                List<Collection> colls = new List<Collection>(cn.QueryCollections(userGuid == string.Empty ? Guid.Empty : new Guid(userGuid), SearchMethod.Unknown));

                foreach (Collection coll in colls)
                {
                    //if (coll.GraphUrl == string.Empty)
                    //coll.GraphUrl = System.Configuration.ConfigurationManager.AppSettings["DefaultGraphUrl"].Replace("[$HOSTNAME]", LocalhostName);
                }

                return colls.ToArray();
            }
        }

    }
}
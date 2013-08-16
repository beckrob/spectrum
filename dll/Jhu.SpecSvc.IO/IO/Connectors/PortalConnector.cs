using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    public class PortalConnector : ConnectorBase, IDisposable
    {
        #region Member variables

        private SqlConnection databaseConnection;
        private SqlTransaction databaseTransaction;

        #endregion
        #region Properties

        public SqlConnection DatabaseConnection
        {
            get { return this.databaseConnection; }
            set { this.databaseConnection = value; }
        }

        public SqlTransaction DatabaseTransaction
        {
            get { return this.databaseTransaction; }
            set { this.databaseTransaction = value; }
        }

        #endregion
        #region Constructors and initializers

        public PortalConnector()
        {
            InitializeMembers();
        }

        public PortalConnector(SqlConnection cn, SqlTransaction tn)
        {
            this.databaseConnection = cn;
            this.databaseTransaction = tn;
        }

        private void InitializeMembers()
        {
            this.databaseConnection = null;
            this.databaseTransaction = null;
        }

        public override void Dispose()
        {
        }

        #endregion
        #region Spectrum search functions

        public override Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            var coll = new Collection();
            coll.Id = GetCollectionId(spectrumId);
            LoadCollection(coll);

            using (var conn = coll.GetConnector())
            {
                Spectrum spec = conn.GetSpectrum(userGuid, spectrumId, loadPoints, pointsMask, loadDetails);
                PrefixCollectionId(coll.Id, spec);
                return spec;
            }
        }

        public override IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par)
        {
            var idsbycollection = new Dictionary<string, HashSet<string>>(StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0; i < par.Ids.Length; i++)
            {
                var cid = GetCollectionId(par.Ids[i]);

                if (!idsbycollection.ContainsKey(cid))
                {
                    idsbycollection.Add(cid, new HashSet<string>(StringComparer.InvariantCultureIgnoreCase));
                }

                if (!idsbycollection[cid].Contains(par.Ids[i]))
                {
                    idsbycollection[cid].Add(par.Ids[i]);
                }
            }

            // Run queries in parallel agains each collection
            Exceptions.Clear();

            // Create queue to buffer results as they arrive from the collections
            var buffer = new BlockingCollection<Spectrum>();

            // Query all collections in parallel on a separate thread
            var queries = Task.Factory.StartNew(FindSpectrumByIdWorker, new object[] { idsbycollection, par, buffer });

            // Read from the buffer
            while (!buffer.IsCompleted)
            {
                Spectrum s;
                if (buffer.TryTake(out s))
                {
                    yield return s;
                }
            }

            queries.Wait();


#if false
            // grouping parameters by collection to send requests in a batch
            string[] ids = new string[par.Ids.Length];
            string[] old = par.Ids;                     // original set of ids
            Array.Copy(par.Ids, ids, par.Ids.Length);
            Array.Sort<string>(ids);                    // original set of ids sorted

            List<Exception> exs = new List<Exception>();

            Collection coll = null;
            IEnumerable<Spectrum> temp = null;

            string collid = GetCollectionId(ids[0]);
            int a, b;
            a = b = 0;
            for (int i = 0; i <= ids.Length; i++)       // one more iteration!
            {
                if (i == ids.Length
                    || String.Compare(collid, GetCollectionId(ids[i]), true) != 0)
                {
                    // new collection starts here, the one in collid must be queried now
                    // ids in the array from a to b
                    // copying to the parameter object in order to send to the connector
                    par.Ids = new string[b - a + 1];
                    Array.Copy(ids, a, par.Ids, 0, par.Ids.Length);

                    try
                    {
                        coll = new Collection();
                        coll.Id = collid;
                        LoadCollection(coll);

                        //******* USING
                        ConnectorBase conn = coll.GetConnector();

                        temp = conn.FindSpectrum(par);
                    }
                    catch (System.Exception ex)
                    {
                        if (coll != null)
                            exs.Add(new System.Exception(coll.Name, ex));
                        else
                            exs.Add(new System.Exception("Unknown collection", ex));
                    }

                    if (temp != null)
                    {
                        foreach (Spectrum spec in temp)
                        {
                            PrefixCollectionId(coll.Id, spec);
                            yield return spec;
                        }
                    }

                    // set intervals
                    if (i < ids.Length)
                    {
                        a = b = i;
                        collid = GetCollectionId(ids[i]);
                    }
                }
                else
                {
                    b = i;
                }
            }

            this.exceptions = exs.ToArray();

            // restoring the original ids array in the parameter object
            par.Ids = old;
#endif
        }

        private void FindSpectrumByIdWorker(object state)
        {
            var idsbycollection = (Dictionary<string, HashSet<string>>)((object[])state)[0];
            var par = (IdSearchParameters)((object[])state)[1];
            var buffer = (BlockingCollection<Spectrum>)((object[])state)[2];

            Parallel.ForEach<string>(idsbycollection.Keys, cid =>
            {
                // load collection
                var coll = new Collection();
                coll.Id = cid;
                LoadCollection(coll);

                // Make copy of original parameters, but replace collection id and id list
                var idsp = new IdSearchParameters(par);

                lock (idsbycollection)
                {
                    idsp.Collections = new string[] { cid };
                    idsp.Ids = idsbycollection[cid].ToArray();
                }

                // Get connector
                using (var conn = coll.GetConnector())
                {
                    // Dispatch search
                    IEnumerable<Spectrum> temp = null;

                    // Execute search
                    temp = conn.FindSpectrum(idsp);

                    foreach (var s in temp)
                    {
                        // Prefix with collection id
                        PrefixCollectionId(coll.Id, s);

                        // Enqueue in output queue
                        bool queued = false;
                        while (!queued)
                        {
                            if (buffer.Count < 100)
                            {
                                buffer.Add(s);
                                queued = true;
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            });

            // All collections processed, now mark as done
            buffer.CompleteAdding();
        }

        public override IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public override IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        /*
        public override IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            // Load basis
            Basis basis = new Basis(true);
            basis.Id = par.BasisId;
            LoadBasis(basis);

            // Load templates
            IdSearchParameters ids = new IdSearchParameters(true);
            ids.UserGuid = par.UserGuid;
            ids.LoadPoints = true;
            ids.LoadDetails = true;
            ids.Ids = basis.FitParameters.TemplateList;

            List<Spectrum> templates =
                new List<Spectrum>(GetSpectrum(ids));

            // Preprocess spectrum
            SpectrumProcess.Preprocess(cn, tn, basis.PreprocessParameters, par.Spectrum);

            // Expand spectrum on the basis
            FitResults res = par.Spectrum.Fit(templates.ToArray(), basis.FitParameters);

            par.Coeffs = res.TemplateCoeffs;

            return FindSpectrum((SearchParametersBase)par);
        }
         * */

        public override IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par)
        {
            return FindSpectrumDispatch((SearchParametersBase)par);
        }

        public IEnumerable<Spectrum> FindSpectrum(SkyServerSearchParameters par)
        {
            Remote.CasJobs cj = new Remote.CasJobs();
            cj.Url = par.WsUrl;

            DataSet ds = cj.SubmitShortJob(par.WsId, par.Query, par.Target, "specsvcjob", 0);

            List<string> idlist = new List<string>(par.Collections.Length * ds.Tables[0].Rows.Count);

            foreach (string coll in par.Collections)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    idlist.Add(coll + "#" + dr["SpecObjID"].ToString());
                }
            }

            IdSearchParameters isp = new IdSearchParameters(true);
            isp.LoadDetails = par.LoadDetails;
            isp.LoadPoints = par.LoadPoints;
            isp.UserGuid = par.UserGuid;
            isp.Ids = idlist.ToArray();

            return FindSpectrumDispatch(isp);
        }

        public override IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par)
        {
            return null;
#if false
            //**** MODIFY TO RUN IN PARALLEL
            double ra, dec;

            par = par.GetStandardUnits();

            List<Spectrum> sps = new List<Spectrum>();

            // par.Objects contains a list of ra,dec pairs or object names
            for (int i = 0; i < par.Objects.Length; i++)
            {
                string obj = par.Objects[i];
                if (GetRaDecFromString(obj, out ra, out dec))
                {
                    ConeSearchParameters csp = new ConeSearchParameters(par);

                    csp.Pos.Value = new Jhu.SpecSvc.Schema.Position(ra, dec);
                    csp.Sr = par.Sr;

                    foreach (Spectrum s in FindSpectrum(csp))
                    {
                        if (par.Ids != null && par.Ids.Length > i)
                        {
                            s.DataId.MatchDID = new Jhu.SpecSvc.Schema.TextParam();
                            s.DataId.MatchDID.Value = par.Ids[i];
                        }

                        sps.Add(s);
                    }
                }
            }

            return sps.ToArray();
#endif
        }

        public override IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            throw new NotImplementedException();
        }

        //

        public IEnumerable<Spectrum> FindSpectrumDispatch(SearchParametersBase par)
        {
            Exceptions.Clear();

            // Create queue to buffer results as they arrive from the collections
            var buffer = new BlockingCollection<Spectrum>();

            // Query all collections in parallel on a separate thread
            var queries = Task.Factory.StartNew(FindSpectrumDispatchWorker, new object[] { par, buffer });

            // Read from the buffer
            while (!buffer.IsCompleted)
            {
                yield return buffer.Take();
            }

            queries.Wait();
        }

        private void FindSpectrumDispatchWorker(object state)
        {
            var par = (SearchParametersBase)((object[])state)[0];
            var buffer = (BlockingCollection<Spectrum>)((object[])state)[1];

            Parallel.For(0, par.Collections.Length, i =>
            {
                // load collection
                var coll = new Collection();
                coll.Id = par.Collections[i];
                LoadCollection(coll);

                // Get connector
                using (var conn = coll.GetConnector())
                {
                    // Dispatch search
                    IEnumerable<Spectrum> temp = null;

                    switch (par.Type)
                    {
                        case SearchMethods.Cone:
                            temp = conn.FindSpectrum((ConeSearchParameters)par);
                            break;
                        case SearchMethods.Redshift:
                            temp = conn.FindSpectrum((RedshiftSearchParameters)par);
                            break;
                        case SearchMethods.Advanced:
                            temp = conn.FindSpectrum((AdvancedSearchParameters)par);
                            break;
                        case SearchMethods.Model:
                            temp = conn.FindSpectrum((ModelSearchParameters)par);
                            break;
                        case SearchMethods.Folder:
                            temp = conn.FindSpectrum((FolderSearchParameters)par);
                            break;
                        case SearchMethods.All:
                            temp = conn.FindSpectrum((AllSearchParameters)par);
                            break;
                        case SearchMethods.HtmRange:
                            temp = conn.FindSpectrum((HtmRangeSearchParameters)par);
                            break;
                        case SearchMethods.Similar:
                            temp = conn.FindSpectrum((SimilarSearchParameters)par);
                            break;
                        case SearchMethods.Sql:
                            temp = conn.FindSpectrum((SqlSearchParameters)par);
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    foreach (var s in temp)
                    {
                        // Prefix with collection id
                        PrefixCollectionId(coll.Id, s);

                        // Enqueue in output queue
                        bool queued = false;
                        while (!queued)
                        {
                            if (buffer.Count < 100)
                            {
                                buffer.Add(s);
                                queued = true;
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            });

            // All collections processed, now mark as done
            buffer.CompleteAdding();
        }

        //

        public static bool TryParseFreeText(string query, out double ra, out double dec, bool useNed, bool useSimbad)
        {
            if (AstroUtil.TryParseRaDec(query, out ra, out dec))
            {
                return true;
            }

            // calling the NED service
            if (useNed)
            {
                try
                {
                    Remote.NED ned = new Remote.NED();

                    Remote.ObjInfo obj = ned.ObjByName(query);
                    ra = obj.ra;
                    dec = obj.dec;

                    return true;
                }
                catch (System.Web.Services.Protocols.SoapException)
                {

                }
            }

            // call Simbad
            // *** TODO

            return false;
        }

        private static string GetCollectionId(string id)
        {
            var idx = id.IndexOf('#');

            if (idx > 0)
            {
                return id.Substring(0, idx);
            }
            else
            {
                return id;
            }
        }

        private static void PrefixCollectionId(string collectionId, Spectrum spectrum)
        {
            spectrum.Curation.PublisherDID.Value = collectionId + "#" + spectrum.Id.ToString();
        }

        #endregion
        #region Resultset functions

        public int CreateResultset()
        {
            using (SqlCommand cmd = new SqlCommand("sp_CreateResultset", DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();

                return (int)cmd.Parameters["RETVAL"].Value;
            }
        }

        public void SaveResultsetSpectra(int resultsetId, IEnumerable<Spectrum> spectra)
        {
            // TODO: make it parallel?
            foreach (var s in spectra)
            {
                SaveResultsetSpectrum(resultsetId, s.GetCollectionId(), s);
            }
        }

        public void SaveResultsetSpectrum(int resultsetId, string collectionId, Spectrum spec)
        {
            string sql = "spCreateResult";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@CollectionID", SqlDbType.NVarChar, 128).Value = collectionId;
                cmd.Parameters.Add("@CreatorID", SqlDbType.NVarChar, 128).Value = spec.DataId.CreatorDID.Value;
                cmd.Parameters.Add("@PublisherID", SqlDbType.NVarChar, 128).Value = spec.Curation.PublisherDID.Value;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 128).Value = (spec.DataId.MatchDID == null || spec.DataId.MatchDID.Value == null) ? "" : spec.DataId.MatchDID.Value;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = spec.Target.Name.Value;
                cmd.Parameters.Add("@TargetClass", SqlDbType.NVarChar, 20).Value = spec.Target.Class.Value;
                cmd.Parameters.Add("@SpectralClass", SqlDbType.NVarChar, 20).Value = (spec.Target.SpectralClass == null) ? "" : spec.Target.SpectralClass.Value;
                cmd.Parameters.Add("@CreationType", SqlDbType.NVarChar, 20).Value = spec.DataId.CreationType.Value;
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = spec.DataId.Date.Value;
                cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 15).Value = (spec.DataId.Version == null) ? "" : spec.DataId.Version.Value;
                cmd.Parameters.Add("@Ra", SqlDbType.Float).Value = spec.Target.Pos.Value.Ra;
                cmd.Parameters.Add("@Dec", SqlDbType.Float).Value = spec.Target.Pos.Value.Dec;
                cmd.Parameters.Add("@Snr", SqlDbType.Float).Value = (spec.Derived.SNR != null) ? (object)spec.Derived.SNR.Value : DBNull.Value;
                cmd.Parameters.Add("@Redshift", SqlDbType.Float).Value = spec.Target.Redshift.Value;//******
                cmd.Parameters.Add("@RedshiftStatError", SqlDbType.Float).Value = spec.Derived.Redshift.StatError.Value;
                cmd.Parameters.Add("@RedshiftConfidence", SqlDbType.Float).Value = spec.Derived.Redshift.Confidence.Value;
                cmd.Parameters.Add("@VarAmpl", SqlDbType.Float).Value = (spec.Derived.VarAmpl != null) ? (object)spec.Derived.VarAmpl.Value : DBNull.Value;
                cmd.Parameters.Add("@SpectralCoverageStart", SqlDbType.Float).Value = spec.Data.SpectralAxis.Coverage.Bounds.Start.Value;
                cmd.Parameters.Add("@SpectralCoverageStop", SqlDbType.Float).Value = spec.Data.SpectralAxis.Coverage.Bounds.Stop.Value;
                cmd.Parameters.Add("@SpectralUnit", SqlDbType.NVarChar, 50).Value = spec.Data.SpectralAxis.Value.Unit;
                cmd.Parameters.Add("@SpectralResPower", SqlDbType.Float).Value = spec.Data.SpectralAxis.ResPower.Value;
                cmd.Parameters.Add("@FluxUnit", SqlDbType.NVarChar, 50).Value = spec.Data.FluxAxis.Value.Unit;
                cmd.Parameters.Add("@FluxCalibration", SqlDbType.NVarChar, 20).Value = spec.Data.FluxAxis.Calibration.Value;
                cmd.Parameters.Add("@Url", SqlDbType.NText).Value = (spec.Url == null) ? "" : spec.Url;

                cmd.ExecuteNonQuery();
            }
        }

        private IEnumerable<Spectrum> LoadResultsetsSpectraFromDataReader(SqlDataReader dr)
        {
            return LoadResultsetsSpectraFromDataReader(dr, -1, -1);
        }

        private IEnumerable<Spectrum> LoadResultsetsSpectraFromDataReader(SqlDataReader dr, int start, int count)
        {
            if (start > 0)
            {
                for (int i = 0; i < start; i++)
                {
                    dr.Read();
                }
            }

            int q = 0;
            while (dr.Read() && (q < count || count <= 0))
            {
                Spectrum spec = new Spectrum();
                spec.BasicInitialize();

                LoadResultsetSpectrumFromDataReader(spec, dr);

                yield return spec;

                q++;
            }
        }

        private void LoadResultsetSpectrumFromDataReader(Spectrum spec, SqlDataReader dr)
        {
            int o = -1;

            spec.ResultId = dr.GetInt64(++o); //id
            ++o; //resultsetId = dr.GetInt64(++o);
            ++o; //collectionId = dr.GetString(++o);
            spec.DataId.CreatorDID.Value = dr.GetString(++o);
            spec.Curation.PublisherDID.Value = dr.GetString(++o);

            int l = spec.Curation.PublisherDID.Value.IndexOf('#');
            if (l >= 0)
            {
                spec.PublisherId = spec.Curation.PublisherDID.Value.Substring(0, l);
            }

            if (!dr.IsDBNull(++o))
            {
                if (dr.GetString(o) != string.Empty)
                {
                    spec.DataId.MatchDID = new Jhu.SpecSvc.Schema.TextParam();
                    spec.DataId.MatchDID.Value = dr.GetString(o);
                }
                else
                    spec.DataId.MatchDID = null;
            }
            else
            {
                spec.DataId.MatchDID = null;
            }
            spec.Target.Name.Value = dr.GetString(++o);
            spec.Target.Class.Value = dr.GetString(++o);
            spec.Target.SpectralClass.Value = dr.GetString(++o);
            spec.DataId.CreationType.Value = dr.GetString(++o);
            spec.DataId.Date.Value = dr.GetDateTime(++o);
            spec.DataId.Version.Value = dr.GetString(++o);
            spec.Target.Pos.Value = new Jhu.SpecSvc.Schema.Position(dr.GetDouble(++o), dr.GetDouble(++o));

            spec.Derived.SNR.Value = dr.GetDouble(++o);
            spec.Derived.Redshift.Value.Value = dr.GetDouble(++o);
            spec.Derived.Redshift.StatError.Value = dr.GetDouble(++o);
            spec.Derived.Redshift.Confidence.Value = dr.GetDouble(++o);
            spec.Derived.VarAmpl.Value = dr.GetDouble(++o);

            spec.Target.Redshift.Value = spec.Derived.Redshift.Value.Value;
            spec.Target.VarAmpl.Value = spec.Derived.VarAmpl.Value;

            spec.Data.SpectralAxis.Coverage.Bounds.Start.Value = dr.GetDouble(++o);
            spec.Data.SpectralAxis.Coverage.Bounds.Stop.Value = dr.GetDouble(++o);
            spec.Data.SpectralAxis.Value.Unit = dr.GetString(++o);
            spec.Data.SpectralAxis.ResPower.Value = dr.GetDouble(++o);
            spec.Data.FluxAxis.Value.Unit = dr.GetString(++o);
            spec.Data.FluxAxis.Calibration.Value = dr.GetString(++o);
            spec.ResultSelected = dr.GetBoolean(++o); //spec.selected
            spec.Url = dr.GetString(++o);
        }

        /// <summary>
        /// Returns the number of results in a spectrum search resultset.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <returns>The number of results.</returns>
        public int GetResultsCount(int resultsetId)
        {
            return GetResultsCount(resultsetId, false);
        }

        /*
        /// <summary>
        /// Returns the number of results in a spectrum search resultset.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <param name="selectedOnly">True, if only count selected results</param>
        /// <returns>The number of results.</returns>
        public int GetResultsCount(int resultsetId, bool selectedOnly)
        {
            string sql = "spGetResultsCount";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@SelectedOnly", SqlDbType.Bit).Value = selectedOnly;

                int res = (int)cmd.ExecuteScalar();
                return res;
            }
        }*/

        /// <summary>
        /// Loads all selected spectra from a resultset. It creates connectors to the collections
        /// containing the individual spectra.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <param name="userGuid">ID of the user initiating the operation.</param>
        /// <param name="loadPoints">True, if spectral points are to be returned.</param>
        /// <param name="loadDetails">True, if detailed headers are to be returned.</param>
        /// <returns></returns>
        public IEnumerable<Spectrum> LoadSelectedResults(int resultsetId, Guid userGuid, bool loadPoints, bool loadDetails)
        {
            return LoadResults(resultsetId, true, userGuid, loadPoints, loadDetails);
        }

        public IEnumerable<Spectrum> LoadResults(int resultsetId, bool selectedOnly, Guid userGuid, bool loadPoints, bool loadDetails)
        {
            //*******************************************
            List<string[]> idlist = new List<string[]>(QueryResultIds(resultsetId, selectedOnly));

            IdSearchParameters ids = new IdSearchParameters();
            ids.Ids = new string[idlist.Count];
            for (int i = 0; i < idlist.Count; i++)
            {
                ids.Ids[i] = idlist[i][0];  // PublisherID
            }

            ids.LoadDetails = loadDetails;
            ids.LoadPoints = loadPoints;
            ids.UserGuid = userGuid;

            /*foreach (Spectrum spectrum in FindSpectrum(ids))
            {
                string[] ii = idlist.Find(delegate(string[] match) { return match[0] == spectrum.PublisherId; });
                spectrum.DataId.MatchDID = (ii == null || ii[1] == null) ? null : new Jhu.SpecSvc.Schema.TextParam(ii[1]);

                yield return spectrum;
            }*/


            return FindSpectrumDispatch(ids).Select(s =>
                {
                    // Update match ID
                    string[] ii = idlist.Find(delegate(string[] match) { return match[0] == s.PublisherId; });
                    s.DataId.MatchDID = (ii == null || ii[1] == null) ? null : new Jhu.SpecSvc.Schema.TextParam(ii[1]);

                    return s;
                });
        }

        public int GetResultsCount(int resultsetId, bool selectedOnly)
        {
            string sql = "spGetResultsCount";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@SelectedOnly", SqlDbType.Bit).Value = selectedOnly;
                cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();

                return (int)cmd.Parameters["RETVAL"].Value;
            }

        }

        /// <summary>
        /// Queries all spectrum headers from a given resultset.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <returns>Enumerator going through the spectrum headers.</returns>
        public IEnumerable<Spectrum> QueryResults(int resultsetId)
        {
            return QueryResults(resultsetId, -1, -1, false);
        }

        /// <summary>
        /// Queries selected spectrum headers from a given resultset.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <returns>Enumerator going through the spectrum headers.</returns>
        public IEnumerable<Spectrum> QuerySelectedResults(int resultsetId)
        {
            return QueryResults(resultsetId, 0, 0, true);
        }

        /// <summary>
        /// Queries spectrum headers from a given resultset.
        /// </summary>
        /// <param name="resultsetId">ID of the resultset.</param>
        /// <param name="start">Start index of paging.</param>
        /// <param name="count">Number of results to be returned, if 0 all results will be returned.</param>
        /// <param name="selectedOnly">Return only selected results.</param>
        /// <returns>Enumerator going through the spectrum headers.</returns>
        public IEnumerable<Spectrum> QueryResults(int resultsetId, int start, int count, bool selectedOnly)
        {
            string sql = "spQueryResults";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@SelectedOnly", SqlDbType.Bit).Value = selectedOnly;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (start >= 0 && count > 0)
                    {
                        foreach (Spectrum spectrum in LoadResultsetsSpectraFromDataReader(dr, start, count))
                            yield return spectrum;
                    }
                    else
                    {
                        foreach (Spectrum spectrum in LoadResultsetsSpectraFromDataReader(dr))
                            yield return spectrum;
                    }

                    dr.Close();
                }
            }
        }

        public IEnumerable<string[]> QuerySelectedResultIds(int resultsetId)
        {
            return QueryResultIds(resultsetId, true);
        }

        public IEnumerable<string[]> QueryResultIds(int resultsetId, bool selectedOnly)
        {
            List<string> res = new List<string>();

            string sql = "spQueryResultIds";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@SelectedOnly", SqlDbType.Bit).Value = selectedOnly;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new string[] { dr.GetString(0), dr.GetString(1) };

                    dr.Close();
                }
            }
        }

        public void DeleteResultset(int resultsetId)
        {
            string sql = "spDeleteResultset";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;

                cmd.ExecuteNonQuery();
            }
        }

        public void ChangeSelectionInResultset(int resultsetId, int resultId, bool selected)
        {
            string sql = "spChangeSelectionInResultset";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;
                cmd.Parameters.Add("@ResultID", SqlDbType.BigInt).Value = resultId;
                cmd.Parameters.Add("@Selected", SqlDbType.Bit).Value = selected;

                cmd.ExecuteNonQuery();
            }
        }

        public void SelectAllInResultset(int resultsetId)
        {
            string sql = "spSelectAllInResultset";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeselectAllInResultset(int resultsetId)
        {
            string sql = "spDeselectAllInResultset";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ResultsetID", SqlDbType.Int).Value = resultsetId;

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
        #region Collection functions

        public override IEnumerable<Collection> QueryCollections(Guid userGuid, SearchMethods searchMethod)
        {
            string sql = "sp_FindCollections";

            using (SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = userGuid;
                cmd.Parameters.Add("@SearchMethod", SqlDbType.Int).Value = (int)searchMethod;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Collection cl = new Collection();
                        LoadCollectionFromDataReader(cl, dr);
                        yield return cl;
                    }
                }
            }
        }

        private void LoadCollectionFromDataReader(Collection coll, SqlDataReader dr)
        {
            int o = -1;

            coll.Id = dr.GetString(++o);
            coll.UserGuid = dr.IsDBNull(++o) ? Guid.Empty : dr.GetGuid(o);
            coll.Type = (CollectionTypes)dr.GetInt32(++o);
            coll.LoadDefaults = dr.GetInt32(++o);
            coll.Name = dr.GetString(++o);
            coll.Description = dr.GetString(++o);
            coll.Location = dr.GetString(++o);
            coll.ConnectionString = dr.GetString(++o);
            coll.GraphUrl = dr.GetString(++o);
            coll.Public = dr.GetInt32(++o);
        }

        public void LoadCollection(Collection coll)
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetCollection", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 50).Value = coll.Id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    LoadCollectionFromDataReader(coll, dr);
                    dr.Close();
                }
            }
        }

        public override void SaveCollection(Collection collection, string oldId, Guid userGuid)
        {
            collection.UserGuid = userGuid;
            if (oldId != "")
                ModifyCollection(collection, oldId);
            else
                CreateCollection(collection);

            SaveCollectionSearchMethods(collection);
        }

        private void CreateCollection(Collection coll)
        {
            using (SqlCommand cmd = new SqlCommand("sp_CreateCollection", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AppendCollectionCreateModifyParameters(cmd, coll);
                cmd.ExecuteNonQuery();
            }
        }

        private void ModifyCollection(Collection coll, string oldId)
        {
            using (SqlCommand cmd = new SqlCommand("sp_ModifyCollection", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OldID", SqlDbType.NVarChar, 50).Value = oldId;
                AppendCollectionCreateModifyParameters(cmd, coll);
                cmd.ExecuteNonQuery();
            }
        }

        private void AppendCollectionCreateModifyParameters(SqlCommand cmd, Collection coll)
        {
            cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 50).Value = coll.Id;
            cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = coll.UserGuid;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = coll.Type;
            cmd.Parameters.Add("@LoadDefaults", SqlDbType.Int).Value = coll.LoadDefaults;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = coll.Name;
            cmd.Parameters.Add("@Description", SqlDbType.NText).Value = coll.Description;
            cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 50).Value = coll.Location;
            cmd.Parameters.Add("@ConnectionString", SqlDbType.NVarChar, 255).Value = coll.ConnectionString;
            cmd.Parameters.Add("@GraphUrl", SqlDbType.NVarChar, 255).Value = coll.GraphUrl;
            cmd.Parameters.Add("@Public", SqlDbType.Int).Value = coll.Public;
        }

        public override void DeleteCollection(Collection collection, Guid userGuid)
        {
            collection.UserGuid = userGuid;

            if (CheckCollectionAccess(collection))
            {
                DeleteCollectionSearchMethods(collection);

                using (SqlCommand cmd = new SqlCommand("sp_DeleteCollection", databaseConnection, databaseTransaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 50).Value = collection.Id;
                    cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = collection.UserGuid;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckDuplicateCollectionId(string oldId, string newId)
        {
            string sql = "SELECT COUNT(*) FROM Collections WHERE ID = @ID";
            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 50).Value = newId;

                if ((int)cmd.ExecuteScalar() > 0)
                {
                    if (oldId != newId)
                        return false;	// duplicate
                    else
                        return true;	// no duplicate
                }
                else
                {
                    return true;  // no duplicate
                }
            }
        }

        public void QueryCollectionSearchMethods(Collection collection)
        {
            collection.SearchMethods.Clear();

            string sql = "SELECT SearchMethodID FROM CollectionSearchMethods WHERE CollectionID = @CollectionID";

            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.Parameters.Add("@CollectionID", SqlDbType.NVarChar, 50).Value = collection.Id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        collection.SearchMethods.Add((SearchMethods)dr.GetInt32(0));
                    }

                    dr.Close();
                }
            }
        }

        private bool CheckCollectionAccess(Collection collection)
        {
            string sql = "SELECT COUNT(*) FROM Collections WHERE ID = @CollectionID AND UserGUID = @UserGUID";

            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.Parameters.Add("@CollectionID", SqlDbType.NVarChar, 50).Value = collection.Id;
                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = collection.UserGuid;

                return ((int)cmd.ExecuteScalar()) == 1;
            }
        }

        private void DeleteCollectionSearchMethods(Collection collection)
        {
            string sql = @"DELETE CollectionSearchMethods
FROM CollectionSearchMethods
INNER JOIN Collections ON Collections.ID = CollectionSearchMethods.CollectionID
WHERE UserGUID = @UserGUID";

            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = collection.UserGuid;
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveCollectionSearchMethods(Collection collection)
        {
            if (CheckCollectionAccess(collection))
            {

                // Delete old items
                DeleteCollectionSearchMethods(collection);

                string sql = "INSERT CollectionSearchMethods (CollectionID, SearchMethodID) VALUES (@CollectionID, @SearchMethodID)";
                using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
                {
                    cmd.Parameters.Add("@CollectionID", SqlDbType.NVarChar, 50).Value = collection.Id;
                    cmd.Parameters.Add("@SearchMethodID", SqlDbType.Int);

                    foreach (SearchMethods s in collection.SearchMethods)
                    {
                        cmd.Parameters["@SearchMethodID"].Value = (int)s;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public Dictionary<int, string> QuerySearchMethods()
        {
            Dictionary<int, string> res = new Dictionary<int, string>();

            string sql = "SELECT SearchMethods.* FROM SearchMethods ORDER BY ID";
            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        res.Add(dr.GetInt32(0), dr.GetString(1));
                    }
                    dr.Close();
                }
            }

            return res;
        }

        #endregion
        #region TemplateSet function

        private void LoadTemplateSetFromDataReader(TemplateSet ts, SqlDataReader dr)
        {
            ts.Id = dr.GetInt32(0); // (int)dr["ID"];
            ts.Name = dr.GetString(1); // (string)dr["Name"];
        }

        public void LoadTemplateSet(TemplateSet ts, int id)
        {
            ts.Id = id;
            LoadTemplateSet(ts);
        }

        public void LoadTemplateSet(TemplateSet ts)
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetTemplateSet", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ts.Id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    LoadTemplateSetFromDataReader(ts, dr);
                    dr.Close();
                }
            }
        }

        public string[] QueryTemplates(TemplateSet ts)
        {
            return QueryTemplates(ts.Id);
        }

        public string[] QueryTemplates(int id)
        {
            using (SqlCommand cmd = new SqlCommand("sp_QueryTemplates", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                List<string> res = new List<string>();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        res.Add(dr.GetString(1));

                    dr.Close();
                }
                return res.ToArray();
            }
        }

        public TemplateSet[] QueryTemplateSets()
        {
            using (SqlCommand cmd = new SqlCommand("sp_QueryTemplateSets", databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                List<TemplateSet> res = new List<TemplateSet>();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        TemplateSet ts = new TemplateSet(true);
                        LoadTemplateSetFromDataReader(ts, dr);
                        res.Add(ts);
                    }

                    dr.Close();
                }
                return res.ToArray();
            }
        }


        #endregion
        #region Basis functions

        /*
        public IEnumerable<Basis> QueryBases(Guid userGuid)
        {
            string sql = "sp_QuerySpectrumBases";

            using (SqlCommand cmd = new SqlCommand(sql, cn, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = userGuid;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Basis bas = new Basis();
                        LoadBasisFromDataReader(bas, dr);
                        yield return bas;
                    }
                }
            }
        }
         * */

        /*
        private void LoadBasisFromDataReader(Basis bas, SqlDataReader dr)
        {
            int o = -1;

            bas.Id = dr.GetString(++o);
            bas.UserGuid = dr.IsDBNull(++o) ? Guid.Empty : dr.GetGuid(o);
            bas.Public = dr.GetInt32(++o);
            bas.Name = dr.GetString(++o);
            bas.Description = dr.GetString(++o);

            XmlSerializer ser = new XmlSerializer(typeof(PreprocessParameters));
            bas.PreprocessParameters = (PreprocessParameters)ser.Deserialize(new StringReader(dr.GetString(++o)));

            ser = new XmlSerializer(typeof(FitParameters));
            bas.FitParameters = (FitParameters)ser.Deserialize(new StringReader(dr.GetString(++o)));

        }
         * */

        /*
        public void LoadBasis(Basis bas)
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetSpectrumBasis", cn, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 128).Value = bas.Id;
                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = bas.UserGuid;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    LoadBasisFromDataReader(bas, dr);
                    dr.Close();
                }
            }
        }
         * */

        /*
        public void SaveBasis(Basis bas, string oldId)
        {
            if (oldId != "")
                ModifyBasis(bas, oldId);
            else
                CreateBasis(bas);
        }

        private void CreateBasis(Basis bas)
        {
            using (SqlCommand cmd = new SqlCommand("sp_CreateSpectrumBasis", cn, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AppendBasisCreateModifyParameters(cmd, bas);
                cmd.ExecuteNonQuery();
            }
        }

        private void ModifyBasis(Basis bas, string oldId)
        {
            using (SqlCommand cmd = new SqlCommand("sp_ModifySpectrumBasis", cn, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OldID", SqlDbType.NVarChar, 128).Value = oldId;
                AppendBasisCreateModifyParameters(cmd, bas);
                cmd.ExecuteNonQuery();
            }
        }

        private void AppendBasisCreateModifyParameters(SqlCommand cmd, Basis bas)
        {
            cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 128).Value = bas.Id;
            cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = bas.UserGuid;
            cmd.Parameters.Add("@Public", SqlDbType.Int).Value = bas.Public;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = bas.Name;
            cmd.Parameters.Add("@Description", SqlDbType.NText).Value = bas.Description;

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(PreprocessParameters));
            ser.Serialize(sw, bas.PreprocessParameters);

            cmd.Parameters.Add("@PreprocessParameters", SqlDbType.NText).Value = sw.ToString();

            sw = new System.IO.StringWriter();
            ser = new System.Xml.Serialization.XmlSerializer(typeof(FitParameters));
            ser.Serialize(sw, bas.FitParameters);

            cmd.Parameters.Add("@FitParameters", SqlDbType.NText).Value = sw.ToString();
        }


        public void DeleteBasis(Basis bas)
        {
            using (SqlCommand cmd = new SqlCommand("sp_DeleteSpectrumBasis", cn, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 50).Value = bas.Id;
                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = bas.UserGuid;

                cmd.ExecuteNonQuery();
            }
        }

        public bool CheckDuplicateBasisId(string oldId, string newId)
        {
            string sql = "SELECT COUNT(ID) FROM SpectrumBases WHERE ID = @ID";
            using (SqlCommand cmd = new SqlCommand(sql, cn, tn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 128).Value = newId;

                if ((int)cmd.ExecuteScalar() > 0)
                {
                    if (oldId != newId)
                        return false;	// duplicate
                    else
                        return true;	// no duplicate
                }
                else
                {
                    return true;  // no duplicate
                }
            }
        }
        */

        #endregion
        #region Misc functions

        public void QuerySpectralLines(out string[] titles, out double[] wavelengths)
        {
            List<string> t = new List<string>();
            List<double> w = new List<double>();

            string sql = "SELECT * FROM Lines ORDER BY Wavelength";

            using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        t.Add(dr.GetString(1));
                        w.Add(dr.GetFloat(2));
                    }
                }
            }

            titles = t.ToArray();
            wavelengths = w.ToArray();
        }

        public double GetExtinction(double ra, double dec)
        {
            double sr = 10;

            while (true)
            {
                string sql = "SELECT Dust.ext FROM Dust WHERE " + GetHtmRanges(ra, dec, sr);
                object ext = null;

                using (SqlCommand cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
                {
                    ext = cmd.ExecuteScalar();
                }

                if (ext != null)
                    return (double)(float)ext;

                sr += 10;
            }
        }

        private static string GetHtmRanges(double ra, double dec, double sr)
        {
            string where = "";

            Spherical.Region r = new Spherical.Region(new Spherical.Halfspace(ra, dec, sr));
            r.Simplify();
            Spherical.Htm.Cover c = new Spherical.Htm.Cover(r);
            c.SetTunables(20, 40, 15);
            c.Run();

            foreach (Spherical.Htm.Int64Pair pair in c.GetPairs(Spherical.Htm.Markup.Outer))
            {
                where += " OR HTMID BETWEEN " + pair.lo.ToString() + " AND " + pair.hi.ToString();
            }

            if (where != "")
                return where.Substring(4);  // strip of the first OR
            else
                return "";
        }

        #endregion
    }
}
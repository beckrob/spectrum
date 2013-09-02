#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SqlConnector.cs,v 1.5 2008/10/30 16:22:25 dobos Exp $
 *   Revision:    $Revision: 1.5 $
 *   Date:        $Date: 2008/10/30 16:22:25 $
 */
#endregion
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    /// <summary>
    /// Summary description for Sql2SpectrumConnector.
    /// </summary>
    public class SqlConnector : ConnectorBase, IDisposable
    {
        #region Member variables

        private string connectionString;
        private string publisherId;

        private DbConnection databaseConnection;
        private DbTransaction databaseTransaction;
        private bool isConnectionOwned;
        private bool isTransactionOwned;

        #endregion
        #region Properties

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        /// Gets or sets the database connection context object
        /// </summary>
        public DbConnection DatabaseConnection
        {
            get { return this.databaseConnection; }
            set
            {
                Close();
                this.databaseConnection = value;
                this.isConnectionOwned = false;
            }
        }

        /// <summary>
        /// Gets or sets the database transaction context object
        /// </summary>
        public DbTransaction DatabaseTransaction
        {
            get { return this.databaseTransaction; }
            set
            {
                Close();
                this.databaseTransaction = value;
                this.isTransactionOwned = false;
            }
        }

        /// <summary>
        /// IVOA publisher ID associated with a Collection (ie. ivo://elte/sdss)
        /// All spectrum IDs are going to be prefixed with this when the database is queried
        /// </summary>
        public string PublisherId
        {
            get { return publisherId; }
            set { publisherId = value; }
        }

        #endregion
        #region Constructors and initializers

        /// <summary>
        /// Default constructor, sets connection to null
        /// </summary>
        public SqlConnector()
        {
            InitializeMembers();
        }

        /// <summary>
        /// Constructor to initialize connection and transaction context
        /// </summary>
        /// <param name="cn">Database connection</param>
        /// <param name="tn">Already started transaction on the same connection</param>
        public SqlConnector(DbConnection cn, DbTransaction tn)
        {
            InitializeMembers();

            this.databaseConnection = cn;
            this.databaseTransaction = tn;
            if (cn != null) isConnectionOwned = false;
            if (tn != null) isTransactionOwned = false;
        }

        /// <summary>
        /// Constructor to initialize object according to a Collection. It opens database connection
        /// and starts a transaction too.
        /// </summary>
        /// <param name="collection">Spectrum Collection (dataset)</param>
        public SqlConnector(Collection collection)
        {
            if (collection.Type != CollectionType.Sql)
                throw new System.Exception("Not valid collection type");

            InitializeMembers();

            this.connectionString = collection.ConnectionString;
            this.collectionId = collection.Id;

            Open();
        }

        /// <summary>
        /// Initializes the member variables to their default values.
        /// </summary>
        private void InitializeMembers()
        {
            this.connectionString = string.Empty;
            this.collectionId = string.Empty;
            this.publisherId = string.Empty;

            this.databaseConnection = null;
            this.databaseTransaction = null;
        }

        public override void Dispose()
        {
            Close();
        }

        #endregion

        public void Open()
        {
            if (connectionString.ToLower().Contains("data source"))
            {
                this.databaseConnection = new System.Data.SqlClient.SqlConnection(connectionString);
            }
            else if (connectionString.ToLower().Contains("server"))
            {
                this.databaseConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            }
            else
            {
                throw new IOException("Server type cannot be inferred from connection string.");
            }

            this.databaseConnection.Open();
            this.isConnectionOwned = true;

            this.databaseTransaction = this.databaseConnection.BeginTransaction();
            this.isTransactionOwned = true;
        }

        public void Close()
        {
            if (isTransactionOwned && databaseTransaction != null)
            {
                try
                {
                    databaseTransaction.Commit();    // Might be commited already
                }
                finally
                {
                    databaseTransaction.Dispose();
                }
            }

            if (isConnectionOwned && databaseConnection != null)
            {
                try
                {
                    databaseConnection.Close();
                }
                finally
                {
                    databaseConnection.Dispose();
                }
            }
        }

        #region Search functions

        /// <summary>
        /// Loads a single spectrum from the database identified by its ID
        /// </summary>
        /// <param name="userGuid">User identifier</param>
        /// <param name="spectrumId">IVOA id of the spectrum, ie. ivo://elte/sdss/dr6/spec/2.5#12345677</param>
        /// <param name="loadPoints">Determines if data points are returned</param>
        /// <param name="loadDetails">Determines if detailed info is returned</param>
        /// <returns></returns>
        public override Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            Spectrum spec = new Spectrum(false);
            spec.BasicInitialize();

            LoadSpectrum(spec, userGuid, Spectrum.GetId(spectrumId), loadPoints, pointsMask, loadDetails);

            return spec;
        }

        /// <summary>
        /// Gets multiple spectra according to the contents of the IdSearchParameters class
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par)
        {
            Exceptions.Clear();
            par = par.GetStandardUnits();

            var res = par.Ids.AsParallel().Select(id =>
            {
                Spectrum spec = new Spectrum(false);
                spec.BasicInitialize();

                try
                {
                    LoadSpectrum(spec, par.UserGuid, Spectrum.GetId(id), par.LoadPoints, par.PointsMask, par.LoadDetails);
                    return spec;
                }
                catch (Exception ex)
                {
                    Exceptions.Add(ex);
                    return null;
                }
            });

            return res.AsSequential();

            // Delete if code above works
            /*
            if (parallelized)
            {
                ParallelExecuter<string, object, Spectrum> pe = new ParallelExecuter<string, object, Spectrum>();
                pe.Parameters = null; //ppar;
                pe.InQueue = par.Ids;

                pe.WorkerFunction = delegate(string id, object x)
                {
                    Spectrum spec = new Spectrum(false);
                    spec.BasicInitialize();

                    if (LoadSpectrum(spec, par.UserGuid, Spectrum.GetId(id), par.LoadPoints, par.LoadDetails))
                        return spec;
                    else
                        return null;
                };

                foreach (Spectrum s in pe.RunParallelizer())
                    yield return s;
            }
            else
            {
             *
                foreach (string id in par.Ids)
                {
                    Spectrum spec = new Spectrum(false);
                    spec.BasicInitialize();

                    if (LoadSpectrum(spec, par.UserGuid, Spectrum.GetId(id), par.LoadPoints, par.LoadDetails))
                        yield return spec;
                }
            //}
             * */
        }

        /// <summary>
        /// Returns spectra according to the contents of the ConeSearchParameters class
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par)
        {
            // TODO: it's based on coarse HTM cover now. Exact region cuts are necessary

            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE ({0})
      AND (UserGUID = @UserGUID OR [Public] > 0)
";

            par = par.GetStandardUnits();

            string where = string.Empty;
            where = GetHtmRanges(par.Pos.Value.Ra, par.Pos.Value.Dec, par.Sr.Value);
            if (where == string.Empty)
            {
                throw new IOException("Unable to construct cone HTM cover.");
            }

            sql = String.Format(sql, where);

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        /// <summary>
        /// Returns spectra according to the contents of the RedshiftSearchParameters class
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE Redshift BETWEEN @RedshiftFrom AND @RedshiftTo
      AND (UserGUID = @UserGUID OR [Public] > 0)
";

            par = par.GetStandardUnits();

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);
                AddCommandParameter(cmd, "@RedshiftFrom", DbType.Double, null, par.Redshift.Min.Value);
                AddCommandParameter(cmd, "@RedshiftTo", DbType.Double, null, par.Redshift.Max.Value);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        /// <summary>
        /// Returns spectra according to the contents of the FolderSearchParameters class.
        /// This method is used to retrieve spectra from a folder of the user database (MySpectra), not used with
        /// large survey datasets.
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE (UserFolderID = @UserFolderID OR @UserFolderID IS NULL)
      AND (UserGUID = @UserGUID)";

            par = par.GetStandardUnits();

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);
                AddCommandParameter(cmd, "@UserFolderID", DbType.Int32, null, (par.FolderId == -1) ? DBNull.Value : (object)par.FolderId);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        /// <summary>
        /// Returns spectra according to the contents of the AdvancedSearchParameters class.
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE ({0})
      AND (UserGUID = @UserGUID OR [Public] > 0)";

            par = par.GetStandardUnits();

            var where = string.Empty;

            if (par.Keyword != null && par.Keyword != string.Empty)
                where += " AND ([Name] LIKE '%" + par.Keyword.Replace("'", string.Empty) + "%')";
            if (par.Keyword != null && par.Name != string.Empty)
                where += " AND ([Name] LIKE '%" + par.Name.Replace("'", string.Empty) + "%')";

            if (par.TargetClass != null && par.TargetClass.Length > 0)
            {
                where += " AND TargetClass IN (";
                foreach (string tc in par.TargetClass) where += "'" + tc.Trim().Replace("'", string.Empty) + "'";
                where += ") ";
            }

            if (par.SpectralClass != null && par.SpectralClass.Length > 0)
            {
                where += " AND SpectralClass IN (";
                foreach (string tc in par.SpectralClass) where += "'" + tc.Trim().Replace("'", string.Empty) + "'";
                where += ") ";
            }

            if (par.CreationType != null && par.CreationType.Length > 0)
            {
                where += " AND CreationType IN (";
                foreach (string tc in par.CreationType) where += "'" + tc.Trim().Replace("'", string.Empty) + "'";
                where += ") ";
            }

            if (par.Date != null && par.Date.Start != null)
                where += " AND (Date >= #" + par.Date.Start.Value.ToString() + "#)";
            if (par.Date != null && par.Date.Stop != null)
                where += " AND (Date <= #" + par.Date.Stop.Value.ToString() + "#)";


            if (par.Version != null && par.Version != string.Empty)
                where += " AND (Version LIKE '" + par.Version.Replace("'", string.Empty).Replace("*", "%") + "')";

            // cone search
            if (par.Pos != null && par.Sr != null)
            {
                string htm = GetHtmRanges(par.Pos.Value.Ra, par.Pos.Value.Dec, par.Sr.Value);
                where += " AND (" + htm + ")";
            }

            if (par.Snr != null && par.Snr.Min != null)
                where += " AND (Snr >= " + par.Snr.Min.Value.ToString() + ")";
            if (par.Snr != null && par.Snr.Max != null)
                where += " AND (Snr <= " + par.Snr.Max.Value.ToString() + ")";

            if (par.VarAmpl != null && par.VarAmpl.Min != null)
                where += " AND (VarAmpl >= " + par.VarAmpl.Min.Value.ToString() + ")";
            if (par.VarAmpl != null && par.VarAmpl.Max != null)
                where += " AND (VarAmpl <= " + par.VarAmpl.Max.Value.ToString() + ")";


            if (par.Redshift != null && par.Redshift.Min != null)
                where += " AND (Redshift >= " + par.Redshift.Min.Value.ToString() + ")";
            if (par.Redshift != null && par.Redshift.Max != null)
                where += " AND (Redshift <= " + par.Redshift.Max.Value.ToString() + ")";

            if (par.RedshiftStatError != null && par.RedshiftStatError.Min != null)
                where += " AND (RedshiftStatError >= " + par.RedshiftStatError.Min.Value.ToString() + ")";
            if (par.RedshiftStatError != null && par.RedshiftStatError.Max != null)
                where += " AND (RedshiftStatError <= " + par.RedshiftStatError.Max.Value.ToString() + ")";

            if (par.RedshiftConfidence != null && par.RedshiftConfidence.Min != null)
                where += " AND (RedshiftConfidence >= " + par.RedshiftConfidence.Min.Value.ToString() + ")";
            if (par.RedshiftConfidence != null && par.RedshiftConfidence.Max != null)
                where += " AND (RedshiftConfidence <= " + par.RedshiftConfidence.Max.Value.ToString() + ")";

            if (par.SpectralCoverage != null && par.SpectralCoverage.Min != null)
                where += " AND (SpectralCoverageStart >= " + par.SpectralCoverage.Min.Value.ToString() + ")";
            if (par.SpectralCoverage != null && par.SpectralCoverage.Max != null)
                where += " AND (SpectralCoverageStop <= " + par.SpectralCoverage.Max.Value.ToString() + ")";

            if (par.SpectralResPower != null && par.SpectralResPower.Min != null)
                where += " AND (SpectralResPower >= " + par.SpectralResPower.Min.Value.ToString() + ")";
            if (par.SpectralResPower != null && par.SpectralResPower.Max != null)
                where += " AND (SpectralResPower <= " + par.SpectralResPower.Max.Value.ToString() + ")";

            if (par.FluxCalibration != null && par.FluxCalibration.Length > 0)
            {
                where += " AND FluxCalibration IN (";
                foreach (string fc in par.FluxCalibration) where += "'" + fc.Trim().Replace("'", string.Empty) + "'";
                where += ") ";
            }

            sql = String.Format(sql, where.Substring(5));

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        public override IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
INNER JOIN SpectrumModelParameters ON Spectra.ID = SpectrumModelParameters.SpectrumID
WHERE ({0})
      AND (UserGUID = @UserGUID OR [Public] > 0)";
            
            par = par.GetStandardUnits();

            string where = string.Empty;

            if (par.Z_met != null && par.Z_met.Min != null)
                where += " AND (Z_met >= " + par.Z_met.Min.Value.ToString() + ")";
            if (par.Z_met != null && par.Z_met.Max != null)
                where += " AND (Z_met <= " + par.Z_met.Max.Value.ToString() + ")";

            if (par.T_eff != null && par.T_eff.Min != null)
                where += " AND (T_eff >= " + par.T_eff.Min.Value.ToString() + ")";
            if (par.T_eff != null && par.T_eff.Max != null)
                where += " AND (T_eff <= " + par.T_eff.Max.Value.ToString() + ")";

            if (par.Log_g != null && par.Log_g.Min != null)
                where += " AND (Log_g >= " + par.Log_g.Min.Value.ToString() + ")";
            if (par.Log_g != null && par.Log_g.Max != null)
                where += " AND (Log_g <= " + par.Log_g.Max.Value.ToString() + ")";

            if (par.Tau_V0 != null && par.Tau_V0.Min != null)
                where += " AND (Tau_V0 >= " + par.Tau_V0.Min.Value.ToString() + ")";
            if (par.Tau_V0 != null && par.Tau_V0.Max != null)
                where += " AND (Tau_V0 <= " + par.Tau_V0.Max.Value.ToString() + ")";

            if (par.Mu != null && par.Mu.Min != null)
                where += " AND (Mu >= " + par.Mu.Min.Value.ToString() + ")";
            if (par.Mu != null && par.Mu.Max != null)
                where += " AND (Mu <= " + par.Mu.Max.Value.ToString() + ")";

            if (par.T_form != null && par.T_form.Min != null)
                where += " AND (T_form >= " + par.T_form.Min.Value.ToString() + ")";
            if (par.T_form != null && par.T_form.Max != null)
                where += " AND (T_form <= " + par.T_form.Max.Value.ToString() + ")";

            if (par.Gamma != null && par.Gamma.Min != null)
                where += " AND (Gamma >= " + par.Gamma.Min.Value.ToString() + ")";
            if (par.Gamma != null && par.Gamma.Max != null)
                where += " AND (Gamma <= " + par.Gamma.Max.Value.ToString() + ")";

            if (par.N_bursts != null && par.N_bursts.Min != null)
                where += " AND (N_bursts >= " + par.N_bursts.Min.Value.ToString() + ")";
            if (par.N_bursts != null && par.N_bursts.Max != null)
                where += " AND (N_bursts <= " + par.N_bursts.Max.Value.ToString() + ")";

            if (par.Age != null && par.Age.Min != null)
                where += " AND (Age >= " + par.Age.Min.Value.ToString() + ")";
            if (par.Age != null && par.Age.Max != null)
                where += " AND (Age <= " + par.Age.Max.Value.ToString() + ")";

            if (par.Age_lastBurst != null && par.Age_lastBurst.Min != null)
                where += " AND (Age_lastBurst >= " + par.Age_lastBurst.Min.Value.ToString() + ")";
            if (par.Age_lastBurst != null && par.Age_lastBurst.Max != null)
                where += " AND (Age_lastBurst <= " + par.Age_lastBurst.Max.Value.ToString() + ")";

            sql = String.Format(sql, where);

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        /// <summary>
        /// Returns spectra according to the contents of the HtmRangeSearchParameters class.
        /// This method is used to retrieve spectra covered by a region on the sky described by its
        /// HTM intervals.
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE ({0})
      AND (UserGUID = @UserGUID OR [Public] > 0)";

            par = par.GetStandardUnits();

            string where = string.Empty;
            foreach (HtmRangeSearchParameters.HtmRange pair in par.Ranges)
            {
                where += " OR HTMID BETWEEN " + pair.Lo.ToString() + " AND " + pair.Hi.ToString();
            }

            sql = String.Format(sql, where.Substring(4));

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        /// <summary>
        /// Returns all (or a fraction of all) spectra from the dataset
        /// </summary>
        /// <param name="par">Parameter class</param>
        /// <returns></returns>
        public override IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra {0}
WHERE (UserGUID = @UserGUID OR [Public] > 0) AND ID > 0";

            par = par.GetStandardUnits();

            string tablesample = "";
            if (databaseConnection.GetType() == typeof(System.Data.SqlClient.SqlConnection) && par.SampleFraction != 1.0f)
            {
                tablesample = "TABLESAMPLE (" + (par.SampleFraction * 100).ToString() + " PERCENT)";
            }

            sql = String.Format(sql, tablesample);

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        public override IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par)
        {
            var sql = @"
SELECT *
FROM ({0}) s
WHERE (UserGUID = @UserGUID OR [Public] > 0) AND ID <> 0
ORDER BY ID
";

            par = par.GetStandardUnits();

            sql = String.Format(sql, par.Query);

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                cmd.CommandTimeout = 600;

                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        public override IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            string sql = @"
SELECT Spectra.*
FROM Spectra 
INNER JOIN dbo.NearestNeighbors('grid1', @coeffs, @count) nn
ON nn.Id = Spectra.ID
WHERE (UserGUID = @UserGUID OR [Public] > 0)";

            par = par.GetStandardUnits();

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@coeffs", DbType.Binary, null, VectorConverter.ToBinary(par.Coeffs));
                AddCommandParameter(cmd, "@count", DbType.Int32, null, par.ResultsCount);
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, par.UserGuid);

                return LoadSpectraFromCommand(cmd, par.UserGuid, par.LoadPoints, par.PointsMask, par.LoadDetails);
            }
        }

        public override IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Spectrum load functions

        private Spectrum LoadSpectrum(Guid userGuid, long id, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            Spectrum spec = new Spectrum(false);
            spec.BasicInitialize();

            LoadSpectrum(spec, userGuid, id, loadPoints, pointsMask, loadDetails);
            return spec;
        }

        public override void LoadSpectrum(Spectrum spec, Guid userGuid, long id, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            var sql = @"
SELECT Spectra.*
FROM Spectra
WHERE ID = @ID AND (UserGUID = @UserGUID OR [Public] > 0)";

            try
            {
                using (DbCommand cmd = CreateTextCommand(sql))
                {
                    AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, userGuid);
                    AddCommandParameter(cmd, "@ID", DbType.Int64, null, id);

                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            LoadSpectrumFromReader(spec, dr);
                        }
                    }
                }

                if (loadDetails)
                {
                    // store publisher id
                    string pubid = spec.Curation.PublisherDID.Value;
                    LoadSpectrumFields(spec, userGuid, true);
                    spec.Curation.PublisherDID.Value = pubid;
                }

                if (loadPoints)
                    LoadSpectrumData(spec, userGuid, pointsMask);

                PrefixCollectionId(spec);
            }
            catch (Exception ex)
            {
                throw new IOException("Error loading spectrum.", ex); // *** TODO
            }
        }

        private void LoadSpectrumFromReader(Spectrum spec, DbDataReader dr)
        {
            int o = -1;

            spec.PublisherId = this.publisherId;

            spec.Id = dr.GetInt64(++o);
            spec.UserGuid = dr.IsDBNull(++o) ? Guid.Empty : dr.GetGuid(o);
            spec.UserFolderId = dr.GetInt32(++o);
            spec.Public = dr.GetInt32(++o);

            spec.DataId.CreatorDID.Value = dr.GetString(++o);
            spec.Target.Name.Value = dr.GetString(++o);
            spec.Target.Class.Value = dr.GetString(++o);
            spec.Target.SpectralClass.Value = dr.GetString(++o);

            spec.DataId.CreationType.Value = dr.GetString(++o);
            spec.DataId.Date.Value = dr.GetDateTime(++o);
            spec.DataId.Version.Value = dr.GetString(++o);

            spec.Target.Pos.Value = new Position(dr.GetDouble(++o), dr.GetDouble(++o));
            spec.Data.SpatialAxis.Coverage.Location.Value.Value = spec.Target.Pos.Value;

            spec.HtmId = dr.GetInt64(++o);

            spec.Derived.SNR.Value = dr.GetDouble(++o);
            spec.Target.Redshift.Value = dr.GetDouble(++o);
            spec.Derived.Redshift.Value.Value = spec.Target.Redshift.Value;
            spec.Derived.Redshift.StatError.Value = dr.GetDouble(++o);
            spec.Derived.Redshift.Confidence.Value = dr.GetDouble(++o);

            spec.Derived.VarAmpl.Value = dr.GetDouble(++o);

            spec.Data.SpectralAxis.Coverage.Bounds.Start.Value = dr.GetDouble(++o);
            spec.Data.SpectralAxis.Coverage.Bounds.Stop.Value = dr.GetDouble(++o);
            spec.Data.SpectralAxis.Value.Unit = dr.GetString(++o);
            spec.Data.SpectralAxis.ResPower.Value = dr.GetDouble(++o);

            spec.Data.FluxAxis.Value.Unit = dr.GetString(++o);
            spec.Data.FluxAxis.Calibration.Value = dr.GetString(++o);
        }

        public override void LoadSpectrumFields(Spectrum spec, Guid userGuid)
        {
            LoadSpectrumFields(spec, userGuid, spec.Id);
        }

        public void LoadSpectrumFields(Spectrum spec, Guid userGuid, bool loadDefaults)
        {
            if (loadDefaults)
            {
                LoadSpectrumFields(spec, userGuid, 0);
            }

            LoadSpectrumFields(spec, userGuid, spec.Id);
        }

        protected void LoadSpectrumFields(Spectrum spec, Guid userGuid, long id)
        {
            var sql = @"
SELECT SpectrumFields.*
FROM SpectrumFields
WHERE SpectrumID = @ID";

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, userGuid);
                AddCommandParameter(cmd, "@ID", DbType.Int64, null, id);

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        try
                        {
                            string name = dr.GetString(0);
                            Jhu.SpecSvc.Schema.ParamBase param = null;

                            name = name.Substring("Spectrum".Length + 1);
                            param = (Jhu.SpecSvc.Schema.ParamBase)Jhu.SpecSvc.Schema.Spectrum.Spectrum.GetField(spec, name, dr.GetInt16(2));

                            if (param == null)
                            {
                                // **** Console.WriteLine("!");
                            }
                            else
                            {
                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.TextParam))
                                    param.SetValue(dr.IsDBNull(3) ? null : dr.GetString(3));

                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.IntParam))
                                    param.SetValue(dr.GetInt64(4));

                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.DoubleParam))
                                    param.SetValue(dr.GetDouble(5));

                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.TimeParam))
                                    param.SetValue(dr.GetDateTime(7));

                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.PositionParam))
                                {
                                    ((PositionParam)param).Value =
                                        new Position(dr.GetDouble(5), dr.GetDouble(6));
                                }

                                if (param.GetType() == typeof(Jhu.SpecSvc.Schema.BoolParam))
                                    param.SetValue(dr.GetInt64(4) > 0);

                                param.Unit = dr.IsDBNull(9) ? null : dr.GetString(9);
                                param.Ucd = dr.IsDBNull(10) ? null : dr.GetString(10);
                                param.Key = dr.IsDBNull(11) ? null : dr.GetString(11);
                            }
                        }
                        catch (System.InvalidCastException)
                        {
                            // field name found but types differ
                        }
                        catch (System.NullReferenceException)
                        {
                            // field not found
                        }
                    }
                }
            }
        }

        private DbCommand CreateLoadSpectrumDataCommand(string[] pointsMask)
        {
            var sql = @"
SELECT SpectrumData.*
FROM SpectrumData
WHERE SpectrumID = @ID";

            if (pointsMask != null)
            {
                sql += " AND FieldName IN (";

                for (int i = 0; i < pointsMask.Length; i++)
                {
                    if (i > 0)
                    {
                        sql += ",";
                    }
                    
                    sql += "'" + pointsMask[i] + "'";
                }

                sql += ")";
            }

            return CreateTextCommand(sql);
        }

        public override void LoadSpectrumData(Spectrum spec, Guid userGuid, string[] pointsMask)
        {
            using (DbCommand cmd = CreateLoadSpectrumDataCommand(pointsMask))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.Guid, null, userGuid);
                AddCommandParameter(cmd, "@ID", DbType.Int64, null, spec.Id);

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // dr["FieldName"] holds the name of the variable to load data into
                        string name = dr.GetString(0);
                        name = name.Substring(name.LastIndexOf(".") + 1);

                        FieldInfo fld = spec.GetType().GetField(name);
                        using (BinaryReader buffer = GetBinaryFromReader(dr, 3))
                        {
                            if (fld.FieldType == typeof(double[]))
                            {
                                double[] val = new double[buffer.BaseStream.Length / sizeof(double)];
                                byte[] temp = new byte[buffer.BaseStream.Length];

                                buffer.Read(temp, 0, temp.Length);
                                Buffer.BlockCopy(temp, 0, val, 0, temp.Length);

                                fld.SetValue(spec, val);
                            }
                            else if (fld.FieldType == typeof(long[]))
                            {
                                long[] val = new long[buffer.BaseStream.Length / sizeof(long)];
                                byte[] temp = new byte[buffer.BaseStream.Length];

                                buffer.Read(temp, 0, temp.Length);
                                Buffer.BlockCopy(temp, 0, val, 0, temp.Length);

                                fld.SetValue(spec, val);
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<Spectrum> LoadSpectraFromCommand(DbCommand cmd, Guid userGuid, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            var res = LoadSpectraFromCommand(cmd).AsParallel().WithDegreeOfParallelism(4).Select(s =>
            {
                try
                {
                    // store publisher id
                    if (loadDetails)
                    {
                        string pubid = s.Curation.PublisherDID.Value;
                        LoadSpectrumFields(s, userGuid, true);
                        s.Curation.PublisherDID.Value = pubid;
                    }

                    if (loadPoints)
                        LoadSpectrumData(s, userGuid, pointsMask);

                    PrefixCollectionId(s);

                    return s;
                }
                catch (Exception ex)
                {
                    Exceptions.Add(ex);
                    return null;
                }
            });

            return res.AsSequential();
        }

        private IEnumerable<Spectrum> LoadSpectraFromCommand(DbCommand cmd)
        {
            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    Spectrum spec;

                    spec = new Spectrum();
                    spec.BasicInitialize();

                    LoadSpectrumFromReader(spec, dr);
                    PrefixCollectionId(spec);

                    yield return spec;
                }
            }
        }

        #endregion
        #region Spectrum save functions

        public override long SaveSpectrum(Spectrum spec, Guid userGuid)
        {
            //*** user access check
            spec.CalculateHtmId();
            spec.UserGuid = userGuid;

            if (spec.Id <= 0)
                CreateSpectrum(spec, userGuid);
            else
            {
                ModifySpectrum(spec, userGuid);
                DeleteSpectrumFields(spec, userGuid);
                DeleteSpectrumData(spec, userGuid);
            }

            SaveSpectrumFields(spec, userGuid);
            SaveSpectrumData(spec, userGuid);

            return spec.Id;
        }

        public void CreateSpectrum(Spectrum spec, Guid userGuid)
        {
            string sql = null;

            if (spec.Id == 0)
                sql = @"spCreateSpectrum";
            else
                sql = @"spCreateSpectrum_ID";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                if (spec.Id != 0)
                    AddCommandParameter(cmd, "@ID", DbType.Int64, -1, spec.Id);

                AppendCreateModifyParameters(cmd, spec);

                if (spec.Id == 0)
                {
                    AddCommandParameter(cmd, "@NewID", DbType.Int64, null, null);
                    GetCommandParameter(cmd, "@NewID").Direction = ParameterDirection.Output;
                }

                cmd.ExecuteNonQuery();

                if (spec.Id == 0)
                    spec.Id = (long)GetCommandParameter(cmd, "@NewID").Value;
            }
        }

        private void ModifySpectrum(Spectrum spec, Guid userGuid)
        {
            string sql = "spModifySpectrum";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int64, -1, spec.Id);

                AppendCreateModifyParameters(cmd, spec);

                cmd.ExecuteNonQuery();
            }
        }

        private void AppendCreateModifyParameters(DbCommand cmd, Spectrum spec)
        {
            AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, spec.UserGuid.ToString());
            AddCommandParameter(cmd, "@UserFolderId", DbType.Int32, -1, spec.UserFolderId);
            AddCommandParameter(cmd, "@Public", DbType.Int32, -1, spec.Public);
            AddCommandParameter(cmd, "@CreatorID", DbType.String, 128, DbUtil.DBNull(spec.DataId.CreatorDID.Value));
            AddCommandParameter(cmd, "@Name", DbType.String, 50, DbUtil.DBNull(spec.Target.Name.Value));
            AddCommandParameter(cmd, "@TargetClass", DbType.String, 20, DbUtil.DBNull(spec.Target.Class.Value));
            AddCommandParameter(cmd, "@SpectralClass", DbType.String, 20, DbUtil.DBNull(spec.Target.SpectralClass.Value));
            AddCommandParameter(cmd, "@CreationType", DbType.String, 20, DbUtil.DBNull(spec.DataId.CreationType.Value));
            AddCommandParameter(cmd, "@Date", DbType.DateTime, -1, DbUtil.DBNull(spec.DataId.Date.Value));
            AddCommandParameter(cmd, "@Version", DbType.String, 20, DbUtil.DBNull(spec.DataId.Version.Value));
            AddCommandParameter(cmd, "@Ra", DbType.Double, -1, spec.Target.Pos.Value.Ra);
            AddCommandParameter(cmd, "@Dec", DbType.Double, -1, spec.Target.Pos.Value.Dec);
            AddCommandParameter(cmd, "@HTMID", DbType.Int64, -1, spec.HtmId);
            AddCommandParameter(cmd, "@Snr", DbType.Double, -1, DbUtil.DBNull(spec.Derived.SNR.Value));
            AddCommandParameter(cmd, "@Redshift", DbType.Double, -1, DbUtil.DBNull(spec.Derived.Redshift.Value.Value));
            AddCommandParameter(cmd, "@RedshiftStatError", DbType.Double, -1, DbUtil.DBNull(spec.Derived.Redshift.StatError.Value));
            AddCommandParameter(cmd, "@RedshiftConfidence", DbType.Double, -1, DbUtil.DBNull(spec.Derived.Redshift.Confidence.Value));
            AddCommandParameter(cmd, "@VarAmpl", DbType.Double, -1, DbUtil.DBNull(spec.Derived.VarAmpl.Value));
            AddCommandParameter(cmd, "@SpectralCoverageStart", DbType.Double, -1, DbUtil.DBNull(spec.Data.SpectralAxis.Coverage.Bounds.Start.Value));
            AddCommandParameter(cmd, "@SpectralCoverageStop", DbType.Double, -1, DbUtil.DBNull(spec.Data.SpectralAxis.Coverage.Bounds.Stop.Value));
            AddCommandParameter(cmd, "@SpectralUnit", DbType.String, 50, DbUtil.DBNull(spec.Data.SpectralAxis.Value.Unit));
            AddCommandParameter(cmd, "@SpectralResPower", DbType.Double, -1, DbUtil.DBNull(spec.Data.SpectralAxis.ResPower.Value));
            AddCommandParameter(cmd, "@FluxUnit", DbType.String, 50, DbUtil.DBNull(spec.Data.FluxAxis.Value.Unit));
            AddCommandParameter(cmd, "@FluxCalibration", DbType.String, 20, DbUtil.DBNull(spec.Data.FluxAxis.Calibration.Value));
        }

        public override void DeleteSpectrum(Spectrum spec, Guid userGuid)
        {
            string sql = "spDeleteSpectrum";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int64, null, spec.Id);
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, userGuid.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteSpectrumFields(Spectrum spec, Guid userGuid)
        {
            string sql = "DELETE SpectrumFields WHERE SpectrumID = @ID";

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int64, null, spec.Id);

                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteSpectrumData(Spectrum spec, Guid userGuid)
        {
            string sql = "DELETE SpectrumData WHERE SpectrumID = @ID";

            using (DbCommand cmd = CreateTextCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int64, null, spec.Id);

                cmd.ExecuteNonQuery();
            }
        }

        //

        public void SaveSpectrumFields(Spectrum spec, Guid userGuid)
        {
            string sql = "spSaveSpectrumFields";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@FieldName", DbType.String, 128, null);
                AddCommandParameter(cmd, "@SpectrumId", DbType.Int64, -1, spec.Id);
                AddCommandParameter(cmd, "@DataType", DbType.Int16, null, null);
                AddCommandParameter(cmd, "@Value_String", DbType.String, 255, null);
                AddCommandParameter(cmd, "@Value_Int", DbType.Int64, null, null);
                AddCommandParameter(cmd, "@Value_Double", DbType.Double, null, null);
                AddCommandParameter(cmd, "@Value_Double2", DbType.Double, null, null);
                AddCommandParameter(cmd, "@Value_Time", DbType.Date, null, null);
                AddCommandParameter(cmd, "@Value_Time2", DbType.Date, null, null);
                AddCommandParameter(cmd, "@Unit", DbType.String, 50, null);
                AddCommandParameter(cmd, "@Ucd", DbType.String, 50, null);
                AddCommandParameter(cmd, "@Key", DbType.String, 50, null);

                SaveSpectrumFields_Group(cmd, spec, "Spectrum");
            }
        }

        private void SaveSpectrumFields_Group(DbCommand cmd, object obj, string name)
        {
            // adding parameters and groups
            if (obj != null)
            {
                foreach (FieldInfo field in obj.GetType().GetFields())
                {
                    try
                    {
                        // ***** correct and remove
                        // throws exception wheren Axis.Ucd is get and value equals to null
                        SaveSpectrumField(cmd, field.FieldType, field.GetValue(obj), name + "." + field.Name);
                    }
                    catch (System.Exception)
                    {
                    }
                }

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        // ***** correct and remove
                        // throws exception wheren Axis.Ucd is get and value equals to null
                        SaveSpectrumField(cmd, prop.PropertyType, prop.GetValue(obj, Type.EmptyTypes), name + "." + prop.Name);
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }
        }

        private void SaveSpectrumField(DbCommand cmd, Type type, object value, string name)
        {
            if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.ParamBase)))
            {
                SaveSpectrumFields_Param(cmd, value, name);
            }
            else if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.Group)))
            {
                SaveSpectrumFields_Group(cmd, value, name);
            }
            else if (type == typeof(Jhu.SpecSvc.Schema.ParamCollection))
            {
                SaveSpectrumFields_ParamCollection(cmd, (ParamCollection)value, name);
            }
        }

        private void SaveSpectrumFields_ParamCollection(DbCommand cmd, ParamCollection coll, string name)
        {
            if (coll != null)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    ParamBase par = (ParamBase)coll[i];
                    SaveSpectrumFields_Param(cmd, par, name + "." + par.Key);
                }
            }
        }

        private void SaveSpectrumFields_Param(DbCommand cmd, object par, string name)
        {
            ResetSpectrumFieldCommand(cmd);

            GetCommandParameter(cmd, "@FieldName").Value = name;

            ParamBase param = (ParamBase)par;

            if (param != null)
            {
                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.TextParam))
                {
                    GetCommandParameter(cmd, "@Value_String").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.TextParam)param).Value);
                    GetCommandParameter(cmd, "@DataType").Value = 0;
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.IntParam))
                {
                    GetCommandParameter(cmd, "@Value_Int").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.IntParam)param).Value);
                    GetCommandParameter(cmd, "@DataType").Value = 1;
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.DoubleParam))
                {
                    GetCommandParameter(cmd, "@Value_Double").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.DoubleParam)param).Value);
                    GetCommandParameter(cmd, "@DataType").Value = 2;
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.TimeParam))
                {
                    GetCommandParameter(cmd, "@Value_Time").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.TimeParam)param).Value);
                    GetCommandParameter(cmd, "@DataType").Value = 3;
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.PositionParam))
                {
                    GetCommandParameter(cmd, "@Value_Double").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.PositionParam)param).Value.Ra);
                    GetCommandParameter(cmd, "@Value_Double2").Value = DbUtil.DBNull(((Jhu.SpecSvc.Schema.PositionParam)param).Value.Dec);
                    GetCommandParameter(cmd, "@DataType").Value = 4;
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.BoolParam))
                {
                    GetCommandParameter(cmd, "@Value_Int").Value = DbUtil.DBNull(Convert.ToInt64(((Jhu.SpecSvc.Schema.TimeParam)param).Value));
                    GetCommandParameter(cmd, "@DataType").Value = 5;
                }

                GetCommandParameter(cmd, "@Unit").Value = DbUtil.DBNull(param.Unit);
                GetCommandParameter(cmd, "@Ucd").Value = DbUtil.DBNull(param.Ucd);
                GetCommandParameter(cmd, "@Key").Value = DbUtil.DBNull(param.Key);

                cmd.ExecuteNonQuery();
            }
        }

        public void SaveSpectrumData(Spectrum spec, Guid userGuid)
        {
            string sql = "spSaveSpectrumData";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {

                AddCommandParameter(cmd, "@FieldName", DbType.String, 128, null);
                AddCommandParameter(cmd, "@SpectrumId", DbType.Int64, -1, spec.Id);
                AddCommandParameter(cmd, "@DataType", DbType.Int16, null, null);
                AddCommandParameter(cmd, "@Data", DbType.Binary, null, null);

                foreach (FieldInfo field in spec.GetType().GetFields())
                {
                    if (field.FieldType == typeof(double[]) ||
                        field.FieldType == typeof(long[]))
                    {
                        SaveSpectrumData_Array(cmd, field, spec, field.Name);
                    }
                }
            }
        }

        private void SaveSpectrumData_Array(DbCommand cmd, FieldInfo field, object obj, string name)
        {
            GetCommandParameter(cmd, "@FieldName").Value = name;

            Array data = (Array)field.GetValue(obj);

            if (data != null)
                if (data.Length > 0)
                {
                    MemoryStream buffer = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(buffer);

                    if (field.FieldType == typeof(long[]))
                    {
                        GetCommandParameter(cmd, "@DataType").Value = 0;

                        long[] a = (long[])data;
                        for (int i = 0; i < a.Length; i++)
                            writer.Write(a[i]);
                    }

                    if (field.FieldType == typeof(double[]))
                    {
                        GetCommandParameter(cmd, "@DataType").Value = 1;

                        double[] a = (double[])data;
                        for (int i = 0; i < a.Length; i++)
                            writer.Write(a[i]);
                    }

                    GetCommandParameter(cmd, "@Data").Value = buffer.ToArray();

                    cmd.ExecuteNonQuery();
                }
        }

        private void ResetSpectrumFieldCommand(DbCommand cmd)
        {
            GetCommandParameter(cmd, "@Value_String").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Value_Int").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Value_Double").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Value_Double2").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Value_Time").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Value_Time2").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Unit").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Ucd").Value = DBNull.Value;
            GetCommandParameter(cmd, "@Key").Value = DBNull.Value;
        }

        #endregion

        #region Utility functions

        private string GetHtmRanges(double ra, double dec, double sr)
        {
            string where = string.Empty;

            Spherical.Region r = new Spherical.Region(new Spherical.Halfspace(ra, dec, sr));
            r.Simplify();
            Spherical.Htm.Cover c = new Spherical.Htm.Cover(r);
            c.SetTunables(20, 40, 15);
            c.Run();

            foreach (Spherical.Htm.Int64Pair pair in c.GetPairs(Spherical.Htm.Markup.Outer))
            {
                where += " OR HTMID BETWEEN " + pair.lo.ToString() + " AND " + pair.hi.ToString();
            }

            if (where != string.Empty)
                return where.Substring(4);  // strip of the first OR
            else
                return string.Empty;
        }

        // static util methods
        protected static System.Type GetBaseType(System.Type type)
        {
            System.Type nt = type;
            while (nt != typeof(System.Object))
            {
                if (nt.BaseType == typeof(System.Object) ||
                    nt.BaseType == typeof(System.Collections.CollectionBase) ||
                    nt.BaseType == typeof(System.Collections.Specialized.NameObjectCollectionBase))
                    return nt;
                nt = nt.BaseType;
            }
            return nt;
        }

        private DbCommand CreateStoredProcCommand(string sql)
        {
            DbCommand cmd = databaseConnection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = databaseTransaction;

            return cmd;
        }

        private DbCommand CreateTextCommand(string sql)
        {
            DbCommand cmd = databaseConnection.CreateCommand();

            if (databaseConnection.GetType() == typeof(System.Data.SqlClient.SqlConnection))
            {
                cmd.CommandText = sql;
            }
            else if (databaseConnection.GetType() == typeof(MySql.Data.MySqlClient.MySqlConnection))
            {
                cmd.CommandText = sql.Replace('[', '`').Replace(']', '`').Replace('@', '?');
            }

            cmd.CommandType = CommandType.Text;
            cmd.Transaction = databaseTransaction;

            return cmd;
        }

        private void AddCommandParameter(DbCommand cmd, string name, DbType type, int? size, object value)
        {
            DbParameter par = cmd.CreateParameter();
            par.DbType = type;

            if (databaseConnection.GetType() == typeof(System.Data.SqlClient.SqlConnection))
            {
                par.ParameterName = name;
            }
            else if (databaseConnection.GetType() == typeof(MySql.Data.MySqlClient.MySqlConnection))
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                    par.ParameterName = name.Replace('@', '_');
                else
                    par.ParameterName = name.Replace('@', '?');
            }

            if (size.HasValue && size.Value != -1) par.Size = size.Value;
            par.Value = value;

            cmd.Parameters.Add(par);
        }

        private DbParameter GetCommandParameter(DbCommand cmd, string name)
        {
            if (cmd.GetType() == typeof(System.Data.SqlClient.SqlCommand))
            {
                return cmd.Parameters[name];
            }
            else if (cmd.GetType() == typeof(MySql.Data.MySqlClient.MySqlCommand))
            {
                return cmd.Parameters[name.Replace('@', '_')];
            }

            return null;
        }

        private BinaryReader GetBinaryFromReader(DbDataReader dr, int index)
        {
            if (dr is System.Data.SqlClient.SqlDataReader)
            {
                System.Data.SqlClient.SqlDataReader sdr = (System.Data.SqlClient.SqlDataReader)dr;
                return new BinaryReader(sdr.GetSqlBytes(index).Stream);
            }
            else
            {
                return new BinaryReader(new MemoryStream((byte[])dr.GetValue(index)));
            }
        }

        #endregion

        #region MySpectrum functions

        public override UserFolder GetUserFolder(Guid userGuid, int id)
        {
            string sql = "spGetUserFolder";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int32, -1, id);
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, userGuid.ToString());

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();

                    UserFolder folder = new UserFolder(true);
                    LoadUserFolderFromDataReader(folder, dr);

                    return folder;
                }
            }
        }

        public override UserFolder[] QueryUserFolders(Guid userGuid)
        {
            string sql = "spQueryUserFolders";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, userGuid.ToString());

                List<UserFolder> folders = new List<UserFolder>();
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        UserFolder f = new UserFolder(true);
                        LoadUserFolderFromDataReader(f, dr);
                        folders.Add(f);
                    }
                }

                // root

                sql = @"spGetRootUserFolder";
                cmd.CommandText = sql;

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();

                    UserFolder f = new UserFolder(true);
                    LoadUserFolderFromDataReader(f, dr);
                    folders.Insert(0, f);
                }

                return folders.ToArray();
            }
        }

        public void LoadUserFolderFromDataReader(UserFolder folder, DbDataReader dr)
        {
            //***** kitakarítani, ha mûködik
            int o = -1;

            folder.Id = dr.GetInt32(++o);   // Convert.ToInt32(dr["Id"]);
            folder.UserGuid = dr.IsDBNull(++o) ? Guid.Empty : new Guid(dr.GetGuid(o).ToString());
            folder.Name = dr.GetString(++o); // (string)dr["Name"];
            folder.PublisherId = dr.GetString(++o); // (string)dr["PublisherID"];
            folder.Count = dr.IsDBNull(++o) ? 0 : dr.GetInt32(o);
            //if (dr["Cnt"] == DBNull.Value)
            //    folder.Count = 0;
            //else
            //    folder.Count = Convert.ToInt32(dr["Cnt"]);
        }

        public override void SaveUserFolder(UserFolder folder, Guid userGuid)
        {
            folder.UserGuid = userGuid;
            if (folder.Id <= 0)
                CreateUserFolder(folder);
            else
            {
                ModifyUserFolder(folder);
            }
        }

        private void CreateUserFolder(UserFolder folder)
        {
            string sql = "spCreateUserFolder";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@Name", DbType.String, -1, folder.Name);
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, folder.UserGuid.ToString());
                AddCommandParameter(cmd, "@PublisherID", DbType.StringFixedLength, 50, folder.PublisherId);

                AddCommandParameter(cmd, "@NewID", DbType.Int32, null, null);
                GetCommandParameter(cmd, "@NewID").Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                folder.Id = (int)GetCommandParameter(cmd, "@NewID").Value;
            }
        }

        private void ModifyUserFolder(UserFolder folder)
        {
            string sql = "spModifyUserFolder";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int32, -1, folder.Id);
                AddCommandParameter(cmd, "@Name", DbType.String, -1, folder.Name);
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, folder.UserGuid.ToString());
                AddCommandParameter(cmd, "@PublisherID", DbType.StringFixedLength, 50, folder.PublisherId);

                cmd.ExecuteNonQuery();
            }
        }

        public override void DeleteUserFolder(UserFolder folder, Guid userGuid)
        {
            string sql = "spDeleteUserFolder";

            using (DbCommand cmd = CreateStoredProcCommand(sql))
            {
                AddCommandParameter(cmd, "@ID", DbType.Int32, null, folder.Id);
                AddCommandParameter(cmd, "@UserGUID", DbType.StringFixedLength, 36, userGuid.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SqlConnector.cs,v $
        Revision 1.5  2008/10/30 16:22:25  dobos
        *** empty log message ***

        Revision 1.4  2008/10/27 20:17:37  dobos
        *** empty log message ***

        Revision 1.3  2008/10/25 18:26:22  dobos
        *** empty log message ***

        Revision 1.2  2008/09/11 10:45:00  dobos
        Bugfixes and parallel execution added to PortalConnector

        Revision 1.1  2008/01/08 22:01:36  dobos
        Initial checkin


*/
#endregion
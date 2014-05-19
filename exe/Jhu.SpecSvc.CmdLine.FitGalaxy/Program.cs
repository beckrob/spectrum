using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Util;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;
using Jhu.SpecSvc.FilterLib;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Jhu.SpecSvc.CmdLine.FitGalaxy
{
    class Program
    {
        // Connection string
        const string portcstr = @"data source=retdb01;initial catalog=SpectrumPortal3.1;multipleactiveresultsets=true;Integrated Security=true";
        const string speccstr = @"data source=retdb01;initial catalog=DR7Spectrum;multipleactiveresultsets=true;Integrated Security=true";
        const string tempcstr = @"data source=retdb01;initial catalog=SpectrumTemplates3.1;multipleactiveresultsets=true;Integrated Security=true";

        //const string rescstr = @"data source=maxwell\sql2008;initial catalog=DR7SpectrumFitsTest;multipleactiveresultsets=true;Integrated Security=true";
        //const string rescstr = @"data source=retdb01;initial catalog=dobos;multipleactiveresultsets=true;Integrated Security=true";
        const string rescstr = @"data source=retdb01;initial catalog=Dr7SpectrumFits;multipleactiveresultsets=true;Integrated Security=true";

        //const string rescstr = @"data source=localhost;initial catalog=GalaxyFitSmall;multipleactiveresultsets=true;Integrated Security=true";

        // Templates DB, required for continuum fitting
        static string mstr = "Data Source=RETDB01;Integrated Security=true;Initial Catalog=SpectrumTemplates3.1;MultipleActiveResultsets=true";

        //static List<List<Spectrum>> templates = new List<List<Spectrum>>();
        static double[] metallicity = new double[] { 0.004, 0.008, 0.02, 0.05 };

        static object syncRoot = new object();

        static long atlasmask = (long)(PointMask.NoPlug |
                PointMask.BadTrace |
                PointMask.BadFlat |
                PointMask.BadArc |
                PointMask.Manybadcol |
                PointMask.ManyReject |
                PointMask.LowFlat |
                PointMask.FullReject |
                PointMask.ScatLight |
                PointMask.CrossTalk |
                PointMask.NoSky |
            //PointMask.BrightSky |
                PointMask.NoData |
                PointMask.CombineRej |
                PointMask.BadFluxFactor |
                PointMask.BadSkyChi |
                PointMask.RedMonster);

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            MathNet.Numerics.Control.LinearAlgebraProvider = new MathNet.Numerics.Algorithms.LinearAlgebra.ManagedLinearAlgebraProvider();
            //MathNet.Numerics.Control.LinearAlgebraProvider = new MathNet.Numerics.Algorithms.LinearAlgebra.Acml.AcmlLinearAlgebraProvider();
            //MathNet.Numerics.Control.LinearAlgebraProvider = new MathNet.Numerics.Algorithms.LinearAlgebra.Mkl.MklLinearAlgebraProvider();

            DateTime start = DateTime.Now;

            //GalaxyFitFiles("../../../output/*.spec.txt", 3400, 8800);

#if false
            string filt = String.Format(" AND ID % {0} = {1}", args[0], args[1]);
            GalaxyFit("SELECT s.* FROM Spectra s WHERE SpectralClass = 'GALAXY' AND ID IN (SELECT ID FROM DR7SpectrumFits..Missing)" + filt, 3000, 9000);
            //GalaxyFit("SELECT s.* FROM Spectra s WHERE ID IN (75093974913646592,75094093696335872,75094094900101120,75094095168536576,75375474548670464,75375475333005312,75375475584663552,75657055888736256,75657056090062848,75657056559824896,75657056748568576)", 3000, 9000);
            //GalaxyFit("SELECT s.* FROM Spectra s WHERE ID = 79034667633737728", 3000, 9000);

            // bad
            //GalaxyFit("SELECT s.* FROM Spectra s WHERE ID IN (75093975341465600)", 3000, 9000);


            //ModelFlux(args);
            //EWUpdate(args);
            //Main_Photo(args);

            //ExportModelsWithLines("SELECT * FROM Spectra");

            //LickIndices();

            //ContinuumPCA("SELECT $TOP$ s.* FROM Spectra s WHERE Redshift BETWEEN 0.05 AND 0.1", 200, 1000000, 3);
            //ContinuumPCA("SELECT $TOP$ s.* FROM Spectra s", 200, 1000000, 3);

            Console.WriteLine((DateTime.Now - start).TotalSeconds);
            Console.Beep();
            //Console.ReadLine();
#endif

            GalaxyFit(new string[] { @"C:\Data\dobos\temp\20140519\Dougie_host_restfr.dat" }, 3000, 6500);

        }

        static void LickIndices()
        {
            SqlConnection portcn = new SqlConnection(portcstr);
            portcn.Open();
            SqlTransaction porttn = portcn.BeginTransaction();

            using (SqlConnection cn = new SqlConnection(speccstr))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction())
                {
                    SqlConnector speccn = new SqlConnector(cn, tn);

                    SqlSearchParameters ssp = new SqlSearchParameters(true);
                    ssp.LoadDetails = false;
                    ssp.LoadPoints = true;

                    ssp.Query = @"
";

                    SpectrumPipeline ph = new SpectrumPipeline(new PortalConnector(portcn, porttn));
                    //ph.SkipExceptions = false;

                    DereddenStep dered = new DereddenStep();
                    ph.Steps.Add(dered);

                    RedshiftStep reds = new RedshiftStep();
                    reds.Method = RedshiftStep.RedshiftMethod.RestFrame;
                    ph.Steps.Add(reds);

                    SpectralIndexStep idxs = new SpectralIndexStep();
                    ph.Steps.Add(idxs);


                    // -----------

                    SqlConnection rescn = new SqlConnection(rescstr);
                    rescn.Open();
                    SqlTransaction restn = rescn.BeginTransaction();

                    string cmdsql = @"
INSERT ContinuumIndices (SpectrumID {0})
VALUES (@SpectrumID {1})";

                    string pars1 = "";
                    string pars2 = "";
                    for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
                    {
                        pars1 += String.Format(", {0}", Constants.SpectralIndexDefinitions[i].Name);
                        pars2 += String.Format(", @{0}", Constants.SpectralIndexDefinitions[i].Name);
                    }

                    cmdsql = String.Format(cmdsql, pars1, pars2);

                    SqlCommand idxcmd = new SqlCommand(cmdsql, rescn, restn);
                    idxcmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt);
                    for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
                    {
                        idxcmd.Parameters.Add(String.Format("@{0}", Constants.SpectralIndexDefinitions[i].Name), SqlDbType.Float);
                    }


                    ph.InitializePipeline();

                    int q = 0;
                    foreach (Spectrum spec in ph.Execute(speccn.FindSpectrum(ssp)))
                    {

                        Console.WriteLine("{0}", spec.Id);

                        idxcmd.Parameters["@SpectrumID"].Value = spec.Id;
                        for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
                        {
                            if (double.IsNaN(spec.SpectralIndices.Value[i]))
                            {
                                idxcmd.Parameters[1 + i].Value = DBNull.Value;
                            }
                            else
                            {
                                idxcmd.Parameters[1 + i].Value = spec.SpectralIndices.Value[i];
                            }
                        }

                        idxcmd.Transaction = restn;
                        idxcmd.ExecuteNonQuery();

                        q++;

                        if (q % 1000 == 0)
                        {
                            restn.Commit();
                            restn = rescn.BeginTransaction();
                        }
                    }

                    restn.Commit();
                }
            }
        }

#if false

        static void Main_Photo(string[] args)
        {
            SqlConnection portcn = new SqlConnection(portcstr);
            portcn.Open();
            SqlTransaction porttn = portcn.BeginTransaction();

            using (SqlConnection cn = new SqlConnection(speccstr))
            {
                cn.Open();
                SqlTransaction tn = cn.BeginTransaction();

                SqlConnector speccn = new SqlConnector(cn, tn);

                SqlSearchParameters ssp = new SqlSearchParameters(true);
                ssp.LoadDetails = false;
                ssp.LoadPoints = true;
                ssp.Query = @"SELECT  * FROM Spectra";

                int[] fids = { 14, 15, 16, 17, 18 };
                Filter[] ff = new Filter[fids.Length];

                for (int i = 0; i < ff.Length; i++)
                {
                    ff[i] = new Filter(portcn, porttn);
                    ff[i].Load(fids[i]);
                    ff[i].LoadResponses();
                }

                using (SqlConnection rescn = new SqlConnection(speccstr))
                {
                    rescn.Open();
                    SqlTransaction restn = rescn.BeginTransaction();

                    string sql = @"INSERT INTO {0}
           ([SpectrumID]
           ,[U]
           ,[G]
           ,[R]
           ,[I]
           ,[Z])
     VALUES
           (@SpectrumID,
           @U,
           @G,
           @R,
           @I,
           @Z)";

                    using (SqlCommand cmd = new SqlCommand(sql, rescn, restn))
                    {

                        cmd.Parameters.Add("@SpectrumID", System.Data.SqlDbType.BigInt);
                        cmd.Parameters.Add("@U", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@G", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@R", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@I", System.Data.SqlDbType.Float);
                        cmd.Parameters.Add("@Z", System.Data.SqlDbType.Float);

                        //foreach (Spectrum s in speccn.FindSpectrum(ssp))

                        int q = 0;
                        new ParallelLib.ParallelForEach<Spectrum>(speccn.FindSpectrum(ssp)).Execute(
                        delegate(Spectrum s)
                        {


                            double[][] fluxes = { s.Flux_Value,
                                                  s.Flux_Continuum,
                                                    s.Model_Lines};

                            string[] tables = { "Fluxes", "ContinuumFluxes", "LineFluxes" };

                            for (int t = 0; t < tables.Length; t++)
                            {
                                double[] flux = new double[5];

                                for (int i = 0; i < ff.Length; i++)
                                {
                                    s.Flux_Value = fluxes[t];
                                    flux[i] = NonNan(SynthMag.Flux(ff[i], s));

                                }

                                lock (syncRoot)
                                {
                                    Console.WriteLine("{0}\t{1}", q, s.Id);

                                    cmd.CommandText = String.Format(sql, tables[t]);
                                    cmd.Parameters["@SpectrumID"].Value = s.Id;

                                    for (int i = 0; i < flux.Length; i++)
                                    {
                                        cmd.Parameters[1 + i].Value = flux[i];
                                    }

                                    cmd.Transaction = restn;
                                    cmd.ExecuteNonQuery();

                                    q++;

                                    if (q % 10 == 0)
                                    {
                                        restn.Commit();
                                        restn = rescn.BeginTransaction();
                                    }
                                }
                            }
                        });

                    }

                    restn.Commit();
                    restn.Dispose();
                }

                tn.Commit();
                tn.Dispose();
            }

            Console.Write("done.");
            Console.ReadLine();
        }

        static void EWUpdate(string[] args)
        {
            using (SqlConnection cn = new SqlConnection(rescstr))
            {
                cn.Open();
                SqlTransaction tn = cn.BeginTransaction();

                SqlConnector speccn = new SqlConnector(cn, tn);

                SqlSearchParameters ssp = new SqlSearchParameters(true);
                //ssp.Query = "SELECT TOP 1000 * FROM Spectra ORDER BY ID";
                ssp.Query = "SELECT * FROM Spectra";
                ssp.LoadPoints = true;

                int q = 0;

                //foreach (Spectrum s in speccn.FindSpectrum(ssp))
                new ParallelLib.ParallelForEach<Spectrum>(speccn.FindSpectrum(ssp)).Execute(
                    delegate(Spectrum spec)
                    {

                        //long id = 82132248639307776;
                        //Spectrum s = speccn.GetSpectrum(Guid.Empty, "#" + id.ToString());

                        long id = spec.Id;

                        //s.Vac2Air();
                        spec.Restframe();

                        // load lines
                        string sql = "SELECT * FROM LineFits WHERE SpectrumID = @SpectrumID AND Detected=1 ORDER BY LineID";
                        using (SqlCommand cmd = new SqlCommand(sql, cn, tn))
                        {
                            cmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt).Value = id;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    int lineid = dr.GetInt32(1);
                                    double wl = dr.GetDouble(4);
                                    double a = dr.GetDouble(6);
                                    double s = dr.GetDouble(8);
                                    double ew;

                                    spec.CalculateEquivalentWidth(wl, a, s, out ew);


                                    lock (syncRoot)
                                    {
                                        sql = "UPDATE LineFits SET EW = @EW WHERE SpectrumID = @SpectrumID AND LineID = @LineID";

                                        using (SqlCommand ccmd = new SqlCommand(sql, cn, tn))
                                        {
                                            ccmd.Parameters.Add("@EW", SqlDbType.Float).Value = NonNan(ew);
                                            ccmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt).Value = id;
                                            ccmd.Parameters.Add("@LineID", SqlDbType.Int).Value = lineid;

                                            ccmd.ExecuteNonQuery();
                                        }

                                        //q++;

                                        //if (q % 10000 == 0)
                                        //{
                                        //    tn.Commit();
                                        //    tn = cn.BeginTransaction();
                                        //}
                                    }
                                }
                            }
                        }

                        q++;
                        Console.WriteLine("{0} : {1}", q, id);
                    });

                tn.Commit();
                tn.Dispose();
            }

            Console.WriteLine("done.");
            Console.ReadLine();
        }

#endif

#if false
        static void SaveContinuum(Spectrum spec, Spectrum[][] models, SqlConnection rescn, SqlTransaction restn)
        {
            string sql = @"
INSERT ContinuumFits
    (SpectrumID, Chi2, Ndf, Tau_V, Tau_VError, Mu, MuError, VDisp, VDispError, Z, Age{0})
VALUES
    (@SpectrumID, @Chi2, @Ndf, @Tau_V, @Tau_VError, @Mu, @MuError, @VDisp, @VDispError, @Z, @Age{1})";


            // Compute average values and build coeff part of query
            string pars1 = "";
            string pars2 = "";

            double age = 0;
            double met = 0;
            double metlog = 0;
            double norm = 0;
            for (int i = 0; i < spec.ContinuumFitParameters.Coeffs.Length; i++)
            {
                pars1 += String.Format(", Coeff{0}", i);
                pars2 += String.Format(", @Coeff{0}", i);


                age += spec.ContinuumFitParameters.Coeffs[i] *
                    models[spec.ContinuumFitBest][i].ModelParameters.T_form.Value;

                met += spec.ContinuumFitParameters.Coeffs[i] *
                    models[spec.ContinuumFitBest][i].ModelParameters.Z_met.Value;

                metlog += spec.ContinuumFitParameters.Coeffs[i] *
                    Math.Log(models[spec.ContinuumFitBest][i].ModelParameters.Z_met.Value);

                norm += spec.ContinuumFitParameters.Coeffs[i];
            }

            sql = String.Format(sql, pars1, pars2);

            using (SqlCommand cmd = new SqlCommand(sql, rescn, restn))
            {
                cmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt).Value = spec.Id;
                cmd.Parameters.Add("@Chi2", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Chi2);
                cmd.Parameters.Add("@Ndf", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Ndf);
                cmd.Parameters.Add("@Tau_V", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Tau_V);
                cmd.Parameters.Add("@Tau_VError", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Tau_VError);
                cmd.Parameters.Add("@Mu", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Mu);
                cmd.Parameters.Add("@MuError", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@VDisp", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.VDisp);
                cmd.Parameters.Add("@VDispError", SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.VDispError);
                cmd.Parameters.Add("@Z", SqlDbType.Float).Value = NonNan(models[spec.ContinuumFitBest][0].ModelParameters.Z_met.Value);
                //cmd.Parameters.Add("@Z", SqlDbType.Float).Value = Math.Exp(metlog / norm);
                cmd.Parameters.Add("@Age", SqlDbType.Float).Value = NonNan(age / norm);
                for (int i = 0; i < spec.ContinuumFitParameters.Coeffs.Length; i++)
                {
                    cmd.Parameters.Add(String.Format("@Coeff{0}", i), SqlDbType.Float).Value = NonNan(spec.ContinuumFitParameters.Coeffs[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }
#endif

        static void SaveLines(Spectrum spec, SqlConnection rescn, SqlTransaction restn)
        {
            using (SqlCommand cmd = new SqlCommand("", rescn, restn))
            {

                cmd.CommandText = @"
INSERT INTO [LineFits]
           ([SpectrumID]
           ,[LineID]
           ,LineModel
           ,[lambda]
           ,[lambda_err]
           ,[A]
           ,[A_err]
           ,[sigma]
           ,[sigma_err]
           ,[lambda2]
           ,[lambda2_err]
           ,[A2]
           ,[A2_err]
           ,[sigma2]
           ,[sigma2_err]
           ,[vdisp]
           ,[vdisp_err]
           ,[EW]
           ,[EW_err]
           ,[Chi2]
           ,[Snr])
     VALUES
           (@SpectrumID
           ,@LineID
           ,@LineModel
           ,@lambda
           ,@lambda_err
           ,@A
           ,@A_err
           ,@sigma
           ,@sigma_err
           ,@lambda2
           ,@lambda2_err
           ,@A2
           ,@A2_err
           ,@sigma2
           ,@sigma2_err
           ,@vdisp
           ,@vdisp_err
           ,@EW
           ,@EW_err
           ,@Chi2
           ,@Snr)
";

                cmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt).Value = spec.Id;
                cmd.Parameters.Add("@LineID", SqlDbType.Int);
                cmd.Parameters.Add("@LineModel", SqlDbType.TinyInt);
                cmd.Parameters.Add("@lambda", SqlDbType.Float);
                cmd.Parameters.Add("@lambda_err", SqlDbType.Float);
                cmd.Parameters.Add("@A", SqlDbType.Float);
                cmd.Parameters.Add("@A_err", SqlDbType.Float);
                cmd.Parameters.Add("@sigma", SqlDbType.Float);
                cmd.Parameters.Add("@sigma_err", SqlDbType.Float);
                cmd.Parameters.Add("@lambda2", SqlDbType.Float);
                cmd.Parameters.Add("@lambda2_err", SqlDbType.Float);
                cmd.Parameters.Add("@A2", SqlDbType.Float);
                cmd.Parameters.Add("@A2_err", SqlDbType.Float);
                cmd.Parameters.Add("@sigma2", SqlDbType.Float);
                cmd.Parameters.Add("@sigma2_err", SqlDbType.Float);
                cmd.Parameters.Add("@vdisp", SqlDbType.Float);
                cmd.Parameters.Add("@vdisp_err", SqlDbType.Float);
                cmd.Parameters.Add("@EW", SqlDbType.Float);
                cmd.Parameters.Add("@EW_err", SqlDbType.Float);
                cmd.Parameters.Add("@Chi2", SqlDbType.Float);
                cmd.Parameters.Add("@Snr", SqlDbType.Float);

                for (int i = 0; i < spec.LineFit.Lines.Length; i++)
                {
                    if (spec.LineFit.Lines[i].Detected)
                    {
                        cmd.Parameters["@LineID"].Value = i;
                        cmd.Parameters["@LineModel"].Value = (int)spec.LineFit.Lines[i].Model;
                        cmd.Parameters["@lambda"].Value = NonNan(spec.LineFit.Lines[i].Wavelength);
                        cmd.Parameters["@lambda_err"].Value = NonNan(spec.LineFit.Lines[i].WavelengthError);
                        cmd.Parameters["@A"].Value = NonNan(spec.LineFit.Lines[i].Amplitude);
                        cmd.Parameters["@A_err"].Value = NonNan(spec.LineFit.Lines[i].AmplitudeError);
                        cmd.Parameters["@sigma"].Value = NonNan(spec.LineFit.Lines[i].Sigma);
                        cmd.Parameters["@sigma_err"].Value = NonNan(spec.LineFit.Lines[i].SigmaError);
                        cmd.Parameters["@lambda2"].Value = NonNan(spec.LineFit.Lines[i].Wavelength2);
                        cmd.Parameters["@lambda2_err"].Value = NonNan(spec.LineFit.Lines[i].Wavelength2Error);
                        cmd.Parameters["@A2"].Value = NonNan(spec.LineFit.Lines[i].Amplitude2);
                        cmd.Parameters["@A2_err"].Value = NonNan(spec.LineFit.Lines[i].Amplitude2Error);
                        cmd.Parameters["@sigma2"].Value = NonNan(spec.LineFit.Lines[i].Sigma2);
                        cmd.Parameters["@sigma2_err"].Value = NonNan(spec.LineFit.Lines[i].Sigma2Error);
                        cmd.Parameters["@vdisp"].Value = NonNan(spec.LineFit.Lines[i].LineVDisp);
                        cmd.Parameters["@vdisp_err"].Value = NonNan(spec.LineFit.Lines[i].LineVDispError);
                        cmd.Parameters["@EW"].Value = NonNan(spec.LineFit.Lines[i].EqWidth);
                        cmd.Parameters["@EW_err"].Value = NonNan(spec.LineFit.Lines[i].EqWidthError);
                        cmd.Parameters["@Chi2"].Value = NonNan(spec.LineFit.Lines[i].LineFitChi2);
                        cmd.Parameters["@Snr"].Value = NonNan(spec.LineFit.Lines[i].Snr);

                        cmd.ExecuteNonQuery();
                    }
                }

            }

        }

        static void SaveLickIndices(Spectrum spec, SqlConnection rescn, SqlTransaction restn)
        {
            string cmdsql = @"
INSERT SpectralIndices (SpectrumID {0})
VALUES (@SpectrumID {1})";

            string pars1 = "";
            string pars2 = "";
            for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
            {
                pars1 += String.Format(", {0}", Constants.SpectralIndexDefinitions[i].Name);
                pars2 += String.Format(", @{0}", Constants.SpectralIndexDefinitions[i].Name);
            }

            cmdsql = String.Format(cmdsql, pars1, pars2);

            SqlCommand idxcmd = new SqlCommand(cmdsql, rescn, restn);
            idxcmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt);
            for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
            {
                idxcmd.Parameters.Add(String.Format("@{0}", Constants.SpectralIndexDefinitions[i].Name), SqlDbType.Float);
            }

            idxcmd.Parameters["@SpectrumID"].Value = spec.Id;
            for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
            {
                if (double.IsNaN(spec.SpectralIndices.Value[i]))
                {
                    idxcmd.Parameters[1 + i].Value = DBNull.Value;
                }
                else
                {
                    idxcmd.Parameters[1 + i].Value = spec.SpectralIndices.Value[i];
                }
            }

            idxcmd.ExecuteNonQuery();

        }

        static void GalaxyFit(string sql, double wlfrom, double wlto)
        {
            using (SqlConnection cn = new SqlConnection(speccstr))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction())
                {
                    // Get number of spectra
                    int count;
                    double wlmin, wlmax;
                    using (SqlCommand cmd = new SqlCommand(String.Format("SELECT COUNT(*), MIN([SpectralCoverageStart] / (1 + Redshift)), MAX([SpectralCoverageStop] / (1 + Redshift)) FROM ({0}) q",
                        sql.Replace("$TOP$", "")), cn, tn))
                    {
                        cmd.CommandTimeout = 3000;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dr.Read();
                            count = (int)dr.GetInt32(0);
                            wlmin = dr.GetDouble(1);
                            wlmax = dr.GetDouble(2);
                        }
                    }


                    SqlSearchParameters ssp = new SqlSearchParameters(true);
                    ssp.LoadDetails = true;
                    ssp.LoadPoints = true;
                    ssp.Query = ssp.Query = sql.Replace("$TOP$", "");

                    SqlConnector speccn = new SqlConnector(cn, tn);
                    GalaxyFit(speccn.FindSpectrum(ssp), count, wlfrom, wlto);
                }
            }
        }

        static void GalaxyFit(string[] files, double wlfrom, double wlto)
        {
            List<Spectrum> speclist = new List<Spectrum>();
            for (int i = 0; i < files.Length; i++)
            {
                AsciiConnector asc = new AsciiConnector(files[i]);
                asc.Columns = new string[] { "Spectral_Value", "Flux_Value" };

                Spectrum spec = asc.GetSpectrum(Guid.Empty, String.Format("#{0}", i));

                Util.Vector.Multiply(1e-38, spec.Flux_Value, out spec.Flux_Value);

                spec.FindBins();

                speclist.Add(spec);
            }

            GalaxyFit(speclist, speclist.Count, wlfrom, wlto);
        }

        static void GalaxyFit(IEnumerable<Spectrum> spectra, int count, double wlfrom, double wlto)
        {
            SqlConnection portcn = new SqlConnection(portcstr);
            portcn.Open();
            SqlTransaction porttn = portcn.BeginTransaction();

            //using (SqlConnection cn = new SqlConnection(speccstr))
            //{
            //    cn.Open();
            //    using (SqlTransaction tn = cn.BeginTransaction())
            //    {
            //        SqlConnector speccn = new SqlConnector(cn, tn);

            SpectrumPipeline ph = new SpectrumPipeline(new PortalConnector(portcn, porttn));
            //ph.SkipExceptions = false;
            ph.Count = count;
            ph.SkipExceptions = false;

            DereddenStep dered = new DereddenStep();
            //ph.Steps.Add(dered);

            // ------------------------------------------------->
            // Use cross-correlation redshift
#if false
                    CustomStep getxcorrredshift = new CustomStep();
                    getxcorrredshift.Function = delegate(Spectrum s)
                    {
                        using (SqlConnection cnxcorr = new SqlConnection("Data Source=retdb02;Integrated Security=true;"))
                        {
                            cnxcorr.Open();

                            string sql = "SELECT CAST(z AS float) FROM csabai.dbo.lrgNiceSpecDR7 WHERE specobjid = " + s.Id.ToString();

                            using (SqlCommand cmdxcorr = new SqlCommand(sql, cnxcorr))
                            {
                                double xcorrz = (double)cmdxcorr.ExecuteScalar();

                                s.Derived.Redshift.Value.Value = xcorrz;
                                s.Target.Redshift.Value = xcorrz;
                            }
                        }

                        return s;
                    };
                    ph.Steps.Add(getxcorrredshift);
#endif
            // <------------------------------------------------------

            RedshiftStep reds = new RedshiftStep();
            reds.Method = RedshiftStep.RedshiftMethod.RestFrame;
            //ph.Steps.Add(reds);

            // ---------------------------------------------
            foreach (string bcfile in
                new[] { @"C:\Data\dobos\project\spectrum\data\templates\bc03\m22.spec",
                        @"C:\Data\dobos\project\spectrum\data\templates\bc03\m32.spec",
                        @"C:\Data\dobos\project\spectrum\data\templates\bc03\m42.spec",
                        @"C:\Data\dobos\project\spectrum\data\templates\bc03\m52.spec",
                        @"C:\Data\dobos\project\spectrum\data\templates\bc03\m62.spec",
                        @"C:\Data\dobos\project\spectrum\data\templates\bc03\m72.spec",
                })
            {


                ContinuumFitStep fs = new ContinuumFitStep();
                //fs.AddSdssLines();
                fs.AddSdssEmissionLines();
                fs.MaskLines = false;
                fs.Mask = atlasmask;
                fs.MaskSkyLines = true;
                fs.MaskZeroError = false;
                fs.SubtractAverage = false;
                fs.Method = ContinuumFitStep.FitMethod.NonNegativeLeastSquares;
                // Load models
                fs.Templates = LoadTemplates_BCFile(bcfile, 10);
                fs.ApplyVDisp = true;
                fs.FitVDisp = false;
                fs.VDisp.Value = 150;

                fs.ApplyExtinction = false;
                fs.FitExtinction = false;
                fs.Tau_V.Value = 0.5;

                fs.WeightWithError = true;

                ph.Steps.Add(fs);
            }

            SelectBestFitStep bfs = new SelectBestFitStep();
            ph.Steps.Add(bfs);
            // ---------------------------------------------


            //  Fit lines
            LineFitStep ls = new LineFitStep();
            ls.Lines = Constants.UpdatedEmissionLines;
            ls.Mask = atlasmask;
            //ph.Steps.Add(ls);

            // Calculate Lick indices
            SpectralIndexStep idxs = new SpectralIndexStep();
            //ph.Steps.Add(idxs);

            // put back to original frame
            RedshiftStep reds2 = new RedshiftStep();
            reds2.Method = RedshiftStep.RedshiftMethod.ObservationFrame;
            //ph.Steps.Add(reds2);

            /*SqlConnection rescn = new SqlConnection(rescstr);
            rescn.Open();
            SqlTransaction restn = rescn.BeginTransaction();*/

            ph.ProgressChanged += new Jhu.SpecSvc.Pipeline.ProgressChangedEventHandler(ph_ProgressChanged);
            ph.InitializePipeline();

            int q = 0;
            foreach (Spectrum spec in ph.Execute(spectra))
            {
                //Console.WriteLine("{0} {1} {2}", spec.Id, spec.ContinuumFits[spec.ContinuumFitBest].VDisp, spec.ContinuumFits[spec.ContinuumFitBest].Tau_V);
                Console.WriteLine(spec.Id);

                AsciiConnector asc = new AsciiConnector(String.Format(@"C:\Data\dobos\temp\20140519\spec{0}.txt", q));
                asc.Columns = new string[]
                            {
                                "Spectral_Value",
                                "Flux_Value",
                                "Flux_Continuum",
                                "Model_Continuum",
                                "Flux_Lines",
                                "Model_Lines"
                            };
                asc.SaveSpectrum(spec, Guid.Empty);

                using (StreamWriter outfile = new StreamWriter(String.Format(@"C:\Data\dobos\temp\20140519\spec{0}.fit.txt", q)))
                {
                    outfile.WriteLine("chi^2 = {0}", spec.ContinuumFits[spec.ContinuumFitBest].Chi2);
                    outfile.WriteLine("tau_V = {0}", spec.ContinuumFits[spec.ContinuumFitBest].Tau_V);
                    outfile.WriteLine("mu = {0}", spec.ContinuumFits[spec.ContinuumFitBest].Mu);
                    outfile.WriteLine("sigma_v = {0}", spec.ContinuumFits[spec.ContinuumFitBest].VDisp);

                    for (int i = 0; i < spec.ContinuumFits[spec.ContinuumFitBest].Coeffs.Length; i++)
                    {
                        outfile.WriteLine("{0}\t{1}", i+1, spec.ContinuumFits[spec.ContinuumFitBest].Coeffs[i]);
                    }
                }

                try
                {
                    /*SqlConnector resconn = new SqlConnector(rescn, restn);
                    resconn.CreateSpectrum(spec, Guid.Empty);
                    resconn.SaveSpectrumFields(spec, Guid.Empty);
                    resconn.SaveSpectrumData(spec, Guid.Empty);

                    SaveContinuum(spec, models, rescn, restn);
                    SaveLickIndices(spec, rescn, restn);
                    SaveLines(spec, rescn, restn);*/
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                q++;

                if (q % 100 == 0)
                {
                    //restn.Commit();
                    //restn = rescn.BeginTransaction();
                }
            }

            lock (syncRoot)
            {
                //restn.Commit();
                //rescn.Close();
            }
        }

        static void ModelFlux(Spectrum[][] templates)
        {
            Filter[] filters = LoadFilters();
            double[][] filtwl = new double[filters.Length][];
            double[][] filtrp = new double[filters.Length][];
            for (int i = 0; i < filters.Length; i++)
            {
                filtwl[i] = new double[filters[i].Responses.Count];
                filtrp[i] = new double[filters[i].Responses.Count];

                for (int j = 0; j < filters[i].Responses.Count; j++)
                {
                    filtwl[i][j] = filters[i].Responses[j].Wavelength;
                    filtrp[i][j] = filters[i].Responses[j].Value;
                }
            }

            LoadTemplates_Tremonti();

            double[] tempwl = templates[0][0].Spectral_Value;
            double[][] tempfl = templates[0].ToArray().Select(s => s.Flux_Value).ToArray();
            double[] co = new double[tempfl.Length];
            double[] fl = new double[tempwl.Length];

            string coeffs = string.Empty;
            for (int i = 0; i < tempfl.Length; i++)
            {
                coeffs += String.Format(", Coeff{0}", i);
            }


            string sql = String.Format("SELECT SpectrumID {0} FROM ContinuumFits", coeffs);

            using (SqlConnection cn = new SqlConnection(rescstr))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    sql = "INSERT ContinuumFluxes (SpectrumID, U, G, R, I, Z) VALUES (@SpectrumID, @U, @G, @R, @I, @Z)";

                    using (SqlCommand rescmd = new SqlCommand(sql, cn))
                    {
                        rescmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt);
                        rescmd.Parameters.Add("@U", SqlDbType.Float);
                        rescmd.Parameters.Add("@G", SqlDbType.Float);
                        rescmd.Parameters.Add("@R", SqlDbType.Float);
                        rescmd.Parameters.Add("@I", SqlDbType.Float);
                        rescmd.Parameters.Add("@Z", SqlDbType.Float);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                rescmd.Parameters["@SpectrumID"].Value = dr.GetInt64(0);

                                for (int i = 0; i < co.Length; i++)
                                {
                                    co[i] = dr.GetDouble(1 + i);
                                }

                                Vector.Sum(tempfl, co, out fl);

                                bool error = false;

                                for (int f = 0; f < filters.Length; f++)
                                {
                                    bool err;
                                    double flux = Integral.Integrate(tempwl, fl, filtwl[f], filtrp[f], out err);
                                    error |= err;

                                    flux = flux / Constants.LightSpeed * 1e-27;
                                    flux = -2.5 * Math.Log10(flux) - 48.6;

                                    rescmd.Parameters[1 + f].Value = NonNan(flux);

                                    Console.Write("{0}\t", flux);
                                }

                                //if (!error)
                                //{
                                rescmd.ExecuteNonQuery();
                                //}

                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }

        static Spectrum[][] LoadTemplates_Tremonti()
        {
            Spectrum[][] models;

            using (SqlConnection cn = new SqlConnection(mstr))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction())
                {
                    SqlConnector conn = new SqlConnector(cn, tn);

                    models = new Spectrum[4][];
                    for (int i = 0; i < models.Length; i++)
                    {
                        models[i] = new Spectrum[10];

                        for (int j = 0; j < models[i].Length; j++)
                        {
                            //int firstid = 1;
                            int firstid = 360;
                            models[i][j] = conn.GetSpectrum(Guid.Empty, String.Format("#{0}", firstid + i + j * models.Length), true, null, true);

                            models[i][j].Air2Vac();
                        }
                    }


                    tn.Commit();
                }
            }

            return models;
        }

        static Filter[] LoadFilters()
        {
            Filter[] filters = new Filter[5];

            using (SqlConnection cn = new SqlConnection(portcstr))
            {
                cn.Open();

                using (SqlTransaction tn = cn.BeginTransaction())
                {
                    for (int i = 0; i < filters.Length; i++)
                    {
                        filters[i] = new Filter(cn, tn);
                        filters[i].Load(14 + i);
                        filters[i].LoadResponses();
                    }
                }
            }

            return filters;
        }

        static Spectrum[] LoadTemplates_BCFile(string filename, int n)
        {
            List<double> wl = new List<double>();
            List<double>[] fl = new List<double>[n];
            for (int i = 0; i < n; i++)
            {
                fl[i] = new List<double>();
            }

            using (StreamReader infile = new StreamReader(filename))
            {
                string line;
                while ((line = infile.ReadLine()) != null)
                {
                    if (!line.StartsWith("#"))
                    {
                        string[] parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        wl.Add(double.Parse(parts[0]));

                        for (int i = 0; i < n; i++)
                        {
                            fl[i].Add(double.Parse(parts[1 + i]));
                        }
                    }
                }
            }

            List<Spectrum> t = new List<Spectrum>();

            for (int i = 0; i < n; i++)
            {
                Spectrum s = new Spectrum(true);

                s.Spectral_Value = wl.ToArray();
                s.Flux_Value = fl[i].ToArray();

                //s.Air2Vac();

                s.FindBins();

                t.Add(s);
            }

            return t.ToArray();
        }

        static void ContinuumPCA(string sql, int init, int top, int repeat)
        {
            using (SqlConnection cn = new SqlConnection(rescstr))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction())
                {

                    int count;
                    double wlmin, wlmax;

                    // Count spectra and determine wavelength coverage

                    using (SqlCommand cmd = new SqlCommand(
                        String.Format(
                            "SELECT COUNT(*), MIN([SpectralCoverageStart] / (1 + Redshift)), MAX([SpectralCoverageStop] / (1 + Redshift)) FROM ({0}) q",
                            sql.Replace("$TOP$", "")), cn, tn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dr.Read();

                            count = dr.GetInt32(0);
                            wlmin = dr.GetDouble(1);
                            wlmax = dr.GetDouble(2);
                        }
                    }

                    wlmin = 3800;
                    wlmax = 8000;

                    Console.WriteLine("Count = {0}, min = {1}, max = {2}", count, wlmin, wlmax);


                    SqlConnection pcn = new SqlConnection(portcstr);
                    pcn.Open();
                    SqlTransaction ptn = pcn.BeginTransaction();
                    PortalConnector pc = new PortalConnector(pcn, ptn);

                    SqlConnector conn = new SqlConnector(cn, tn);

                    IEnumerable<Spectrum> spectra;

                    SqlSearchParameters ssp;

                    Random rnd = new Random();

                    ssp = new SqlSearchParameters(true);
                    ssp.LoadPoints = true;
                    ssp.LoadDetails = false;
                    ssp.Query = sql.Replace("$TOP$", String.Format("TOP {0}", init / 2)) + " ORDER BY Redshift ASC";
                    spectra = conn.FindSpectrum(ssp);

                    ssp = new SqlSearchParameters(true);
                    ssp.LoadPoints = true;
                    ssp.LoadDetails = false;
                    ssp.Query = sql.Replace("$TOP$", String.Format("TOP {0}", init / 2)) + " ORDER BY Redshift DESC";
                    spectra = spectra.Concat(conn.FindSpectrum(ssp));

                    for (int i = 0; i < repeat; i++)
                    {
                        ssp = new SqlSearchParameters(true);
                        ssp.LoadPoints = true;
                        ssp.LoadDetails = false;
                        ssp.Query = sql.Replace("$TOP$", top == 0 ? "" : String.Format("TOP {0}", top)) +
                            String.Format(" ORDER BY (HTMID / 31 * {0}) * s.Ra - ROUND((HTMID / 31 * {0}) * s.Ra, 0) DESC", rnd.NextDouble());
                        spectra = spectra.Concat(conn.FindSpectrum(ssp));
                    }

                    SpectrumPipeline ph = CreateContinuumPipeline(3 * count, pc, wlmin, wlmax, 1.0, 0.0, 1.0, 0.0);
                    //ph.SkipExceptions = false;

                    PcaStep pca = new PcaStep();
                    pca.Components = 5;
                    pca.Mask = atlasmask;
                    pca.SubtractAverage = true;
                    pca.InitCount = init * 2;
                    ph.Steps.Add(pca);

                    ph.ProgressChanged += new Jhu.SpecSvc.Pipeline.ProgressChangedEventHandler(ph_ProgressChanged);
                    ph.InitializePipeline();

                    using (SqlConnection rcn = new SqlConnection(rescstr))
                    {
                        rcn.Open();
                        using (SqlTransaction rtn = rcn.BeginTransaction())
                        {

                            SqlConnector rcc = new SqlConnector(rcn, rtn);

                            List<Spectrum> sps = new List<Spectrum>();
                            foreach (Spectrum ss in ph.Execute(spectra))
                            {
                                Console.Write("-");
                                sps.Add(ss);
                            }

                            for (int q = 0; q < sps.Count; q++)
                            {
                                Spectrum s = sps[q];

                                s.Id = 0;
                                s.Derived.Redshift.Value.Value = s.Target.Redshift.Value = 0;

                                s.Target.Name.Value =
                                    String.Format("PCA {0}", q);

                                // Recalculate error
                                if (s.Flux_Accuracy_StatError != null && s.Counts_Value != null)
                                {
                                    for (int wl = 0; wl < s.Counts_Value.Length; wl++)
                                    {
                                        s.Counts_Value[wl] /= repeat;
                                        s.Flux_Accuracy_StatError[wl] /= Math.Sqrt(s.Counts_Value[wl]);
                                    }
                                }

                                Console.WriteLine(s.Target.Redshift.Value);

                                //AsciiConnector asc = new AsciiConnector(String.Format(@"C:\Data0\dobos\project\LRGCosmo\composite\{0}.txt", s.Target.Redshift.Value));
                                AsciiConnector asc = new AsciiConnector(String.Format(@"..\..\output\{0}.txt", q));
                                asc.Columns = new string[] { "Spectral_Value", "Flux_Value", "Flux_Accuracy_Quality" };
                                //asc.SaveSpectrum(s, Guid.Empty);

                                //rcc.SaveSpectrum(s, Guid.Empty);
                            }

                            /*
                            string sql = @"
INSERT PCA (Code, ES0, ES1, ES2, ES3, ES4, ES5, L1, L2, L3, L4, L5)
VALUES (@Code, @ES0, @ES1, @ES2, @ES3, @ES4, @ES5, @L1, @L2, @L3, @L4, @L5)";

                            using (SqlCommand cmd = new SqlCommand(sql, rcn, rtn))
                            {
                                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = cl.Name;

                                for (int i = 0; i <= 5; i++)
                                {
                                    cmd.Parameters.Add(String.Format("@ES{0}", i), SqlDbType.BigInt).Value = sps[i].Id;
                                }

                                for (int i = 1; i <= 5; i++)
                                {
                                    cmd.Parameters.Add(String.Format("@L{0}", i), SqlDbType.Float).Value = sps[i].Derived.Eigenvalue.Value;
                                }

                                cmd.ExecuteNonQuery();
                            }
                             * */

                            rtn.Commit();

                        }

                    }

                }
            }
        }

        static SpectrumPipeline CreateContinuumPipeline(int count, PortalConnector pc, double wlmin, double wlmax, double resolution, double zfrom, double zto, double zref)
        {
            SpectrumPipeline ph = new SpectrumPipeline();
            ph.Count = count;
            ph.Connector = pc;
            ph.SkipExceptions = true;

            CustomStep cs = new CustomStep();
            cs.Function = delegate(Spectrum s)
            {
                s.Flux_Value = s.Model_Continuum;
                s.Flux_Accuracy_StatErrHigh = null;
                s.Flux_Accuracy_StatErrLow = null;

                Console.Write(".");

                return s;
            };
            ph.Steps.Add(cs);

            /*DereddenStep ds = new DereddenStep();
            ph.Steps.Add(ds);*/

            NormalizeStep ns = new NormalizeStep();
            ns.Method = NormalizeStep.NormalizeMethod.FluxMedianInRanges;
            ns.Template = NormalizeStep.NormalizeTemplate.Galaxy;
            ns.Flux.Value = 1;
            ph.Steps.Add(ns);

            RedshiftStep rs = new RedshiftStep();
            rs.Method = RedshiftStep.RedshiftMethod.Custom;
            rs.Redshift.Value = zref;
            ph.Steps.Add(rs);

            // Calculate range
            RebinStep bs = new RebinStep();
            bs.RebinLimits.Min.Value = Math.Ceiling(wlmin / 50.0) * 50.0 - 0.5;
            bs.RebinLimits.Max.Value = Math.Floor(wlmax / 50.0) * 50.0 + 0.5;
            bs.RebinBinSize.Value = resolution;
            bs.CalculateRebinGrid();
            ph.Steps.Add(bs);

            return ph;
        }

        static void ExportModelsWithLines(string sql)
        {
            SqlConnection portcn = new SqlConnection(portcstr);
            portcn.Open();
            SqlTransaction porttn = portcn.BeginTransaction();

            using (SqlConnection cn = new SqlConnection(rescstr))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction())
                {
                    // Get number of spectra
                    int count;
                    double wlmin, wlmax;
                    using (SqlCommand cmd = new SqlCommand(String.Format("SELECT COUNT(*), MIN([SpectralCoverageStart] / (1 + Redshift)), MAX([SpectralCoverageStop] / (1 + Redshift)) FROM ({0}) q",
                        sql.Replace("$TOP$", "")), cn, tn))
                    {
                        cmd.CommandTimeout = 3000;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dr.Read();
                            count = (int)dr.GetInt32(0);
                            wlmin = dr.GetDouble(1);
                            wlmax = dr.GetDouble(2);
                        }
                    }


                    SqlConnector speccn = new SqlConnector(cn, tn);

                    SqlSearchParameters ssp = new SqlSearchParameters(true);
                    ssp.LoadDetails = true;
                    ssp.LoadPoints = true;
                    ssp.Query = ssp.Query = sql.Replace("$TOP$", "");

                    SpectrumPipeline ph = new SpectrumPipeline(new PortalConnector(portcn, porttn));
                    ph.Count = count;

                    RedshiftStep rs = new RedshiftStep();
                    rs.Method = RedshiftStep.RedshiftMethod.RestFrame;
                    ph.Steps.Add(rs);

                    NormalizeStep ns = new NormalizeStep();
                    ns.Method = NormalizeStep.NormalizeMethod.FluxMedianInRanges;
                    ns.Template = NormalizeStep.NormalizeTemplate.Galaxy;
                    ph.Steps.Add(ns);

                    RebinStep bs = new RebinStep();
                    bs.RebinLimits.Min.Value = 3800;
                    bs.RebinLimits.Max.Value = 8800;
                    bs.RebinBinSize.Value = 1.0;
                    bs.CalculateRebinGrid();
                    ph.Steps.Add(bs);

                    int q = 0;
                    foreach (Spectrum spec in ph.Execute(speccn.FindSpectrum(ssp)))
                    {
                        for (int i = 0; i < spec.Flux_Value.Length; i++)
                        {
                            spec.Flux_Value[i] = spec.Model_Continuum[i] + spec.Model_Lines[i];
                        }


                        AsciiConnector asc = new AsciiConnector(String.Format(@"..\..\output\spec_{0:D4}.txt", q + 1));
                        asc.Columns = new string[] { "Spectral_Value", "Flux_Value" };
                        asc.Format = AsciiConnector.AsciiFileType.Tabular;
                        asc.WriteFields = false;

                        asc.SaveSpectrum(spec, Guid.Empty);

                        asc.FileName = String.Format(@"..\..\output\line_{0:D4}.txt", q + 1);
                        asc.Columns = new string[] { "Spectral_Value", "Model_Lines" };
                        asc.SaveSpectrum(spec, Guid.Empty);

                        using (StreamWriter outfile = new StreamWriter(String.Format(@"..\..\output\lpar_{0:D4}.txt", q + 1)))
                        {
                            outfile.WriteLine("#LineID\tLambda\tA\tsigma");

                            string lsql = "SELECT * FROM LineFits WHERE SpectrumID = @SpectrumID ORDER BY LineID";
                            using (SqlCommand lcmd = new SqlCommand(lsql, cn, tn))
                            {
                                lcmd.Parameters.Add("@SpectrumID", SqlDbType.BigInt).Value = spec.Id;

                                using (SqlDataReader dr = lcmd.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        outfile.Write("{0}", dr.GetInt32(1));   // LineID
                                        outfile.Write("\t{0:F2}", dr.GetDouble(2));   // Lambda
                                        outfile.Write("\t{0:F2}", dr.GetDouble(4));   // A
                                        outfile.Write("\t{0:F2}", dr.GetDouble(6));   // Sigma
                                        outfile.WriteLine();
                                    }
                                }
                            }

                        }

                        q++;
                    }
                }
            }
        }

        private static double NonNan(double value)
        {
            if (Math.Abs(value) < 1e-300)
                return 0;
            else
                return (Double.IsNaN(value) || Double.IsInfinity(value)) ? -9999 : value;
        }

        static void ph_ProgressChanged(object sender, Jhu.SpecSvc.Pipeline.ProgressChangedEventArgs args)
        {
            Console.WriteLine(args.Progress);
        }
    }
}

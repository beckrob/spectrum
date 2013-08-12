#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: FitsConnector.cs,v 1.1 2008/01/08 22:01:35 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:35 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using nom.tam.fits;
using VoServices.Spectrum.Lib;

namespace VoServices.Spectrum.IO
{
    public class FitsConnector : ConnectorBase, IDisposable
    {
        #region Member variables

        private string fileName;
        Fits f;

        #endregion
        #region Constructors

        public FitsConnector()
        {
            InitializeMembers();
        }

        public FitsConnector(string fileName)
        {
            InitializeMembers();

            this.fileName = fileName;
        }

        #endregion
        #region Properties

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        #endregion

        private void InitializeMembers()
        {
            this.fileName = "";
        }

        #region Search functions
        
        public override VoServices.Spectrum.Lib.Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, bool loadDetails)
		{
            f = new Fits(FileName);
            BasicHDU[] hdus = new BasicHDU[] { f.readHDU() };

            VoServices.Spectrum.Lib.Spectrum s = new VoServices.Spectrum.Lib.Spectrum(true);
            s.BasicInitialize();
			s.UserGuid = Guid.Empty;
			s.Public = 1;
			// primary hdu

            /*
            if (hdus[0].Header.ContainsKey("EQUINOX")) s.MainSegment.Frame.Sky.Equinox.Value = hdus[0].Header.GetDoubleValue("EQUINOX");
            if (hdus[0].Header.ContainsKey("RADECSYS")) s.MainSegment.Frame.Sky.Type.Value = hdus[0].Header.GetStringValue("RADECSYS");
			s.MainSegment.Frame.Time.Type.Value = "MJD";     //***
			s.MainSegment.Frame.Time.System.Value = "TAI";   //***
			//s.MainSegment.Frame.Time.RefDir.Value = "TOPOCENTRIC"; //***
			s.MainSegment.Frame.Time.RefPos.Value = "Topocentric"; //***
			s.MainSegment.Frame.Time.Zero.Value = 0.0;
			s.MainSegment.Frame.Spectral.RefPos.Value = "Topocentric";

			DateTime dt = DateTime.Parse(hdus[0].Header.GetStringValue("DATE-OBS"));
			DateTime tm = DateTime.Parse(hdus[0].Header.GetStringValue("TAIHMS"));
			s.MainSegment.Coverage.Location.Time.Value =
				new DateTime(dt.Year, dt.Month, dt.Day, tm.Hour, tm.Minute, tm.Second, tm.Millisecond);

			s.MainSegment.Coverage.Extent = new VoServices.Schema.Sed.CoverageExtent();
			s.MainSegment.Coverage.Extent.Sky.Value = 3.0 / 3600.0;	// 3 arcsec
			s.MainSegment.Coverage.Extent.Time.Value = hdus[0].Header.GetDoubleValue("TAI-END") - hdus[0].Header.GetDoubleValue("TAI-BEG");

			s.MainSegment.Coverage.Region = new VoServices.Schema.Sed.CoverageRegion();
			s.MainSegment.Coverage.Region.Sky = null;
			s.MainSegment.Coverage.Region.Time.Start.Value = s.MainSegment.Coverage.Location.Time.Value.AddSeconds(
					hdus[0].Header.GetDoubleValue("TAI-BEG") - hdus[0].Header.GetDoubleValue("TAI"));
			s.MainSegment.Coverage.Region.Time.Stop.Value = s.MainSegment.Coverage.Location.Time.Value.AddSeconds(
					hdus[0].Header.GetDoubleValue("TAI-END") - hdus[0].Header.GetDoubleValue("TAI"));

			s.MainSegment.Coverage.Extent.Time.Value = hdus[0].Header.GetDoubleValue("EXPTIME");


			s.MainSegment.DataId.Version.Value = hdus[0].Header.GetStringValue("VERSION").Substring(1).Replace('_', '.');
			s.MainSegment.DataId.Instrument.Value = hdus[0].Header.GetStringValue("TELESCOP").Trim() + " " + hdus[0].Header.GetStringValue("CAMVER").Trim();
			//s.MainSegment.Curation.ContactName.Value = hdus[0].Header.GetStringValue("OBSERVER");

			s.MainSegment.Coverage.Location.Sky.Ra = hdus[0].Header.GetDoubleValue("RAOBJ");
			s.MainSegment.Coverage.Location.Sky.Dec = hdus[0].Header.GetDoubleValue("DECOBJ");

			s.Target.Description.Value = hdus[0].Header.GetStringValue("NAME");
			s.Target.Name.Value = SdssName(
				s.MainSegment.Coverage.Location.Sky.Ra,
				s.MainSegment.Coverage.Location.Sky.Dec);
			s.Target.Class.Value = FirstUpperCase(hdus[0].Header.GetStringValue("OBJTYPE"));
			s.Target.Pos.Ra = hdus[0].Header.GetDoubleValue("RAOBJ");
			s.Target.Pos.Dec = hdus[0].Header.GetDoubleValue("DECOBJ");
			switch (hdus[0].Header.GetIntValue("SPEC_CLN"))
			{
				default:
				case 0:
					s.Target.SpectralClass.Value = "Unknown"; break;
				case 1:
					s.Target.SpectralClass.Value = "Star"; break;
				case 2:
					s.Target.SpectralClass.Value = "Galaxy"; break;
				case 3:
					s.Target.SpectralClass.Value = "Qso"; break;
				case 4:
					s.Target.SpectralClass.Value = "Qso"; break;        // Hi z QSO
				case 5:
					s.Target.SpectralClass.Value = "Sky"; break;
				case 6:
					s.Target.SpectralClass.Value = "Star"; break;       // Star late
				case 7:
					s.Target.SpectralClass.Value = "Galaxy"; break;     // emission line galaxy
			}

			s.Target.Redshift.Value.Value = hdus[0].Header.GetDoubleValue("Z");
			s.Target.Redshift.Accuracy.StatErrorLow.Value = hdus[0].Header.GetDoubleValue("Z_ERR");
			s.Target.Redshift.Accuracy.StatErrorHigh.Value = hdus[0].Header.GetDoubleValue("Z_ERR");
			s.Target.Redshift.Accuracy.Confidence.Value = hdus[0].Header.GetDoubleValue("Z_CONF");

			s.MainSegment.DerivedData.Redshift.Value.Value = hdus[0].Header.GetDoubleValue("Z");
			s.MainSegment.DerivedData.Redshift.Accuracy.StatErrorLow.Value = hdus[0].Header.GetDoubleValue("Z_ERR");
			s.MainSegment.DerivedData.Redshift.Accuracy.StatErrorHigh.Value = hdus[0].Header.GetDoubleValue("Z_ERR");
			s.MainSegment.DerivedData.Redshift.Accuracy.Confidence.Value = hdus[0].Header.GetDoubleValue("Z_CONF");
			s.MainSegment.DerivedData.Snr.Value = Math.Min(Math.Min(
				hdus[0].Header.GetDoubleValue("SN_G"),
				hdus[0].Header.GetDoubleValue("SN_R")),
				hdus[0].Header.GetDoubleValue("SN_I"));

			s.MainSegment.DataId.Collection.Value = "ivo://sdss/dr4/spec";
			s.MainSegment.DataId.DatasetId.Value = "ivo://elte/sdss/dr4/spec";
			s.MainSegment.DataId.Date.Value = s.MainSegment.Coverage.Location.Time.Value;
			s.MainSegment.DataId.CreationType.Value = "Archival";

			s.MainSegment.Derivation.Value = "Observed";
			s.MainSegment.Type.Value = "Spectrum";

			s.MainSegment.Points.Flux.Value.Unit = "10**(-17) erg/cm**2/s/A";
			s.MainSegment.Points.Flux.Accuracy.Calibration.Value = "Absolute";
			s.MainSegment.Points.SpectralCoord.Value.Unit = "A";
			s.MainSegment.Points.SpectralCoord.Accuracy = new VoServices.Schema.Sed.SpectralCoordAccuracy();
			s.MainSegment.Points.SpectralCoord.Accuracy.Calibration.Value = "Absolute";
            */

            s.Target.Class.Value = "STAR";
            s.Target.Description.Value = "MILES";
            s.Target.Name.Value = hdus[0].Header.GetStringValue("OBJECT");


			Single[][] data;
			data = new Single[hdus[0].Axes[0]][];
			for (int i = 0; i < hdus[0].Axes[0]; i++)
			{
				data[i] = (Single[])((object[])hdus[0].Data.DataArray)[i];
			}

            double wl = hdus[0].Header.GetDoubleValue("CRVAL1");
            double wlinc = hdus[0].Header.GetDoubleValue("CDELT1");
            int numpoints = hdus[0].Header.GetIntValue("NAXIS1");

            s.MainSegment.SpectralCoord_Value = new double[numpoints];
            s.MainSegment.SpectralCoord_Accuracy_BinLow = new double[numpoints];
            s.MainSegment.SpectralCoord_Accuracy_BinHigh = new double[numpoints];

            s.MainSegment.Flux_Value = new double[numpoints];
            s.MainSegment.Flux_Accuracy_StatErrLow = new double[numpoints];
            s.MainSegment.Flux_Accuracy_StatErrHigh = new double[numpoints];
            s.MainSegment.Flux_Accuracy_Quality = new long[numpoints];

            // copy data to the spectrum points
            for (int i = 0; i < numpoints; i++)
            {
                //Console.WriteLine(Math.Pow(10.0, (coeff0 + i * coeff1)));
                s.MainSegment.SpectralCoord_Value[i] = wl + i * wlinc;
                s.MainSegment.SpectralCoord_Accuracy_BinLow[i] = s.MainSegment.SpectralCoord_Value[i] - wlinc / 2;
                s.MainSegment.SpectralCoord_Accuracy_BinHigh[i] = s.MainSegment.SpectralCoord_Value[i] + wlinc / 2;

                s.MainSegment.Flux_Value[i] = ((double)data[0][i]);
                s.MainSegment.Flux_Accuracy_StatErrLow[i] = 0;
                s.MainSegment.Flux_Accuracy_StatErrHigh[i] = 0;

                if (((double)data[0][i]) == 0.0)
                    s.MainSegment.Flux_Accuracy_Quality[i] = 1;
                else
                    s.MainSegment.Flux_Accuracy_Quality[i] = 0;

            }

            /*
			double coeff0 = hdus[0].Header.GetDoubleValue("COEFF0");
			double coeff1 = hdus[0].Header.GetDoubleValue("COEFF1");


			s.MainSegment.SpectralCoord_Accuracy_BinLow = new VoServices.Schema.DoubleCollection();
			s.MainSegment.SpectralCoord_Accuracy_BinHigh = new VoServices.Schema.DoubleCollection();
			// copy data to the spectrum points
			for (int i = 0; i < hdus[0].Axes[1]; i++)
			{
				//Console.WriteLine(Math.Pow(10.0, (coeff0 + i * coeff1)));
				s.MainSegment.SpectralCoord_Value.Add(Math.Pow(10.0, (coeff0 + i * coeff1)));
				s.MainSegment.SpectralCoord_Accuracy_BinLow.Add(Math.Pow(10.0, (coeff0 + (i - 0.5) * coeff1)));
				s.MainSegment.SpectralCoord_Accuracy_BinHigh.Add(Math.Pow(10.0, (coeff0 + (i + 0.5) * coeff1)));

				s.MainSegment.Flux_Value.Add((double)data[0][i]);
				s.MainSegment.Flux_Accuracy_StatErrLow.Add((double)data[2][i]);
				s.MainSegment.Flux_Accuracy_StatErrHigh.Add((double)data[2][i]);
				s.MainSegment.Flux_Accuracy_Quality.Add((int)data[3][i]);

			}

			s.MainSegment.Coverage.Region.Spectral.Min.Value = Math.Pow(10.0, (coeff0 + 0 * coeff1));
			s.MainSegment.Coverage.Region.Spectral.Max.Value = Math.Pow(10.0, (coeff0 + (hdus[0].Axes[1] - 1) * coeff1));

			s.MainSegment.Coverage.Extent.Spectral.Value =
				s.MainSegment.Coverage.Region.Spectral.Max.Value -
				s.MainSegment.Coverage.Region.Spectral.Min.Value;

			// sloan specobjid
			s.MainSegment.DataId.CreatorId.Value = "ivo://sdss/dr4/spec#" +
				toSpecID64(hdus[0].Header.GetIntValue("PLATEID"),
							hdus[0].Header.GetIntValue("MJD"),
							hdus[0].Header.GetIntValue("FIBERID"),
							0, 0).ToString();

			s.Id = toSpecID64(hdus[0].Header.GetIntValue("PLATEID"),
				hdus[0].Header.GetIntValue("MJD"),
				hdus[0].Header.GetIntValue("FIBERID"),
				0, 0);
             * */

			return s;
		}

        #endregion
        #region Spectrum load functions
        /*
        private VoServices.Spectrum.Lib.Spectrum LoadSpectrum(Guid userGuid, long id, bool loadPoints, bool loadDetails)
        {
            VoServices.Spectrum.Lib.Spectrum spec = new VoServices.Spectrum.Lib.Spectrum(true);
            spec.BasicInitialize();
            LoadSpectrum(spec, userGuid, id, loadPoints, loadDetails);
            return spec;
        }

        public override void LoadSpectrum(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid, long id, bool loadPoints, bool loadDetails)
        {
            spec.BasicInitialize();

            string sql = "spGetSpectrum";

            using (SqlCommand cmd = new SqlCommand(sql, tn.Connection, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = userGuid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        LoadSpectrumFromReader(spec, dr);

                    dr.Close();
                }
            }

            // store publisher id
            if (loadDetails)
            {
                string pubid = spec.MainSegment.DataId.DatasetId.Value;
                LoadSpectrumFields(spec, userGuid);
                spec.MainSegment.DataId.DatasetId.Value = pubid;
            }

            if (loadPoints)
                LoadSpectrumData(spec, userGuid);

            PrefixCollectionId(spec);


        }
        
        private IEnumerable<VoServices.Spectrum.Lib.Spectrum> LoadSpectrumMultiple(Guid userGuid, string[] ids, bool loadPoints, bool loadDetails)
        {
            IdSearchParameters isp = new IdSearchParameters(true);
            isp.Ids = ids;
            isp.UserGuid = userGuid;
            isp.LoadDetails = loadDetails;
            isp.LoadPoints = loadPoints;

            return GetSpectrum(isp);
        }

        private void LoadSpectrumFromReader(VoServices.Spectrum.Lib.Spectrum spec, SqlDataReader dr)
        {
            int o = -1;

            //***
            spec.PublisherId = ""; //***System.Configuration.ConfigurationSettings.AppSettings["DefaultPublisherID"];
            spec.Id = dr.GetInt64(++o);
            spec.UserGuid = dr.IsDBNull(++o) ? Guid.Empty : dr.GetGuid(o);
            spec.UserFolderId = dr.GetInt32(++o);
            spec.Public = dr.GetInt32(++o);

            spec.MainSegment.DataId.CreatorId.Value = dr.GetString(++o);
            spec.Target.Name.Value = dr.GetString(++o);
            spec.Target.Class.Value = dr.GetString(++o);
            spec.Target.SpectralClass.Value = dr.GetString(++o);



            spec.MainSegment.Derivation.Value = dr.GetString(++o);
            spec.MainSegment.DataId.Date.Value = dr.GetDateTime(++o);
            spec.MainSegment.DataId.Version.Value = dr.GetString(++o);

            spec.Target.Pos.Value = new Position(dr.GetFloat(++o), dr.GetFloat(++o));
            spec.MainSegment.Coverage.Location.Sky.Value = spec.Target.Pos.Value;

            spec.HtmId = dr.GetInt64(++o);

            spec.MainSegment.DerivedData.Snr = dr.IsDBNull(++o) ? null : new DoubleParam(dr.GetFloat(o), "");
            spec.MainSegment.DerivedData.VarAmpl = dr.IsDBNull(++o) ? null : new DoubleParam(dr.GetFloat(o), "");

            spec.MainSegment.DerivedData.Redshift.Value.Value = dr.GetFloat(++o);
            spec.MainSegment.DerivedData.Redshift.Accuracy.StatErrorHigh.Value =
                spec.MainSegment.DerivedData.Redshift.Accuracy.StatErrorLow.Value = dr.GetFloat(++o);

            spec.MainSegment.Coverage.Region.Spectral.Min.Value = dr.GetFloat(++o);
            spec.MainSegment.Coverage.Region.Spectral.Max.Value = dr.GetFloat(++o);
            if (dr.GetBoolean(++o))
                spec.MainSegment.Points.Flux.Value.Unit = "10**(-17) erg/cm**2/s/A";	//***
            else
                spec.MainSegment.Points.Flux.Value.Unit = "ADU";
        }
        
        public override void LoadSpectrumFields(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            string sql = "spGetSpectrumFields";

            using (SqlCommand cmd = new SqlCommand(sql, tn.Connection, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = userGuid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = spec.Id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        try
                        {
                            string name = dr.GetString(0);
                            VoServices.Schema.ParamBase param = null;

                            if (name.StartsWith("Segment[0]"))
                            {
                                // stripping the Segment[0]. part		/****
                                name = name.Substring("Segment[0]".Length + 1);
                                param = (VoServices.Schema.ParamBase)VoServices.Spectrum.Lib.Spectrum.GetField(spec.MainSegment, name, dr.GetInt16(2));
                            }
                            else if (name.StartsWith("Target"))
                            {
                                name = name.Substring("Target".Length + 1);
                                param = (VoServices.Schema.ParamBase)VoServices.Spectrum.Lib.Spectrum.GetField(spec.Target, name, dr.GetInt16(2));
                            }

                            if (param == null)
                            {
                                // **** Console.WriteLine("!");
                            }
                            else
                            {
                                if (param.GetType() == typeof(VoServices.Schema.TextParam))
                                    param.SetValue(dr.IsDBNull(3) ? null : dr.GetString(3));

                                if (param.GetType() == typeof(VoServices.Schema.IntParam))
                                    param.SetValue(dr.GetInt64(4));

                                if (param.GetType() == typeof(VoServices.Schema.DoubleParam))
                                    param.SetValue(dr.GetDouble(5));

                                if (param.GetType() == typeof(VoServices.Schema.TimeParam))
                                    param.SetValue(dr.GetDateTime(7));

                                if (param.GetType() == typeof(VoServices.Schema.PositionParam))
                                {
                                    ((PositionParam)param).Value =
                                        new Position(dr.GetDouble(5), dr.GetDouble(6));
                                }

                                if (param.GetType() == typeof(VoServices.Schema.BoolParam))
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

                    dr.Close();
                }
            }
        }

        public override void LoadSpectrumData(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            spec.MainSegment.SpectralCoord_Value = null;
            spec.MainSegment.SpectralCoord_Accuracy_BinLow = null;
            spec.MainSegment.SpectralCoord_Accuracy_BinHigh = null;
            spec.MainSegment.SpectralCoord_Accuracy_BinSize = null;
            spec.MainSegment.SpectralCoord_Accuracy_StatErrLow = null;
            spec.MainSegment.SpectralCoord_Accuracy_StatErrHigh = null;

            spec.MainSegment.Time_Value = null;
            spec.MainSegment.Time_Accuracy_BinLow = null;
            spec.MainSegment.Time_Accuracy_BinHigh = null;
            spec.MainSegment.Time_Accuracy_BinSize = null;
            spec.MainSegment.Time_Accuracy_StatErrLow = null;
            spec.MainSegment.Time_Accuracy_StatErrHigh = null;


            spec.MainSegment.Flux_Value = null;
            spec.MainSegment.Flux_Accuracy_Quality = null;
            spec.MainSegment.Flux_Accuracy_StatErrLow = null;
            spec.MainSegment.Flux_Accuracy_StatErrHigh = null;

            ///

            string sql = "spGetSpectrumData";

            using (SqlCommand cmd = new SqlCommand(sql, tn.Connection, tn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = userGuid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = spec.Id;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // dr["FieldName"] holds the name of the variable to load data into
                        string name = dr.GetString(0);
                        name = name.Substring(name.LastIndexOf(".") + 1);

                        FieldInfo fld = spec.MainSegment.GetType().GetField(name);
                        BinaryReader buffer = new BinaryReader(dr.GetSqlBytes(3).Stream);

                        if (fld.FieldType == typeof(double[]))
                        {
                            double[] val = new double[buffer.BaseStream.Length / sizeof(double)];
                            for (int i = 0; i < val.Length; i++)
                                val[i] = buffer.ReadDouble();

                            fld.SetValue(spec.MainSegment, val);
                        }
                        else if (fld.FieldType == typeof(long[]))
                        {
                            long[] val = new long[buffer.BaseStream.Length / sizeof(long)];
                            for (int i = 0; i < val.Length; i++)
                                val[i] = buffer.ReadInt64();

                            fld.SetValue(spec.MainSegment, val);
                        }
                    }

                    dr.Close();
                }
            }
        }
        
        private IEnumerable<VoServices.Spectrum.Lib.Spectrum> LoadSpectraFromReader(SqlDataReader dr, Guid userGuid, bool loadPoints, bool loadDetails)
        {
            while (dr.Read())
            {
                VoServices.Spectrum.Lib.Spectrum spec;

                spec = new VoServices.Spectrum.Lib.Spectrum(true);
                spec.BasicInitialize();
                LoadSpectrumFromReader(spec, dr);
                PrefixCollectionId(spec);

                // store publisher id
                if (loadDetails)
                {
                    string pubid = spec.MainSegment.DataId.DatasetId.Value;
                    LoadSpectrumFields(spec, userGuid);
                    spec.MainSegment.DataId.DatasetId.Value = pubid;
                }

                if (loadPoints)
                    LoadSpectrumData(spec, userGuid);

                PrefixCollectionId(spec);

                yield return spec;
            }

            dr.Close();
            dr.Dispose();
        }
        */
        #endregion
        #region Spectrum save functions
        /*
        public override long SaveSpectrum(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            //*** user access check
            spec.CalculateHtmId();
            spec.UserGuid = userGuid;

            outfile = new StreamWriter(fileName);
            
            CreateSpectrum(spec, userGuid);
            
            SaveSpectrumFields(spec, userGuid);
            SaveSpectrumData(spec, userGuid);

            outfile.Close();
            outfile.Dispose();
            outfile = null;

            //return spec.Id;
            return 0;
        }

        private void CreateSpectrum(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            outfile.WriteLine("# Exported from Spectrum Service: http://voservices.net/spectrum");

            //DbUtil.AddParameter(cmd, "UserGUID", DbType.StringFixedLength, 36, spec.UserGuid.ToString());
            outfile.WriteLine("# UserFolderId: {0}", spec.UserFolderId);
            outfile.WriteLine("# Public: {0}", spec.Public);
            outfile.WriteLine("# CreatorID: {0}", spec.MainSegment.DataId.CreatorId.Value);
            outfile.WriteLine("# Name: {0}", spec.Target.Name.Value);
            outfile.WriteLine("# TargetClass: {0}", spec.Target.Class.Value);
            outfile.WriteLine("# SpectralClass: {0}", spec.Target.SpectralClass.Value);
            outfile.WriteLine("# Derivation: {0}", spec.MainSegment.Derivation.Value);
            outfile.WriteLine("# Date: {0}", spec.MainSegment.DataId.Date.Value);
            outfile.WriteLine("# Version: {0}", spec.MainSegment.DataId.Version.Value);
            outfile.WriteLine("# Ra: {0}", spec.Target.Pos.Value.Ra);
            outfile.WriteLine("# Dec: {0}", spec.Target.Pos.Value.Dec);
            outfile.WriteLine("# HTMID: {0}", spec.HtmId);
            outfile.WriteLine("# Snr: {0}", spec.MainSegment.DerivedData.Snr.Value);
            outfile.WriteLine("# VarAmpl: {0}", spec.MainSegment.DerivedData.VarAmpl.Value);
            outfile.WriteLine("# Redshift: {0}", spec.MainSegment.DerivedData.Redshift.Value.Value);
            outfile.WriteLine("# RedshiftError: {0}", spec.MainSegment.DerivedData.Redshift.Accuracy.StatErrorHigh.Value);
            outfile.WriteLine("# WavelengthMin: {0}", spec.MainSegment.Coverage.Region.Spectral.Min.Value);
            outfile.WriteLine("# WavelengthMax: {0}", spec.MainSegment.Coverage.Region.Spectral.Max.Value);
            outfile.WriteLine("# FluxCalibrated: {0}", (string.Compare(spec.MainSegment.Points.Flux.Value.Unit.Trim().ToLower(), "adu", true) == 0) ? 0 : 1);
        }

        private void SaveSpectrumFields(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            //DbUtil.AddParameter(cmd, "FieldName", DbType.String, 128);
            //DbUtil.AddParameter(cmd, "SpectrumId", DbType.Int64, -1, spec.Id);
            //DbUtil.AddParameter(cmd, "DataType", DbType.Int16);
            //DbUtil.AddParameter(cmd, "Value_String", DbType.String, 255);
            //DbUtil.AddParameter(cmd, "Value_Int", DbType.Int64);
            //DbUtil.AddParameter(cmd, "Value_Double", DbType.Double);
            //DbUtil.AddParameter(cmd, "Value_Double2", DbType.Double);
            //DbUtil.AddParameter(cmd, "Value_Time", DbType.Date);
            //DbUtil.AddParameter(cmd, "Value_Time2", DbType.Date);
            //DbUtil.AddParameter(cmd, "Unit", DbType.String, 50);
            //DbUtil.AddParameter(cmd, "Ucd", DbType.String, 50);
            //DbUtil.AddParameter(cmd, "Key", DbType.String, 50);

            SaveSpectrumFields_Group(spec.Target, "Target");
            SaveSpectrumFields_Group(spec.MainSegment, "Segment[0]");
        }

        private void SaveSpectrumFields_Group(object obj, string name)
        {
            // adding parameters and groups
            if (obj != null)
            {
                FieldInfo[] flds = obj.GetType().GetFields();
                for (int i = 0; i < flds.Length; i++)
                {
                    if (GetBaseType(flds[i].FieldType) == typeof(VoServices.Schema.ParamBase))
                    {
                        SaveSpectrumFields_Param(flds[i].GetValue(obj), name + "." + flds[i].Name);
                    }
                    if (GetBaseType(flds[i].FieldType) == typeof(VoServices.Schema.Group))
                    {
                        SaveSpectrumFields_Group(flds[i].GetValue(obj), name + "." + flds[i].Name);
                    }
                    if (flds[i].FieldType == typeof(VoServices.Schema.ParamCollection))
                    {
                        SaveSpectrumFields_ParamCollection((ParamCollection)flds[i].GetValue(obj), name + "." + flds[i].Name);
                    }
                }
            }
        }

        private void SaveSpectrumFields_ParamCollection(ParamCollection coll, string name)
        {
            if (coll != null)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    ParamBase par = (ParamBase)coll[i];
                    SaveSpectrumFields_Param(par, name + "." + par.Key);
                }
            }
        }

        private void SaveSpectrumFields_Param(object par, string name)
        {
            if (par != null)
            {
                ParamBase param = (ParamBase)par;
                outfile.WriteLine("# ${0}: {1}", name, param.GetValue());
            }
            
            /*
            if (param != null)
            {
                if (par.GetType() == typeof(VoServices.Schema.TextParam))
                {
                    DbUtil.GetParameter(cmd, "Value_String").Value = DbUtil.DBNull(((VoServices.Schema.TextParam)param).Value);
                    DbUtil.GetParameter(cmd, "DataType").Value = 0;
                }

                if (par.GetType() == typeof(VoServices.Schema.IntParam))
                {
                    DbUtil.GetParameter(cmd, "Value_Int").Value = DbUtil.DBNull(((VoServices.Schema.IntParam)param).Value);
                    DbUtil.GetParameter(cmd, "DataType").Value = 1;
                }

                if (par.GetType() == typeof(VoServices.Schema.DoubleParam))
                {
                    DbUtil.GetParameter(cmd, "Value_Double").Value = DbUtil.DBNull(((VoServices.Schema.DoubleParam)param).Value);
                    DbUtil.GetParameter(cmd, "DataType").Value = 2;
                }

                if (par.GetType() == typeof(VoServices.Schema.TimeParam))
                {
                    DbUtil.GetParameter(cmd, "Value_Time").Value = DbUtil.DBNull(((VoServices.Schema.TimeParam)param).Value);
                    DbUtil.GetParameter(cmd, "DataType").Value = 3;
                }

                if (par.GetType() == typeof(VoServices.Schema.PositionParam))
                {
                    DbUtil.GetParameter(cmd, "Value_Double").Value = DbUtil.DBNull(((VoServices.Schema.PositionParam)param).Value.Ra);
                    DbUtil.GetParameter(cmd, "Value_Double2").Value = DbUtil.DBNull(((VoServices.Schema.PositionParam)param).Value.Dec);
                    DbUtil.GetParameter(cmd, "DataType").Value = 4;
                }

                if (par.GetType() == typeof(VoServices.Schema.BoolParam))
                {
                    DbUtil.GetParameter(cmd, "Value_Int").Value = DbUtil.DBNull(Convert.ToInt64(((VoServices.Schema.TimeParam)param).Value));
                    DbUtil.GetParameter(cmd, "DataType").Value = 5;
                }

                DbUtil.GetParameter(cmd, "Unit").Value = DbUtil.DBNull(param.Unit);
                DbUtil.GetParameter(cmd, "Ucd").Value = DbUtil.DBNull(param.Ucd);
                DbUtil.GetParameter(cmd, "Key").Value = DbUtil.DBNull(param.Key);

                cmd.ExecuteNonQuery();
            }
             * *
        }

        private void SaveSpectrumData(VoServices.Spectrum.Lib.Spectrum spec, Guid userGuid)
        {
            SaveSpectrumData_Segment(spec.MainSegment, "Segment[0]");
        }

        private void SaveSpectrumData_Segment(object obj, string name)
        {
            // Write headers
            /* ***
            outfile.Write("#");
            FieldInfo[] flds = obj.GetType().GetFields();
            for (int i = 0; i < flds.Length; i++)
            {
                if (flds[i].FieldType == typeof(double[]) ||
                    flds[i].FieldType == typeof(long[]))
                {
                    outfile.Write(" " + flds[i].Name);

                    //SaveSpectrumData_Array(flds[i], obj, name + "." + flds[i].Name);
                }
            }

            outfile.WriteLine();
             * *

            // Write data
            VoServices.Schema.Sed.Segment seg = (VoServices.Schema.Sed.Segment) obj;
            outfile.WriteLine("# Wavelength Flux ErrorLo ErrorHi Mask");
            for (int i = 0; i < seg.SpectralCoord_Value.Length; i++)
            {
                
                outfile.WriteLine("{0} {1} {2} {3}",
                    seg.SpectralCoord_Value == null ? 0 : seg.SpectralCoord_Value[i],
                     seg.Flux_Value == null ? 0 : seg.Flux_Value[i],
                     seg.Flux_Accuracy_StatErrLow == null ? 0 : seg.Flux_Accuracy_StatErrLow[i],
                     seg.Flux_Accuracy_StatErrHigh == null ? 0 : seg.Flux_Accuracy_StatErrHigh[i]
                     //seg.Flux_Accuracy_Quality == null ? 0 : seg.Flux_Accuracy_Quality[i]
                     );
            }
        }

        private void SaveSpectrumData_Array(FieldInfo fld, object obj, string name)
        {
            /*
            DbUtil.GetParameter(cmd, "FieldName").Value = name;

            Array data = (Array)fld.GetValue(obj);

            if (data != null)
                if (data.Length > 0)
                {
                    MemoryStream buffer = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(buffer);

                    if (fld.FieldType == typeof(long[]))
                    {
                        DbUtil.GetParameter(cmd, "DataType").Value = 0;
                        for (int i = 0; i < data.Length; i++)
                            writer.Write(((long[])data)[i]);
                    }

                    if (fld.FieldType == typeof(double[]))
                    {
                        DbUtil.GetParameter(cmd, "DataType").Value = 1;
                        for (int i = 0; i < data.Length; i++)
                            writer.Write(((double[])data)[i]);
                    }

                    DbUtil.GetParameter(cmd, "Data").Value = buffer.ToArray();

                    cmd.ExecuteNonQuery();
                }
             * *
        }
        */
        #endregion

        #region Utility functions

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

        #endregion
        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: FitsConnector.cs,v $
        Revision 1.1  2008/01/08 22:01:35  dobos
        Initial checkin


*/
#endregion
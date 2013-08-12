using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Jhu.SpecSvc.FilterLib
{

	public enum FilterWavelengthScale
	{
		Any = -1,
		Linear = 0,
		Logarithmic = 1,
		Other = 2
	}

	public enum FilterResponseValueMethod
	{
		Exact = 0,
		LinearInterpolation = 1,
		NearestWavelength = 2
	}

	public class Filter : BaseObject
	{

		// Header data
		[XmlAttribute] public int Id = 0;				// Database id of the filter
		[XmlIgnore] public string UserGuid = "";	    // Id of the registrant of this filter
		[XmlAttribute] public string Name = "";			// Name of the filter
		[XmlElement]   public string Description = "";	// Long description of the filter
		[XmlAttribute] public string Ucd = "";			// UCD of filter
		[XmlAttribute] public string Version = "";		// Version of profile
		[XmlAttribute] public DateTime DateCreated;
		[XmlAttribute] public DateTime DateModified;
		[XmlAttribute] public double WavelengthMin = 0;	// Minimum wavelength
		[XmlAttribute] public double WavelengthMax = 0;	// Maximum wavelength
		[XmlAttribute] public double WavelengthEff = 0;	// Effective wavelength
		[XmlIgnore] public FilterWavelengthScale WavelengthScale = FilterWavelengthScale.Linear;	// Scale of the wavelength axis
		[XmlAttribute] public double EffectiveWidth = 0; // Effective width
		[XmlAttribute] public string Unit = "";

		[XmlArray("ArrayOfResponse")]
		[XmlArrayItem("Response", typeof(Response))]
		public ResponseCollection Responses;	// Array for storing wavelength-response pairs

		/// <summary>
		/// Default constructor
		/// </summary>
		public Filter()
		{
			InitializeMembers();
		}

		/// <summary>
		/// Constructor with database connection
		/// </summary>
		/// <param name="databaseConnetion"></param>
		public Filter(SqlConnection databaseConnetion, SqlTransaction databaseTransaction)
            : base(databaseConnetion, databaseTransaction)
		{
			InitializeMembers();
		}

		[XmlAttribute("WavelengthScale")]
		public int WavelengthScale_ForXml
		{
			get
			{
				return (int) WavelengthScale;
			}
			set
			{
				WavelengthScale = (FilterWavelengthScale) value;
			}
		}

		/// <summary>
		/// Initializes member variables. Called by all constructors
		/// </summary>
		private void InitializeMembers()
		{
			Responses = new ResponseCollection(this);
			DateCreated = DateTime.Now;
			DateModified = DateTime.Now;
		}

		/// <summary>
		/// Initializes the header variable from the passed DataRow objects
		/// </summary>
		/// <param name="dr">DataRow object to initialize header variable from</param>
		public void LoadFromDatarow(DataRow dr)
		{
			Id = (int) dr["ID"];
			UserGuid = ((Guid) dr["UserGUID"]).ToString();
			Name = (string) dr["Name"];
			Description = (string) dr["Description"];
			Ucd = (string) dr["UCD"];
			Version = (string) dr["Version"];
			DateCreated = (DateTime) dr["DateCreated"];
			DateModified = (DateTime) dr["DateModified"];
			WavelengthMin = (double) dr["WavelengthMin"];
			WavelengthMax = (double) dr["WavelengthMax"];
			WavelengthEff = (double) dr["WavelengthEff"];
			WavelengthScale = (FilterWavelengthScale) dr["WaveLengthScale"];
			EffectiveWidth = (double) dr["EffectiveWidth"];
			Unit = (string) dr["Unit"];
		}

		/// <summary>
		/// Creates a new Filter object based on data in the passed DataRow object
		/// </summary>
		/// <param name="dr">DataRow from which the new Filter object should be created</param>
		/// <returns>Returns the new Filter object</returns>
		public static Filter CreateFromDatarow(DataRow dr)
		{
			Filter filter = new Filter();
			filter.LoadFromDatarow(dr);

			return filter;
		}

		/// <summary>
		/// Loads the filter from the database. The Id property must be set first!
		/// </summary>
		public void Load()
		{
			DataSet ds = LoadAsDataSet();

			// Data now stored in ds DataSet, now filling the member variables
            LoadFromDatarow(ds.Tables[0].Rows[0]);

			ds.Dispose();
		}

		/// <summary>
		/// Loads the filter header data from the database.
		/// </summary>
		/// <param name="id">Database id of the filter</param>
		public void Load(int id)
		{
			Id = id;
			Load();
		}

		public DataSet LoadAsDataSet()
		{
			// Building and executing of the database command
			string sql = "sp_GetFilter";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;

			DataSet ds = ExecuteCommand(cmd);
			
			cmd.Dispose();

			return ds;
		}

		public DataSet LoadAsDataSet(int id)
		{
			Id = id;
			return LoadAsDataSet();
		}

		/// <summary>
		/// Stores the filter in the database. It creates a new record, if required, or
		/// just modifies the exiting one
		/// </summary>
		public void Save()
		{
			if (Id == 0)
			{
				// Creating a new filter object in the database
				string sql = "sp_CreateFilter";
				SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = new Guid(UserGuid);
				cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = Name;
				cmd.Parameters.Add("@Description", SqlDbType.NText).Value = Description;
				cmd.Parameters.Add("@UCD", SqlDbType.NVarChar, 255).Value = Ucd;
				cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 15).Value = Version;
				cmd.Parameters.Add("@WavelengthScale", SqlDbType.Int).Value = WavelengthScale;
				cmd.Parameters.Add("@Unit", SqlDbType.NVarChar, 15).Value = Unit;
				cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;

				ExecuteCommandNonquery(cmd);

				Id = (int) cmd.Parameters["@NewID"].Value;
			
				cmd.Dispose();
			}
			else
			{
				// Modifying an existing filter object in the database

				string sql = "sp_ModifyFilter";
				SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;
				cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = new Guid(UserGuid);
				cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = Name;
				cmd.Parameters.Add("@Description", SqlDbType.NText).Value = Description;
				cmd.Parameters.Add("@UCD", SqlDbType.NVarChar, 255).Value = Ucd;
				cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 15).Value = Version;
				cmd.Parameters.Add("@WavelengthScale", SqlDbType.Int).Value = WavelengthScale;
				cmd.Parameters.Add("@Unit", SqlDbType.NVarChar, 15).Value = Unit;

				ExecuteCommandNonquery(cmd);
			
				cmd.Dispose();
			}
		}

		/// <summary>
		/// Deletes this filter from the database
		/// </summary>
		public void Delete()
		{
			// Building and executing of the database command
			string sql = "sp_DeleteFilter";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;

			ExecuteCommandNonquery(cmd);
			
			cmd.Dispose();
		}

		/// <summary>
		/// Prints the whole filter data in tabular format for debug purposes
		/// </summary>
		public void Print(TextWriter writer)
		{
			for (int i = 0; i < Responses.Count; i++)
			{
				writer.WriteLine("{0}, {1}", Responses[i].Wavelength, Responses[i].Value);
			}
		}

		/// <summary>
		/// Loads all wavelenth-response pairs from the database and puts into the Responses collection
		/// </summary>
		public void LoadResponses()
		{
			DataSet ds = LoadResponsesAsDataSet();

			DataRowCollection rows = ds.Tables[0].Rows;

			for (int i = 0; i < rows.Count; i++)
			{
				Response res = new Response(DatabaseConnection, DatabaseTransaction);
				res.LoadFromDatarow(rows[i]);
				Responses.Add(res);
			}

			ds.Dispose();
		}

		public DataSet LoadResponsesAsDataSet()
		{
			// Building and executing of the database command
			string sql = "sp_QueryFilterResponses";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;

			DataSet ds = ExecuteCommand(cmd);

			cmd.Dispose();

			return ds;
		}

		/// <summary>
		/// Loads responses from a whitespace delimited string (text file)
		/// </summary>
		/// <param name="data"></param>
		public void LoadResponsesFromString(string data)
		{
			try
			{
				StringReader reader = new StringReader(data);
				string line;

				Regex regex = new Regex("([\\+\\-]?[0-9]+[\\.\\,]?[0-9]*[Ee]?[\\+\\-]?[0-9]*)");				

				while ((line = reader.ReadLine()) != null)
				{
					if (!line.TrimStart().StartsWith("#"))
					{
						MatchCollection m = regex.Matches(line);

						Response response = new Response(DatabaseConnection, DatabaseTransaction);

						response.Id = 0;
						response.FilterId = Id;
						response.Wavelength = double.Parse(m[0].Groups[0].Value);
						response.Value = double.Parse(m[1].Groups[0].Value);

						Responses.Add(response);
					}
				}

                reader.Close();
			}
			catch (System.Exception)
			{
			}
		}

		/// <summary>
		/// Deletes every wavelength-response pairs from the database belonging to the filter and saves all
		/// data stored in the Responses collection
		/// </summary>
		public void SaveResponses()
		{
			// Building and executing of the database command
			string sql = "sp_DeleteFilterResponses";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;

			ExecuteCommandNonquery(cmd); 

			// Saving the rows one-by-one
			for (int i = 0; i < Responses.Count; i++)
			{
				Responses[i].DatabaseConnection = this.DatabaseConnection;
				Responses[i].FilterId = Id;
				Responses[i].Id = 0;
				Responses[i].Save();
			}

			cmd.Dispose();

			// Updating WavelengthMin etc. fields
			UpdateMinMaxEffValues();
		}

		/// <summary>
		/// Executes a stored procedure on the database to update the fields storing the minimum, maximum and
		/// effective wavelength of the filter
		/// </summary>
		public void UpdateMinMaxEffValues()
		{
			// Building and executing of the database command
			string sql = "sp_UpdateFilterMinMaxEffValues";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;

			ExecuteCommandNonquery(cmd);

			cmd.Dispose();
		}

		/// <summary>
		/// Reads a single response value of the filter. Can use several methods to interpolate between available values.
		/// </summary>
		/// <param name="wavelength">Wavelength parameter of the filter</param>
		/// <param name="method">Method of interpolation, see FilterResponseValueMethod enum also.</param>
		/// <returns>The response value at the given wavelength</returns>
		public double GetResponseValue(double wavelength, FilterResponseValueMethod method)
		{
			// Building and executing of the database command
			string sql = "";
			
			switch (method)
			{
				case FilterResponseValueMethod.Exact:
					sql = "sp_GetFilterResponseValue";
					break;
				case FilterResponseValueMethod.LinearInterpolation:
					sql = "sp_GetFilterResponseValue_Interpolate";
					break;
				case FilterResponseValueMethod.NearestWavelength:
					sql = "sp_GetFilterResponseValue_Nearest";
					break;
			}
			
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = Id;
			cmd.Parameters.Add("@Wavelength", SqlDbType.Float).Value = wavelength;
			cmd.Parameters.Add("@Value", SqlDbType.Float).Direction = ParameterDirection.Output;

			ExecuteCommandNonquery(cmd);

			double retval = 0;
			if (cmd.Parameters["@Value"].Value != DBNull.Value)
				retval = (double) cmd.Parameters["@Value"].Value;

			cmd.Dispose();

			return retval;			
		}

		/// <summary>
		/// This method resamples the filter function to a linear or logarithimic scale using linear interpolation
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="start"></param>
		/// <param name="inc"></param>
		public void Resample(FilterWavelengthScale scale, double start, double inc, int points)
		{
			// Creating the new ResponseCollection to store the new points
			ResponseCollection newres = new ResponseCollection(this);

			// Sorting the old ResponseCollection
			Responses.Sort();

			// Calculating new points
			int pointer = 0;
			for (int i = 0; i < points; i ++)
			{
				double wavelength;
				double response;
				
				if (scale == FilterWavelengthScale.Linear)
					wavelength = start + i * inc;	
				else
					wavelength = Math.Pow(10.0, start + i * inc);

				// If the wavelength is outside the sampled area, the resampled value must be 0
				if (wavelength < Responses[0].Wavelength)
					response = 0;
				else if (wavelength > Responses[Responses.Count - 1].Wavelength)
					response = 0;
				else
				{
					// in this case the filter covers the wavelength
					// now we have to step with the pointer as far as the pointed wavelength is larger than the required
					while (Responses[pointer].Wavelength < wavelength) pointer ++;
					
					if (Responses[pointer].Wavelength == wavelength)
					{
						// Exact match
						response = Responses[pointer].Value;
					}
					else
					{
						// interpolating between pointer - 1 and pointer 
						//response = Responses[pointer - 1].Value + (Responses[pointer].Value - Responses[pointer - 1].Value) / (Responses[pointer].Wavelength - Responses[pointer - 1].Wavelength) * (wavelength - Responses[pointer - 1].Wavelength);

						// using the nearest neighbor instead of interpolation
						if (Responses[pointer - 1].Wavelength - wavelength < Responses[pointer].Wavelength)
							response = Responses[pointer - 1].Value;
						else
							response = Responses[pointer].Value;

					}
				}

				// Now we can add it to the new collection
				Response r = new Response(DatabaseConnection, DatabaseTransaction);
				r.FilterId = Id;
				r.Id = 0;
				r.Wavelength = wavelength;
				r.Value = response;
				newres.Add(r);
			}

			// Replacing the old collection with the new one, the old one will be disposed by the framework
			Responses = newres;
			this.WavelengthScale = scale;
			this.WavelengthMin = Responses[0].Wavelength;
			this.WavelengthMax = Responses[Responses.Count - 1].Wavelength;
		}

		public void Normalize()
		{
			double maxvalue = 0;
			for (int i = 0; i < Responses.Count; i ++)
			{
				if (maxvalue < Responses[i].Value) maxvalue = Responses[i].Value;
			}

			if (maxvalue != 0)
			{
				for (int i = 0; i < Responses.Count; i ++)
				{
					Responses[i].Value /= maxvalue;
				}
			}
		}

		public void Normalize(double start, double end, double valueTo)
		{
		}

		/// <summary>
		/// Returns the index of the Response object which represents the point with less then the given wavelength.
		/// It returns -1 if the given wavelength is less than the minimum or more than the maximum
		/// 
		/// Function assumes that the ResponseCollection already sorted.
		/// </summary>
		/// <param name="wavelength"></param>
		/// <returns></returns>
		private int FindResponseIndex(double wavelength)
		{
			if (Responses.Count == 0) return -1;
			if (wavelength < Responses[0].Wavelength ||
				wavelength > Responses[Responses.Count - 1].Wavelength) return -1;

			// Now we have to find the required index
			int result = Responses.BinarySearch(new Response(wavelength, 0));

			if (result < 0)
			{
				return ~result;
			}
			else
			{
				return result;
			}
		}

		/// <summary>
		/// Gets the CVS revision number (string)
		/// </summary>
		static public string Revision
		{
			get { return "$Revision: 1.1 $"; }
		}

//		public void PlotGraph(int width, int height, int margin,
//			double wavelengthmin, double wavelengthmax, double valuemin, double valuemax,
//			Graphics g, Pen pen)
//		{
//			if (Responses.Count < 2) return;
//
//			Point ptfrom, ptto;
//			ptfrom = new Point(0,0);
//
//			for (int i = 0; i < Responses.Count; i++)
//			{
//				ptto = new Point((int) (margin + (width - margin) / (wavelengthmax - wavelengthmin) * (Responses[i].Wavelength - wavelengthmin)), 
//					height - margin - 1 - (int) ((height - margin) / (valuemax - valuemin) * (Responses[i].Value - valuemin)));
//
//				if (i > 0)
//					g.DrawLine(pen, ptfrom, ptto);
//				
//				ptfrom = ptto;
//			}
//
//			ptfrom = new Point((int) (margin + (width - margin) / (wavelengthmax - wavelengthmin) * (WavelengthEff - wavelengthmin)), 
//				 height - margin - 4);
//			ptto = new Point(ptfrom.X, height - margin - 7);
//			g.DrawLine(pen, ptfrom, ptto);
//		}

		public string GetInAscii()
		{
			StringWriter writer = new StringWriter();

			writer.WriteLine("# Generated by the Filter Profile Service @ http://skyservice.pha.jhu.edu");
			writer.WriteLine("# Id = {0}", this.Id);
			writer.WriteLine("# Name = \"{0}\"", this.Name);
			writer.WriteLine("# Description = \"{0}\"", this.Description.Replace("\n", "|"));
			writer.WriteLine("# UCD = \"{0}\"", this.Ucd);
			writer.WriteLine("# Version = \"{0}\"", this.Version);
			writer.WriteLine("# DateCreated = {0}", this.DateCreated.ToString());
			writer.WriteLine("# DateModified = {0}", this.DateModified.ToString());
			writer.WriteLine("# WavelengthMin = {0}", this.WavelengthMin.ToString());
			writer.WriteLine("# WavelengthMax = {0}", this.WavelengthMax.ToString());
			writer.WriteLine("# WavelengthEff = {0}", this.WavelengthEff.ToString());
			writer.WriteLine("# WavelengthScale = {0}", this.WavelengthScale.ToString());
			writer.WriteLine("# EffectiveWidth = {0}", this.EffectiveWidth.ToString());
			writer.WriteLine("# Unit = {0}", this.Unit);

			writer.WriteLine("# Wavelength Value");

			for (int i = 0; i < Responses.Count; i ++)
			{
				writer.WriteLine("{0} {1}", Responses[i].Wavelength, Responses[i].Value);
			}

			return writer.GetStringBuilder().ToString();
		}
	}
}

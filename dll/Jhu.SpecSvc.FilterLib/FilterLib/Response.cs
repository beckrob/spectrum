using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.FilterLib
{

	public class Response : BaseObject, IComparable
	{
		[XmlIgnore] public int Id = 0;
		[XmlIgnore] public int FilterId = 0;
		[XmlAttribute] public double Wavelength = 0;
		[XmlAttribute] public double Value = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public Response()
		{
		}

		/// <summary>
		/// Constructor with database connection initialization
		/// </summary>
		/// <param name="databaseConnection"></param>
		public Response(SqlConnection databaseConnection, SqlTransaction databaseTransaction)
            : base(databaseConnection, databaseTransaction)
		{
		}

		public Response(double wavelength, double response)
		{
			Wavelength = wavelength;
			Value = response;
		}

		/// <summary>
		/// Initializes member variables based on the passed DataRow object
		/// </summary>
		/// <param name="dr">DataRow object to initialize from</param>
		public void LoadFromDatarow(DataRow dr)
		{
			Id = (int) dr["ID"];
			FilterId = (int) dr["FilterID"];
			Wavelength = (double) dr["Wavelength"];
			Value = (double) dr["Value"];
		}

		/// <summary>
		/// Static member creates a new Response object based on the passed DataRow object. Does not set the
		/// database connection property
		/// </summary>
		/// <param name="dr">DataRow object to create from</param>
		/// <returns></returns>
		public static Response CreateFromDatarow(DataRow dr)
		{
			Response response = new Response();
			response.LoadFromDatarow(dr);

			return response;
		}

		/// <summary>
		/// Loads the Response object from the database. The Id member variable determines which Response to load
		/// </summary>
		public void Load()
		{
			// Building and executing of the database command
			string sql = "sp_GetResponse";
            SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Id;

			DataSet ds = ExecuteCommand(cmd);
			
			cmd.Dispose();

			// Data now stored in ds DataSet, now filling the member variables
			LoadFromDatarow(ds.Tables[0].Rows[0]);

			ds.Dispose();
		}

		/// <summary>
		/// Loads the Response object from the database. The id parameter determines which Response to load
		/// </summary>
		/// <param name="id">Determines which response to load</param>
		public void Load(int id)
		{
			Id = id;
			Load();
		}

		/// <summary>
		/// Saves the Response object to the database. If the Id member variable is zero, it creates a new record,
		/// anyway modifies an exisitng one (modification is not implemented!)
		/// </summary>
		public void Save()
		{
			if (Id == 0)
			{
				// Creating a new filter object in the database
				string sql = "sp_CreateFilterResponse";
                SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.Add("@FilterID", SqlDbType.Int).Value = FilterId;
				cmd.Parameters.Add("@Wavelength", SqlDbType.Real).Value = Wavelength;
				cmd.Parameters.Add("@Value", SqlDbType.Real).Value = Value;
				cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;

				ExecuteCommandNonquery(cmd);

				Id = (int) cmd.Parameters["@NewID"].Value;
			
				cmd.Dispose();
			}
			else
			{
				// Modifying an existing filter object in the database
				// *******************
				//Not implemented
			}
		}

		/// <summary>
		/// Deletes the Response object from the database identified by the Id member variable. Not implemented!
		/// </summary>
		public void Delete()
		{
			// Building and executing of the database command
			// ****************
			// Not implemented
		}

		/// <summary>
		/// Compares two Response object by their Wavelength parameter. Required for sorting the ResponseCollection list
		/// by wavelength
		/// </summary>
		/// <param name="obj">Response object to compare with</param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			Response fp = (Response) obj;
			if (this.Wavelength < fp.Wavelength)
				return -1;
			else if (this.Wavelength == fp.Wavelength)
				return 0;
			else
				return 1;
		}

		/// <summary>
		/// Gets the CVS revision number (string)
		/// </summary>
		static public string Revision
		{
			get { return "$Revision: 1.1 $"; }
		}
	}
}

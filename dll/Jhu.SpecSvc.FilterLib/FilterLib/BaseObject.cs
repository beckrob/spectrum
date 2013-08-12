using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;


namespace Jhu.SpecSvc.FilterLib
{
	[XmlType("FilterBaseObject")]
	public class BaseObject
	{
		[XmlIgnore] public SqlConnection DatabaseConnection;
        [XmlIgnore] public SqlTransaction DatabaseTransaction;
		
		public BaseObject()
		{
		}

		public BaseObject(SqlConnection databaseConnection, SqlTransaction databaseTransaction)
		{
			DatabaseConnection = databaseConnection;
            DatabaseTransaction = databaseTransaction;
		}

		public void ExecuteCommandNonquery(SqlCommand cmd)
		{
			cmd.ExecuteNonQuery();
		}

		public DataSet ExecuteCommand(SqlCommand cmd)
		{
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			da.Fill(ds);

			da.Dispose();

			return ds;
		}

		protected static string GetTypeBehaviour(System.Type type)
		{
			if (type.IsArray) return "Array";
			else if (type.IsClass) return "Class";
			else if (type.IsInterface) return "Interface";
			else return "Value";
		}
	}
}
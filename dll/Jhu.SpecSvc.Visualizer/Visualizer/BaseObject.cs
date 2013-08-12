#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Visualizer classes are for plotting spectra on
 * the webpage
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: BaseObject.cs,v 1.1 2008/01/08 22:53:42 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:53:42 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Collections;


namespace VoServices.SpecSvc.Visualizer
{
	public class BaseObject
	{
		[XmlIgnore] public SqlConnection DatabaseConnection;
		
		public BaseObject()
		{
		}

		public BaseObject(SqlConnection databaseConnection)
		{
			DatabaseConnection = databaseConnection;
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

		protected object GetColumn(object source, string name)
		{
			try
			{
				if (source.GetType() == typeof(DataRow))
				{
					return ((DataRow) source)[name];
				}
				else if (source.GetType() == typeof(SqlDataReader))
				{
					return ((SqlDataReader) source).GetValue(((SqlDataReader) source).GetOrdinal(name));
				}
			}
			catch
			{
			}
			return null;
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
#region Revision History
/* Revision History

        $Log: BaseObject.cs,v $
        Revision 1.1  2008/01/08 22:53:42  dobos
        Initial checkin


*/
#endregion
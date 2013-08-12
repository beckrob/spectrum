#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: DbUtil.cs,v 1.1 2008/01/08 22:01:34 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:34 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Jhu.SpecSvc.IO
{
	/// <summary>
	/// Summary description for DbUtil.
	/// </summary>
	public class DbUtil
	{
		public DbUtil()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void SetCommandString(object cmd, string sql)
		{
			if (cmd.GetType() == typeof(SqlCommand))
			{
				SqlCommand scmd = (SqlCommand) cmd;
				scmd.CommandText = sql;
			}
			else if (cmd.GetType() == typeof(MySqlCommand))
			{
				MySqlCommand mcmd = (MySqlCommand) cmd;
				mcmd.CommandText = sql.Replace("@@", "$$").Replace("@", "?").Replace("$$", "@@").Replace("[", "`").Replace("]", "`");
			}
		}

		public static IDataParameter AddParameter(object cmd, string name, DbType type, int size, object value)
		{
			if (cmd.GetType() == typeof(SqlCommand))
			{
				SqlCommand scmd = (SqlCommand) cmd;
				SqlParameter par = new SqlParameter();
				
				par.ParameterName = "@" + name;
				par.DbType = type;
				if (size != -1)
					par.Size = size;
				if (value != null)
					par.Value = value;

				scmd.Parameters.Add(par);
				return par;
			}
			else if (cmd.GetType() == typeof(MySqlCommand))
			{
				MySqlCommand mcmd = (MySqlCommand) cmd;
				MySqlParameter par = new MySqlParameter();

				par.ParameterName = name;
				par.DbType = type;
				if (size != -1)
					par.Size = size;
				if (value != null)
					par.Value = value;

				mcmd.Parameters.Add(par);
				return par;
			}

			return null;
		}

		public static IDataParameter GetParameter(object cmd, string name)
		{
			if (cmd.GetType() == typeof(SqlCommand))
			{
				return ((SqlCommand) cmd).Parameters["@" + name];
			}
			else if (cmd.GetType() == typeof(MySqlCommand))
			{
				return ((MySqlCommand) cmd).Parameters[name];
			}
			return null;
		}

		public static IDataParameter AddParameter(object cmd, string name, DbType type)
		{
			return AddParameter(cmd, name, type, -1, null);
		}

		public static IDataParameter AddParameter(object cmd, string name, DbType type, int size)
		{
			return AddParameter(cmd, name, type, size, null);
		}

        /*
		public static DataSet ExecuteCommand(object cmd)
		{
			DataSet ds = new DataSet();

			if (cmd.GetType() == typeof(SqlCommand))
			{
				SqlDataAdapter sda = new SqlDataAdapter((SqlCommand) cmd);
				sda.Fill(ds);
				sda.Dispose();
			}
			else if (cmd.GetType() == typeof(MySqlCommand))
			{
				MySqlDataAdapter mda = new MySqlDataAdapter((MySqlCommand) cmd);
				mda.Fill(ds);
				mda.Dispose();
			}

			return ds;
		}
         * */

		public static object DBNull(object value)
		{
			if (value == null)
				return System.DBNull.Value;
			else
				return value;
		}

        /*
		public static DataSet ExecuteCommand(SqlCommand cmd)
		{
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			da.Fill(ds);

			da.Dispose();
			cmd.Dispose();

			return ds;
		}
         * */
         
        /*
		public static DataSet ExecuteCommand(SqlCommand cmd, int startRecord, int maxRecords, out int found, string srcTable)
		{
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			found = da.Fill(ds, startRecord, maxRecords, srcTable);
			da.Dispose();

			return ds;
		}
         * */

		public static void ExecuteCommandNonquery(SqlCommand cmd)
		{
			cmd.ExecuteNonQuery();
		}
	}
}
#region Revision History
/* Revision History

        $Log: DbUtil.cs,v $
        Revision 1.1  2008/01/08 22:01:34  dobos
        Initial checkin


*/
#endregion
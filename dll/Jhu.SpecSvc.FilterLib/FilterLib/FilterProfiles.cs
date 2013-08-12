using System;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.SpecSvc.FilterLib
{

	public struct FilterFindParameters
	{
		public string Keyword;
		public string Name;
		public string Description;
		public string Ucd;
		public string Version;
		public DateTime DateCreatedFrom;
		public DateTime DateCreatedTo;
		public DateTime DateModifiedFrom;
		public DateTime DateModifiedTo;
		public double WavelengthMinFrom;
		public double WavelengthMinTo;
		public double WavelengthMaxFrom;
		public double WavelengthMaxTo;
		public double WavelengthEffFrom;
		public double WavelengthEffTo;
		public FilterWavelengthScale WavelengthScale;
		public double EffectiveWidthFrom;
		public double EffectiveWidthTo;
	}

	public class FilterProfiles : BaseObject
	{
		public FilterProfiles()
		{
		}

		public FilterProfiles(SqlConnection databaseConnection, SqlTransaction databaseTransaction)
            :base(databaseConnection, databaseTransaction)
		{
		}

		protected FilterCollection DataSet2Collection(DataSet ds, bool returnResponses)
		{
			FilterCollection filters = new FilterCollection();

			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				Filter filter = Filter.CreateFromDatarow(dr);
				filter.DatabaseConnection = DatabaseConnection;
				if (returnResponses) filter.LoadResponses();
				filters.Add(filter);
			}

			return filters;
		}

		public DataSet QueryFiltersAsDataSet()
		{
			string sql = "sp_QueryFilters";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			return ExecuteCommand(cmd);
		}

		public FilterCollection QueryFilters(bool returnResponses)
		{
			return DataSet2Collection(QueryFiltersAsDataSet(), returnResponses);
		}

		public FilterCollection QueryFilters()
		{
			return QueryFilters(false);
		}

		public DataSet FindFilterAsDataSet(string keyword)
		{
			string sql = "sp_FindFilter_Keyword";
			SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 255).Value = keyword;

			return ExecuteCommand(cmd);
		}

		public FilterCollection FindFilter(string keyword, bool returnResponses)
		{
			return DataSet2Collection(FindFilterAsDataSet(keyword), returnResponses);
		}

		public FilterCollection FindFilter(string keyword)
		{
			return FindFilter(keyword, false);
		}

		public DataSet FindFilterAsDataSet(FilterFindParameters par)
		{
			string sql = "sp_FindFilter";
            SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 255).Value = par.Keyword;
			cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = par.Name;
			cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = par.Description;
			cmd.Parameters.Add("@UCD", SqlDbType.NVarChar, 255).Value = par.Ucd;
			cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 15).Value = par.Version;
			cmd.Parameters.Add("@DateCreatedFrom", SqlDbType.DateTime).Value = par.DateCreatedFrom == DateTime.MinValue ? (object) DBNull.Value : (object) par.DateCreatedFrom;
			cmd.Parameters.Add("@DateCreatedTo", SqlDbType.DateTime).Value = par.DateCreatedTo == DateTime.MinValue ? (object) DBNull.Value : (object) par.DateCreatedTo;
			cmd.Parameters.Add("@DateModifiedFrom", SqlDbType.DateTime).Value = par.DateModifiedFrom == DateTime.MinValue ? (object) DBNull.Value : (object) par.DateModifiedFrom;
			cmd.Parameters.Add("@DateModifiedTo", SqlDbType.DateTime).Value = par.DateModifiedTo == DateTime.MinValue ? (object) DBNull.Value : (object) par.DateModifiedTo;
			cmd.Parameters.Add("@WavelengthMinFrom", SqlDbType.Float).Value = par.WavelengthMinFrom == -1 ? (object) DBNull.Value : (object) par.WavelengthMinFrom;
			cmd.Parameters.Add("@WavelengthMinTo", SqlDbType.Float).Value = par.WavelengthMinTo == -1 ? (object) DBNull.Value : (object) par.WavelengthMinTo;
			cmd.Parameters.Add("@WavelengthMaxFrom", SqlDbType.Float).Value = par.WavelengthMaxFrom == -1 ? (object) DBNull.Value : (object) par.WavelengthMaxFrom;
			cmd.Parameters.Add("@WavelengthMaxTo", SqlDbType.Float).Value = par.WavelengthMaxTo == -1 ? (object) DBNull.Value : (object) par.WavelengthMaxTo;
			cmd.Parameters.Add("@WavelengthEffFrom", SqlDbType.Float).Value = par.WavelengthEffFrom == -1 ? (object) DBNull.Value : (object) par.WavelengthEffFrom;
			cmd.Parameters.Add("@WavelengthEffTo", SqlDbType.Float).Value =  par.WavelengthEffTo == -1 ? (object) DBNull.Value : (object) par.WavelengthEffTo;
			cmd.Parameters.Add("@WavelengthScale", SqlDbType.Int).Value = par.WavelengthScale == FilterWavelengthScale.Any ? (object) DBNull.Value : (object) par.WavelengthScale;
			cmd.Parameters.Add("@EffectiveWidthFrom", SqlDbType.Float).Value = par.EffectiveWidthFrom == -1 ? (object) DBNull.Value : (object) par.EffectiveWidthFrom;
			cmd.Parameters.Add("@EffectiveWidthTo", SqlDbType.Float).Value =  par.EffectiveWidthTo == -1 ? (object) DBNull.Value : (object) par.EffectiveWidthTo;

			return ExecuteCommand(cmd);
		}

		public FilterCollection FindFilter(FilterFindParameters par, bool returnResponses)
		{
			return DataSet2Collection(FindFilterAsDataSet(par), returnResponses);
		}

		public FilterCollection FindFilter(FilterFindParameters par)
		{
			return FindFilter(par, false);
		}

		public DataSet FindFilterAsDataSet_UserGuid(string userGuid)
		{
			string sql = "sp_FindFilter_UserGUID";
            SqlCommand cmd = new SqlCommand(sql, DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@UserGUID", SqlDbType.UniqueIdentifier).Value = new Guid(userGuid);

			return ExecuteCommand(cmd);
		}

		public FilterCollection FindFilter_UserGuid(string userGuid, bool returnResponses)
		{
			return DataSet2Collection(FindFilterAsDataSet_UserGuid(userGuid), returnResponses);
		}

		public FilterCollection FindFilter_UserGuid(string userGuid)
		{
			return FindFilter_UserGuid(userGuid, false);
		}

		public FilterCollection FindFilter_BetterResponseAt(double wavelength, double limit, bool relToPeak)
		{
			return null;
		}

		public DataSet FindFilterAsDataSet_SqlQuery(string sql)
		{
            SqlCommand cmd = new SqlCommand(PreparseSql(sql), DatabaseConnection, DatabaseTransaction);
			cmd.CommandType = CommandType.Text;

			return ExecuteCommand(cmd);
		}

		public FilterCollection FindFilter_SqlQuery(string sql, bool returnResponses)
		{
			return DataSet2Collection(FindFilterAsDataSet_SqlQuery(sql), returnResponses);
		}

		public FilterCollection FindFilter_SqlQuery(string sql)
		{
			return FindFilter_SqlQuery(sql, false);
		}

//		public Bitmap PlotFilterGraphs(FilterCollection filters, int width, int height)
//		{
//			int margin = 20;
//			// Determining graph intervals
//			double wavelengthmin = 0, wavelengthmax = 0, valuemin = 0, valuemax = 0;
//
//			foreach (Filter filter in filters)
//			{
//				foreach (Response res in filter.Responses)
//				{
//					if (wavelengthmin == 0) wavelengthmin = res.Wavelength;
//					if (valuemin == 0) valuemin = res.Value;
//
//					wavelengthmax = wavelengthmax < res.Wavelength ? res.Wavelength : wavelengthmax;
//					wavelengthmin = wavelengthmin > res.Wavelength ? res.Wavelength : wavelengthmin;
//
//					valuemax = valuemax < res.Value ? res.Value : valuemax;
//					valuemin = valuemin > res.Value ? res.Value : valuemin;
//				}
//			}
//
//			wavelengthmin = Math.Floor(wavelengthmin / 1000) * 1000;
//			wavelengthmax = Math.Ceiling(wavelengthmax / 1000) * 1000;
//
//			valuemin = 0; //Math.Floor(valuemin / 0.1) * 0.1;
//			valuemax = Math.Ceiling(valuemax / 0.1) * 0.1;
//
//			Bitmap bmp = new Bitmap(width, height);
//			Graphics g = Graphics.FromImage(bmp);
//			Pen pen = new Pen(Color.Black, 1);
//			Font font = new Font(FontFamily.GenericSansSerif, 10);
//			
//			// Clearing the background
//			g.Clear(Color.White);
//			g.DrawRectangle(pen, margin, 0, width - margin - 1, height - margin - 1);
//
//			// Drawing thousand tick on wavelength axis
//			int px;
//			double stepsize = 1000.0;
//			if (((wavelengthmax - wavelengthmin) / 1000.0) > 5)
//				stepsize = Math.Floor((wavelengthmax - wavelengthmin) / 5000.0) * 1000.0;
//			for (double x = wavelengthmin; x <= wavelengthmax; x += stepsize)
//			{
//				px = (int) (margin + (width - margin) / (wavelengthmax - wavelengthmin) * (x - wavelengthmin));
//				if (x == wavelengthmax) px--;
//				g.DrawLine(pen, px, height - margin + 3, px, height - margin - 1);
//				g.DrawString(String.Format("{0} Å", x), font, Brushes.Black, px, height - margin + 4);
//			}
//
//			// Drawing 1/10 ticks on the response axis
//			int py;
//			for (double y = valuemin; y <= valuemax; y += 0.1)
//			{
//				py = height - margin - 1 - (int) ((height - margin) / (valuemax - valuemin) * (y - valuemin));
//				if (y == valuemax) py++;
//				g.DrawLine(pen, margin - 3, py, margin, py);
//				g.DrawString(String.Format("{0:0.0}", y), font, Brushes.Black, 0, py - 16);
//			}
//
//			int i = 0;
//			foreach (Filter filter in filters)
//			{
//				switch (i % 8)
//				{
//					case 0:
//						pen.Color = Color.Blue; break;
//					case 1:
//						pen.Color = Color.Red; break;
//					case 2:
//						pen.Color = Color.Green; break;
//					case 3:
//						pen.Color = Color.Orange; break;
//					case 4:
//						pen.Color = Color.Brown; break;
//					case 5:
//						pen.Color = Color.Purple; break;
//					case 6:
//						pen.Color = Color.Cyan; break;
//					case 7:
//						pen.Color = Color.Gray; break;
//				}
//	
//                filter.PlotGraph(width, height, margin, wavelengthmin, wavelengthmax, valuemin, valuemax, g, pen);				
//
//				Point ptfr = new Point(width - margin, (i + 1) * margin);
//				Point ptto = new Point(width - 2 * margin, (i + 1) * margin);
//
//				g.DrawLine(pen, ptfr, ptto);
//				g.DrawString(filter.Name, font, Brushes.Black, width - 2 * margin - g.MeasureString(filter.Name, font).Width, (i + 1) * margin - margin / 2);
//
//				i++;
//			}
//
//			return bmp;
//		}

		protected string PreparseSql(string sql)
		{
			string res = sql;

			// objects
			res = res.Replace("Filters", "SecureFiltersView");

			// functions
			res = res.Replace("GetFilterResponsePeak_Value", "dbo.fn_GetFilterResponsePeak_Value");
			res = res.Replace("GetFilterResponsePeak_Wavelength", "dbo.fn_GetFilterResponsePeak_Wavelength");
			res = res.Replace("GetFilterResponseValue_Interpolate", "dbo.fn_GetFilterResponseValue_Interpolate");
			res = res.Replace("GetFilterWavelengthEff", "dbo.fn_GetFilterWavelengthEff");
			

			// constants
			res = res.Replace("LINEAR", "0");
			res = res.Replace("LOGARITHMIC", "1");
			res = res.Replace("OTHER", "2");

//			res = res.Replace("STAR_LATE", "6");
//			res = res.Replace("BRIGHTSTARHALO", "102");
//			res = res.Replace("STAR", "1");
//			res = res.Replace("GALAXY", "2");
//			res = res.Replace("HIZ_QSO", "4");
//			res = res.Replace("QSO", "3");
//			res = res.Replace("SKY", "5");
//			res = res.Replace("GAL_EM", "7");
//			res = res.Replace("MERGER", "101");
			

			return res;
		}

		public static string[] Revisions
		{
			get
			{
				string[] revs = {
									typeof(Filter).ToString() + " : " + GetTypeBehaviour(typeof(Filter)) + " : " + Filter.Revision,
									typeof(Response).ToString() + " : " + GetTypeBehaviour(typeof(Response)) + " : " + Response.Revision
								};

				return revs;
			}
		}


	}
}

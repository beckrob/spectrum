using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web
{
    public class PageBase : Jhu.Graywulf.Web.PageBase
    {
        private PortalConnector connector;
        private SqlConnection databaseConnection;
        private SqlTransaction databaseTransaction;

        public SqlConnection DatabaseConnection
        {
            get
            {
                if (databaseConnection == null)
                {
                    databaseConnection = new SqlConnection(AppSettings.ConnectionString);
                    databaseConnection.Open();
                }

                return databaseConnection;
            }
        }

        public SqlTransaction DatabaseTransaction
        {
            get
            {
                if (databaseTransaction == null && databaseConnection != null && databaseConnection.State == ConnectionState.Open)
                {
                    databaseTransaction = databaseConnection.BeginTransaction();
                }

                return databaseTransaction;
            }
        }

        public PortalConnector Connector
        {
            get
            {
                if (connector == null)
                {
                    connector = new PortalConnector(DatabaseConnection, DatabaseTransaction);
                }
                else
                {
                    connector.DatabaseConnection = DatabaseConnection;
                    connector.DatabaseTransaction = DatabaseTransaction;
                }

                return connector;
            }
        }

        public int ResultsetId
        {
            get { return (int)(Session[Constants.SessionResultsetId] ?? -1); }
            set { Session[Constants.SessionResultsetId] = value; }
        }

        public DegreeFormat DegreeFormat
        {
            get { return (DegreeFormat)(Session[Constants.SessionDegreeFormat] ?? DegreeFormat.Decimal); }
            set { Session[Constants.SessionDegreeFormat] = value; }
        }

        public List<PipelineStep> Pipeline
        {
            get { return (List<PipelineStep>)Session[Constants.SessionPipeline]; }
            set { Session[Constants.SessionPipeline] = value; }
        }

        public OutputTarget OutputTarget
        {
            get { return (OutputTarget)Session[Constants.SessionOutputTarget]; }
            set { Session[Constants.SessionOutputTarget] = value; }
        }

        override protected void OnUnload(EventArgs e)
        {
            if (connector != null)
                connector.Dispose();

            if (databaseTransaction != null)
            {
                databaseTransaction.Commit();
                databaseTransaction.Dispose();
            }

            if (databaseConnection != null)
            {
                databaseConnection.Close();
                databaseConnection.Dispose();
            }

            if (connector != null)
            {
                connector.Dispose();
            }

            base.OnUnload(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.DataBind();

            base.OnPreRender(e);
        }
    }
}
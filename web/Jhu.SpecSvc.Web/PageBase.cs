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
        private SqlConnection databaseConnection;
        private SqlTransaction databaseTransaction;

        private PortalConnector portalConnector;
        private PipelineConnector pipelineConnector;

        public SqlConnection DatabaseConnection
        {
            get
            {
                if (databaseConnection == null)
                {
                    databaseConnection = new SqlConnection(Jhu.SpecSvc.IO.AppSettings.PortalConnectionString);
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

        public PortalConnector PortalConnector
        {
            get
            {
                if (portalConnector == null)
                {
                    portalConnector = new PortalConnector(DatabaseConnection, DatabaseTransaction);
                }

                return portalConnector;
            }
        }

        public PipelineConnector PipelineConnector
        {
            get
            {
                if (pipelineConnector == null)
                {
                    pipelineConnector = new PipelineConnector(DatabaseConnection, DatabaseTransaction);
                }

                return pipelineConnector;
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

        public SpectrumPipeline Pipeline
        {
            get { return (SpectrumPipeline)Session[Constants.SessionPipeline]; }
            set { Session[Constants.SessionPipeline] = value; }
        }

        override protected void OnUnload(EventArgs e)
        {
            if (portalConnector != null)
            {
                portalConnector.Dispose();
            }

            if (pipelineConnector != null)
            {
                pipelineConnector.Dispose();
            }

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

            if (portalConnector != null)
            {
                portalConnector.Dispose();
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
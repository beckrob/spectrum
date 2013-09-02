using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.MySpectrum
{
    public class PageBase : System.Web.UI.Page
    {
        private SqlConnection databaseConnection;
        private SqlTransaction databaseTransaction;

        private PortalConnector portalConnector;

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

        public Guid UserGuid
        {
            get { return AppSettings.UserWebServiceGuid; }
        }

        override protected void OnUnload(EventArgs e)
        {
            if (portalConnector != null)
            {
                portalConnector.Dispose();
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
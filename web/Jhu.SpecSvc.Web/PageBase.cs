﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.IO;

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
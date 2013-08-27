using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Pipeline
{
    public class PipelineConnector : IDisposable
    {
        #region Member variables

        private SqlConnection databaseConnection;
        private SqlTransaction databaseTransaction;

        #endregion
        #region Properties

        public SqlConnection DatabaseConnection
        {
            get { return this.databaseConnection; }
            set { this.databaseConnection = value; }
        }

        public SqlTransaction DatabaseTransaction
        {
            get { return this.databaseTransaction; }
            set { this.databaseTransaction = value; }
        }

        #endregion
        #region Constructors and initializers

        public PipelineConnector()
        {
            InitializeMembers();
        }

        public PipelineConnector(SqlConnection cn, SqlTransaction tn)
        {
            this.databaseConnection = cn;
            this.databaseTransaction = tn;
        }

        private void InitializeMembers()
        {
            this.databaseConnection = null;
            this.databaseTransaction = null;
        }

        public void Dispose()
        {
        }

        #endregion
        #region Pipeline functions

        public void SavePipeline(SpectrumPipeline pipeline, Guid userGuid)
        {
            if (pipeline.ID == 0)
            {
                CreatePipeline(pipeline, userGuid);
            }
            else
            {
                ModifyPipeline(pipeline, userGuid);
            }
        }

        private void CreatePipeline(SpectrumPipeline pipeline, Guid userGuid)
        {
            string sql = "spCreatePipeline";

            using (var cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AppendCreateModifyParameters(cmd, pipeline, userGuid);
                
                cmd.ExecuteNonQuery();

                pipeline.ID = (int)cmd.Parameters["RETVAL"].Value;
            }
        }

        private void ModifyPipeline(SpectrumPipeline pipeline, Guid userGuid)
        {
            string sql = "spModifyPipeline";

            using (var cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = pipeline.ID;
                AppendCreateModifyParameters(cmd, pipeline, userGuid);

                cmd.ExecuteNonQuery();
            }
        }

        private void AppendCreateModifyParameters(SqlCommand cmd, SpectrumPipeline pipeline, Guid userGuid)
        {
            cmd.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = userGuid;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = pipeline.Name;
            cmd.Parameters.Add("@Pipeline", SqlDbType.NVarChar).Value = SerializePipeline(pipeline);
            cmd.Parameters.Add("RETVAL", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
        }

        public SpectrumPipeline LoadPipeline(int id, Guid userGuid)
        {
            string sql = "spGetPipeline";

            using (var cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = userGuid;

                using (var dr = cmd.ExecuteReader())
                {
                    dr.Read();

                    return LoadFromDataReader(dr);
                }
            }
        }

        public IEnumerable<SpectrumPipeline> QueryPipelines(Guid userGuid)
        {
            string sql = "spQueryPipelines";

            using (var cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = userGuid;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        yield return LoadFromDataReader(dr);
                    }
                }
            }
        }

        public void DeletePipeline(int id, Guid userGuid)
        {
            string sql = "spDeletePipeline";

            using (var cmd = new SqlCommand(sql, databaseConnection, databaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = userGuid;

                cmd.ExecuteNonQuery();
            }
        }

        private string SerializePipeline(SpectrumPipeline pipeline)
        {
            var xml = new StringWriter();
            var ser = new XmlSerializer(typeof(SpectrumPipeline));

            ser.Serialize(xml, pipeline);

            return xml.ToString();
        }

        private SpectrumPipeline DeserializePipeline(string xml)
        {
            var xmlr = new StringReader(xml);
            var ser = new XmlSerializer(typeof(SpectrumPipeline));

            return (SpectrumPipeline)ser.Deserialize(xmlr);
        }

        private SpectrumPipeline LoadFromDataReader(SqlDataReader dr)
        {
            var pipeline = DeserializePipeline(dr.GetString(3));

            pipeline.ID = dr.GetInt32(0);
            pipeline.Name = dr.GetString(2);

            return pipeline;
        }

        #endregion
    }
}

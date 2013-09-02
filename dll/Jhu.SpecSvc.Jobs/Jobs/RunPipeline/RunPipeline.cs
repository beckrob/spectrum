using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Activities;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Jobs.RunPipeline
{
    public class RunPipeline : GraywulfAsyncCodeActivity, IGraywulfActivity
    {
        [RequiredArgument]
        public InArgument<Guid> JobGuid { get; set; }
        [RequiredArgument]
        public InArgument<Guid> UserGuid { get; set; }

        [RequiredArgument]
        public InArgument<int> ResultsetID { get; set; }

        [RequiredArgument]
        public InArgument<SpectrumPipeline> Pipeline { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext activityContext, AsyncCallback callback, object state)
        {
            var resultsetID = ResultsetID.Get(activityContext);
            var pipeline = Pipeline.Get(activityContext);
            var userGuid = UserGuid.Get(activityContext);

            var workflowInstanceGuid = activityContext.WorkflowInstanceId;
            var activityInstanceId = activityContext.ActivityInstanceId;
            return EnqueueAsync(_ => OnAsyncExecute(workflowInstanceGuid, activityInstanceId, resultsetID, userGuid, pipeline), callback, state);
        }

        private void OnAsyncExecute(Guid workflowInstanceGuid, string activityInstanceId, int resultsetID, Guid userGuid, SpectrumPipeline pipeline)
        {
            RegisterCancelable(workflowInstanceGuid, activityInstanceId, pipeline);

            // Open portal connector
            using (var cn = new SqlConnection(Jhu.SpecSvc.IO.AppSettings.PortalConnectionString))
            {
                cn.Open();
                using (var tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    using (var conn = new PortalConnector(cn, tn))
                    {

                        // Load spectra
                        var spectra = conn.LoadSelectedResults(resultsetID, userGuid, true, true);

                        // Run pipeline
                        pipeline.InitializePipeline();
                        pipeline.ExecuteAll(spectra);
                        pipeline.DeinitializePipeline();

                    }

                    tn.Commit();
                }
            }

            UnregisterCancelable(workflowInstanceGuid, activityInstanceId, pipeline);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Jhu.Graywulf.Tasks;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline
{
    public class SpectrumPipeline : ICancelableTask
    {
        private HashSet<ProgressChangedEventHandler> progressChangedEventHandlers;

        private PortalConnector connector;

        private int id;
        private string name;
        private List<PipelineStep> steps;
        private OutputTarget target;

        private int count;
        private bool skipExceptions;

        private Dictionary<PipelineStep, double> stepProgress;
        private double lastProgress;
        private double progress;
        private bool isInitialized;
        private bool isCanceled;

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { progressChangedEventHandlers.Add(value); }
            remove { progressChangedEventHandlers.Remove(value); }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public PortalConnector Connector
        {
            get { return connector; }
            set { connector = value; }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public int ID
        {
            get { return id; }
            internal set { id = value; }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<PipelineStep> Steps
        {
            get { return steps; }
        }

        public OutputTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public bool SkipExceptions
        {
            get { return skipExceptions; }
            set { skipExceptions = value; }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsCanceled
        {
            get { return isCanceled; }
        }

        public SpectrumPipeline()
        {
            InitializeMembers();
        }

        public SpectrumPipeline(PortalConnector connector)
        {
            InitializeMembers();
            this.connector = connector;
        }

        private void InitializeMembers()
        {
            this.progressChangedEventHandlers = new HashSet<ProgressChangedEventHandler>();

            this.connector = null;

            this.id = 0;
            this.name = "new pipeline";
            this.steps = new List<PipelineStep>();
            this.target = null;

            this.count = 0;
            this.skipExceptions = true;
            this.isInitialized = false;
            this.isCanceled = false;
        }

        /// <summary>
        /// Initializes the pipeline and its steps.
        /// </summary>
        public void InitializePipeline()
        {
            // To calculate the progress, we need to know the estimated number of
            // spectra after each step. TotalCount is originally set to the number
            // of spectra emitted by the connector. Some steps can reduce the number
            // of spectra significantly (i.e. PCA)
            int count = this.count;

            // Keep track of progress
            stepProgress = new Dictionary<PipelineStep, double>();
            progress = 0;
            lastProgress = -1;

            // Initialize each step
            foreach (var step in steps)
            {
                step.Connector = connector;
                step.Count = count;
                step.ResetProgress();
                step.InitializeStep();
                step.SkipExceptions = skipExceptions;
                stepProgress.Add(step, 0.0);
                step.ProgressChanged += new ProgressChangedEventHandler(step_ProgressChanged);

                // Get the output spectrum count
                count = step.GetOutputCount();
            }

            if (target != null)
            {
                target.InitializeTarget();
            }

            isInitialized = true;
        }

        public void DeinitializePipeline()
        {
            // Deinitialize each step
            foreach (var step in steps)
            {
                step.ProgressChanged -= new ProgressChangedEventHandler(step_ProgressChanged);
            }

            if (target != null)
            {
                //target.DeinitializeTarget();
            }
        }

        public void ExecuteAll(IEnumerable<Spectrum> spectra)
        {
            foreach (var s in Execute(spectra))
            {
                // TODO: this might not work correctly, test!
                if (isCanceled)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Executes spectrum processing over a stream of spectra
        /// </summary>
        /// <param name="spectra"></param>
        /// <returns></returns>
        public IEnumerable<Spectrum> Execute(IEnumerable<Spectrum> spectra)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Pipeline must be initialized first.");
            }

            isInitialized = false;

            // TODO: add graywulf cancellation logic

            // Chain up processing steps using PLINQ
            var ss = spectra.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered);

            foreach (var step in steps)
            {
                ss = step.Execute(ss);
            }

            if (target != null)
            {
                ss = target.Execute(ss.AsSequential()).AsParallel();
            }

            return ss.AsSequential();
        }

        public int GetOutputCount()
        {
            if (steps != null && steps.Count > 0)
            {
                return steps[steps.Count - 1].GetOutputCount();
            }
            else
            {
                return this.count;
            }
        }

        /// <summary>
        /// Callback to step progress event. Updates progress information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void step_ProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            this.stepProgress[(PipelineStep)sender] = args.Progress;

            double newprogress = GetProgress().Progress;

            if (newprogress - lastProgress > 0.05)
            {
                lastProgress = newprogress;
                ReportProgress();
            }

            progress = newprogress;

            // TODO: implement graywulf job progress logic here
        }

        public ProgressChangedEventArgs GetProgress()
        {
            if (steps != null && steps.Count > 0)
            {
                double newprogress = 0;

                foreach (PipelineStep step in steps)
                {
                    newprogress += this.stepProgress[step];
                }
                newprogress /= steps.Count;

                return new ProgressChangedEventArgs(newprogress);
            }
            else
            {
                return new ProgressChangedEventArgs(1.0);
            }
        }

        private void ReportProgress()
        {
            if (progressChangedEventHandlers != null)
            {
                var a = GetProgress();
                foreach (var eh in progressChangedEventHandlers)
                {
                    eh(this, a);
                }
            }
        }

        public void Cancel()
        {
            if (isCanceled)
            {
                throw new InvalidOperationException();
            }

            isCanceled = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline
{
    public class SpectrumProcessHelper
    {
        protected List<ProgressChangedEventHandler> progressChangedEventHandlers = new List<ProgressChangedEventHandler>();

        public event ProgressChangedEventHandler ProgressChanged
        {
            add
            {
                progressChangedEventHandlers.Add(value);
            }
            remove
            {
                progressChangedEventHandlers.Remove(value);
            }
        }

        private PortalConnector connector;

        private int count;

        private bool skipExceptions;

        private List<PipelineStep> steps;

        private Dictionary<PipelineStep, double> stepProgress;
        private double lastProgress;
        private double progress;

        public PortalConnector Connector
        {
            get { return connector; }
            set { connector = value; }
        }

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

        public List<PipelineStep> Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public SpectrumProcessHelper()
        {
            InitializeMembers();
        }

        public SpectrumProcessHelper(PortalConnector connector)
        {
            InitializeMembers();
            this.connector = connector;
        }

        private void InitializeMembers()
        {
            this.connector = null;

            this.count = 0;

            this.skipExceptions = true;

            this.steps = new List<PipelineStep>();

            //this.stepProgress = null;
            //this.lastProgress = 0;
            //this.progress = 0;
            //this.cancel = false;
        }

        public void InitializePipeline()
        {          
            int count = this.count;

            stepProgress = new Dictionary<PipelineStep, double>();
            progress = 0;
            lastProgress = -1;

            foreach (PipelineStep step in steps)
            {
                step.Connector = connector;
                step.InitializeStep(count);
                if (skipExceptions)
                {
                    step.SkipExceptions = true;
                }
                stepProgress.Add(step, 0.0);
                step.ProgressChanged += new ProgressChangedEventHandler(step_ProgressChanged);
                count = step.GetOutputCount();
            }
        }

        public IEnumerable<Spectrum> Execute(IEnumerable<Spectrum> spectra)
        {
            // Chain up processing steps using PLINQ
            ParallelQuery<Spectrum> ss = spectra.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered);

            foreach (PipelineStep step in steps)
            {
                ss = step.Execute(ss);
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
                return this.count;
        }

        void step_ProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            this.stepProgress[(PipelineStep)sender] = args.Progress;

            double newprogress = GetProgress();

            if (newprogress - lastProgress > 0.05)
            {
                lastProgress = newprogress;

                lock (this)
                {
                    if (progressChangedEventHandlers != null)
                    {
                        ProgressChangedEventArgs a = new ProgressChangedEventArgs(newprogress);
                        foreach (ProgressChangedEventHandler eh in progressChangedEventHandlers)
                        {
                            eh(this, a);
                            //this.cancel = a.Cancel;
                        }
                    }
                }
            }

            progress = newprogress;
        }

        public double GetProgress()
        {
            if (steps != null && steps.Count > 0)
            {
                double newprogress = 0;

                foreach (PipelineStep step in steps)
                {
                    newprogress += this.stepProgress[step];
                }
                newprogress /= steps.Count;

                return newprogress;
            }
            else
            {
                return 1.0;
            }
        }
    }
}

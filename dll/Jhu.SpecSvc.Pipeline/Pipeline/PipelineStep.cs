using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Pipeline
{
    [XmlInclude(typeof(Steps.ArithmeticStep))]
    [XmlInclude(typeof(Steps.BinByStep))]
    [XmlInclude(typeof(Steps.CompositeStep))]
    [XmlInclude(typeof(Steps.ContinuumFitStep))]
    [XmlInclude(typeof(Steps.ConvolutionStep))]
    [XmlInclude(typeof(Steps.CustomStep))]
    [XmlInclude(typeof(Steps.DereddenStep))]
    [XmlInclude(typeof(Steps.FluxStep))]
    [XmlInclude(typeof(Steps.LineFitStep))]
    [XmlInclude(typeof(Steps.NormalizeStep))]
    [XmlInclude(typeof(Steps.PcaStep))]
    [XmlInclude(typeof(Steps.RebinStep))]
    [XmlInclude(typeof(Steps.RedshiftStep))]
    [XmlInclude(typeof(Steps.SpectralIndexStep))]
    [XmlInclude(typeof(Steps.WavelengthConversionStep))]
    public abstract class PipelineStep
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

        protected bool active;
        protected PortalConnector connector;
        protected bool skipExceptions;
        protected List<Exception> exceptions;
        protected int iteration;

        protected int count;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        [XmlIgnore]
        public PortalConnector Connector
        {
            get { return connector; }
            set { connector = value; }
        }

        public bool SkipExceptions
        {
            get { return skipExceptions; }
            set { skipExceptions = value; }
        }

        [XmlIgnore]
        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        [XmlIgnore]
        public int Iteration
        {
            get { return iteration; }
        }

        public abstract string Title
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public PipelineStep()
        {
            InitializeMembers();
        }

        public PipelineStep(PipelineStep old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.active = true;
            this.connector = null;
            this.skipExceptions = false;
            this.exceptions = new List<Exception>();
        }

        private void CopyMembers(PipelineStep old)
        {
            this.active = old.active;
            this.connector = old.connector;
            this.skipExceptions = false;
            this.exceptions = new List<Exception>(old.exceptions);
        }

        /// <summary>
        /// Called by the processing framework before running the pipeline
        /// </summary>
        /// <param name="count"></param>
        public virtual void InitializeStep(int count)
        {
            this.exceptions = new List<Exception>();
            this.count = count;
        }

        public virtual ParallelQuery<Spectrum> Execute(ParallelQuery<Spectrum> spectra /*, bool skipExceptions*/ )
        {
            exceptions.Clear();

            // Hook up worker
            return spectra.Select(s =>
                {
                    // Report progress
                    lock (this)
                    {
                        iteration++;
                        if (progressChangedEventHandlers != null)
                        {
                            ProgressChangedEventArgs args = new ProgressChangedEventArgs(GetProgress());
                            foreach (ProgressChangedEventHandler eh in progressChangedEventHandlers)
                            {
                                eh(this, args);
                            }
                        }
                    }

                    try
                    {
                        return Execute(s);
                    }
                    catch (System.Exception ex)
                    {
                        if (skipExceptions)
                        {
                            exceptions.Add(ex);
                            return null;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }).Where(s => s != null);
        }

        protected virtual Spectrum Execute(Spectrum spectrum)
        {
            throw new NotImplementedException();
        }

        public virtual int GetOutputCount()
        {
            return count;
        }

        protected virtual double GetProgress()
        {
            double progress = 0;

            lock (this)
            {
                progress = (double)iteration / (double)count;
            }

            return progress;
        }
    }
}

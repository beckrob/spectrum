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
    [XmlInclude(typeof(CompositeStep))]
    [XmlInclude(typeof(ContinuumFitStep))]
    [XmlInclude(typeof(ConvolutionStep))]
    [XmlInclude(typeof(DereddenStep))]
    [XmlInclude(typeof(LineFitStep))]
    [XmlInclude(typeof(FluxStep))]
    [XmlInclude(typeof(NormalizeStep))]
    [XmlInclude(typeof(PcaStep))]
    [XmlInclude(typeof(RebinStep))]
    [XmlInclude(typeof(RedshiftStep))]
    [XmlInclude(typeof(WavelengthConversionStep))]
    public abstract class ProcessStep
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

        public ProcessStep()
        {
            InitializeMembers();
        }

        public ProcessStep(ProcessStep old)
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

        private void CopyMembers(ProcessStep old)
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

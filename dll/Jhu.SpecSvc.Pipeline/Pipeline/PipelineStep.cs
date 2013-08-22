using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

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
    public abstract class PipelineStep : PipelineObjectBase<Spectrum>
    {
        protected bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
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
        }

        private void CopyMembers(PipelineStep old)
        {
            this.active = old.active;
        }

        public virtual void InitializeStep()
        {
        }

        /// <summary>
        /// Returns the (estimated) number output items
        /// </summary>
        /// <returns></returns>
        public virtual int GetOutputCount()
        {
            return Count;
        }

        public virtual ParallelQuery<Spectrum> Execute(ParallelQuery<Spectrum> items)
        {
            // Hook up worker
            return items.Select(item => ExecuteWorker(item)).Where(i => i != null);
        }

        private Spectrum ExecuteWorker(Spectrum item)
        {
            // Report progress
            lock (this)
            {
                Iteration++;
                ReportProgress();
            }

            try
            {
                return OnExecute(item);
            }
            catch (System.Exception ex)
            {
                if (SkipExceptions)
                {
                    LogException(ex);
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
        }

        protected abstract Spectrum OnExecute(Spectrum item);
    }
}

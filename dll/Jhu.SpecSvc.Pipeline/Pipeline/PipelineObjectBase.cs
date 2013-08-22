using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Pipeline
{
    public abstract class PipelineObjectBase<T>
        where T : new()
    {
        private HashSet<ProgressChangedEventHandler> progressChangedEventHandlers;

        private PortalConnector connector;
        private bool skipExceptions;
        private List<Exception> exceptions;
        protected TextWriter log;

        private int count;
        private int iteration;

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { progressChangedEventHandlers.Add(value); }
            remove { progressChangedEventHandlers.Remove(value); }
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
        public TextWriter Log
        {
            get { return log; }
            set { log = value; }
        }

        [XmlIgnore]
        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        [XmlIgnore]
        public int Iteration
        {
            get { return iteration; }
            protected set { iteration = value; }
        }

        public PipelineObjectBase()
        {
            InitializeMembers();
        }

        public PipelineObjectBase(PipelineObjectBase<T> old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.progressChangedEventHandlers = new HashSet<ProgressChangedEventHandler>();

            this.connector = null;
            this.skipExceptions = false;
            this.exceptions = new List<Exception>();

            this.count = 0;
            this.iteration = 0;
        }

        private void CopyMembers(PipelineObjectBase<T> old)
        {
            this.connector = old.connector;
            this.skipExceptions = false;
            this.exceptions = new List<Exception>(old.exceptions);

            this.count = old.count;
            this.iteration = old.iteration;
        }

        public void ResetProgress()
        {
            exceptions.Clear();
            count = 0;
            iteration = 0;
        }

        protected virtual ProgressChangedEventArgs GetProgress()
        {
            lock (this)
            {
                return new ProgressChangedEventArgs((double)iteration / (double)count);
            }
        }

        protected void ReportProgress()
        {
            if (progressChangedEventHandlers != null)
            {
                var args = GetProgress();
                foreach (var eh in progressChangedEventHandlers)
                {
                    eh(this, args);
                }
            }
        }

        protected void LogException(Exception ex)
        {
            exceptions.Add(ex);

            if (log != null)
            {
                log.WriteLine("{0:o} Exception: {1}", DateTime.Now, ex.GetType().Name);
                log.WriteLine("{0:o} Message: {1}", DateTime.Now, ex.Message);
            }
        }
    }
}

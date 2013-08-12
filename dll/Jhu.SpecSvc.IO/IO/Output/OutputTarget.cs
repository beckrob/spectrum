using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
    [Serializable]
    [XmlInclude(typeof(FileTarget))]
    [XmlInclude(typeof(MySpectrumTarget))]
    public abstract class OutputTarget
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

        protected TextWriter log;
        protected bool skipExceptions;
        protected int iteration;

        protected int count;

        [XmlIgnore]
        public TextWriter Log
        {
            get { return log; }
            set { log = value; }
        }

        [XmlIgnore]
        public bool SkipExceptions
        {
            get { return this.skipExceptions; }
            set { this.skipExceptions = value; }
        }

        [XmlIgnore]
        public int Iteration
        {
            get { return iteration; }
        }

        public OutputTarget()
        {
            InitializeMembers();
        }

        public OutputTarget(OutputTarget old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.skipExceptions = true;

            this.iteration = 0;
            this.count = 0;
        }

        private void CopyMembers(OutputTarget old)
        {
            this.skipExceptions = old.skipExceptions;
        }

        public virtual void Init(int count)
        {
            this.count = count;
        }

        public virtual double GetProgress()
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

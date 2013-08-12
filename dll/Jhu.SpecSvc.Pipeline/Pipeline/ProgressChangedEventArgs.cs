using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Pipeline
{
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs args);

    public class ProgressChangedEventArgs : EventArgs
    {
        private double progress;
        private bool cancel;

        public double Progress
        {
            get { return progress; }
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

        internal ProgressChangedEventArgs(double progress)
        {
            this.progress = progress;
            this.cancel = false;
        }
    }
}

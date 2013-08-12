using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VoServices.SpecSvc.Lib;
using VoServices.SpecSvc.IO;

namespace VoServices.SpecSvc.IO
{
    public class SpectrumOutputHelper
    {
        private int count;

        private bool skipExceptions;
        private List<Exception> exceptions;

        private OutputTarget target;

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

        public List<Exception> Exceptions
        {
            get { return exceptions; }
        }

        public OutputTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        public SpectrumOutputHelper()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.skipExceptions = true;
            this.exceptions = new List<Exception>();
        }

        public void Execute(IEnumerable<Spectrum> spectra)
        {
            this.exceptions.Clear();

            target.Init();
            target.Execute(spectra);
            target.Finalize();



            /*
            if (!skipExceptions)
            {
                foreach (ProcessStep step in steps)
                {
                    this.exceptions.AddRange(step.Exceptions);
                }
            }*/
        }
    }
}

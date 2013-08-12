using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public class Magnitudes
    {
        public double[] Redshift;
        public int[] FilterId;
        public string[] FilterName;
        public double[][] Flux;
        public bool[][] Error;
    }
}

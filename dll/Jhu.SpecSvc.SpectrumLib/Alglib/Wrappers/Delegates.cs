using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public delegate double ScalarFunction(double[] x);
    public delegate double[] VectorFunction(double[] x);
}

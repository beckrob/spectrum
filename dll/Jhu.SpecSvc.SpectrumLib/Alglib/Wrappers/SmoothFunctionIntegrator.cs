using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public class SmoothFunctionIntegrator : FunctionIntegrator
    {
        public SmoothFunctionIntegrator(Func<double, double> f, double a, double b)
            : base(f, a, b)
        {
        }

        public override double Integrate()
        {
            double v;
            alglib.autogkstate s;
            alglib.autogkreport rep;

            alglib.autogksmooth(a, b, out s);
            alglib.autogkintegrate(s, WrappedFunctionCallback, null);
            alglib.autogkresults(s, out v, out rep);

            return v;
        }
    }
}

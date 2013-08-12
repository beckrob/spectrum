using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public abstract class FunctionIntegrator
    {
        protected Func<double, double> f;
        protected double a;
        protected double b;

        protected FunctionIntegrator(Func<double, double> f, double a, double b)
        {
            this.f = f;
            this.a = a;
            this.b = b;
        }

        protected void WrappedFunctionCallback(double x, double xminusa, double bminusx, ref double y, object obj)
        {
            y = f(x);
        }

        public abstract double Integrate();
    }
}

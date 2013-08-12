using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public class BLEICMinimizer : FunctionMinimizer
    {

        alglib.minbleicstate state;

        public BLEICMinimizer()
        {
        }

        public override StopCriterium OptimizeScalar(ScalarFunctionOptimizationProblem p)
        {
            int n = p.Parameters.Length;              // number of parameters

            // Initialize
            alglib.minbleiccreatef(n, p.Parameters, p.Diffstep, out state);

            // Set boundary conditions
            if (p.MinLimits != null && p.MaxLimits != null)
            {
                alglib.minbleicsetbc(state, p.MinLimits, p.MaxLimits);
            }

            // Set stopping conditions
            alglib.minbleicsetinnercond(state,
                p.Epsg,          // condition on gradient vector
                p.Epsf,          // condition on F
                p.Epsx           // condition on step vector
                );

            alglib.minbleicsetoutercond(state,
                p.Epsx,
                p.Epsi);

            alglib.minbleicsetmaxits(state, p.MaxIterations);

            // Turn off reporting
            alglib.minbleicsetxrep(state, false);

            // Set maximum step length
            if (!double.IsNaN(p.StepMax))
                alglib.minbleicsetstpmax(state, p.StepMax);

            // Set per parameter scaling
            //double[] s = new double[n];
            alglib.minbleicsetscale(state, p.Scale);

            // Execute
            alglib.minbleicoptimize(
                state,
                delegate(double[] arg, ref double func, object obj)
                {
                    func = p.Function(arg);
                },
                null,
                null);

            // Extract results
            alglib.minbleicreport rep;
            double[] op;
            alglib.minbleicresults(state, out op, out rep);

            // Optimized parameters
            p.Parameters = op;

            return (StopCriterium)rep.terminationtype;
        }

        protected override double[,] GetQuadratic()
        {
            int n = state.innerobj.g.Length;
            double[,] q = new double[n,n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    q[i, j] = state.innerobj.g[i] * state.innerobj.g[j];
                }
            }

            return q;
        }

    }
}

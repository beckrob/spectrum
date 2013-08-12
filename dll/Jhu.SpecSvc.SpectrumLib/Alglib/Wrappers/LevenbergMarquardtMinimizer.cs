using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public class LevenbergMarquardtMinimizer : FunctionMinimizer
    {
        private alglib.minlmstate state;

        public LevenbergMarquardtMinimizer()
        {
        }

        public override StopCriterium OptimizeScalar(ScalarFunctionOptimizationProblem p)
        {
            int n = p.Parameters.Length;              // number of parameters
            //int m = function(parameters).Length;    // number of functions
            int m = 1;

            // Initialize
            alglib.minlmcreatev(n, m, p.Parameters, p.Diffstep, out state);

            // Set boundary conditions
            if (p.MinLimits != null && p.MaxLimits != null)
            {
                alglib.minlmsetbc(state, p.MinLimits, p.MaxLimits);
            }

            // Set stopping conditions
            alglib.minlmsetcond(state,
                p.Epsg,          // condition on gradient vector
                p.Epsf,          // condition on F
                p.Epsx,          // condition on step vector
                p.MaxIterations         // max iterations
                );

            // Turn off per iteration reporting
            alglib.minlmsetxrep(state, false);

            // Set maximum step length
            if (!double.IsNaN(p.StepMax))
            {
                alglib.minlmsetstpmax(state, p.StepMax);
            }

            // Set per parameter scaling
            if (p.Scale != null)
            {
                alglib.minlmsetscale(state, p.Scale);
            }

            // Execute
            alglib.minlmoptimize(
                state,
                delegate(double[] arg, double[] fi, object obj)
                {
                    // Minimizes f^2, thus the Sqrt!
                    //fi[0] = Math.Sqrt(p.Function(arg));
                    fi[0] = p.Function(arg);
                },
                null,               // reporting callback
                null            // state to send to the function
                );

            // Extract results
            alglib.minlmreport rep;
            double[] op;
            alglib.minlmresults(state, out op, out rep);

            p.Parameters = op;

            return (StopCriterium)rep.terminationtype;
        }

        protected override double[,] GetQuadratic()
        {
            return state.innerobj.quadraticmodel;
        }
    }
}

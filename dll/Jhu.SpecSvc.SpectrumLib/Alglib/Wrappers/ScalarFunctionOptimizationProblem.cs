using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{

    public class ScalarFunctionOptimizationProblem : FunctionOptimizationProblem<ScalarFunction>
    {
        public ScalarFunctionOptimizationProblem()
            : base()
        {
        }

        public override StopCriterium Optimize(FunctionMinimizer min)
        {
            return min.OptimizeScalar(this);
        }

        public override double[,] GetQuadratic()
        {
            int n = parameters.Length;

            double[] x = new double[n];
            double[] df = new double[n];
            double fp;

            //
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = parameters[i];
            }

            //
            for (int i = 0; i < x.Length; i++)
            {
                double oldx = x[i];

                x[i] = oldx + scale[i] * diffstep;
                double fh = function(x);
                
                x[i] = oldx - scale[i] * diffstep;
                double fl = function(x);

                df[i] = (fh - fl) / (2 * scale[i] * diffstep);

                x[i] = oldx;
            }

            double[,] q = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    q[i, j] = df[i] * df[j];
                }
            }

            return q;
        }

        public override double[] GetErrors()
        {
            double fp = function(parameters);

            int n = parameters.Length;

            double[,] q = GetQuadratic();

            double[] w = new double[n];
            double[,] u = new double[n, n];
            double[,] vt = new double[n, n];
            alglib.svd.rmatrixsvd(q, n, n, 1, 1, 2, ref w, ref u, ref vt);

            double[,] s = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                if (w[i] != 0)
                    s[i, i] = 1 / w[i];   // inverz
                else
                    s[i, i] = 0;
            }

            // inverz:
            double[,] C = new double[n, n];
            alglib.ablas.rmatrixgemm(n, n, n, 1, vt, 0, 0, 1, s, 0, 0, 0, 0, ref C, 0, 0);
            double[,] D = new double[n, n];
            alglib.ablas.rmatrixgemm(n, n, n, 1, C, 0, 0, 0, u, 0, 0, 1, 0, ref D, 0, 0);

            double[] err = new double[n];
            for (int i = 0; i < n; i++)
            {
                err[i] = Math.Sqrt(D[i, i]);
            }

            return err;
        }
    }
}

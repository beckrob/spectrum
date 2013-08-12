using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public abstract class FunctionMinimizer
    {
        public abstract StopCriterium OptimizeScalar(ScalarFunctionOptimizationProblem p);

        protected abstract double[,] GetQuadratic();

        protected double[] GetError()
        {
            int n = 2;

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
                //s[i, i] = w[i];         // eredeti
                else
                    s[i, i] = 0;
            }

            // inverz:
            alglib.ablas.rmatrixgemm(n, n, n, 1, vt, 0, 0, 1, s, 0, 0, 0, 0, ref q, 0, 0);
            double[,] D = new double[n, n];
            alglib.ablas.rmatrixgemm(n, n, n, 1, q, 0, 0, 0, u, 0, 0, 1, 0, ref D, 0, 0);

            // eredeti:
            //alglib.ablas.rmatrixgemm(n, n, n, 1, u, 0, 0, 0, s, 0, 0, 0, 0, ref C, 0, 0);
            //double[,] D = new double[n, n];
            //alglib.ablas.rmatrixgemm(n, n, n, 1, C, 0, 0, 0, vt, 0, 0, 0, 0, ref D, 0, 0);

            double[] err = new double[n];
            for (int i = 0; i < n; i++)
            {
                err[i] = Math.Sqrt(D[i, i]);
            }

            return err;
        }
    }
}

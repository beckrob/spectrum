using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;

namespace Jhu.SpecSvc.Util
{
    public static class Fit
    {

        private static void BuildFitMatrices(double[] y, bool[] mask, double[] weight, double[][] fx, out DenseMatrix M, out DenseVector F)
        {
            M = new DenseMatrix(fx.Length);
            F = new DenseVector(fx.Length);

            // Prepare weight vector
            double[] ww = new double[y.Length];
            for (int x = 0; x < y.Length; x++)
            {
                ww[x] = (mask != null && mask[x]) ? 0.0 : (weight == null) ? 1.0 : weight[x];
            }


            for (int j = 0; j < fx.Length; j++)
            {
                double[] fxj = fx[j];

                for (int i = 0; i < fx.Length; i++)
                {
                    double[] fxi = fx[i];
                    double mij = 0;

                    for (int x = 0; x < y.Length; x++)
                    {

                        //M[i, j] += (w * fx[i][x] * fx[j][x]);
                        mij += (ww[x] * fxi[x] * fxj[x]);

                        /* Debug code, remove, because slow!
                        if (double.IsNaN(M[i, j]))
                            throw new Exception();
                         * */
                    }

                    M[i, j] = mij;
                }
            }


            for (int i = 0; i < fx.Length; i++)
            {
                double fi = 0;

                for (int x = 0; x < y.Length; x++)
                {
                    fi += (ww[x] * y[x] * fx[i][x]);

                    /* Debug code, remove, because slow!
                    if (double.IsNaN(f[i]))
                    {
                        throw new Exception();
                    }
                     * */
                }

                F[i] = fi;
            }
        }

        public static void FitLeastSquares(double[] y, bool[] mask, double[] weight, double[][] fx, out double[] a, out double[][] C, out double rcoeff)
        {
            DenseVector F;
            DenseMatrix M;
            BuildFitMatrices(y, mask, weight, fx, out M, out F);

            DenseVector am;
            DenseMatrix Cm;

            FitSvd(M, F, out am, out Cm);

            // Copy results
            a = am.ToArray();

            C = new double[Cm.RowCount][];
            for (int i = 0; i < Cm.RowCount; i++)
            {
                C[i] = Cm.Row(i).ToArray();
            }

            rcoeff = RegressionCoeff(M, F, am);
        }

        public static void FitNonNegativeLeastSquares(double[] y, bool[] mask, double[] weight, double[][] fx, out double[] a, out double[][] C, out double rcoeff)
        {
            DenseVector F;
            DenseMatrix M;

            BuildFitMatrices(y, mask, weight, fx, out M, out F);

            DenseVector am;

            FitNnls(M, F, out am);

            // Copy results
            a = am.ToArray();
            C = null;
            rcoeff = RegressionCoeff(M, F, am);
        }

        private static void FitSvd(DenseMatrix A, DenseVector b, out DenseVector a, out DenseMatrix C)
        {
            DenseSvd svd = new DenseSvd(A, true);

            a = (DenseVector)svd.Solve(b);

            // ********** TEST:
            // Old code from Lapack wrapper: C[k, l] += Vt[k, i] * Vt[l, i] * (wi * wi);	// cov matrix

            //C = svd.VT().TransposeAndMultiply(svd.VT());

            //throw new NotImplementedException();

            C = (DenseMatrix)svd.VT().TransposeAndMultiply(svd.VT());
        }

        /// <summary>
        /// Non-negative least-square algorithm
        /// </summary>
        /// <param name="E">Input matrix with the vectors to fit in the columns</param>
        /// <param name="f">Function to fit to</param>
        /// <param name="x">Solution</param>
        private static void FitNnls(DenseMatrix E, DenseVector f, out DenseVector x)
        {
            // Step 1.

            x = new DenseVector(E.ColumnCount);		// solution vector
            bool[] Z = new bool[E.ColumnCount];		// Z contains indexes held at zero, if true, element is in Z
            for (int i = 0; i < x.Count; i++)
            {
                x[i] = 0;					//  all elements to 0
                Z[i] = true;				// all elements are in Z, P is Z's complementary set!
            }

            // Step 2. -- main loop
            int l1count = 0;
            while (l1count < 1000)
            {
                l1count++;
                // negative gradient vector
                DenseVector w = (DenseVector)E.TransposeThisAndMultiply(f - E * x);

                /* optimized loop implementation of the code above
                double[] ED = E.Data;
                double[] EjkXk = new double[E.Rows];
                double[] xd = x.Data;
                for (int j = 0; j < E.Rows; j++)
                    for (int k = 0; k < E.Columns; k++)
                        EjkXk[j] += ED[j + k * E.Rows] * xd[k];


                Matrix w = new Matrix(E.Columns, 1);
                double[] wd = w.Data;
                double[] fd = f.Data;
                for (int i = 0; i < w.Rows; i++)
                {
                    int ri = i * E.Rows;
                    for (int j = 0; j < E.Rows; j++)
                    {
                        wd[i] += ED[j + ri] * (fd[j] - EjkXk[j]);
                    }
                }*/

                // Step 3.

                // if Z is empty or all elements of w with indices in Z are <= 0 we have a solution
                bool empty = true;
                bool allwneg = true;
                double wmax = 0.0;
                int wmaxidx = -1;
                for (int i = 0; i < w.Count; i++)
                {
                    if (Z[i]) empty = false;
                    if ((w[i] > 0) && Z[i]) allwneg = false;
                    if (w[i] > wmax)	// finds max elem of w to be used @ step 4.
                    {
                        wmax = w[i];
                        wmaxidx = i;
                    }
                }

                if (empty || allwneg)
                    return;			// x is always the solution

                // Step 4.

                // wmax stores maximum element in w, it is positive, move its index from Z to P
                Z[wmaxidx] = false;		// Z -> P since they're complementary sets

                // Step 5. -- possible inner loop
                int l2count = 0;
                while (l2count < 100)
                {
                    l2count++;

#if FITDEBUG
					Console.WriteLine(l1count.ToString() + " " + l2count.ToString());
					Console.WriteLine(x.Transpose().ToString());
#endif
                    // #of indexes not in Z
                    int notinz = 0;
                    for (int j = 0; j < Z.Length; j++) if (!Z[j]) notinz++;

                    DenseMatrix Epos = new DenseMatrix(E.RowCount, notinz);
                    int newcol = 0;
                    for (int j = 0; j < E.ColumnCount; j++)
                        if (!Z[j])
                        {
                            for (int i = 0; i < E.RowCount; i++)
                            {
                                //Epos.Data[i + newcol * Epos.RowCount] = E.Data[i + j * E.Rows];
                                Epos[i, newcol] = E[i, j];
                                // **** check and remove if ok
                            }

                            newcol++;
                        }

                    // solving unconstraint least square Epos * z = f;
                    DenseVector zpos;	// solution of uncorrelated problem
                    DenseMatrix C;	// correlation matrix (not used)

                    FitSvd(Epos, f, out zpos, out C);

                    // set all elements of z to 0, whose index is in Z
                    // check whether all other elements of z are >= 0,
                    // if it is true, z is a good solution, set x = z and countinue to main loop
                    DenseVector z = new DenseVector(x.Count);
                    newcol = 0;
                    bool allpos = true;
                    for (int j = 0; j < Z.Length; j++)
                        if (Z[j])
                            z[j] = 0.0;
                        else
                        {
                            z[j] = zpos[newcol];
                            if (z[j] <= 0.0) allpos = false;
                            newcol++;
                        }

#if FITDEBUG
					Console.WriteLine(allpos.ToString());
#endif
                    // Step 6.
                    if (allpos)
                    {
                        x = new DenseVector(z);
                        break;
                    }

                    // Step 7.
                    // find the minimum of alpha, and it's index;
                    int q = -1;		//
                    double alpha = double.PositiveInfinity;
                    for (int i = 0; i < z.Count; i++)
                    {
                        if (z[i] >= 0) continue;
                        double a = x[i] / (x[i] - z[i]);
                        if (a < alpha)
                        {
                            q = i;
                            alpha = a;
                        }
                    }

                    // Step 8.
                    x = x + (alpha * (z - x));

                    // Step 9.
                    // move from P to Z (ie set Z[i]=true) where x[i]=0.0
                    for (int i = 0; i < x.Count; i++)
                        Z[i] |= (x[i] == 0.0);

                } // end inner loop

            }	// end outer loop


        }

        private static double RegressionCoeff(DenseMatrix A, DenseVector f, DenseVector y)
        {
            DenseVector x = Linearize(A, f, y);
            double sumx = 0;
            double sumy = 0;
            double sumxx = 0;
            double sumyy = 0;
            double sumxy = 0;
            for (int j = 0; j < x.Count; j++)
            {
                sumx += j;
                sumy += x[j];
                sumxx += j * j;
                sumyy += x[j] * x[j];
                sumxy += j * x[j];
            }

            int num = x.Count;
            double coeff = 0;
            coeff = (num * sumxy - sumx * sumy) * (num * sumxy - sumx * sumy);
            coeff = coeff / (num * sumxx - sumx * sumx);
            coeff = coeff / (num * sumyy - sumy * sumy);
            return coeff;
        }

        private static DenseVector Linearize(DenseMatrix A, DenseVector f, DenseVector x)
        {
            DenseVector Linx = new DenseVector(x);
            DenseVector Z = A * x;

            for (int j = 0; j < x.Count; j++)
            {
                Linx[j] = (Z[j] / f[j]) * j;
            }

            return Linx;
        }

        public delegate double[] BrentFunction(double p, out double[] error, out double chi2);

        // Brent's method for parabolic interpolation from numrec
        public static double[] FitBrent(BrentFunction f, ref double par, double dpar, int n, out double chiSquare, out double[] error)
        {
            const int ITMAX = 100;
            const double CGOLD = 0.3819660;
            const double ZEPS = 1.0e-10;

            double ax = par - dpar, bx = par, cx = par + dpar;
            const double tol = 0.01;

            double a, b, d = 0.0, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
            double[] cou = new double[n];
            double[] cov = new double[n];
            double[] cow = new double[n];
            double[] cox = new double[n];
            double[] eru = new double[n];
            double[] erv = new double[n];
            double[] erw = new double[n];
            double[] erx = new double[n];
            double e = 0.0;				// This will be the distance moved on the step before last.
            a = (ax < cx ? ax : cx);	// a and b must be in ascending order, but input abscissas need not be.
            b = (ax > cx ? ax : cx);
            x = w = v = bx;				//Initializations...


            // fitting
            //fw=fv=fx=(*f)(x);
            //Fit_Lines_Linear(spectrum, lines, x, out erx).CopyTo(cox, 0);
            cox = f(x, out erx, out fw);

            cox.CopyTo(cow, 0);
            cox.CopyTo(cov, 0);
            erx.CopyTo(erw, 0);
            erx.CopyTo(erv, 0);
            //fw = fv = fx = Fit_Lines_ChiSquare(spectrum, lines, cox, x);
            fv = fx = fw;

            for (int iter = 1; iter <= ITMAX; iter++)
            {
                //Main program loop.
                xm = 0.5 * (a + b);
                tol2 = 2.0 * (tol1 = tol * Math.Abs(x) + ZEPS);
                if (Math.Abs(x - xm) <= (tol2 - 0.5 * (b - a)))
                {
                    //Test for done here.
                    par = x;
                    error = erx;
                    chiSquare = fx;
                    return cox;
                }
                if (Math.Abs(e) > tol1)
                {
                    //Construct a trial parabolic fit.
                    r = (x - w) * (fx - fv);
                    q = (x - v) * (fx - fw);
                    p = (x - v) * q - (x - w) * r;
                    q = 2.0 * (q - r);
                    if (q > 0.0) p = -p;
                    q = Math.Abs(q);
                    etemp = e;
                    e = d;
                    if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (a - x) || p >= q * (b - x))
                        d = CGOLD * (e = (x >= xm ? a - x : b - x));
                    //The above conditions determine the acceptability of the parabolic fit. Here we
                    //take the golden section step into the larger of the two segments.
                    else
                    {
                        d = p / q; //Take the parabolic step.
                        u = x + d;
                        if (u - a < tol2 || b - u < tol2)
                            d = ((xm - x) >= 0.0 ? Math.Abs(tol1) : -Math.Abs(tol1));

                    }
                }
                else
                {
                    d = CGOLD * (e = (x >= xm ? a - x : b - x));
                }
                u = Math.Abs(d) >= tol1 ? x + d : x + ((d >= 0.0 ? Math.Abs(tol1) : -Math.Abs(tol1)));


                // fitting
                // fu = (*f)(u);
                // This is the one function evaluation per iteration.
                //Fit_Lines_Linear(spectrum, lines, u, out eru).CopyTo(cou, 0);
                //fu = Fit_Lines_ChiSquare(spectrum, lines, cou, u);
                cou = f(u, out eru, out fu);

                if (fu <= fx)
                {
                    //Now decide what to do with our function evaluation.
                    if (u >= x) a = x; else b = x;

                    //Housekeeping follows:
                    v = w; w = x; x = u;
                    fv = fw; fw = fx; fx = fu;
                    cov = cow; cow = cox; cox = cou;	//
                    erv = erw; erw = erx; erx = eru;

                    //SHFT(v,w,x,u) 
                    //SHFT(fv,fw,fx,fu)
                }
                else
                {
                    if (u < x) a = u; else b = u;
                    if (fu <= fw || w == x)
                    {
                        v = w;
                        w = u;
                        fv = fw;
                        cov = cow;	//
                        erv = erw;
                        fw = fu;
                        cow = cou;	//
                        erw = eru;
                    }
                    else if (fu <= fv || v == x || v == w)
                    {
                        v = u;
                        fv = fu;
                        cov = cou;	//
                        erv = eru;
                    }
                } //Done with housekeeping. Back for another iteration. 
            }
            //nrerror("Too many iterations in brent");
            par = x;
            error = erx;
            chiSquare = fx;
            return cox;
        }

    }
}

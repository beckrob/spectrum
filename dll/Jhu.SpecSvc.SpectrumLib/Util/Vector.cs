using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Util
{
    public static class Vector
    {
        public delegate double WeightFunction(double x, double y);

        public static void Multiply(double m, double[] y, out double[] ny)
        {
            ny = new double[y.Length];

            if (y != null)
            {
                for (int j = 0; j < y.Length; j++)
                {
                    ny[j] = y[j] * m;
                }
            }
        }

        public static void Multiply(double m, double[][] y, out double[][] ny)
        {
            ny = new double[y.Length][];
            for (int i = 0; i < y.Length; i++)
            {
                ny[i] = (y[i] == null) ? null : new double[y[i].Length];
            }

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] != null)
                {
                    for (int j = 0; j < y[i].Length; j++)
                    {
                        ny[i][j] = y[i][j] * m;
                    }
                }
            }
        }

        public static void Sum(double[][] y, double[] m, out double[] ny)
        {
            ny = new double[y[0].Length];

            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < y[i].Length; j++)
                {
                    ny[j] += y[i][j] * m[i];
                }
            }
        }

        public static void Sum(double[][] a, double[][] b, double am, double bm, out double[][] ny)
        {
            ny = new double[a.Length][];

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null && b[i] != null)
                {
                    ny[i] = new double[a[i].Length];
                    for (int j = 0; j < a[i].Length; j++)
                    {
                        ny[i][j] = am * a[i][j] + bm * b[i][j];
                    }
                }
                else
                {
                    ny[i] = null;
                }
            }
        }

        public static void Add(double[] y, double[] b, out double[] ny)
        {
            ny = new double[y.Length];

            for (int i = 0; i < y.Length; i++)
            {
                ny[i] += y[i] + b[i];
            }
        }

        public static void Subtract(double[] y, double[] b, out double[] ny)
        {
            ny = new double[y.Length];

            for (int i = 0; i < y.Length; i++)
            {
                ny[i] += y[i] - b[i];
            }
        }

        public static void Weight(double[] x, double[][] y, WeightFunction f, out double[][] ny)
        {
            ny = new double[y.Length][];
            for (int i = 0; i < y.Length; i++)
            {
                ny[i] = (y[i] == null) ? null : new double[y[i].Length];
            }

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] != null)
                {
                    for (int j = 0; j < y[i].Length; j++)
                    {
                        ny[i][j] = f(x[j], y[i][j]);
                    }
                }
            }
        }

        public static void Weight(double[] y, double[] f, out double[] ny)
        {
            double[][] nny;
            Weight(new double[][] { y }, f, out nny);
            ny = nny[0];
        }

        public static void Weight(double[][] y, double[] f, out double[][] ny)
        {
            ny = new double[y.Length][];
            for (int i = 0; i < y.Length; i++)
            {
                ny[i] = (y[i] == null) ? null : new double[y[i].Length];
            }

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] != null)
                {
                    for (int j = 0; j < y[i].Length; j++)
                    {
                        ny[i][j] = f[j] * y[i][j];
                    }
                }
            }
        }

        public static double ChiSquared(double[] y, double[] fy, bool[] mask, out int ndf)
        {
            double chi2 = 0;
            ndf = 0;

            for (int i = 0; i < y.Length; i++)
            {
                if (mask == null || !mask[i])
                {
                    double d = y[i] - fy[i];
                    chi2 += d * d;
                    ndf++;
                }
            }

            return chi2;
        }

        public static double Median(double[] x)
        {
            return Median(x, null);
        }

        public static double Median(double[] x, bool[] mask)
        {
            List<double> y = new List<double>(x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                if (mask == null || !mask[i])
                {
                    y.Add(x[i]);
                }
            }

            if (y.Count > 0)
            {
                y.Sort();
                return y[y.Count / 2];
            }
            else
            {
                return double.NaN;
            }
        }

        public static void AvgVar(double[] x, bool[] mask, out double avg, out double var)
        {
            avg = 0;
            var = 0;
            int cnt = 0;

            for (int i = 0; i < x.Length; i++)
            {
                if (mask == null || !mask[i])
                {
                    cnt++;
                    avg += x[i];
                    var += x[i] * x[i];
                }
            }

            if (cnt > 0)
            {
                avg /= cnt;
                var = var / cnt - avg * avg;
            }
            else
            {
                throw new Exception("No points or all masked");
            }
        }

        public static void AvgVar(double[] x, bool[] mask, out double avg, out double var, double sigma, int repeat)
        {
            double aavg, avar;
            

            // Calculate first estimate
            AvgVar(x, mask, out avg, out var);
            aavg = avg;
            avar = var;

            for (int r = 0; r < repeat; r++)
            {
                avg = var = 0;
                int cnt = 0;
                
                for (int i = 0; i < x.Length; i++)
                {
                    if (mask == null || !mask[i])
                    {
                        double d = (x[i] - aavg) * (x[i] - aavg);

                        if (d < avar * sigma)
                        {
                            cnt++;
                            avg += x[i];
                            var += x[i] * x[i];
                        }
                    }
                }

                if (cnt > 0)
                {
                    avg /= cnt;
                    var = var / cnt - avg * avg;

                    aavg = avg;
                    avar = var;
                }
                else
                {
                    throw new Exception();
                }                
            }
        }

        public static double[] Copy(double[] x)
        {
            double[] y = new double[x.Length];
            x.CopyTo(y, 0);
            return y;
        }
    }
}

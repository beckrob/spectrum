using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Util
{
    public static class Grid
    {
        public static void FindBins(double[] x, out double[] xmin, out double[] xmax, out double[] xsize)
        {
            xmin = new double[x.Length];
            xmax = new double[x.Length];
            xsize = new double[x.Length];

            for (int i = 0; i < x.Length - 1; i++)
            {
                xmax[i] = xmin[i + 1] = (x[i] + x[i + 1]) / 2;
            }
            xmin[0] = 2 * x[0] - xmax[0];
            xmax[x.Length - 1] = 2 * x[x.Length - 1] - xmin[x.Length - 1];
        }

        public static void Rebin(double[] xmin, double[] xmax, double[] y, long[] mask, double[] nxmin, double[] nxmax, out double[] ny, out long[] nmask)
        {
            double[][] tny;

            Rebin(xmin, xmax, new double[][] { y }, mask, nxmin, nxmax, out tny, out nmask);
            ny = tny[0];
        }

        public static void Rebin(double[] xmin, double[] xmax, double[][] y, long[] mask, double[] nxmin, double[] nxmax, out double[][] ny, out long[] nmask)
        {
            // --- Initialize result arrays

            ny = new double[y.Length][];
            for (int i = 0; i < y.Length; i++)
            {
                ny[i] = (y[i] == null) ? null : new double[nxmin.Length];
            }
            nmask = (mask == null) ? null : new long[nxmin.Length];

            // --- Set up pointers and accumulators

            int op = 0; // old pointer going through the old grid
            int np = 0; // new pointer going through the new grid
            double[] accvalue = new double[ny.Length];
            long accmask;

            // Both pointers have to be inside the array index
            while (np < nxmin.Length)
            {
                // --- overhanging

                // If the new bin is not entirely covered it's value should be 0
                if (op == xmin.Length || nxmin[np] <= xmin[0] || nxmax[np] >= xmax[xmax.Length - 1])
                {
                    for (int i = 0; i < ny.Length; i++)
                    {
                        if (ny[i] != null) ny[i][np] = 0;
                    }

                    if (nmask != null)
                    {
                        nmask[np] = 0x0FFFFFFFFFFFFFFF;
                    }

                    np++;
                    continue;
                }

                // Skip if the old bin is not covered by the new bins
                if (nxmin[np] >= xmax[op])
                {
                    op++;
                    continue;
                }

                // Reset accumulators
                for (int i = 0; i < accvalue.Length; i++)
                {
                    accvalue[i] = 0.0;
                }
                accmask = 0;

                // Integrate flux and mask
                while (true)
                {
                    // old ------|------|-------
                    // new ----|-====|----------

                    // old ----|--------|-------
                    // new ----|=====|----------
                    if (nxmin[np] <= xmin[op] &&
                        nxmax[np] < xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                accvalue[i] += y[i][op] * (nxmax[np] - xmin[op]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        break;
                    }

                    // old ------|------|-------
                    // new --------|=====---|---

                    // old ------|------|-------
                    // new ------|=====-----|---
                    if (nxmin[np] >= xmin[op] &&
                        nxmax[np] > xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                accvalue[i] += y[i][op] * (xmax[op] - nxmin[np]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        op++;
                        continue;
                    }



                    // old ------|======|-------
                    // new ---|--------------|--
                    if (nxmin[np] < xmin[op] &&
                        nxmax[np] > xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                accvalue[i] += y[i][op] * (xmax[op] - xmin[op]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        op++;
                        continue;
                    }

                    // old ---|------------|----
                    // new -------|=====|-------
                    if (nxmin[np] > xmin[op] &&
                        nxmax[np] < xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                accvalue[i] += y[i][op] * (nxmax[np] - nxmin[np]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        break;
                    }

                    // old ---------|------|----
                    // new -----|---=======|----

                    // old -----|----------|----
                    // new -----|==========|----
                    if (nxmin[np] <= xmin[op] &&
                        nxmax[np] == xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                ///*** remove old buggy
                                //accvalue[i] += y[i][op] * (xmax[np] - xmin[np]);
                                accvalue[i] += y[i][op] * (xmax[op] - xmin[op]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        op++;
                        break;
                    }

                    // old -----|----------|----
                    // new ---------|======|----
                    if (nxmin[np] > xmin[op] &&
                        nxmax[np] == xmax[op])
                    {
                        for (int i = 0; i < ny.Length; i++)
                        {
                            if (y[i] != null)
                            {
                                accvalue[i] += y[i][op] * (nxmax[np] - nxmin[np]);
                            }
                        }
                        if (mask != null)
                        {
                            accmask |= mask[op];
                        }

                        op++;
                        break;
                    }

                    throw new Exception();
                }

                for (int i = 0; i < ny.Length; i++)
                {
                    if (ny[i] != null)
                    {
                        ny[i][np] = accvalue[i] / (nxmax[np] - nxmin[np]);
                    }
                }
                if (nmask != null)
                {
                    nmask[np] = accmask;
                }

                np++;
            }
        }

        public static void GetRange(double[] x, double[] y, bool[] mask, double xmin, double xmax, out double[] nx, out double[] ny,out bool[] nmask)
        {
            double[][] nny;
            GetRange(x, new double[][] { y }, mask, xmin, xmax, out nx, out nny, out nmask);

            ny = nny[0];
        }

        public static void GetRange(double[] x, double[][] y, bool[] mask, double xmin, double xmax, out double[] nx, out double[][] ny, out bool[] nmask)
        {
            GetRange(x, y, mask, xmin, xmax, out nx, out ny, out nmask, false);
        }

        public static void GetRange(double[] x, double[] y, bool[] mask, double xmin, double xmax, out double[] nx, out double[] ny, out bool[] nmask, bool addTrailingZero)
        {
            double[][] nny;
            GetRange(x, new double[][] { y }, mask, xmin, xmax, out nx, out nny, out nmask, addTrailingZero);

            ny = nny[0];
        }


        // Trailing zero required for certain numrec functions
        public static void GetRange(double[] x, double[][] y, bool[] mask, double xmin, double xmax, out double[] nx, out double[][] ny, out bool[] nmask, bool addTrailingZero)
        {
            List<double> nnx = new List<double>();
            if (addTrailingZero) nnx.Add(0.0);

            List<bool> nnmask = new List<bool>();
            if (addTrailingZero) nnmask.Add(true);

            List<double>[] nny = new List<double>[y.Length];
            for (int i = 0; i < nny.Length; i++)
            {
                nny[i] = new List<double>();
                if (addTrailingZero) nny[i].Add(0.0);
            }


            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] >= xmin && x[i] <= xmax)
                {
                    nnx.Add(x[i]);
                    if (mask != null) nnmask.Add(mask[i]);

                    for (int j = 0; j < nny.Length; j++)
                    {
                        nny[j].Add(y[j][i]);
                    }
                }
            }

            nx = nnx.ToArray();
            if (mask != null) nmask = nnmask.ToArray(); else nmask = null;
            ny = new double[y.Length][];
            for (int i = 0; i < ny.Length; i++)
            {
                ny[i] = nny[i].ToArray();
            }
        }
    }
}

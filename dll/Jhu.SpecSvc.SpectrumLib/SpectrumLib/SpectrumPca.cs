#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumPca.cs,v 1.1 2008/01/08 21:36:58 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:58 $
 */
#endregion
using System;
using Lapack;
using VoServices.Schema.Spectrum;
using VoServices.SpecSvc.Lib;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace VoServices.SpecSvc.Lib
{
    public class SpectrumPca
    {

        public class CovMatrix
        {
            public int Count;
            public int NumPoints;
            public double[] Wl;
            public double[] Avg;
            public double[][] Cov;

            public int[] AvgCountGappy;
            public int[][] CovCountGappy;
        }

        public static PcaResults DoPca(IEnumerable<Spectrum> spectra, PcaParameters par)
        {
            Svd svd;
            CovMatrix cmr;
            double[] wl;

            if (!par.Gappy)
                PcaClassic(spectra, par, out svd, out cmr);
            else
                PcaClassicGappy(spectra, par, out svd, out cmr);

            return ExtractPcaResults(svd, cmr, par);
        }

        
        // Assumes spectra are on the same grid
        // No gappy pca, doesn't care about masks or errors, good for model spectra
        public static void PcaClassic(IEnumerable<Spectrum> spectra, PcaParameters par, out Svd svd, out CovMatrix cmr)
        {
            ParallelExecuter<Spectrum, PcaParameters, CovMatrix> pe = new ParallelExecuter<Spectrum, PcaParameters, CovMatrix>();
            //pe.PoolSize = 1;    //******
            pe.Parameters = par;
            pe.InQueue = spectra;

            pe.AccumulatorFunction = delegate(CovMatrix cm, Spectrum s, PcaParameters ppar)
            {
                // Initialize on the first iteration
                if (cm == null)
                {
                    cm = new CovMatrix();
                    cm.Count = 0;

                    cm.NumPoints = s.Spectral_Value.Length;

                    cm.Wl = new double[cm.NumPoints];
                    cm.Avg = new double[cm.NumPoints];
                    cm.Cov = new double[cm.NumPoints][];
                    for (int i = 0; i < cm.NumPoints; i++)
                        cm.Cov[i] = new double[cm.NumPoints];

                    for (int i = 0; i < cm.Wl.Length; i++)
                        cm.Wl[i] = s.Spectral_Value[i];

                }

                // accumulate

                for (int i = 0; i < cm.NumPoints; i++)
                {
                    for (int j = 0; j < cm.NumPoints; j++)
                    {
                        cm.Cov[i][j] += (s.Flux_Value[i] * s.Flux_Value[j]);
                    }
                    cm.Avg[i] += s.Flux_Value[i];
                }

                cm.Count++;
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " " + cm.Count.ToString());

                return cm;
            };

            pe.MergerFunction = delegate(CovMatrix cm1, CovMatrix cm2, PcaParameters ppar)
            {
                if (cm1 == null)
                    return cm2;
                else
                {
                    for (int i = 0; i < cm1.Cov.Length; i++)
                    {
                        cm1.Avg[i] += cm2.Avg[i];
                        for (int j = 0; j < cm1.Cov[i].Length; j++)
                            cm1.Cov[i][j] += cm2.Cov[i][j];
                    }

                    cm1.Count += cm2.Count;

                    return cm1;
                }
            };            
            
            // Execute
            cmr = pe.RunAccumulator();
            
            // calculate final covariance matrix

            for (int k = 0; k < cmr.Avg.Length; k++)
                cmr.Avg[k] /= cmr.Count;

            for (int i = 0; i < cmr.Cov.Length; i++)
                for (int j = 0; j < cmr.Cov[i].Length; j++)
                {
                    cmr.Cov[i][j] = cmr.Cov[i][j] / cmr.Count - cmr.Avg[i] * cmr.Avg[j];
                }

            // do SVD on the covariance matrix

            // Copy to a matrix object to make it Lapack compatible (FORTRAN item order)
            Matrix C = new Matrix(cmr.NumPoints);
            for (int i = 0; i < cmr.NumPoints; i++)
                for (int j = 0; j < cmr.NumPoints; j++)
                    C[i, j] = cmr.Cov[i][j];

            svd = new Svd(C, Implementation.Managed);
        }

        public static void PcaClassicGappy(IEnumerable<Spectrum> spectra, PcaParameters par, out Svd svd, out CovMatrix cmr)
        {
            ParallelExecuter<Spectrum, PcaParameters, CovMatrix> pe = new ParallelExecuter<Spectrum, PcaParameters, CovMatrix>();
            //pe.PoolSize = 1;    //******
            pe.Parameters = par;
            pe.InQueue = spectra;

            pe.AccumulatorFunction = delegate(CovMatrix cm, Spectrum s, PcaParameters ppar)
            {
                // Initialize on the first iteration
                if (cm == null)
                {
                    cm = new CovMatrix();

                    cm.NumPoints = s.Spectral_Value.Length;

                    cm.Wl = new double[cm.NumPoints];
                    cm.Avg = new double[cm.NumPoints];
                    cm.Cov = new double[cm.NumPoints][];
                    for (int i = 0; i < cm.NumPoints; i++)
                        cm.Cov[i] = new double[cm.NumPoints];

                    cm.AvgCountGappy = new int[cm.NumPoints];
                    cm.CovCountGappy = new int[cm.NumPoints][];
                    for (int i = 0; i < cm.NumPoints; i++)
                        cm.CovCountGappy[i] = new int[cm.NumPoints];

                    for (int i = 0; i < cm.Wl.Length; i++)
                        cm.Wl[i] = s.Spectral_Value[i];

                }

                // accumulate

                for (int i = 0; i < cm.NumPoints; i++)
                {
                    for (int j = 0; j < cm.NumPoints; j++)
                    {
                        if (s.Flux_Accuracy_Quality[i] == 0 && s.Flux_Accuracy_Quality[j] == 0)
                        {
                            cm.Cov[i][j] += (s.Flux_Value[i] * s.Flux_Value[j]);
                            cm.CovCountGappy[i][j]++;
                        }
                    }
                    if (s.Flux_Accuracy_Quality[i] == 0)
                    {
                        cm.Avg[i] += s.Flux_Value[i];
                        cm.AvgCountGappy[i]++;
                    }
                }

                cm.Count++;
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " " + cm.Count.ToString());

                return cm;
            };

            pe.MergerFunction = delegate(CovMatrix cm1, CovMatrix cm2, PcaParameters ppar)
            {
                if (cm1 == null)
                    return cm2;
                else
                {
                    for (int i = 0; i < cm1.Cov.Length; i++)
                    {
                        cm1.Avg[i] += cm2.Avg[i];
                        for (int j = 0; j < cm1.Cov[i].Length; j++)
                            cm1.Cov[i][j] += cm2.Cov[i][j];
                    }

                    for (int i = 0; i < cm1.CovCountGappy.Length; i++)
                    {
                        for (int j = 0; j < cm1.CovCountGappy[i].Length; j++)
                        {
                            cm1.CovCountGappy[i][j] += cm2.CovCountGappy[i][j];
                        }
                        cm1.AvgCountGappy[i] += cm2.AvgCountGappy[i];
                    }

                    return cm1;
                }
            };

            // Execute
            cmr = pe.RunAccumulator();

            // calculate final covariance matrix

            for (int k = 0; k < cmr.Avg.Length; k++)
            {
                if (cmr.AvgCountGappy[k] != 0)
                    cmr.Avg[k] /= cmr.AvgCountGappy[k];
            }

            for (int i = 0; i < cmr.Cov.Length; i++)
                for (int j = 0; j < cmr.Cov[i].Length; j++)
                {
                    if (cmr.CovCountGappy[i][j] != 0)
                        cmr.Cov[i][j] = cmr.Cov[i][j] / cmr.CovCountGappy[i][j] - cmr.Avg[i] * cmr.Avg[j];
                }

            // do SVD on the covariance matrix

            // Copy to a matrix object to make it Lapack compatible (FORTRAN item order)
            Matrix C = new Matrix(cmr.NumPoints);
            for (int i = 0; i < cmr.NumPoints; i++)
                for (int j = 0; j < cmr.NumPoints; j++)
                    C[i, j] = cmr.Cov[i][j];

            svd = new Svd(C, Implementation.Managed);
        }

        /*
        public static PcaResults PcaRobust(IEnumerable<Spectrum> trainingSet, IEnumerable<Spectrum> spectra, PcaParameters par)
        {
            // Run pca on the training set to get average and cov estimation
            Svd svd;
            CovMatrix cmr;

            PcaClassic(trainingSet, par, out svd, out cmr);

            //
            // truncate
            int p = 1;
            Matrix U = svd.U.Submatrix(0, svd.U.Rows - 1, 0, p - 1);
            Matrix W = svd.Vt.Submatrix(0, p - 1, 0, p - 1);

            Matrix M = new Matrix(cmr.Avg.Length, 1);
            M.SetColumn(0, cmr.Avg);

            // iteration to improve pca
            IncrementalPrincipalComponents ipc = new IncrementalPrincipalComponents(M, U, W);

            TimeSpan time = TimeSpan.Zero;

            using (StreamWriter w = new StreamWriter(@"_inc.txt"))
            {
                int iter = 0;
                foreach (Spectrum s in spectra)
                {
                    int i = iter + cmr.Count; //Xsub.Columns;
                    //Matrix x = X.Submatrix(0, X.Rows - 1, i, i);

                    Matrix x = new Matrix(s.Flux_Value.Length, 1);
                    x.SetColumn(0, s.Flux_Value);

                    double alpha = 1 - 1.0 / i;
                    double beta = 2.3849;
                    DateTime start = DateTime.Now;
                    ipc.Step(x, alpha, beta);
                    time += DateTime.Now - start;

                    if (iter % 10 == 0)
                    {
                        w.WriteLine("{0} {1} {2} {3} {4} {5} {6}", // {7}",
                            iter, i, ipc.W[0, 0],
                            ipc.U[0, 0], ipc.U[1, 0],
                            ipc.M[0, 0], ipc.M[1, 0]);// 1 - Proj.Trace);
                    }
                }
                //Console.Out.WriteLine("Time: {0}", time);
                Console.Out.WriteLine("M:\n{0}", ipc.M);
                Console.Out.WriteLine("W:\n{0}", ipc.W);
                Console.Out.WriteLine("U:\n{0}", ipc.U);
            }

            // Create result object
            PcaResults res = new PcaResults();

            res.Average = new Spectrum(true);
            res.Average.BasicInitialize();
            res.Average.Target.Name.Value = "Average";
            res.Average.Spectral_Value = new double[M.Rows];
            res.Average.Flux_Value = new double[M.Rows];
            res.Average.Flux_Accuracy_StatErrLow = null;
            res.Average.Flux_Accuracy_StatErrHigh = null;
            res.Average.Flux_Accuracy_Quality = null;
            for (int j = 0; j < cmr.NumPoints; j++)
            {
                res.Average.Spectral_Value[j] = cmr.Wl[j];
                res.Average.Flux_Value[j] = M[j,0];
            }

            // copy eigenvectors to the results object
            res.Eigenspectra = new Spectrum[par.Dimensions];
            for (int i = 0; i < par.Dimensions; i++)
            {
                res.Eigenspectra[i] = new Spectrum(true);
                res.Eigenspectra[i].BasicInitialize();
                res.Eigenspectra[i].Target.Name.Value = "Eigenspectrum - " + i.ToString();
                res.Eigenspectra[i].Spectral_Value = new double[cmr.NumPoints];
                res.Eigenspectra[i].Flux_Value = new double[cmr.NumPoints];
                res.Eigenspectra[i].Flux_Accuracy_StatErrLow = null;
                res.Eigenspectra[i].Flux_Accuracy_StatErrHigh = null;
                res.Eigenspectra[i].Flux_Accuracy_Quality = null;

                for (int j = 0; j < cmr.NumPoints; j++)
                {
                    res.Eigenspectra[i].Spectral_Value[j] = cmr.Wl[j];
                    res.Eigenspectra[i].Flux_Value[j] = W[j, i];
                }
            }

            return res;

        }
         * */

        private static PcaResults ExtractPcaResults(Svd svd, CovMatrix cmr, PcaParameters par)
        {
            // Create result object
            PcaResults res = new PcaResults();

            res.Average = new Spectrum(true);
            res.Average.BasicInitialize();
            res.Average.Target.Name.Value = "Average";
            res.Average.Spectral_Value = new double[cmr.NumPoints];
            res.Average.Flux_Value = new double[cmr.NumPoints];
            res.Average.Flux_Accuracy_StatErrLow = null;
            res.Average.Flux_Accuracy_StatErrHigh = null;
            res.Average.Flux_Accuracy_Quality = null;
            for (int j = 0; j < cmr.NumPoints; j++)
            {
                res.Average.Spectral_Value[j] = cmr.Wl[j];
                res.Average.Flux_Value[j] = cmr.Avg[j];
            }

            // copy eigenvectors to the results object
            res.Eigenspectra = new Spectrum[par.Dimensions];
            for (int i = 0; i < par.Dimensions; i++)
            {
                res.Eigenspectra[i] = new Spectrum(true);
                res.Eigenspectra[i].BasicInitialize();
                res.Eigenspectra[i].Target.Name.Value = "Eigenspectrum - " + i.ToString() + " - eigenvalue=" + svd.Diagonal[i].ToString();
                res.Eigenspectra[i].Spectral_Value = new double[cmr.NumPoints];
                res.Eigenspectra[i].Flux_Value = new double[cmr.NumPoints];
                res.Eigenspectra[i].Flux_Accuracy_StatErrLow = null;
                res.Eigenspectra[i].Flux_Accuracy_StatErrHigh = null;
                res.Eigenspectra[i].Flux_Accuracy_Quality = null;

                for (int j = 0; j < cmr.NumPoints; j++)
                {
                    res.Eigenspectra[i].Spectral_Value[j] = cmr.Wl[j];
                    res.Eigenspectra[i].Flux_Value[j] = svd.Vt[j, i];
                }
            }

            res.Eigenvalues = new double[par.Dimensions];
            Array.Copy(svd.Diagonal, res.Eigenvalues, par.Dimensions);

            return res;
        }

        //public static PcaResults DoPca(IEnumerable<Spectrum> spectra, PcaParameters par)
        //{
        //}
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumPca.cs,v $
        Revision 1.1  2008/01/08 21:36:58  dobos
        Initial checkin


*/
#endregion
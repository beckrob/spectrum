using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class PcaStep : PipelineStep
    {
        private Spectrum cache;

        private string function;
        private long mask;
        private int components;
        private bool subtractAverage;
        private int initCount;

        public string Function
        {
            get { return function; }
            set { function = value; }
        }

        public long Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public int Components
        {
            get { return components; }
            set { components = value; }
        }

        public bool SubtractAverage
        {
            get { return subtractAverage; }
            set { subtractAverage = value; }
        }

        public int InitCount
        {
            get { return initCount; }
            set { initCount = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.PcaTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.PcaDescription; }
        }

        public PcaStep()
        {
            InitializeMembers();
        }

        public PcaStep(PcaStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.function = "Flux_Value";
            this.mask = 0;
            this.components = 10;
            this.subtractAverage = false;
            this.initCount = 250;
        }

        private void CopyMembers(PcaStep old)
        {
            this.function = old.function;
            this.mask = old.mask;
            this.components = old.components;
            this.subtractAverage = old.subtractAverage;
            this.initCount = old.initCount;
        }

        public override void InitializeStep(int count)
        {
            base.InitializeStep(count);

            this.count = count;
        }

        public override ParallelQuery<Spectrum> Execute(ParallelQuery<Spectrum> spectra)
        {
            Pca.StreamingPca pca = new Pca.StreamingPca(components, components);
            pca.subtractAverage = this.subtractAverage;

            // Initialize streaming parameters
            pca.alpha = 1.0 - 1.0 / (double)count;
            pca.delta = 0.5;
            double c = 0.787;
            pca.W = (t => 1 / (c * c + Math.Pow((Math.PI / 2 * t / c), 2)));
            pca.Wstar = (t => 2 / Math.PI / t * Math.Atan(Math.PI / 2 * t / c / c));

            Console.WriteLine("PCA initialization started with {0} spectra.", initCount);

            pca.InitializePca(spectra.Select(s => GetVectorFromSpectrum(s)).Where(v => v != null), initCount, Pca.InitializationMethod.DualPCA);

            Console.WriteLine("PCA initialized with {0} spectra.", initCount);

            // *** debug code to write out basis after initialization
            /*
            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter("basis.txt"))
            {
                for (int i = 0; i < cache.Spectral_Value.Length; i++)
                {
                    outfile.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                        cache.Spectral_Value[i],
                        pca.m[i],
                        pca.E[i, 0],
                        pca.E[i, 1],
                        pca.E[i, 2],
                        pca.E[i, 3]);
                }
            }
             * */

            // Reset counter
            for (int i = 0; i < pca.n.Length; i++)
            {
                pca.n[i] = 0;
            }

            // Run pca
            while (pca.Step())
            {
                Console.Write("+");
            }

            // Finished, extract principal components

            var res = new List<Jhu.SpecSvc.SpectrumLib.Spectrum>();

            for (int i = -1; i < components; i++)
            {
                if (subtractAverage || i != -1)
                {
                    var ss = new Jhu.SpecSvc.SpectrumLib.Spectrum(true);
                    ss.BasicInitialize();

                    int points = cache.Flux_Value.Length;

                    ss.Data = new Jhu.SpecSvc.Schema.Spectrum.Data(cache.Data);

                    ss.Counts_Value = new double[points];
                    for (int k = 0; k < ss.Counts_Value.Length; k++)
                    {
                        ss.Counts_Value[k] = pca.n[k];
                    }
                    ss.Spectral_Value = new double[points];
                    ss.Spectral_Accuracy_BinLow = new double[points];
                    ss.Spectral_Accuracy_BinHigh = new double[points];
                    ss.Flux_Value = new double[points];

                    if (i == -1)
                    {
                        //ss.Flux_Accuracy_StatErrLow = new double[points];
                        //ss.Flux_Accuracy_StatErrHigh = new double[points];
                        ss.Flux_Accuracy_StatError = new double[points];
                        ss.Flux_Accuracy_Quality = new long[points];
                    }

                    if (i == -1)
                    {
                        ss.Target.Name.Value = "Principal Component Average";
                        ss.Derived.Eigenvalue = new Jhu.SpecSvc.Schema.DoubleParam(0, "");
                    }
                    else
                    {
                        ss.Target.Name.Value = String.Format("Principal Component i = {0}; l = {1}", i, pca.L[i]);
                        ss.Derived.Eigenvalue = new Jhu.SpecSvc.Schema.DoubleParam(pca.L[i], "");
                    }
                    ss.Data.FluxAxis.Value.Unit = "ADU";

                    ss.DataId.CreationType.Value = Jhu.SpecSvc.Schema.Spectrum.DataId.DERIVED;

                    // Copy data
                    cache.Spectral_Value.CopyTo(ss.Spectral_Value, 0);
                    cache.Spectral_Accuracy_BinLow.CopyTo(ss.Spectral_Accuracy_BinLow, 0);
                    cache.Spectral_Accuracy_BinHigh.CopyTo(ss.Spectral_Accuracy_BinHigh, 0);
                    if (i == -1)
                    {
                        for (int k = 0; k < ss.Flux_Value.Length; k++)
                        {
                            ss.Flux_Value[k] = pca.m[k];
                            ss.Flux_Accuracy_StatError[k] = Math.Sqrt(pca.s2[k]);
                            ss.Flux_Accuracy_Quality[k] = (long)PointMask.Ok;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < ss.Flux_Value.Length; k++)
                        {
                            ss.Flux_Value[k] = pca.E[k, i];
                        }
                    }

                    res.Add(ss);
                }
            }

            return res.AsParallel();
        }

        private Pca.Vector GetVectorFromSpectrum(Spectrum s)
        {
            if (s != null)
            {
                // ******* TODO: add weight vector

                cache = s;

                double[] value = (double[])typeof(Jhu.SpecSvc.SpectrumLib.Spectrum).GetField(function).GetValue(s);

                if (mask != 0)
                {
                    bool[] ma = new bool[s.Flux_Accuracy_Quality.Length];

                    int masked = 0;
                    for (int i = 0; i < ma.Length; i++)
                    {
                        ma[i] = ((s.Flux_Accuracy_Quality[i] & mask) != 0);
                        if (ma[i])
                        {
                            masked++;
                        }
                    }

                    return new Pca.Vector(value, null, ma);
                }
                else
                {
                    return new Pca.Vector(value, null, null);
                }
            }
            else
            {
                return null;
            }
        }

        public override int GetOutputCount()
        {
            return components;
        }

        protected override double GetProgress()
        {
            return (double)iteration / (double)count;
        }
    }
}

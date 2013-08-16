using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class CompositeStep : PipelineStep
    {
        [Flags]
        public enum CompositeMethod
        {
            Average = 1,
            Sum = 2,
            Variance = 4,
            Median = 8
        }

        private class CompositeResult
        {
            private CompositeMethod method;
            private long mask;

            private double[] sum;
            private double[] sum2;
            private double[] err;
            private long[] quality;

            /// <summary>
            /// Counts in wavelength bins to compute averate, stdev etc
            /// </summary>
            int[] counts;

            /// <summary>
            /// Used for computing median
            /// </summary>
            private List<double>[] fluxes;

            Spectrum cache;

            public CompositeResult()
            {
                InitializeMembers();
            }

            private void InitializeMembers()
            {

            }

            public void Initialize(Spectrum s, CompositeMethod method, long mask)
            {
                cache = s;

                this.method = method;
                this.mask = mask;

                int points = s.Flux_Value.Length;

                sum = new double[points];
                sum2 = new double[points];
                err = new double[points];
                quality = new long[points];
                counts = new int[points];

                if ((method & CompositeMethod.Median) != 0)
                {
                    fluxes = new List<double>[points];
                    for (int i = 0; i < fluxes.Length; i++)
                    {
                        fluxes[i] = new List<double>();
                    }
                }
            }

            public void Accumulate(Spectrum s)
            {
                for (int wl = 0; wl < sum.Length; wl++)
                {
                    if ((s.Flux_Accuracy_Quality[wl] & mask) == 0)
                    {
                        sum[wl] += s.Flux_Value[wl];
                        sum2[wl] += s.Flux_Value[wl] * s.Flux_Value[wl];

                        if (s.Flux_Accuracy_StatError != null)
                        {
                            err[wl] += s.Flux_Accuracy_StatError[wl] * s.Flux_Accuracy_StatError[wl];
                        }
                        if (s.Flux_Accuracy_Quality != null)
                        {
                            quality[wl] |= s.Flux_Accuracy_Quality[wl];
                        }

                        if ((method & CompositeMethod.Median) != 0)
                        {
                            fluxes[wl].Add(s.Flux_Value[wl]);
                        }

                        counts[wl]++;
                    }
                }
            }

            private Spectrum InitializeCompositeSpectrum(Spectrum s, CompositeMethod m)
            {
                Spectrum comp = new Spectrum();
                comp.BasicInitialize();

                comp.Data = new Jhu.SpecSvc.Schema.Spectrum.Data(s.Data);
                comp.Target.Redshift.Value = s.Target.Redshift.Value;

                comp.DataId.CreationType.Value = Jhu.SpecSvc.Schema.Spectrum.DataId.COMPOSITE;

                int points = s.Spectral_Value.Length;

                comp.Spectral_Value = s.Spectral_Value;
                comp.Spectral_Accuracy_BinLow = s.Spectral_Accuracy_StatErrLow;
                comp.Spectral_Accuracy_BinHigh = s.Spectral_Accuracy_StatErrHigh;
                comp.Flux_Value = new double[points];
                comp.Flux_Accuracy_StatError = new double[points];
                comp.Flux_Accuracy_Quality = new long[points];
                comp.Counts_Value = new double[points];

                switch (m)
                {
                    case CompositeMethod.Average:
                        comp.Target.Name.Value = "Average composite spectrum";
                        comp.Data.FluxAxis.Value.Unit = "ADU";
                        break;
                    case CompositeMethod.Sum:
                        comp.Target.Name.Value = "Sum spectrum";
                        comp.Data.FluxAxis.Value.Unit = s.Data.FluxAxis.Value.Unit;
                        break;
                    case CompositeMethod.Variance:
                        comp.Target.Name.Value = "Variance spectrum";
                        comp.Data.FluxAxis.Value.Unit = "ADU";
                        break;
                    case CompositeMethod.Median:
                        comp.Target.Name.Value = "Median composite spectrum";
                        comp.Data.FluxAxis.Value.Unit = "ADU";
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return comp;
            }

            private Spectrum FinishAverage()
            {
                Spectrum comp = InitializeCompositeSpectrum(cache, CompositeMethod.Average);

                for (int wl = 0; wl < sum.Length; wl++)
                {
                    comp.Flux_Value[wl] = sum[wl] / counts[wl];
                    comp.Flux_Accuracy_StatError[wl] = Math.Sqrt((sum2[wl] / counts[wl]) - (sum[wl] / counts[wl]) * (sum[wl] / counts[wl])) / Math.Sqrt((double)counts[wl]);
                    comp.Flux_Accuracy_Quality[wl] = (long)(counts[wl] == 0 ? PointMask.NoData : PointMask.Ok);

                    comp.Counts_Value[wl] = counts[wl];
                }

                return comp;
            }

            private Spectrum FinishSum()
            {
                Spectrum comp = InitializeCompositeSpectrum(cache, CompositeMethod.Average);

                for (int wl = 0; wl < comp.Spectral_Value.Length; wl++)
                {
                    comp.Flux_Value[wl] = sum[wl];
                    comp.Flux_Accuracy_StatError[wl] = Math.Sqrt(err[wl]);
                    comp.Flux_Accuracy_Quality[wl] = quality[wl];

                    comp.Counts_Value[wl] = counts[wl];
                }

                return comp;
            }

            private Spectrum FinishVariance()
            {
                Spectrum comp = InitializeCompositeSpectrum(cache, CompositeMethod.Variance);

                for (int wl = 0; wl < comp.Spectral_Value.Length; wl++)
                {
                    comp.Flux_Value[wl] = sum2[wl] / counts[wl] - (sum[wl] / counts[wl]) * (sum[wl] / counts[wl]);
                    comp.Flux_Accuracy_Quality[wl] = (long)(counts[wl] == 0 ? PointMask.NoData : PointMask.Ok);

                    comp.Counts_Value[wl] = counts[wl];
                }

                return comp;
            }

            private Spectrum FinishMedian()
            {
                Spectrum comp = InitializeCompositeSpectrum(cache, CompositeMethod.Median);

                for (int wl = 0; wl < sum.Length; wl++)
                {
                    fluxes[wl].Sort();
                    if (fluxes[wl].Count > 0)
                    {
                        comp.Flux_Value[wl] = fluxes[wl][fluxes[wl].Count / 2];
                        comp.Flux_Accuracy_Quality[wl] |= (long)PointMask.Ok;
                    }
                    else
                    {
                        comp.Flux_Value[wl] = 0;
                        comp.Flux_Accuracy_Quality[wl] |= (long)PointMask.NoData;
                    }

                    comp.Counts_Value[wl] = counts[wl];
                }

                return comp;
            }

            public IEnumerable<Spectrum> Finish()
            {
                if ((method & CompositeMethod.Average) != 0)
                {
                    yield return FinishAverage();
                }
                if ((method & CompositeMethod.Sum) != 0)
                {
                    yield return FinishSum();
                }
                if ((method & CompositeMethod.Variance) != 0)
                {
                    yield return FinishVariance();
                }
                if ((method & CompositeMethod.Median) != 0)
                {
                    yield return FinishMedian();
                }
            }

        }

        private Dictionary<long, CompositeResult> results;

        private CompositeMethod method;
        private long mask;

        public CompositeMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public long Mask
        {
            get { return this.mask; }
            set { this.mask = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.CompositeTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.CompositeDescription; }
        }

        public CompositeStep()
        {
            InitializeMembers();
        }

        public CompositeStep(CompositeStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = CompositeMethod.Average;
        }

        private void CopyMembers(CompositeStep old)
        {
            this.method = old.method;
        }

        public override void InitializeStep(int count)
        {
            base.InitializeStep(count);
        }

        public override int GetOutputCount()
        {
            return 1;
        }

        public override ParallelQuery<Spectrum> Execute(ParallelQuery<Spectrum> spectra /*, bool skipExceptions*/)
        {
            // delete: if (skipExceptions) this.exceptions = new List<Exception>();
            exceptions.Clear();

            // Initialize storage for aggregation
            results = new Dictionary<long, CompositeResult>();

            foreach (Spectrum s in spectra)
            {
                iteration++;

                CompositeResult res = null;

                // If hash == -1 -> outside binning
                if (s.GroupByHash == -1)
                {
                    continue;
                }

                if (results.ContainsKey(s.GroupByHash))
                {
                    res = results[s.GroupByHash];
                }

                if (res == null)
                {
                    // Creating the composite
                    res = new CompositeResult();
                    res.Initialize(s, method, mask);

                    results.Add(s.GroupByHash, res);
                }

                try
                {
                    res.Accumulate(s);
                }
                catch (System.Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            // Concatenate results into a single collection
            var cres = new List<Spectrum>();

            int q = 0;
            foreach (CompositeResult res in results.Values)
            {
                res.Finish();

                foreach (Spectrum s in res.Finish())
                {
                    q++;

                    s.DataId.CreatorDID.Value = String.Format("#{0}", q);
                    s.Curation.PublisherDID.Value = String.Format("#{0}", q);
                    cres.Add(s);
                }
            }

            return cres.AsParallel();
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.FilterLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class FluxStep : PipelineStep
    {
        public enum RedshiftMode
        {
            AsIs,
            Variable
        }

        private int[] filterIds;
        private RedshiftMode redshift;
        private DoubleInterval redshiftLimits;
        private DoubleParam redshiftBinSize;

        private string[] filterNames;
        private Filter[] filters;
        private double[][] fx;
        private double[][] fy;


        public int[] FilterIds
        {
            get { return filterIds; }
            set { filterIds = value; }
        }

        public RedshiftMode Redshift
        {
            get { return redshift; }
            set { redshift = value; }
        }

        public DoubleInterval RedshiftLimits
        {
            get { return redshiftLimits; }
            set { redshiftLimits = value; }
        }

        public DoubleParam RedshiftBinSize
        {
            get { return redshiftBinSize; }
            set { redshiftBinSize = value; }
        }

        public Filter[] Filters
        {
            get { return filters; }
            set
            {
                filters = value;
                LoadFilters();
            }
        }

        public override string Title
        {
            get { return StepDescriptions.FluxTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.FluxDescription; }
        }

        public FluxStep()
        {
            InitializeMembers();
        }

        public FluxStep(FluxStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.filterIds = new int[0];
            this.redshift = RedshiftMode.AsIs;
            this.redshiftLimits = new DoubleInterval(ParamRequired.Optional);
            this.redshiftBinSize = new DoubleParam(true);
        }

        private void CopyMembers(FluxStep old)
        {
            this.filterIds = old.filterIds;     //  *** array copy
            this.redshift = old.redshift;
            this.redshiftLimits = new DoubleInterval(old.redshiftLimits);
            this.redshiftBinSize = new DoubleParam(old.redshiftBinSize);
        }

        public override void InitializeStep(int count)
        {
            base.InitializeStep(count);

            // load filters
            if (this.filters == null)
            {
                this.filters = new Filter[this.filterIds.Length];

                for (int i = 0; i < this.filterIds.Length; i++)
                {
                    Filter filter = new Filter(connector.DatabaseConnection, connector.DatabaseTransaction);
                    filter.Load(this.filterIds[i]);
                    filter.LoadResponses();

                    filters[i] = filter;
                }

                LoadFilters();
            }
        }

        private void LoadFilters()
        {
            filterIds = new int[this.filters.Length];
            filterNames = new string[this.filters.Length];
            fx = new double[this.filters.Length][];
            fy = new double[this.filters.Length][];

            for (int i = 0; i < this.filters.Length; i++)
            {
                filterIds[i] = filters[i].Id;
                filterNames[i] = filters[i].Name;

                fx[i] = new double[filters[i].Responses.Count];
                fy[i] = new double[filters[i].Responses.Count];
                for (int j = 0; j < fx[i].Length; j++)
                {
                    fx[i][j] = filters[i].Responses[j].Wavelength;
                    fy[i][j] = filters[i].Responses[j].Value;
                }
            }
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            if (spectrum.Magnitudes == null)
            {
                spectrum.Magnitudes = new List<Magnitudes>();
            }

            Magnitudes mm = new Magnitudes();

            mm.FilterId = this.filterIds;
            mm.FilterName = this.filterNames;

            int count;
            if (redshift == RedshiftMode.AsIs)
            {
                count = 1;
            }
            else
            {
                count = 0;
                for (double z = redshiftLimits.Min; z <= redshiftLimits.Max; z += redshiftBinSize.Value)
                {
                    count++;
                }
            }

            mm.Redshift = new double[count];
            mm.Flux = new double[filters.Length][];
            mm.Error = new bool[filters.Length][];

            for (int i = 0; i < filters.Length; i++)
            {
                mm.Flux[i] = new double[count];
                mm.Error[i] = new bool[count];
            }

            int q = 0;
            switch (redshift)
            {
                case RedshiftMode.AsIs:
                    mm.Redshift[q] = spectrum.Derived.Redshift.Value.Value;
                    for (int f = 0; f < filters.Length; f++)
                    {
                        ComputeMagnitudes(spectrum, f, out mm.Flux[f][q], out mm.Error[f][q]);

                        if (double.IsNaN(mm.Flux[f][q]))
                        {
                            Console.WriteLine("!");
                        }
                    }
                    break;
                case RedshiftMode.Variable:
                    double origredshift = spectrum.Derived.Redshift.Value.Value;

                    for (double z = redshiftLimits.Min; z <= redshiftLimits.Max; z += redshiftBinSize.Value)
                    {
                        spectrum.Redshift(z);
                        mm.Redshift[q] = z;
                        for (int f = 0; f < filters.Length; f++)
                        {
                            ComputeMagnitudes(spectrum, f, out mm.Flux[f][q], out mm.Error[f][q]);
                        }

                        q++;
                    }

                    spectrum.Redshift(origredshift);
                    break;
            }

            spectrum.Magnitudes.Add(mm);

            return spectrum;
        }

        private void ComputeMagnitudes(Spectrum spectrum, int f, out double flux, out bool error)
        {
            // ****** Itt az 1e-27 csak akkor jó, ha a fénysebesség m/s-ban van!
            // -27 = -17 - 10   // SDSS egység + AA / m átváltás
            flux = Util.Integral.Integrate(spectrum.Spectral_Value, spectrum.Flux_Value, fx[f], fy[f], out error);
            flux = flux / Constants.LightSpeed * 1e-27;

            //switch (system)
            //{
            //    case MagnitudeSystem.ABMagnitude:
            //        flux = -2.5 * Math.Log10(flux) - 48.6;
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}

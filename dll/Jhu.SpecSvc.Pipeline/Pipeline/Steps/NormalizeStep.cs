using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline.Steps
{
    public class NormalizeStep : PipelineStep
    {
        public enum NormalizeMethod
        {
            FluxAtWavelength,
            FluxMedianInRanges,
            FluxIntegralInRanges,
        }

        public enum NormalizeTemplate
        {
            Galaxy,
            Qso
        }

        public static readonly double[] limitsStart_galaxy = { 4250, 4600, 5400, 5600 };
        public static readonly double[] limitsEnd_galaxy = { 4300, 4800, 5500, 5800 };
        public static readonly double[] limitsStart_qso = { 1430, 2150, 3020, 4150 };
        public static readonly double[] limitsEnd_qso = { 1480, 2230, 3100, 4250 };
        public const double power_galaxy = 1.0;
        public const double factor_galaxy = 1.0;
        public const double power_qso = 1.4;
        public const double factor_qso = 1e-4;

        private NormalizeMethod method;
        private DoubleParam wavelength;
        private DoubleParam flux;
        private NormalizeTemplate template;
        private bool weight;
        private DoubleArrayParam limitsStart;
        private DoubleArrayParam limitsEnd;
        private double factor;
        private double power;

        public NormalizeMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public DoubleParam Wavelength
        {
            get { return this.wavelength; }
            set { this.wavelength = value; }
        }

        public DoubleParam Flux
        {
            get { return this.flux; }
            set { this.flux = value; }
        }

        [XmlIgnore]
        public NormalizeTemplate Template
        {
            get { return this.template; }
            set
            {
                this.template = value;

                switch (this.template)
                {
                    case NormalizeTemplate.Galaxy:
                        this.limitsStart = (DoubleArrayParam)limitsStart_galaxy;
                        this.limitsEnd = (DoubleArrayParam)limitsEnd_galaxy;
                        this.power = power_galaxy;
                        this.factor = factor_galaxy;
                        this.weight = false;
                        break;
                    case NormalizeTemplate.Qso:
                        this.limitsStart = (DoubleArrayParam)limitsStart_qso;
                        this.limitsEnd = (DoubleArrayParam)limitsEnd_qso;
                        this.power = power_qso;
                        this.factor = factor_qso;
                        this.weight = true;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                flux.Value = 0;
                for (int i = 0; i < limitsStart.Value.Length; i++)
                {
                    flux.Value += LimitsEnd.Value[i] - limitsStart.Value[i];
                }
            }
        }

        public bool Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }

        public DoubleArrayParam LimitsStart
        {
            get { return this.limitsStart; }
            set { this.limitsStart = value; }
        }

        public DoubleArrayParam LimitsEnd
        {
            get { return this.limitsEnd; }
            set { this.limitsEnd = value; }
        }

        public double Factor
        {
            get { return this.factor; }
            set { this.factor = value; }
        }

        public double Power
        {
            get { return this.power; }
            set { this.power = value; }
        }


        public override string Title
        {
            get { return StepDescriptions.NormalizeTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.NormalizeDescription; }
        }

        public NormalizeStep()
        {
            InitializeMembers();
        }

        public NormalizeStep(NormalizeStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = NormalizeMethod.FluxAtWavelength;
            this.wavelength = new DoubleParam(4000.0, "A");
            this.flux = new DoubleParam(1.0, "ADU");
            this.template = NormalizeTemplate.Galaxy;
            this.limitsStart = new DoubleArrayParam(true);
            this.limitsEnd = new DoubleArrayParam(true);
            this.factor = 1.0;
            this.power = 1.0;
        }

        private void CopyMembers(NormalizeStep old)
        {
            this.method = old.method;
            this.wavelength = new DoubleParam(old.wavelength);
            this.template = old.template;
            this.limitsStart = old.limitsStart == null ? null : new DoubleArrayParam(old.limitsStart);
            this.limitsEnd = old.limitsEnd == null ? null : new DoubleArrayParam(old.limitsEnd);
            this.factor = old.factor;
            this.power = old.power;
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            switch (method)
            {
                case NormalizeMethod.FluxMedianInRanges:
                    spectrum.Normalize_Median(limitsStart.Value, limitsEnd.Value, flux.Value, weight, power, factor);
                    break;
                case NormalizeMethod.FluxAtWavelength:
                    spectrum.Normalize(wavelength.Value, flux.Value);
                    break;
                case NormalizeMethod.FluxIntegralInRanges:
                    spectrum.Normalize_Integral(limitsStart.Value, limitsEnd.Value, flux.Value);
                    break;
            }

            return spectrum;
        }
    }
}

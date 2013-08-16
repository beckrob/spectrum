using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Pipeline
{
    public class SpectralIndexStep : PipelineStep
    {
        private long mask;

        public long Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public override string Title
        {
            get { return StepDescriptions.IndexTitle; }
        }

        public override string Description
        {
            get { return StepDescriptions.IndexDescription; }
        }

        public SpectralIndexStep()
        {
            InitializeMembers();
        }

        public SpectralIndexStep(SpectralIndexStep old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(SpectralIndexStep old)
        {
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            double redshift = spectrum.Derived.Redshift.Value.Value;

            // Create mask vector
            bool[] nmask = new bool[spectrum.Flux_Accuracy_Quality.Length];
            for (int i = 0; i < nmask.Length; i++)
            {
                nmask[i] = (spectrum.Flux_Accuracy_Quality[i] & mask) != 0;
            }

            //
            spectrum.SpectralIndices = new SpectralIndices();
            spectrum.SpectralIndices.Name = new string[Constants.SpectralIndexDefinitions.Length];
            spectrum.SpectralIndices.Value = new double[Constants.SpectralIndexDefinitions.Length];

            for (int i = 0; i < Constants.SpectralIndexDefinitions.Length; i++)
            {
                SpectralIndexDefinition d = Constants.SpectralIndexDefinitions[i];

                d.Redshift(redshift);

                double bluecenter = 0.5 * (d.BlueWaveStop + d.BlueWaveStart);
                double blue = Util.Integral.Integrate(
                    spectrum.Spectral_Accuracy_BinLow,
                    spectrum.Spectral_Accuracy_BinHigh,
                    spectrum.Flux_Value,
                    d.BlueWaveStart, d.BlueWaveStop) / (d.BlueWaveStop - d.BlueWaveStart);

                double redcenter = 0.5 * (d.RedWaveStop + d.RedWaveStart);
                double red = Util.Integral.Integrate(
                    spectrum.Spectral_Accuracy_BinLow,
                    spectrum.Spectral_Accuracy_BinHigh,
                    spectrum.Flux_Value,
                    d.RedWaveStart, d.RedWaveStop) / (d.RedWaveStop - d.RedWaveStart);


                // Formulas from:
                // Worthey et al., 1994, ApJS, 94, 687 
                // Bruzual, 1983, ApJ, 273, 105

                double v = 0.0;
                switch (d.Unit)
                {
                    case SpectralIndexUnit.EW:
                        v = (d.IndexWaveStop - d.IndexWaveStart) -
                            Util.Integral.Integrate(spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, spectrum.Flux_Value, d.IndexWaveStart, d.IndexWaveStop,
                                x => 1.0 / (blue + (x - bluecenter) * (red - blue) / (redcenter-bluecenter)));
                        break;
                    case SpectralIndexUnit.Mag:
                        v = -2.5 * Math.Log10(1.0 / (d.IndexWaveStop - d.IndexWaveStart) *
                            Util.Integral.Integrate(spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, spectrum.Flux_Value, d.IndexWaveStart, d.IndexWaveStop,
                                x => 1.0 / (blue + (x - bluecenter) * (red - blue) / (redcenter-bluecenter))));
                        break;
                    case SpectralIndexUnit.Ratio:
                        double[] fl2;
                        Util.Vector.Weight(spectrum.Spectral_Value, spectrum.Flux_Value, out fl2);
                        Util.Vector.Weight(spectrum.Spectral_Value, fl2, out fl2);

                        v = (d.BlueWaveStop - d.BlueWaveStart) / (d.RedWaveStop - d.RedWaveStart) *
                            Util.Integral.Integrate(spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, fl2, d.RedWaveStart, d.RedWaveStop) /
                            Util.Integral.Integrate(spectrum.Spectral_Accuracy_BinLow, spectrum.Spectral_Accuracy_BinHigh, fl2, d.BlueWaveStart, d.BlueWaveStop);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                spectrum.SpectralIndices.Name[i] = d.Name;
                spectrum.SpectralIndices.Value[i] = v;
            }

            return spectrum;
        }
    }
}

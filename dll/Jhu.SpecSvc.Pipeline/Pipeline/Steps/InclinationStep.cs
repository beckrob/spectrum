using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.SpectrumLib;

/*
Hi Tamas

The log^4 model seems to work quite well for individual wavelengths. I
have some plots if you want to check them out. So we have:

f_lambda(b/a = 1) = f_lambda(b/a) * s(lambda, b/a)

where

s(lambda, b/a) = 10^{ 0.4 * eta_lambda * [log10(b/a)]^4 }

and

eta_lambda = ( Sum_{j=0}^{j=3} a_j nu^j ) / [log10(0.17)]^4

Table 4 of the attached ms.dvi lists the values of a_j, and nu is lambda
in inverse micron. I used "expab_r" as a proxy for b/a.

cheers
Ching-Wa
 
 
 * coeffs
 *   0   -0.554 +/- 0.014
 *   1    0.564 +/- 0.023
 *   2   -0.057 +/- 0.012
 *   3    0.003 +/- 0.002
 */

namespace Jhu.SpecSvc.Pipeline
{
    public class InclinationStep : ProcessStep
    {
        public static readonly double[] coeff = { -0.554, 0.564, -0.057, 0.003 };

        private DoubleParam inclination;
        private DoubleParam m04log10inc4norm;

        public DoubleParam Inclination
        {
            get { return this.inclination; }
            set 
            { 
                this.inclination = value;

                this.m04log10inc4norm = new DoubleParam
                    (-0.4 * Math.Pow(Math.Log10(value.Value), 4) 
                          / Math.Pow(Math.Log10(0.17), 4), 
                          "");
            }
        }

        public DoubleParam M04Log10Inc4Norm
        {
            get { return m04log10inc4norm; }
        }

        public override string Title
        {
            get { return "Inclination"; }
        }

        public InclinationStep()
        {
            InitializeMembers();
        }

        public InclinationStep(InclinationStep old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.inclination = new DoubleParam(true);
            this.m04log10inc4norm = new DoubleParam(true);
        }

        private void CopyMembers(InclinationStep old)
        {
            this.inclination = new DoubleParam(old.inclination);
            this.m04log10inc4norm = new DoubleParam(old.m04log10inc4norm);
        }

        protected override Spectrum Execute(Spectrum spectrum)
        {
            // 1 angstrom = 1e-4 micron
            // nu shd be in inverse micron
            double c = 1e4;

            for (int i = 0; i < spectrum.Flux_Value.Length; i++)
            {
                double nu = c / spectrum.Spectral_Value[i];
                double eta = 0;
                double q = 1;

                for (int j = 0; j < coeff.Length; j++)
                {
                    eta += coeff[j] * q; // Math.Pow(nu, j);
                    q *= nu;
                }

                spectrum.Flux_Value[i] *= Math.Pow(10, eta * this.m04log10inc4norm.Value);
            }

            return spectrum;
        }
    }
}

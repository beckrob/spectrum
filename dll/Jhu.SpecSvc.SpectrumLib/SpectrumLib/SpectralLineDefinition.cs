using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public struct SpectralLineDefinition
    {
        public string Name;
        public string NameLatex;
        public double Wavelength;
        public bool Emission;

        public SpectralLineDefinition(string name, string nameLatex, double wavelength)
            :this(name, nameLatex, wavelength, false)
        {
        }

        public SpectralLineDefinition(string name, string nameLatex, double wavelength, bool emission)
        {
            this.Name = name;
            this.NameLatex = nameLatex;
            this.Wavelength = wavelength;
            this.Emission = emission;
        }

        public void Redshift(double redshift)
        {
            Wavelength *= (1 + redshift);
        }
    }
}

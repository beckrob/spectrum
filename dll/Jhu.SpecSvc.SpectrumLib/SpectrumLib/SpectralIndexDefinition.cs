using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public enum SpectralIndexUnit
    {
        EW,
        Mag,
        Ratio
    }

    public struct SpectralIndexDefinition
    {
        public string Name;
        public string NameLatex;
        public double IndexWaveStart;
        public double IndexWaveStop;
        public double BlueWaveStart;
        public double BlueWaveStop;
        public double RedWaveStart;
        public double RedWaveStop;
        public SpectralIndexUnit Unit;

        public SpectralIndexDefinition(string name, string nameLatex, double indexWaveStart, double indexWaveStop, double blueWaveStart, double blueWaveStop, double redWaveStart, double redWaveStop, SpectralIndexUnit unit)
        {
            this.Name = name;
            this.NameLatex = nameLatex;
            this.IndexWaveStart = indexWaveStart;
            this.IndexWaveStop = indexWaveStop;
            this.BlueWaveStart = blueWaveStart;
            this.BlueWaveStop = blueWaveStop;
            this.RedWaveStart = redWaveStart;
            this.RedWaveStop = redWaveStop;
            this.Unit = unit;
        }

        public void Redshift(double redshift)
        {
            IndexWaveStart *= (1 + redshift);
            IndexWaveStop *= (1 + redshift);
            BlueWaveStart *= (1 + redshift);
            BlueWaveStop *= (1 + redshift);
            RedWaveStart *= (1 + redshift);
            RedWaveStop *= (1 + redshift);
        }
    }
}

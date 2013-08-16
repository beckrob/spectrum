using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Visualizer
{
    [Serializable]
    public class SpectrumPlotParameters : PlotParameters
    {
        private string[] dataArrays;
        private bool plotError;
        private bool plotSpectralLines;
        private string[] lineLabels;
        private double[] lineWavelengths;

        public string[] DataArrays
        {
            get { return dataArrays; }
            set { dataArrays = value; }
        }

        public bool PlotError
        {
            get { return plotError; }
            set { plotError = value; }
        }

        public bool PlotSpectralLines
        {
            get { return plotSpectralLines; }
            set { plotSpectralLines = value; }
        }

        public string[] LineLabels
        {
            get { return lineLabels; }
            set { lineLabels = value; }
        }

        public double[] LineWavelengths
        {
            get { return lineWavelengths; }
            set { lineWavelengths = value; }
        }

        public SpectrumPlotParameters()
            : base()
        {
            InitializeMembers();
        }

        public SpectrumPlotParameters(SpectrumPlotParameters old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.dataArrays = new string[] { "Flux_Value" };
            this.plotError = false;
            this.plotSpectralLines = false;
            this.lineLabels = null;
            this.lineWavelengths = null;
        }

        private void CopyMembers(SpectrumPlotParameters old)
        {
            this.dataArrays = old.dataArrays;   //***array copy
            this.plotError = old.plotError;
            this.plotSpectralLines = old.plotSpectralLines;
            this.lineLabels = old.lineLabels;
            this.lineWavelengths = old.lineWavelengths;
        }
    }
}
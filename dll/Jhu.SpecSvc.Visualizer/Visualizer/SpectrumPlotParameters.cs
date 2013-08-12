#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Visualizer classes are for plotting spectra on
 * the webpage
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumGraphParameters.cs,v 1.1 2008/01/08 22:53:59 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:53:59 $
 */
#endregion
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
        private string[] lineTitles;
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

        public string[] LineTitles
        {
            get { return lineTitles; }
            set { lineTitles = value; }
        }

        public double[] LineWavelengths
        {
            get { return lineWavelengths; }
            set { lineWavelengths = value; }
        }

        public SpectrumPlotParameters()
            :base()
        {
            InitializeMembers();
        }

        public SpectrumPlotParameters(SpectrumPlotParameters old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.dataArrays = new string[] { "Flux_Value" };
            this.plotError = false;

            this.plotSpectralLines = false;
            this.lineTitles = null;
            this.lineWavelengths = null;
        }

        private void CopyMembers(SpectrumPlotParameters old)
        {
            this.dataArrays = old.dataArrays;   //***array copy
            this.plotError = old.plotError;
            this.plotSpectralLines = old.plotSpectralLines;
            this.lineTitles = old.lineTitles;
            this.lineWavelengths = old.lineWavelengths;
        }
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumGraphParameters.cs,v $
        Revision 1.1  2008/01/08 22:53:59  dobos
        Initial checkin


*/
#endregion
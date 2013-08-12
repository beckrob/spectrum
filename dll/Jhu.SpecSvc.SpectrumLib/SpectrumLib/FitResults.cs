#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: FitResults.cs,v 1.1 2008/01/08 21:36:55 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:55 $
 */
#endregion
using System;

namespace VoServices.SpecSvc.Lib
{
	/// <summary>
	/// Summary description for FitResults.
	/// </summary>
	public class FitResults
	{
        public Spectrum Spectrum;
        //public Spectrum Original;

		//public Spectrum Continuum;
		public string[] TemplateNames;
		public double[] TemplateCoeffs;
		public double[][] TemplateCov;
		public double ContinuumChiSquare;
		public int ContinuumNdf;
		public double ContinuumRegressionCoeff;

		//public Spectrum Difference;
		//public Spectrum Linefit;

        public bool[] LineDetected;
		public string[] LineNames;
        public double[] LineLabWavelength;
		public double[] LineWavelength;
        public double[] LineWavelengthError;
		public double[] LineAmplitude;
		public double[] LineAmplitudeError;
		public double[] LineSigma;
		public double[] LineSigmaError;
		public double[] LineEW;
        public double[] LineEWError;

		public double VDisp;
		public double VDispError;

		public double LineChiSquare;
		public int    LineNdf;

	}
}
#region Revision History
/* Revision History

        $Log: FitResults.cs,v $
        Revision 1.1  2008/01/08 21:36:55  dobos
        Initial checkin


*/
#endregion
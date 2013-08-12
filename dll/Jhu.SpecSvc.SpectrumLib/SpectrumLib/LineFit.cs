using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public enum LineModel
    {
        Gaussian = 1,
        DoubleGaussian = 2,
        DoubleGaussianWithOffset = 3,
        SkewGaussian
    }

    public struct LineParameters
    {
        public bool ExceptionFitting;
        public bool Detected;
        public LineModel Model;
        public string Name;
        public double LabWavelength;
        public double Wavelength;
        public double WavelengthError;
        public double Wavelength2;
        public double Wavelength2Error;
        public double Amplitude;
        public double AmplitudeError;
        public double Amplitude2;
        public double Amplitude2Error;
        public double Sigma;
        public double SigmaError;
        public double Sigma2;
        public double Sigma2Error;
        public double Skew;
        public double LineVDisp;
        public double LineVDispError;
        public double EqWidth;
        public double EqWidthError;
        public double LineFitChi2;
        public double Snr;
    }

    public class LineFit
    {
        public LineParameters[] Lines;

        public double VDisp;
        public double VDispError;

        public double[] LineModel;

        public double Chi2;
        public int Ndf;
    }
}

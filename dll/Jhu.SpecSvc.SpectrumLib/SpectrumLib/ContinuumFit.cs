using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.SpectrumLib
{
    public class ContinuumFit
    {
        public string[] TemplateNames;
        public double[] Coeffs;
        public double[][] Covariance;
        public double Chi2;
        public int Ndf;
        public double RegressionCoeff;

        public double[] Continuum;
        public double[] Residual;

        public double VDisp;
        public double VDispError;
        public double Tau_V;
        public double Tau_VError;
        public double Mu;
        public double MuError;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public abstract class FunctionOptimizationProblem<F>
    {
        protected F function;

        protected double[] parameters;
        protected double[] error;
        protected double[] scale;
        protected double[] minLimits;
        protected double[] maxLimits;

        protected int maxIterations;
        protected double diffstep;
        protected double stepMax;
        protected double epsg;
        protected double epsf;
        protected double epsx;
        protected double epsi;

        public F Function
        {
            get { return function; }
            set { function = value; }
        }

        public double[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public double[] Error
        {
            get { return error; }
            set { error = value; }
        }

        public double[] Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public double[] MinLimits
        {
            get { return minLimits; }
            set { minLimits = value; }
        }

        public double[] MaxLimits
        {
            get { return maxLimits; }
            set { maxLimits = value; }
        }

        public int MaxIterations
        {
            get { return maxIterations; }
            set { maxIterations = value; }
        }

        public double Diffstep
        {
            get { return diffstep; }
            set { diffstep = value; }
        }

        public double StepMax
        {
            get { return stepMax; }
            set { stepMax = value; }
        }

        public double Epsg
        {
            get { return epsg; }
            set { epsg = value; }
        }

        public double Epsf
        {
            get { return epsf; }
            set { epsf = value; }
        }

        public double Epsx
        {
            get { return epsx; }
            set { epsx = value; }
        }

        public double Epsi
        {
            get { return epsi; }
            set { epsi = value; }
        }

        public FunctionOptimizationProblem()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.maxIterations = 200;
            this.diffstep = 1e-7;
            this.stepMax = double.NaN;
            this.epsg = 1e-7;
            this.epsf = 1e-7;
            this.epsx = 1e-7;
            this.epsi = 1e-7;
        }

        public abstract StopCriterium Optimize(FunctionMinimizer min);

        public abstract double[,] GetQuadratic();

        public abstract double[] GetErrors();

    }
}

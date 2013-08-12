using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Visualizer
{
    [Serializable]
    public abstract class PlotParameters
    {
        private float width;
        private float height;
        private bool legend;
        private double xMin, xMax;
        private double yMin, yMax;
        private bool xLogScale;
        private bool yLogScale;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool Legend
        {
            get { return legend; }
            set { legend = value; }
        }

        public double XMin
        {
            get { return xMin; }
            set { xMin = value; }
        }

        public double XMax
        {
            get { return xMax; }
            set { xMax = value; }
        }

        public double YMin
        {
            get { return yMin; }
            set { xMin = value; }
        }

        public double YMax
        {
            get { return yMax; }
            set { yMax = value; }
        }

        public bool XLogScale
        {
            get { return xLogScale; }
            set { xLogScale = value; }
        }

        public bool YLogScale
        {
            get { return yLogScale; }
            set { yLogScale = value; }
        }

        public PlotParameters()
        {
            InitializeMembers();
        }

        public PlotParameters(PlotParameters old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.width = 640;
            this.height = 480;
            this.legend = true;
            this.xMin = -1;
            this.xMax = -1;
            this.yMin = -1;
            this.yMax = -1;
            this.xLogScale = false;
            this.yLogScale = false;
        }

        private void CopyMembers(PlotParameters old)
        {
            this.width = old.width;
            this.height = old.height;
            this.legend = old.legend;
            this.xMin = old.xMin;
            this.xMax = old.xMax;
            this.yMin = old.yMin;
            this.yMax = old.yMax;
            this.xLogScale = old.xLogScale;
            this.yLogScale = old.yLogScale;
        }
    }
}

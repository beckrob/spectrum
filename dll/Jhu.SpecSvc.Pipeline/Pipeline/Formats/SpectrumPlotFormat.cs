using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Jhu.SpecSvc.Visualizer;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class SpectrumPlotFormat : FileOutputFormat
    {
        public enum ImageFileFormat
        {
            Jpeg,
            Gif,
            Png,
            Ps
        }

        private ImageFileFormat imageFormat;
        private SpectrumPlotParameters plotParameters;

        public ImageFileFormat ImageFormat
        {
            get { return imageFormat; }
            set { imageFormat = value; }
        }

        public SpectrumPlotParameters PlotParameters
        {
            get { return plotParameters; }
            set { plotParameters = value; }
        }

        public override string Title
        {
            get { return FormatDescriptions.SpectrumPlotTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.SpectrumPlotDescription; }
        }

        public SpectrumPlotFormat()
        {
            InitializeMembers();
        }

        public SpectrumPlotFormat(SpectrumPlotFormat old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.imageFormat = ImageFileFormat.Gif;
            this.plotParameters = new SpectrumPlotParameters();
        }

        private void CopyMembers(SpectrumPlotFormat old)
        {
            this.imageFormat = old.imageFormat;
            this.plotParameters = new SpectrumPlotParameters(old.plotParameters);
        }

        protected override void Execute(Jhu.SpecSvc.SpectrumLib.Spectrum spectrum, Stream output, out string filename)
        {
            Jhu.SpecSvc.Visualizer.SpectrumVisualizer vis = new Jhu.SpecSvc.Visualizer.SpectrumVisualizer();

            vis.PlotSpectraGraph(plotParameters, spectrum).Save(output, System.Drawing.Imaging.ImageFormat.Gif);

            filename = GetFilenameFromId(spectrum, true, ".gif");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Pipeline.Formats
{
    public class LineFitFormat : TabularFileOutputFormat
    {
        public override string Title
        {
            get { return FormatDescriptions.LineFitTitle; }
        }

        public override string Description
        {
            get { return FormatDescriptions.LineFitDescription; }
        }

        protected override void OnExecute(SpectrumLib.Spectrum spectrum, System.IO.Stream outputStream, out string filename)
        {
            throw new NotImplementedException();
        }
    }
}

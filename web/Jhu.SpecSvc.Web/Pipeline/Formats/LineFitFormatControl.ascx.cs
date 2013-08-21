using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Web.Pipeline.Formats
{

    public partial class LineFitFormatControl : FileOutputFormatControlBase<LineFitFormat>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnUpdateForm(LineFitFormat format)
        {
            FileFormat.FileType = format.FileType;
            LineEnding.LineEnding = format.LineEnding;
            Prefix.Text = format.Prefix;
        }

        protected override void OnSaveForm(LineFitFormat format)
        {
            format.FileType = FileFormat.FileType;
            format.LineEnding = LineEnding.LineEnding;
            format.Prefix = Prefix.Text;   
        }
    }
}
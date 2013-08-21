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

    public partial class MagnitudeFormatControl : FileOutputFormatControlBase<MagnitudeFormat>
    {
        protected override void OnEnabledChanged()
        {
            
        }

        protected override void OnUpdateForm(MagnitudeFormat format)
        {
            MagnitudeSystem.SelectedValue = format.System.ToString();
            FileFormat.FileType = format.FileType;
            LineEnding.LineEnding = format.LineEnding;
            Prefix.Text = format.Prefix;
        }

        protected override void OnSaveForm(MagnitudeFormat format)
        {
            format.System = (MagnitudeFormat.MagnitudeSystem)Enum.Parse(typeof(MagnitudeFormat.MagnitudeSystem), MagnitudeSystem.SelectedValue);
            format.FileType = FileFormat.FileType;
            format.LineEnding = LineEnding.LineEnding;
            format.Prefix = Prefix.Text;   
        }
    }
}
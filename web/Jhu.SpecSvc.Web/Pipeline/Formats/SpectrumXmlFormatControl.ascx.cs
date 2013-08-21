using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Web.Pipeline.Formats
{
public partial class SpectrumXmlFormatControl : FileOutputFormatControlBase<SpectrumXmlFormat>
{

    protected override void OnEnabledChanged()
    {

    }

    protected override void OnUpdateForm(SpectrumXmlFormat format)
    {
        Prefix.Text = format.Prefix;
    }

    protected override void OnSaveForm(SpectrumXmlFormat format)
    {
        format.Prefix = Prefix.Text;
    }

}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class SpectrumRow : SpectrumDetailsBase, ICheckBoxControl
    {
        public event EventHandler CheckedChanged
        {
            add { SelectionCheckbox.CheckedChanged += value; }
            remove { SelectionCheckbox.CheckedChanged -= value; }
        }

        public bool Checked
        {
            get { return SelectionCheckbox.Checked; }
            set { SelectionCheckbox.Checked = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateControl();
        }

        protected override void UpdateControl()
        {
            base.UpdateControl();
        }
    }
}
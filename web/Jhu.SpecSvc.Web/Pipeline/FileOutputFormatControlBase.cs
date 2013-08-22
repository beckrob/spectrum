using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public abstract class FileOutputFormatControlBase<T> : UserControlBase, IFileOutputFormatControl
        where T : FileOutputFormat, new()
    {
        private T format;

        public bool Enabled
        {
            get { return (bool)(ViewState["Enabled"] ?? true); }
            set
            {
                ViewState["Enabled"] = value;
                OnEnabledChanged();
            }
        }

        public virtual string Title
        {
            get { return format.Title; }
        }

        public T Format
        {
            get
            {
                OnSaveForm(format);
                return format;
            }
            set
            {
                format = value;
                OnUpdateForm(format);
            }
        }

        FileOutputFormat IFileOutputFormatControl.Format
        {
            get { return this.Format; }
            set { this.Format = (T)value; }
        }

        public FileOutputFormatControlBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            //this.step = new T();
        }

        protected abstract void OnEnabledChanged();

        protected abstract void OnUpdateForm(T format);

        protected abstract void OnSaveForm(T format);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OnUpdateForm(format);
            }
            else
            {
                OnSaveForm(format);
            }
        }
    }
}
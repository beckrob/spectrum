using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Web.Pipeline
{
    public abstract class PipelineStepControlBase<T> : UserControlBase, IPipelineStepControl
        where T : PipelineStep, new()
    {
        private T step;

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
            get { return step.Title; }
        }

        public T Step
        {
            get
            {
                OnSaveForm(step);
                return step;
            }
            set
            {
                step = value;
                OnUpdateForm(step);
            }
        }

        PipelineStep IPipelineStepControl.Step
        {
            get { return this.Step; }
            set { this.Step = (T)value; }
        }

        public PipelineStepControlBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            //this.step = new T();
        }

        protected abstract void OnEnabledChanged();

        protected abstract void OnUpdateForm(T step);

        protected abstract void OnSaveForm(T step);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OnUpdateForm(step);
            }
            else
            {
                OnSaveForm(step);
            }
        }
    }
}
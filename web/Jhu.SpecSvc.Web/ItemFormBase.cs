using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jhu.SpecSvc.Web
{
    public abstract class ItemFormBase<T> : PageBase
        where T : new()
    {
        private T item;

        public T Item
        {
            get { return item; }
        }

        public ItemFormRequestMethod Method
        {
            get { return (ItemFormRequestMethod)Enum.Parse(typeof(ItemFormRequestMethod), Request.QueryString["method"]); }
        }

        public string[] Ids
        {
            get { return ((string)Request.QueryString["id"]).Split(','); }
        }

        public ItemFormBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.item = default(T);
        }

        protected abstract T OnLoadItem();

        protected abstract void OnCreateItem();

        protected abstract void OnModifyItem();

        protected abstract void OnDeleteItem();

        protected abstract void OnUpdateForm();

        protected abstract void OnSaveForm();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            switch (Method)
            {
                case ItemFormRequestMethod.Create:
                    item = new T();
                    break;
                case ItemFormRequestMethod.Modify:
                case ItemFormRequestMethod.Delete:
                    item = OnLoadItem();
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!IsPostBack)
            {
                OnUpdateForm();
            }
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                OnSaveForm();
                switch (Method)
                {
                    case ItemFormRequestMethod.Create:
                        OnCreateItem();
                        break;
                    case ItemFormRequestMethod.Modify:
                        OnModifyItem();
                        break;
                    case ItemFormRequestMethod.Delete:
                        OnDeleteItem();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            Response.Redirect(OriginalReferer);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(OriginalReferer);
        }
    }
}
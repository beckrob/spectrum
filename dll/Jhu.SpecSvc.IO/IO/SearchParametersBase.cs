using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace Jhu.SpecSvc.IO
{
    public abstract class SearchParametersBase
    {
        private Guid userGuid;
        private Collection[] collections;
        private bool loadPoints;
        private string[] pointsMask;
        private bool loadDetails;

        public virtual SearchMethod Type
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Guid UserGuid
        {
            get { return this.userGuid; }
            set { this.userGuid = value; }
        }

        public Collection[] Collections
        {
            get { return collections; }
            set { collections = value; }
        }

        public bool LoadPoints
        {
            get { return this.loadPoints; }
            set { this.loadPoints = value; }
        }

        public string[] PointsMask
        {
            get { return this.pointsMask; }
            set { this.pointsMask = value; }
        }

        public bool LoadDetails
        {
            get { return this.loadDetails; }
            set { this.loadDetails = value; }
        }

        public SearchParametersBase()
        {
        }

        public SearchParametersBase(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public SearchParametersBase(SearchParametersBase old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.userGuid = Guid.Empty;
            this.collections = null;
            this.loadPoints = false;
            this.pointsMask = null;
            this.loadDetails = false;
        }

        protected void CopyMembers(SearchParametersBase old)
        {
            this.userGuid = old.userGuid;
            this.collections = old.collections;
            this.loadPoints = old.loadPoints;
            this.loadDetails = old.loadDetails;
        }
    }
}
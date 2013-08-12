using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public abstract class IOObjectBase
    {
        protected PortalConnector connector;

        public PortalConnector Connector
        {
            get { return connector; }
            set { connector = value; }
        }

        public IOObjectBase()
        {
            InitializeMembers();
        }

        public IOObjectBase(PortalConnector connector)
        {
            InitializeMembers();

            this.connector = connector;
        }

        public IOObjectBase(IOObjectBase old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.connector = null;
        }

        private void CopyMembers(IOObjectBase old)
        {
            this.connector = old.connector;
        }
    }
}

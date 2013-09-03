using System;
using System.Xml.Serialization;
using System.Data;

namespace Jhu.SpecSvc.IO
{
	/// <summary>
	/// Summary description for UserFolder.
	/// </summary>
	public class UserFolder
	{
        private int id;
        private Guid userGuid;
        private string name;
        private string publisherId;
        private int count;

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        [XmlIgnore]
        public Guid UserGuid
        {
            get { return this.userGuid; }
            set { this.userGuid = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string PublisherId
        {
            get { return this.publisherId; }
            set { this.publisherId = value; }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public UserFolder()
        {
            InitializeMembers();
        }

        public UserFolder(UserFolder old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.userGuid = Guid.Empty;
            this.name = String.Empty;
            this.publisherId = String.Empty;
            this.count = 0;
        }

        private void CopyMembers(UserFolder old)
        {
            this.id = old.id;
            this.userGuid = old.userGuid;
            this.name = old.name;
            this.publisherId = old.publisherId;
            this.count = old.count;
        }
	}
}
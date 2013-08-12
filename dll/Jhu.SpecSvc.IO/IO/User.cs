
using System;
using System.Data;
using System.Data.SqlClient;

namespace Jhu.SpecSvc.IO
{
    public class User : IOObjectBase
    {
        private Guid guid;
        private int groupId;
        private string username;
        private string password;
        private string name;
        private string institute;
        private string email;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Institute
        {
            get { return institute; }
            set { institute = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public User()
            : base()
        {
            InitializeMembers();
        }

        public User(PortalConnector connector)
            : base(connector)
        {
            InitializeMembers();
        }

        public User(User old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.guid = Guid.Empty;
            this.groupId = 0;
            this.username = string.Empty;
            this.password = string.Empty;
            this.name = string.Empty;
            this.institute = string.Empty;
            this.email = string.Empty;
        }

        private void CopyMembers(User old)
        {
        }

        public bool Login()
        {
            using (SqlCommand cmd = new SqlCommand("sp_LoginUser", connector.DatabaseConnection, connector.DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return false;
                    else
                    {
                        LoadFromDataReader(dr);
                        return true;
                    }
                }
            }
        }

        public void LoadFromDataReader(SqlDataReader dr)
        {
            int o = -1;
            Guid = dr.GetGuid(++o);
            GroupId = dr.GetInt32(++o);
            Username = dr.GetString(++o);
            Password = dr.GetString(++o);
            Name = dr.GetString(++o);
            Institute = dr.GetString(++o);
            Email = dr.GetString(++o);
        }

        public void Load()
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetUser", connector.DatabaseConnection, connector.DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = Guid;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    LoadFromDataReader(dr);
                }
            }
        }

        public void Load(string username)
        {
            // used for lost password mailing
            using (SqlCommand cmd = new SqlCommand("sp_GetUserByUsername", connector.DatabaseConnection, connector.DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    LoadFromDataReader(dr);
                }
            }
        }

        public void Save()
        {
            if (Guid == Guid.Empty)
            {
                Create();
            }
            else
            {
                Modify();
            }
        }

        private void Create()
        {
            using (SqlCommand cmd = new SqlCommand("sp_CreateUser", connector.DatabaseConnection, connector.DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AppendCreateModifyParameters(cmd);
                cmd.Parameters.Add("@NewGUID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                Guid = (Guid)cmd.Parameters["@NewGUID"].Value;
            }
        }

        private void Modify()
        {
            using (SqlCommand cmd = new SqlCommand("sp_ModifyUser", connector.DatabaseConnection, connector.DatabaseTransaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = Guid;
                AppendCreateModifyParameters(cmd);

                cmd.ExecuteNonQuery();
            }
        }

        private void AppendCreateModifyParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupId;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = Username;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = Name;
            cmd.Parameters.Add("@Institute", SqlDbType.NVarChar, 50).Value = Institute;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
        }

    }
}
#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Collection.cs,v 1.2 2008/01/10 22:04:00 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/01/10 22:04:00 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
    /// <summary>
    /// Summary description for Collection.
    /// </summary>
    public class Collection
    {
        private string id;					// datasetId
        private Guid userGuid;			// remote WS userGuid
        private CollectionTypes type;
        private int loadDefaults;
        private string name;				// display name
        private string description;		// long description
        private string location;			// long description
        private string connectionString;	// SQL or WS address
        private string graphUrl;			// url points to the graph generator page
        private int @public;				// public user coll

        private List<SearchMethods> searchMethods;

        [XmlElement]
        public string Id
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

        [XmlIgnore]
        public CollectionTypes Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [XmlIgnore]
        public int LoadDefaults
        {
            get { return loadDefaults; }
            set { loadDefaults = value; }
        }

        [XmlElement]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [XmlElement]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [XmlElement]
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        [XmlIgnore]
        public string ConnectionString
        {
            get { return this.connectionString; }
            set { this.connectionString = value; }
        }

        [XmlElement]
        public string GraphUrl
        {
            get { return this.graphUrl; }
            set { this.graphUrl = value; }
        }

        [XmlIgnore]
        public int Public
        {
            get { return this.@public; }
            set { this.@public = value; }
        }

        [XmlIgnore]
        public List<SearchMethods> SearchMethods
        {
            get { return searchMethods; }
            set { searchMethods = value; }
        }


        public Collection()
        {
            InitializeMembers();
        }

        public Collection(Collection old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = string.Empty;
            this.userGuid = Guid.Empty;
            this.type = CollectionTypes.Sql;
            this.loadDefaults = 0;
            this.name = string.Empty;
            this.description = string.Empty;
            this.location = string.Empty;
            this.connectionString = string.Empty;
            this.graphUrl = string.Empty;
            this.@public = 0;
            this.searchMethods = new List<SearchMethods>();
        }

        private void CopyMembers(Collection old)
        {
            this.id = old.id;
            this.userGuid = old.userGuid;
            this.type = old.type;
            this.loadDefaults = old.loadDefaults;
            this.name = old.name;
            this.description = old.description;
            this.location = old.location;
            this.connectionString = old.connectionString;
            this.graphUrl = old.graphUrl;
            this.@public = old.@public;

            this.searchMethods = new List<SearchMethods>(old.searchMethods);
        }


        public bool CheckStatus()
        {
            switch (type)
            {
                case CollectionTypes.Sql:
                    try
                    {
                        SqlConnection cn = new SqlConnection();
                        cn.ConnectionString = ConnectionString;
                        cn.Open();
                        cn.Close();
                        return true;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }
                case CollectionTypes.WebService:
                    /*try
                    {
                        VoServices.SpecSvc.Lib.Remote.search app = new VoServices.SpecSvc.Lib.Remote.search();
                        app.Url = ConnectionString;
                        string[] dummy = app.Revisions();
                        return true;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }*/
                    //****
                    return false;
            }

            return false;
        }

        public ConnectorBase GetConnector()
        {
            switch (Type)
            {
                //case CollectionTypes.LegacySql:
                //return new SqlConnector(this);
                case CollectionTypes.Sql:
                    return new SqlConnector(this);
                case CollectionTypes.WebService:
                    return new WsConnector(this);
                case CollectionTypes.Ssa:
                    return new SsaConnector(this);
                    break;
                case CollectionTypes.Ssap:
                    break;
            }

            return null;
        }

    }

}
#region Revision History
/* Revision History

        $Log: Collection.cs,v $
        Revision 1.2  2008/01/10 22:04:00  dobos
        Multiple search method editing support

        Revision 1.1  2008/01/08 22:00:48  dobos
        Initial checkin


*/
#endregion
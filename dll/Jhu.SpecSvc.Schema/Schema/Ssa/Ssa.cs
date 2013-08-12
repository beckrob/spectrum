#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * Jhu.SpecSvc.Schema classes support the implementation
 * of Virtual Observatory Data Models.
 * Jhu.SpecSvc.Schema.Spectrum implements the spectrum data model
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Ssa.cs,v 1.1 2008/01/08 22:26:59 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:59 $
 */
#endregion
using System;
using System.Collections.Generic;

namespace Jhu.SpecSvc.Schema.Ssa
{
    /// <summary>
    /// Summary description for SsaSedList.
    /// </summary>
    public class Ssa : ICloneable
    {
        public const string Version = "1.04";

        private Query query;
        private Association association;
        private Access access;
        private Dataset dataset;

        private string queryStatus;
        private string serviceProtocol;

        private IEnumerable<Jhu.SpecSvc.Schema.Spectrum.Spectrum> spectra;
        private string getUrl;

        [Field(Required = ParamRequired.Recommended)]
        public Query Query
        {
            get { return this.query; }
            set { this.query = value; }
        }

        [Field(Required = ParamRequired.Optional)]
        public Association Association
        {
            get { return this.association; }
            set { this.association = value; }
        }

        [Field(Required = ParamRequired.Mandatory)]
        public Access Access
        {
            get { return this.access; }
            set { this.access = value; }
        }

        [Field(Required = ParamRequired.Mandatory)]
        public Dataset Dataset
        {
            get { return this.dataset; }
            set { this.dataset = value; }
        }

        public string QueryStatus
        {
            get { return queryStatus; }
            set { queryStatus = value; }
        }

        public string ServiceProtocol
        {
            get { return serviceProtocol; }
            set { serviceProtocol = value; }
        }

        public IEnumerable<Jhu.SpecSvc.Schema.Spectrum.Spectrum> Spectra
        {
            set { spectra = value; }
            get { return spectra; }
        }

        public string GetUrl
        {
            get { return getUrl; }
            set { getUrl = value; }
        }

        #region Constructors
        public Ssa()
        {
            this.InitializeMembers();
        }

        public Ssa(bool initialize)
        {
            if (initialize)
            {
                this.InitializeMembers();
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
            }
        }

        public Ssa(ParamRequired initializationLevel)
        {
            this.InitializeMembers();
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Ssa(Ssa old)
        {
            this.CopyMembers(old);
            SchemaUtil.CopyMembers(this, old);
        }

        public string Score = "1";
        public string AssocType = null;
        public string AssocId = null;
        public string AssocKey = null;
        public string AcRef = "";
        public string Format = "VOTABLE";
        public string DataModel = "";
        public string DataType = "";
        public string DataLength;
        public string DataTimeSI = "";
        public string DataSpectralSI = "";
        public string DataFluxSI = "";

        #endregion

        #region Initialization functions

        private void InitializeMembers()
        {
            this.queryStatus = "OK";
            this.serviceProtocol = "1.0";

            this.spectra = null;
            this.getUrl = "[$ID]";
        }

        private void CopyMembers(Ssa old)
        {
            this.queryStatus = old.queryStatus;
            this.serviceProtocol = old.serviceProtocol;

            this.spectra = old.spectra;
            this.getUrl = old.getUrl;
        }

        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Ssa Clone(Ssa old)
        {
            if (old != null)
            {
                return new Ssa(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Ssa.cs,v $
        Revision 1.1  2008/01/08 22:26:59  dobos
        Initial checkin


*/
#endregion
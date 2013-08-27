#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SkyServerSearchParameters.cs,v 1.2 2008/09/11 10:45:00 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/09/11 10:45:00 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class SkyServerSearchParameters : SearchParametersBase
    {
        private string query;
        private string target;
        private string wsUrl;
        private long wsId;

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public string WsUrl
        {
            get { return wsUrl; }
            set { wsUrl = value; }
        }

        public long WsId
        {
            get { return wsId; }
            set { wsId = value; }
        }

        public SkyServerSearchParameters()
        {
            InitializeMembers();
        }

        public SkyServerSearchParameters(bool initialize)
            : base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public SkyServerSearchParameters(SkyServerSearchParameters old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.query = string.Empty;
            this.target = string.Empty;
            this.wsId = 0;
        }

        private void CopyMembers(SkyServerSearchParameters old)
        {
            this.query = old.query;
            this.target = old.target;
            this.wsId = old.wsId;
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.SkyServer; }
        }

        public SkyServerSearchParameters GetStandardUnits()
        {
            return this;
        }

    }
}
#region Revision History
/* Revision History

        $Log: SkyServerSearchParameters.cs,v $
        Revision 1.2  2008/09/11 10:45:00  dobos
        Bugfixes and parallel execution added to PortalConnector

        Revision 1.1  2008/01/08 22:01:46  dobos
        Initial checkin


*/
#endregion
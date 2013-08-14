#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SsaConnector.cs,v 1.1 2008/01/08 22:01:36 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:36 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    public class SsaConnector : ConnectorBase
    {
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public SsaConnector(Collection collection)
        {
            if (collection.Type != CollectionTypes.Ssa)
                throw new System.Exception("Not valid collection type");

            this.url = collection.ConnectionString;
        }

        public SsaConnector(string url)
        {
            InitializeMembers();

            this.url = url;
        }

        private void InitializeMembers()
        {
            this.url = string.Empty;
        }

        public override IEnumerable<Spectrum> FindSpectrum(ConeSearchParameters par)
        {
            par = par.GetStandardUnits();

            string req = url;

            req += "?REQUEST=queryData";
            req += "&POS=" + par.Pos.Value.Ra.ToString() + "%2C" + par.Pos.Value.Dec.ToString();
            req += "&SIZE=" + (par.Sr.Value / 60).ToString();
            req += "&BAND=";
            req += "&TIME=";
            req += "&COLLECTION=" + par.Collections[0]; ///*****
            req += "&FORMAT=all";

            System.Net.WebRequest w = System.Net.HttpWebRequest.Create(req);
            System.Net.WebResponse res = w.GetResponse();

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(VOTABLE.VOTABLE));
            VOTABLE.VOTABLE vt = (VOTABLE.VOTABLE)ser.Deserialize(res.GetResponseStream());

            Jhu.SpecSvc.Schema.Ssa.Ssa ssa = Jhu.SpecSvc.IO.Mappers.VoTable2Ssa.MapVoTable2Ssa(vt);

            return ssa.Spectra.Cast<Spectrum>();
        }

        public override IEnumerable<Spectrum> FindSpectrum(AdvancedSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(AllSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(FolderSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(HtmRangeSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(IdSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(ModelSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(ObjectSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(RedshiftSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(SimilarSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Spectrum> FindSpectrum(SqlSearchParameters par)
        {
            throw new NotImplementedException();
        }

        public override Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            throw new NotImplementedException();
        }

    }
}
#region Revision History
/* Revision History

        $Log: SsaConnector.cs,v $
        Revision 1.1  2008/01/08 22:01:36  dobos
        Initial checkin


*/
#endregion
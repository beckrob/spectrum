#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Ssa2VoTable.cs,v 1.1 2008/01/08 22:01:38 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:38 $
 */
#endregion
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections.Generic;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Ssa;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.IO.Mappers
{
    public class Ssa2VoTable : VoTableMapper
    {
        private const string NAMESPACE = "ssa";

        public Ssa2VoTable()
        {
        }

        public static VOTABLE.VOTABLE MapSsa2VoTable(Ssa ssa)
        {
            VOTABLE.VOTABLE res = new VOTABLE.VOTABLE();
            res.RESOURCE = new VOTABLE.RESOURCE[1];
            res.RESOURCE[0] = Map(ssa);
            return res;
        }

        protected static VOTABLE.RESOURCE Map(Ssa ssa)
        {
            List<object> l = new List<object>();

            VOTABLE.RESOURCE res = new VOTABLE.RESOURCE();
            res.utype = NAMESPACE + ":" + ssa.GetType().Name;
            res.type = VOTABLE.RESOURCEType.results;

            // ssa base fiedls
            VOTABLE.INFO info = new VOTABLE.INFO();
            info.name = "QUERY_STATUS";
            info.value = ssa.QueryStatus;
            l.Add(info);

            info = new VOTABLE.INFO();
            info.name = "SERVICE_PROTOCOL";
            info.value = ssa.ServiceProtocol;
            info.Value = "SSAP";
            l.Add(info);

            res.Items = l.ToArray();

            VOTABLE.TABLE tab = new VOTABLE.TABLE();
            tab.utype = NAMESPACE + ":" + ssa.GetType().Name;
            res.TABLE = new VOTABLE.TABLE[1];
            res.TABLE[0] = tab;

            List<VOTABLE.FIELD> fields = new List<VOTABLE.FIELD>();
            l = new List<object>();
            bool foundParam;
            object[] objs;


            // Char
            //objs = Characterize((ICharacterization)obj, NAMESPACE + ":" + obj.GetType().Name + ".");
            //l.AddRange(objs);

            // Data
            objs = MapMembers(ssa, ssa, NAMESPACE + ":", false, out foundParam, fields);
            if (foundParam) l.AddRange(objs);

            // Spectrum fields
            objs = MapTypeMembers(typeof(Jhu.SpecSvc.Schema.Spectrum.Spectrum), NAMESPACE + ":", false, out foundParam, fields);
            if (foundParam) l.AddRange(objs);

            foreach (VOTABLE.FIELD field in fields)
                l.Add(field);

            tab.Items = l.ToArray();
            tab.DATA = MapSpectra(ssa, fields);

            return res;
        }

        private static VOTABLE.DATA MapSpectra(Ssa ssa, List<VOTABLE.FIELD> fields)
        {
            VOTABLE.DATA data = new VOTABLE.DATA();
            VOTABLE.TABLEDATA tabdata = new VOTABLE.TABLEDATA();

            if (fields.Count > 0)
            {
                List<VOTABLE.TR> trs = new List<VOTABLE.TR>();

                int sx = 0;
                foreach (Jhu.SpecSvc.Schema.Spectrum.Spectrum spectrum in ssa.Spectra)
                {
                    VOTABLE.TR tr = new VOTABLE.TR();
                    tr.TD = new VOTABLE.TD[fields.Count];

                    int q = 0;
                    foreach (VOTABLE.FIELD field in fields)
                    {
                        tr.TD[q] = new VOTABLE.TD();

                        // disgusting hack
                        switch (field.utype)
                        {
                            case "ssa:Query.Score":
                                tr.TD[q].Value = sx.ToString();
                                break;
                            case "ssa:Association.Type":
                            case "ssa:Association.ID":
                            case "ssa:Association.Key":
                                break;
                            case "ssa:Access.Reference":
                                tr.TD[q].Value = ssa.GetUrl.Replace("[$ID]", System.Web.HttpUtility.UrlEncode(spectrum.Curation.PublisherDID.Value)).Replace("[$Format]", ssa.Format);
                                break;
                            case "ssa:Access.Format":
                                tr.TD[q].Value = ssa.Format;
                                break;
                            case "ssa:Dataset.DataModel":
                                tr.TD[q].Value = spectrum.DataModel.Value;
                                break;
                            case "ssa:Dataset.Type":
                                tr.TD[q].Value = spectrum.Type.Value;
                                break;
                            case "ssa:Dataset.Length":
                                tr.TD[q].Value = ssa.DataLength;
                                break;
                            case "ssa:Dataset.TimeSI":
                                if (spectrum.TimeSI.Value != null && spectrum.TimeSI.Value != string.Empty)
                                    tr.TD[q].Value = new Jhu.SpecSvc.Schema.Units.Unit(spectrum.TimeSI.Value).FormatVOTable();
                                else
                                    tr.TD[q].Value = spectrum.TimeSI.Value;
                                break;
                            case "ssa:Dataset.SpectralSI":
                                if (spectrum.SpectralSI.Value != null && spectrum.SpectralSI.Value != string.Empty)
                                    tr.TD[q].Value = new Jhu.SpecSvc.Schema.Units.Unit(spectrum.SpectralSI.Value).FormatVOTable();
                                else
                                    tr.TD[q].Value = spectrum.SpectralSI.Value;
                                break;
                            case "ssa:Dataset.FluxSI":
                                if (spectrum.FluxSI.Value != null && spectrum.FluxSI.Value != string.Empty)
                                    tr.TD[q].Value = new Jhu.SpecSvc.Schema.Units.Unit(spectrum.FluxSI.Value).FormatVOTable();
                                else
                                    tr.TD[q].Value = spectrum.FluxSI.Value;
                                break;
                            default:
                                object f = Jhu.SpecSvc.SpectrumLib.Spectrum.GetField(spectrum, field.utype.Substring(field.utype.IndexOf(":") + 1), 0, false);
                                ParamBase param = f as ParamBase;
                                if (param != null)
                                {
                                    tr.TD[q].Value = param.ToString();
                                }
                                else
                                {
                                    tr.TD[q].Value = "";
                                }
                                break;
                        }

                        q++;
                    }

                    trs.Add(tr);
                    sx++;
                }

                tabdata.TR = trs.ToArray();
            }

            data.Item = tabdata.TR;
            return data;
        }

    }
}
#region Revision History
/* Revision History

        $Log: Ssa2VoTable.cs,v $
        Revision 1.1  2008/01/08 22:01:38  dobos
        Initial checkin


*/
#endregion
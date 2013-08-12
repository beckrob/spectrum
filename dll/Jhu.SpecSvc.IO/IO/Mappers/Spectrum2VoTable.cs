#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Spectrum2VoTable.cs,v 1.1 2008/01/08 22:01:38 dobos Exp $
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
using Jhu.SpecSvc.Schema.Spectrum;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.IO.Mappers
{
    public class Spectrum2VoTable : VoTableMapper
    {
        private const string NAMESPACE = "spec";

        public Spectrum2VoTable()
        {
        }

        public static VOTABLE.VOTABLE MapSpectrum2VoTable(object spec)
        {
            VOTABLE.VOTABLE res = new VOTABLE.VOTABLE();
            res.RESOURCE = new VOTABLE.RESOURCE[1];
            res.RESOURCE[0] = Map(spec);
            return res;
        }

        protected static VOTABLE.RESOURCE Map(object obj)
        {
            VOTABLE.RESOURCE res = new VOTABLE.RESOURCE();
            res.utype = NAMESPACE + ":" + obj.GetType().Name;

            VOTABLE.TABLE tab = new VOTABLE.TABLE();
            tab.utype = NAMESPACE + ":" + obj.GetType().Name;
            res.TABLE = new VOTABLE.TABLE[1];
            res.TABLE[0] = tab;

            List<VOTABLE.FIELD> fields = new List<VOTABLE.FIELD>();
            List<object> l = new List<object>();
            bool foundParam;

            object[] objs;

            // Char
            objs = Characterize((ICharacterization)obj, NAMESPACE + ":" + obj.GetType().Name + ".");
            l.AddRange(objs);

            // Data
            objs = MapMembers(obj, obj, NAMESPACE + ":", false, out foundParam, fields);
            if (foundParam) l.AddRange(objs);

            foreach (VOTABLE.FIELD field in fields)
                l.Add(field);

            tab.Items = l.ToArray();
            tab.DATA = MapPoints(obj, obj, fields);

            return res;
        }

        private static VOTABLE.DATA MapPoints(object root, object obj, List<VOTABLE.FIELD> fields)
        {
            VOTABLE.DATA data = new VOTABLE.DATA();
            VOTABLE.TABLEDATA tabdata = new VOTABLE.TABLEDATA();

            if (fields.Count > 0)
            {
                int length = ((System.Array)obj.GetType().GetField(fields[0].ID).GetValue(obj)).Length;

                tabdata.TR = new VOTABLE.TR[length];

                for (int i = 0; i < length; i++)
                {
                    tabdata.TR[i] = new VOTABLE.TR();
                    tabdata.TR[i].TD = new VOTABLE.TD[fields.Count];
                }

                int q = 0;
                foreach (VOTABLE.FIELD field in fields)
                {
                    FieldInfo fi = obj.GetType().GetField(field.ID);
                    if (fi.FieldType == typeof(double[]))
                    {
                        double[] val = (double[])fi.GetValue(obj);
                        for (int j = 0; j < val.Length; j++)
                        {
                            tabdata.TR[j].TD[q] = new VOTABLE.TD();
                            tabdata.TR[j].TD[q].Value = val[j].ToString();
                        }
                    }
                    else if (fi.FieldType == typeof(long[]))
                    {
                        long[] val = (long[])fi.GetValue(obj);
                        for (int j = 0; j < val.Length; j++)
                        {
                            tabdata.TR[j].TD[q] = new VOTABLE.TD();
                            tabdata.TR[j].TD[q].Value = val[j].ToString();
                        }
                    }
                    q++;
                }
            }

            data.Item = tabdata.TR;
            return data;
        }


    }
}
#region Revision History
/* Revision History

        $Log: Spectrum2VoTable.cs,v $
        Revision 1.1  2008/01/08 22:01:38  dobos
        Initial checkin


*/
#endregion
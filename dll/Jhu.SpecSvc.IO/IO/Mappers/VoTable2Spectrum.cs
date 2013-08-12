#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: VoTable2Spectrum.cs,v 1.1 2008/01/08 22:01:38 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:38 $
 */
#endregion
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Jhu.SpecSvc.Schema;


namespace Jhu.SpecSvc.IO.Mappers
{
    /// <summary>
    /// Summary description for VoTable2Sed.
    /// </summary>
    public class VoTable2Spectrum
    {
        private const string NAMESPACE = "spec";

        public static Jhu.SpecSvc.SpectrumLib.Spectrum MapVoTable2Spectrum(VOTABLE.VOTABLE votable)
        {
            Jhu.SpecSvc.SpectrumLib.Spectrum spec = new Jhu.SpecSvc.SpectrumLib.Spectrum();
            spec.BasicInitialize();

            MapResource(spec, votable.RESOURCE[0]);

            return spec;
        }

        public static void MapResource(Jhu.SpecSvc.SpectrumLib.Spectrum spec, VOTABLE.RESOURCE res)
        {
            if (res.utype.ToLower() == (NAMESPACE + ":" + spec.GetType().Name).ToLower())
            {
                MapTable(spec, res.TABLE[0]);
            }
        }

        public static void MapTable(Jhu.SpecSvc.SpectrumLib.Spectrum spec, VOTABLE.TABLE tab)
        {
            List<VOTABLE.FIELD> fields = new List<VOTABLE.FIELD>();

            foreach (object item in tab.Items)
            {
                if (item.GetType() == typeof(VOTABLE.GROUP))
                {
                    MapGroup(spec, (VOTABLE.GROUP)item);
                }
                else if (item.GetType() == typeof(VOTABLE.PARAM))
                {
                    MapParam(spec, (VOTABLE.PARAM)item);
                }
                else if (item.GetType() == typeof(VOTABLE.FIELD))
                {
                    MapField(spec, (VOTABLE.FIELD)item);
                    fields.Add((VOTABLE.FIELD)item);
                }
            }

            MapData(spec, tab.DATA, fields);
        }


        public static void MapParam(object obj, VOTABLE.PARAM par)
        {
            try
            {
                Jhu.SpecSvc.Schema.ParamBase prm = (Jhu.SpecSvc.Schema.ParamBase)GetField(obj, par.utype);

                if (par.value != "UNKNOWN")
                    prm.Parse(par.value);
                prm.Unit = par.unit;
                prm.Ucd = par.ucd;
                prm.Name = par.name;
            }
            catch (System.InvalidCastException)
            {
                // field name found but types differ
            }
            catch (System.NullReferenceException)
            {
                // field not found
            }
        }

        private static void MapField(object obj, VOTABLE.FIELD field)
        {
            try
            {
                Jhu.SpecSvc.Schema.ParamBase prm = (Jhu.SpecSvc.Schema.ParamBase)GetField(obj, field.utype);

                prm.Unit = field.unit;
                prm.Ucd = field.ucd;
                prm.Name = field.name;
            }
            catch (System.InvalidCastException)
            {
                // field name found but types differ
            }
            catch (System.NullReferenceException)
            {
                // field not found
            }
        }

        public static void MapGroup(object obj, VOTABLE.GROUP grp)
        {
            try
            {
                Jhu.SpecSvc.Schema.Group group = (Jhu.SpecSvc.Schema.Group)GetField(obj, grp.utype);

                foreach (object item in grp.Items)
                {
                    if (item.GetType() == typeof(VOTABLE.GROUP))
                    {
                        MapGroup(obj, (VOTABLE.GROUP)item);
                    }
                    if (item.GetType() == typeof(VOTABLE.PARAM))
                    {
                        MapParam(obj, (VOTABLE.PARAM)item);
                    }
                }
            }
            catch (System.InvalidCastException)
            {
                // field name found but types differ (Custom group)
                if (grp.utype.ToLower() == "sed:segment.custom")
                {
                    //MapCustomGroup(obj, grp);
                }
            }
            catch (System.NullReferenceException)
            {
                // field not found -> ignore
            }

        }
        /*
		public static void MapCustomGroup(object obj, VOTABLE.GROUP grp)
		{
			if (obj.GetType() == typeof(VoServices.Schema.Sed.Segment))
			{
				VoServices.Schema.Sed.Segment seg = (VoServices.Schema.Sed.Segment) obj;
				seg.Custom = new ParamCollection();

                if (grp.Items != null)
				    foreach (object item in grp.Items)
				    {
					    switch (item.GetType().ToString())
					    {
						    case "VoServices.VOTABLE.GROUP":
							    // ignore
							    //MapGroup(obj, (VOTABLE.GROUP) item);
							    break;
						    case "VoServices.VOTABLE.PARAM":
							    VoServices.VOTABLE.PARAM prm = (VoServices.VOTABLE.PARAM) item;

                                Jhu.SpecSvc.Schema.TextParam newparam = new Jhu.SpecSvc.Schema.TextParam(true);
							    newparam.Ucd = prm.ucd;
							    newparam.Value = prm.value;

							    seg.Custom.Add(prm.utype, newparam);

							    break;
					    }
				    }
			}
		}
		*/

        public static void MapData(Jhu.SpecSvc.SpectrumLib.Spectrum spec, VOTABLE.DATA data, List<VOTABLE.FIELD> fields)
        {
            VOTABLE.TR[] rows = (VOTABLE.TR[])data.Item;

            List<Array> arrays = new List<Array>(fields.Count);

            for (int j = 0; j < fields.Count; j++)
            {
                string refmember = GetReferencedField(spec, fields[j].utype);

                FieldInfo f = spec.GetType().GetField(refmember);
                if (f.FieldType == typeof(double[]))
                {
                    double[] a = new double[rows.Length];
                    f.SetValue(spec, a);
                    arrays.Add(a);
                }
                else if (f.FieldType == typeof(long[]))
                {
                    long[] a = new long[rows.Length];
                    f.SetValue(spec, a);
                    arrays.Add(a);
                }


            }

            for (int i = 0; i < rows.Length; i ++)
            {	
                for (int j = 0; j < arrays.Count; j ++)
                {
                    if (arrays[j].GetType() == typeof(double[]))
                    {
                        arrays[j].SetValue(double.Parse(rows[i].TD[j].Value), i);
                    }
                    else if (arrays[j].GetType() == typeof(long[]))
                    {
                        arrays[j].SetValue(long.Parse(rows[i].TD[j].Value), i);
                    }
                }
            }
        }

        public static object GetField(object obj, string var)
        {
            // 
            string[] names = var.Split(new char[] { '.', ':' });

            object tmp = obj, tmp2;
            // start with 1 because 0 is the xml namespace itself
            int start = 1;

            // characterization fields should be mapped to data
            if (names[start].ToLower() == "char")
                names[start] = "Data";

            //if (names[1].ToLower() == "segment") start = 2; old code
            for (int i = start; i < names.Length; i++)
            {
                tmp2 = null;

                MemberInfo mem = null;
                MemberInfo[] mems = tmp.GetType().GetMember(names[i]);

                if (mems.Length == 0)
                {
                    // querying all interfaces for the member
                    System.Type[] ints = tmp.GetType().GetInterfaces();
                    foreach (System.Type tp in ints)
                    {
                        mems = tp.GetMember(names[i]);
                        if (mems.Length != 0)
                            mem = mems[0];
                    }
                }
                else
                    mem = mems[0];

                if (mem.MemberType == MemberTypes.Field)
                {
                    try
                    {
                        tmp2 = tmp.GetType().GetField(names[i]).GetValue(tmp);
                    }
                    catch (System.Exception)
                    {
                    }

                    if (tmp2 == null)
                    {
                        tmp2 = tmp.GetType().GetField(names[i]).FieldType.GetConstructor(new System.Type[0]).Invoke(new object[0]);
                        tmp.GetType().GetField(names[i]).SetValue(tmp, tmp2);
                    }
                }
                else
                {
                    try
                    {
                        tmp2 = tmp.GetType().GetProperty(names[i]).GetValue(tmp, Type.EmptyTypes);
                    }
                    catch (System.Exception)
                    {
                    }


                    if (tmp2 == null)
                    {
                        tmp2 = tmp.GetType().GetProperty(names[i]).PropertyType.GetConstructor(new System.Type[0]).Invoke(Type.EmptyTypes);
                        tmp.GetType().GetProperty(names[i]).SetValue(tmp, tmp2, Type.EmptyTypes);
                    }
                }
                tmp = tmp2;
            }

            return tmp;
        }

        public static string GetReferencedField(object obj, string var)
        {
            string name = var.Substring(0, var.LastIndexOf('.'));
            object tmp = GetField(obj, name);

            string[] names = var.Split(new char[] { '.', ':' });

            // get field attribute
            MemberInfo[] m = tmp.GetType().GetMember(names[names.Length - 1]);
            if (m != null && m.Length > 0)
            {
                object[] attr = m[0].GetCustomAttributes(typeof(FieldAttribute), true);
                if (attr != null && attr.Length > 0)
                {
                    FieldAttribute fattr = attr[0] as FieldAttribute;
                    if (fattr != null)
                    {
                        return fattr.RefMember;
                    }
                }
            }

            return null;
        }
    }
}
#region Revision History
/* Revision History

        $Log: VoTable2Spectrum.cs,v $
        Revision 1.1  2008/01/08 22:01:38  dobos
        Initial checkin


*/
#endregion
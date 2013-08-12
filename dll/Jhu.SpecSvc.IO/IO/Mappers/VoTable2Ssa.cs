#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: VoTable2Ssa.cs,v 1.1 2008/01/08 22:01:39 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:39 $
 */
#endregion
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


namespace Jhu.SpecSvc.IO.Mappers
{
    /// <summary>
    /// Summary description for VoTable2Sed.
    /// </summary>
    public class VoTable2Ssa
    {
        private const string NAMESPACE = "ssa";

        public static Jhu.SpecSvc.Schema.Ssa.Ssa MapVoTable2Ssa(VOTABLE.VOTABLE votable)
        {
            Jhu.SpecSvc.Schema.Ssa.Ssa ssa = new Jhu.SpecSvc.Schema.Ssa.Ssa();

            MapResource(ssa, votable.RESOURCE[0]);

            return ssa;
        }

        public static void MapResource(Jhu.SpecSvc.Schema.Ssa.Ssa ssa, VOTABLE.RESOURCE res)
        {
            // removed test for services not specifíing a utype
            //if (res.utype.ToLower() == (NAMESPACE + ":" + ssa.GetType().Name).ToLower())
            //{
                MapTable(ssa, res.TABLE[0]);
            //}
        }

        public static void MapTable(Jhu.SpecSvc.Schema.Ssa.Ssa ssa, VOTABLE.TABLE tab)
        {
            List<VOTABLE.FIELD> fields = new List<VOTABLE.FIELD>();

            foreach (object item in tab.Items)
            {
                if (item.GetType() == typeof(VOTABLE.GROUP))
                {
                    MapGroup(ssa, (VOTABLE.GROUP)item);
                }
                else if (item.GetType() == typeof(VOTABLE.PARAM))
                {
                    MapParam(ssa, (VOTABLE.PARAM)item);
                }
                else if (item.GetType() == typeof(VOTABLE.FIELD))
                {
					MapField(ssa, (VOTABLE.FIELD)item);
                    fields.Add((VOTABLE.FIELD)item);
                }
            }

            ssa.Spectra = MapData(ssa, tab.DATA, fields);
        }


        public static void MapParam(object obj, VOTABLE.PARAM par)
        {
            //try
            //{
            Jhu.SpecSvc.Schema.ParamBase prm = (Jhu.SpecSvc.Schema.ParamBase)GetField(obj, par.utype);

            if (par.value != "UNKNOWN")
                prm.Parse(par.value);
            prm.Unit = par.unit;
			prm.Ucd = par.ucd;
			prm.Name = par.name;
            //}
            //catch (System.InvalidCastException)
            //{
            //    // field name found but types differ
            //}
            //catch (System.NullReferenceException)
            //{
            //    // field not found
            //}
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

        public static IEnumerable<Jhu.SpecSvc.Schema.Spectrum.Spectrum> MapData(Jhu.SpecSvc.Schema.Ssa.Ssa ssa, VOTABLE.DATA data, List<VOTABLE.FIELD> fields)
		{
			VOTABLE.TR[] rows = (VOTABLE.TR[]) data.Item;

            foreach (VOTABLE.TR tr in rows)
            {
                Jhu.SpecSvc.SpectrumLib.Spectrum spec = new Jhu.SpecSvc.SpectrumLib.Spectrum();
                spec.BasicInitialize();

                for (int j = 0; j < fields.Count; j++)
                {
                    try
                    {
                        switch (fields[j].utype)
                        {
                            case "ssa:Query.Score":
                            case "ssa:Association.Type":
                            case "ssa:Association.ID":
                            case "ssa:Association.Key":
                                break;
                            case "ssa:Access.Reference":
                                spec.Url = tr.TD[j].Value;
                                break;
                            case "ssa:Access.Format":
                                
                                break;
                            case "ssa:Dataset.DataModel":
                                //spec.DataModel = new Jhu.SpecSvc.Schema.TextParam();
                                spec.DataModel.Value = tr.TD[j].Value;
                                break;
                            case "ssa:Dataset.Type":
                                //spec.Type = new Jhu.SpecSvc.Schema.TextParam();
                                spec.Type.Value = tr.TD[j].Value;
                                break;
                            case "ssa:Dataset.Length":

                                break;
                            case "ssa:Dataset.TimeSI":
                                spec.Data.TimeAxis.Value.Unit = tr.TD[j].Value;
                                break;
                            case "ssa:Dataset.SpectralSI":
                                spec.Data.SpectralAxis.Value.Unit = tr.TD[j].Value;
                                break;
                            case "ssa:Dataset.FluxSI":
                                spec.Data.FluxAxis.Value.Unit = tr.TD[j].Value;
                                break;
                            default:
                                Jhu.SpecSvc.Schema.ParamBase field = (Jhu.SpecSvc.Schema.ParamBase)GetField(spec, fields[j].utype);
                                field.Ucd = fields[j].ucd;
                                field.Unit = fields[j].unit;

                                field.Parse(tr.TD[j].Value);
                                break;
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                }

                // set target.pos from coverage.location or vice versa
                /*
                if (spec.Data != null && spec.Data.SpatialAxis != null && spec.Data.SpatialAxis.Coverage != null && spec.Data.SpatialAxis.Coverage.Location != null && spec.Data.SpatialAxis.Coverage.Location.Value != null)
                {
                    if (spec.Target.Pos.Value.Ra != spec.Data.SpatialAxis.Coverage.Location.Value.Value.Ra ||
                        spec.Target.Pos.Value.Dec != spec.Data.SpatialAxis.Coverage.Location.Value.Value.Dec)
                    {
                        spec.Target.Pos = new Jhu.SpecSvc.Schema.PositionParam(spec.Data.SpatialAxis.Coverage.Location.Value);
                    }
                }
                 * */

                yield return (Jhu.SpecSvc.Schema.Spectrum.Spectrum) spec;
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
                    tmp2 = tmp.GetType().GetField(names[i]).GetValue(tmp);

                    if (tmp2 == null)
                    {
                        tmp2 = tmp.GetType().GetField(names[i]).FieldType.GetConstructor(new System.Type[0]).Invoke(new object[0]);
                        tmp.GetType().GetField(names[i]).SetValue(tmp, tmp2);
                    }
                }
                else
                {
                    tmp2 = tmp.GetType().GetProperty(names[i]).GetValue(tmp, Type.EmptyTypes);

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
    }
}
#region Revision History
/* Revision History

        $Log: VoTable2Ssa.cs,v $
        Revision 1.1  2008/01/08 22:01:39  dobos
        Initial checkin


*/
#endregion
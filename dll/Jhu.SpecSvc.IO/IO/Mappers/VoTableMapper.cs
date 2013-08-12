#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: VoTableMapper.cs,v 1.1 2008/01/08 22:01:39 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:39 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.IO.Mappers
{
    public class VoTableMapper
    {

        #region Characterization mapper functions

        protected static object[] Characterize(ICharacterization obj, string name)
        {
            List<object> res = new List<object>();

            foreach (IDataCube dc in obj.DataCubes)
            {
                if (dc != null)
                {
                    VOTABLE.GROUP g = new VOTABLE.GROUP();
                    g.utype = name + "Char";    //

                    g.Items = MapAxes(dc.Axes, name + "Char");

                    res.Add(g);
                }
            }

            return res.ToArray();
        }

        protected static object[] MapAxes(IAxis[] axes, string name)
        {
            List<object> res = new List<object>();

            foreach (IAxis axis in axes)
            {
                if (axis != null)
                    res.Add(MapAxis(axis, name));
            }

            return res.ToArray();
        }

        protected static object MapAxis(IAxis axis, string name)
        {
            VOTABLE.GROUP g = new VOTABLE.GROUP();
            g.utype = name + "." + axis.Name.Value;

            bool foundParam;
            List<VOTABLE.FIELD> fields = new List<VOTABLE.FIELD>();

            g.Items = MapMembers(null, axis, name + "." + axis.Name.Value + ".", true, out foundParam, fields);

            return g;
        }

        #endregion

        #region General Mapper functions

        protected static object[] MapMembers(object root, object obj, string name, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            List<object> items = new List<object>();

            FieldInfo[] fieldinfos = null;
            PropertyInfo[] properties = null;

            if (!characterization)
            {
                fieldinfos = obj.GetType().GetFields();
                properties = obj.GetType().GetProperties();
            }
            else
            {
                foreach (Type interf in obj.GetType().GetInterfaces())
                {
                    if (interf.GetCustomAttributes(typeof(Jhu.SpecSvc.Schema.CharacterizationAttribute), false).Length > 0)
                    {
                        fieldinfos = interf.GetFields();
                        properties = interf.GetProperties();
                        break;
                    }
                }
            }

            // adding parameters and groups
            foreach (FieldInfo field in fieldinfos)
            {
                bool fp;
                object o = MapMember(root, obj, field.GetValue(obj), name, field,
                    field.FieldType, characterization, out fp, fields);

                foundParam |= fp;

                if (o != null) items.Add(o);
            }
            foreach (PropertyInfo property in properties)
            {
                bool fp = false;
                object o = null;

                if (property.CanRead)
                {
                    o = MapMember(root, obj, property.GetValue(obj, Type.EmptyTypes), name, property,
                        property.PropertyType, characterization, out fp, fields);
                    foundParam |= fp;
                }

                if (o != null) items.Add(o);
            }

            return items.ToArray();
        }

        protected static object MapMember(object root, object obj, object val, string name, MemberInfo member, Type type, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            string childname = name + member.Name;
            object[] fieldattr = member.GetCustomAttributes(typeof(FieldAttribute), false);
            object[] charattr = member.GetCustomAttributes(typeof(CharacterizationAttribute), false);

            if (fields != null && fieldattr.Length > 0 && ((FieldAttribute)fieldattr[0]).ReferenceMode == ReferenceMode.Item)
            {
                object o = MapField(root, obj, (ParamBase)val, childname, (FieldAttribute)fieldattr[0], fields);
                foundParam |= (o != null);
                return o;
            }
            else if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.ParamBase)) &&
                (characterization && (fieldattr.Length == 0 || ((((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Characterization) > 0)) ||
                !characterization && fieldattr.Length > 0 && (((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Data) > 0))
            {
                object o = MapParam(root, obj, (ParamBase)val, childname, true);
                foundParam |= (o != null);
                return o;
            }
            else if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.Group)) &&
                !characterization && fieldattr.Length > 0 && (((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Data) > 0 ||
                characterization && IsCharacterizedType(type))
            {
                bool fp;
                object o = MapGroup(root, obj, (Group)val, childname, characterization, out fp, fields);
                foundParam |= fp;
                if (!fp) o = null;
                return o;
            }
            /*
                if (flds[i].FieldType == typeof(VoServices.Schema.ParamCollection))
                {
                    tab.Items[i] = MapParamArray(null, (ParamCollection)flds[i].GetValue(obj), "sed:" + flds[i].Name);
                }
                // */

            return null;
        }

        protected static VOTABLE.PARAM MapParam(object root, object obj, Jhu.SpecSvc.Schema.ParamBase param, string name, bool writeValue)
        {
            VOTABLE.PARAM par = null;

            if (param != null)
            {
                par = new VOTABLE.PARAM();
                par.name = GetNameFromUType(name);
                par.utype = name;
                //par.arraysize = "*"; //******

                if (writeValue)
                {
                    if (par.name.IndexOf("SI") >= 0 || par.name.IndexOf("unit", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (param.ToString() != null && param.ToString() != string.Empty)
                            par.value = new Jhu.SpecSvc.Schema.Units.Unit(param.ToString()).FormatVOTable();
                        else
                            par.value = param.ToString();
                    }
                    else
                    {
                        if (param.IsNull)
                            par.value = "UNKNOWN";
                        else
                            par.value = param.ToString();
                    }
                }

                par.datatype = param.GetVOTableType();
                par.ucd = param.Ucd;

                if (param.Unit != null && param.Unit != string.Empty)
                    par.unit = new Jhu.SpecSvc.Schema.Units.Unit(param.Unit).FormatVOTable();
                else
                    par.unit = param.Unit;
            }

            return par;
        }

        protected static VOTABLE.FIELDref MapField(object root, object obj, Jhu.SpecSvc.Schema.ParamBase param, string name, FieldAttribute attr, List<VOTABLE.FIELD> fields)
        {
            VOTABLE.FIELDref fieldref = null;

            // check if the referenced array is null
            if (param != null &&
                root.GetType().GetField(attr.RefMember) != null &&
                root.GetType().GetField(attr.RefMember).GetValue(root) != null)
            {
                fieldref = new VOTABLE.FIELDref();
                fieldref.@ref = attr.RefMember;

                VOTABLE.FIELD field = new VOTABLE.FIELD();

                field.ID = attr.RefMember;
                field.name = attr.RefMember; // GetNameFromUType(name);
                field.utype = name;
                //par.arraysize = "*"; //******

                field.datatype = param.GetVOTableType();
                field.ucd = param.Ucd;

                if (param.Unit != null && param.Unit != string.Empty)
                    field.unit = new Jhu.SpecSvc.Schema.Units.Unit(param.Unit).FormatVOTable();
                else
                    field.unit = param.Unit;

                fields.Add(field);
            }

            return fieldref;
        }

        protected static VOTABLE.GROUP MapGroup(object root, object obj, Jhu.SpecSvc.Schema.Group group, string name, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            VOTABLE.GROUP grp = null;

            if (group != null)
            {
                grp = new VOTABLE.GROUP();
                grp.ID = GetNameFromUType(name);
                grp.name = GetNameFromUType(name);
                grp.utype = name;

                grp.Items = MapMembers(root, group, name + ".", characterization, out foundParam, fields);
            }

            return grp;
        }

        protected static object[] MapTypeMembers(Type type, string name, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            List<object> items = new List<object>();

            FieldInfo[] fieldinfos = null;
            PropertyInfo[] properties = null;

            if (!characterization)
            {
                fieldinfos = type.GetFields();
                properties = type.GetProperties();
            }
            else
            {
                foreach (Type interf in type.GetInterfaces())
                {
                    if (interf.GetCustomAttributes(typeof(Jhu.SpecSvc.Schema.CharacterizationAttribute), false).Length > 0)
                    {
                        fieldinfos = interf.GetFields();
                        properties = interf.GetProperties();
                        break;
                    }
                }
            }

            // adding parameters and groups
            foreach (FieldInfo field in fieldinfos)
            {
                bool fp;
                object o = MapTypeMember(name, field,
                    field.FieldType, characterization, out fp, fields);

                foundParam |= fp;

                if (o != null) items.Add(o);
            }
            foreach (PropertyInfo property in properties)
            {
                bool fp = false;
                object o = null;

                if (property.CanRead)
                {
                    o = MapTypeMember(name, property,
                        property.PropertyType, characterization, out fp, fields);
                    foundParam |= fp;
                }

                if (o != null) items.Add(o);
            }

            return items.ToArray();
        }

        protected static object MapTypeMember(string name, MemberInfo member, Type type, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            string childname = name + member.Name;
            object[] fieldattr = member.GetCustomAttributes(typeof(FieldAttribute), false);
            object[] charattr = member.GetCustomAttributes(typeof(CharacterizationAttribute), false);

            //if (fields != null && fieldattr.Length > 0 && ((FieldAttribute)fieldattr[0]).RefMember != null)
            if (fields != null && fieldattr.Length > 0 && ((FieldAttribute)fieldattr[0]).ReferenceMode == ReferenceMode.ArrayItem)
            {
                object o = MapTypeField(type, childname, (FieldAttribute)fieldattr[0], fields);
                foundParam |= (o != null);
                return o;
            }
            // nem kell, mert ssa-nal csak fieldref lehet meg group
            /*            else if (type.IsSubclassOf(typeof(VoServices.Schema.ParamBase)) &&
                            (characterization && (fieldattr.Length == 0 || ((((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Characterization) > 0)) ||
                            !characterization && fieldattr.Length > 0 && (((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Data) > 0))
                        {
                            object o = MapParam(root, obj, (ParamBase)val, childname, true);
                            foundParam |= (o != null);
                            return o;
                        }
             * */
            else if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.Group)) &&
                !characterization && fieldattr.Length > 0 && (((FieldAttribute)fieldattr[0]).SerializationMode & SerializationMode.Data) > 0 ||
                characterization && IsCharacterizedType(type))
            {
                bool fp;
                object o = MapTypeGroup(type, childname, characterization, out fp, fields);
                foundParam |= fp;
                if (!fp) o = null;
                return o;
            }
            /*
                if (flds[i].FieldType == typeof(VoServices.Schema.ParamCollection))
                {
                    tab.Items[i] = MapParamArray(null, (ParamCollection)flds[i].GetValue(obj), "sed:" + flds[i].Name);
                }
                // */

            return null;
        }

        protected static VOTABLE.FIELDref MapTypeField(Type type, string name, FieldAttribute attr, List<VOTABLE.FIELD> fields)
        {
            VOTABLE.FIELDref fieldref = null;

            // check if the referenced array is null
            /*if (param != null &&
                root.GetType().GetField(attr.RefMember) != null &&
                root.GetType().GetField(attr.RefMember).GetValue(root) != null)
            {*/
            fieldref = new VOTABLE.FIELDref();
            fieldref.@ref = attr.RefMember;
            //fieldref.@ref = GetNameFromUType(name);

            VOTABLE.FIELD field = new VOTABLE.FIELD();

            field.ID = attr.RefMember;
            field.name = attr.RefMember;
            //field.ID = GetNameFromUType(name);
            //field.name = GetNameFromUType(name);
            field.utype = name;
            //par.arraysize = "*"; //******

            if (type == typeof(TextParam))
                field.datatype = VOTABLE.dataType.unicodeChar;
            else if (type == typeof(DoubleParam))
                field.datatype = VOTABLE.dataType.@double;
            else if (type == typeof(IntParam))
                field.datatype = VOTABLE.dataType.@long;
            field.ucd = attr.Ucd; // param.Ucd;
            field.unit = attr.DefaultUnit; // param.Unit;

            fields.Add(field);
            //}

            return fieldref;
        }

        protected static VOTABLE.GROUP MapTypeGroup(Type type, string name, bool characterization, out bool foundParam, List<VOTABLE.FIELD> fields)
        {
            foundParam = false;

            VOTABLE.GROUP grp = null;

            //if (group != null)
            //{
            grp = new VOTABLE.GROUP();
            grp.ID = GetNameFromUType(name);
            grp.name = GetNameFromUType(name);
            grp.utype = name;

            grp.Items = MapTypeMembers(type, name + ".", characterization, out foundParam, fields);
            //}

            return grp;
        }

        #endregion

        #region Utility functions

        protected static string GetNameFromUType(string name)
        {
            int i = name.LastIndexOf('.');
            if (i < 0) i = name.LastIndexOf(':');
            return name.Substring(i + 1);
        }

        protected static bool IsCharacterizedType(Type type)
        {
            return (type.GetCustomAttributes(typeof(Jhu.SpecSvc.Schema.CharacterizationAttribute), false).Length > 0);
        }

        #endregion

    }
}
#region Revision History
/* Revision History

        $Log: VoTableMapper.cs,v $
        Revision 1.1  2008/01/08 22:01:39  dobos
        Initial checkin


*/
#endregion
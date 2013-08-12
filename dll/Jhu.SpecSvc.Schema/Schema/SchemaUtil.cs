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
 *   ID:          $Id: Util.cs,v 1.1 2008/01/08 22:26:23 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:23 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Jhu.SpecSvc.Schema
{
	public static class SchemaUtil
	{
		public static void InitializeMembers(object obj, ParamRequired initializationLevel, bool recursive)
		{
			Type thistype = obj.GetType();

			MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			foreach (MemberInfo member in members)
			//foreach (FieldInfo field in obj.GetType().GetFields())
			{
				//object[] attrs = member.GetCustomAttributes(typeof(FieldAttribute), false);
				object[] attrs = member.GetCustomAttributes(typeof(FieldAttribute), false);
				if (attrs != null && attrs.Length > 0)
				{
					FieldAttribute attr;
					if ((attr = attrs[0] as FieldAttribute) != null)
					{
						if ((int)attr.Required <= (int)initializationLevel)
						{
							object newfieldvalue = null;
							//object newfieldvalue = field.FieldType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
							if (member.MemberType == MemberTypes.Field)
							{
								newfieldvalue = member.DeclaringType.GetField(member.Name).FieldType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
							}
							else if (member.MemberType == MemberTypes.Property)
							{
								newfieldvalue = member.DeclaringType.GetProperty(member.Name).PropertyType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
							}

							ParamBase param = newfieldvalue as ParamBase;
							if (param != null)
							{
								param.Name = member.Name;
								param.Unit = attr.DefaultUnit;
								param.Ucd = attr.Ucd;
								if (attr.DefaultValue != null)
									param.SetValue(attr.DefaultValue);
							}

							if (recursive)
							{
								Group group = newfieldvalue as Group;
								if (group != null)
								{
									InitializeMembers(group, initializationLevel, recursive);
								}

								Axis axis = newfieldvalue as Axis;
								if (axis != null)
								{
									InitializeMembers(axis, initializationLevel, recursive);
								}
							}

							if (member.MemberType == MemberTypes.Field)
							{
								member.DeclaringType.GetField(member.Name).SetValue(obj, newfieldvalue);
							}
							else if (member.MemberType == MemberTypes.Property)
							{
								member.DeclaringType.GetProperty(member.Name).SetValue(obj, newfieldvalue, Type.EmptyTypes);
							}
						}
					}
				}
			}
		}

		public static void CopyMembers(object to, object from)
		{
			if (to.GetType() != from.GetType())
				throw new System.ArgumentException("To and From type do not match");

			foreach (FieldInfo field in to.GetType().GetFields())
			{
				object[] attrs = field.GetCustomAttributes(typeof(FieldAttribute), false);
				if (attrs != null && attrs.Length > 0)
				{
					ICloneable val = field.GetValue(from) as ICloneable;
					if (val != null)
					{
						field.SetValue(to, val.Clone());
					}
				}
                if (field.FieldType.IsSubclassOf(typeof(System.Array)))
                {
                    if (field.GetValue(from) != null)
                    {
                        int length = ((System.Array)field.GetValue(from)).Length;
                        field.SetValue(to, field.FieldType.GetConstructor(new Type[] { typeof(int) }).Invoke(
                            new object[] { length }));
                        Array.Copy((System.Array)field.GetValue(from), (System.Array)field.GetValue(to), length);
                    }
                }
			}
            foreach (PropertyInfo prop in to.GetType().GetProperties())
            {
                object[] attrs = prop.GetCustomAttributes(typeof(FieldAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    ICloneable val = prop.GetValue(from, null) as ICloneable;
                    if (val != null)
                    {
                        prop.SetValue(to, val.Clone(), null);
                    }
                }
            }
		}
	}
}
#region Revision History
/* Revision History

        $Log: Util.cs,v $
        Revision 1.1  2008/01/08 22:26:23  dobos
        Initial checkin


*/
#endregion
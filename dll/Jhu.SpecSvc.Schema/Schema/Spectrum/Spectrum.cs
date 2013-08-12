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
 *   ID:          $Id: Spectrum.cs,v 1.3 2008/10/25 18:26:23 dobos Exp $
 *   Revision:    $Revision: 1.3 $
 *   Date:        $Date: 2008/10/25 18:26:23 $
 */
#endregion
using System;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    [XmlType("SpectrumBase")]
    public partial class Spectrum : ICloneable, ICharacterization
    {
        private TextParam type;
        private TextParam dataModel;
        private CoordSys coordSys;
        private Curation curation;
        private DataId dataId;
        private Derived derived;
        private Target target;
        private Environment environment;
        private Data data;

        /// <summary>
        /// Version of data model
        /// </summary>
        [XmlElement(Order = 1)]
        [Field(Required = ParamRequired.Mandatory, DefaultValue = "SPECTRUM-1.0")]
        public TextParam DataModel
        {
            get { return dataModel; }
            set { dataModel = value; }
        }

        /// <summary>
        /// Segment type
        /// </summary>
        [XmlElement(Order = 2)]
        [Field(Required = ParamRequired.Optional, DefaultValue = "Spectrum")]
        public TextParam Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Number of data points
        /// </summary>
        [XmlElement(Order = 3)]
        [Field(Required = ParamRequired.Derived | ParamRequired.Dummy)]
        public IntParam Length
        {
            get
            {
                IntParam p;

                if (Spectral_Value != null)
                    p = new IntParam((long)Spectral_Value.Length);
                else
                    p = new IntParam(-1L);

                p.Ucd = "meta.number";
                return p;
            }
            set { }
        }

        /// <summary>
        /// SI factor and dimensions, maps to data.TimeAxis.Value.Unit
        /// </summary>
        [XmlElement(Order = 4)]
        [Field(Required = ParamRequired.Recommended | ParamRequired.Dummy, Ucd = "time;arith.zp")]
        public TextParam TimeSI
        {
            get
            {
                if (data != null && data.TimeAxis != null && data.TimeAxis.Value != null)
                {
                    TextParam p = new TextParam("time;arith.zp");
                    p.Value = data.TimeAxis.Value.Unit;
                    return p;
                }
                else
                    return null;
            }
            set { }
        }

        /// <summary>
        /// SI factor and dimensions
        /// </summary>
        [XmlElement(Order = 5)]
        [Field(Required = ParamRequired.Recommended | ParamRequired.Dummy)]
        public TextParam SpectralSI
        {
            get
            {
                if (data != null && data.SpectralAxis != null && data.SpectralAxis.Value != null)
                {
                    TextParam p = new TextParam();
                    p.Value = data.SpectralAxis.Value.Unit;
                    return p;
                }
                else
                    return null;
            }
            set { }
        }

        /// <summary>
        /// SI factor and dimensions
        /// </summary>
        [XmlElement(Order = 6)]
        [Field(Required = ParamRequired.Recommended | ParamRequired.Dummy)]
        public TextParam FluxSI
        {
            get
            {
                if (data != null && data.FluxAxis != null && data.FluxAxis.Value != null)
                {
                    TextParam p = new TextParam();
                    p.Value = data.FluxAxis.Value.Unit;
                    return p;
                }
                else
                    return null;
            }
            set { }
        }

        [XmlElement(Order = 7)]
        [Field(Required = ParamRequired.Optional)]
        public CoordSys CoordSys
        {
            get { return coordSys; }
            set { coordSys = value; }
        }

        [XmlElement(Order = 8)]
        [Field(Required = ParamRequired.Mandatory)]
        public Curation Curation
        {
            get { return curation; }
            set { curation = value; }
        }

        [XmlElement(Order = 9)]
        [Field(Required = ParamRequired.Optional)]
        public DataId DataId
        {
            get { return dataId; }
            set { dataId = value; }
        }

        [XmlElement(Order = 10)]
        [Field(Required = ParamRequired.Optional)]
        public Derived Derived
        {
            get { return derived; }
            set { derived = value; }
        }

        [XmlElement(Order = 11)]
        [Field(Required = ParamRequired.Mandatory)]
        public Target Target
        {
            get { return target; }
            set { target = value; }
        }

        [XmlElement(Order = 12)]
        [Field(Required = ParamRequired.Optional)]
        public Environment Environment
        {
            get { return environment; }
            set { environment = value; }
        }

        [XmlElement(Order = 13)]
        [Field(Required = ParamRequired.Mandatory)]
        public Data Data
        {
            get { return data; }
            set { data = value; }
        }

        [XmlArray("CustomParams", Order = 14)]
        public ParamCollection Custom;

        [XmlArray(Order = 100)]
        public double[] Spectral_Value;
        [XmlArray(Order = 101)]
        public double[] Spectral_Accuracy_StatError;
        [XmlArray(Order = 102)]
        public double[] Spectral_Accuracy_StatErrLow;
        [XmlArray(Order = 103)]
        public double[] Spectral_Accuracy_StatErrHigh;
        [XmlArray(Order = 104)]
        public double[] Spectral_Accuracy_BinSize;
        [XmlArray(Order = 105)]
        public double[] Spectral_Accuracy_BinLow;
        [XmlArray(Order = 106)]
        public double[] Spectral_Accuracy_BinHigh;
        [XmlArray(Order = 107)]
        public double[] Flux_Value;
        [XmlArray(Order = 108)]
        public double[] Flux_Accuracy_StatError;
        [XmlArray(Order = 109)]
        public double[] Flux_Accuracy_StatErrLow;
        [XmlArray(Order = 110)]
        public double[] Flux_Accuracy_StatErrHigh;
        [XmlArray(Order = 111)]
        public long[] Flux_Accuracy_Quality;
        [XmlArray(Order = 112)]
        public double[] BackgroundModel_Value;
        [XmlArray(Order = 113)]
        public double[] BackgroundModel_Accuracy_StatError;
        [XmlArray(Order = 114)]
        public double[] BackgroundModel_Accuracy_StatErrLow;
        [XmlArray(Order = 115)]
        public double[] BackgroundModel_Accuracy_StatErrHigh;
        [XmlArray(Order = 116)]
        public long[] BackgroundModel_Accuracy_Quality;
        [XmlArray(Order = 117)]
        public double[] Time_Value;
        [XmlArray(Order = 118)]
        public double[] Time_Accuracy_StatError;
        [XmlArray(Order = 119)]
        public double[] Time_Accuracy_BinSize;
        [XmlArray(Order = 120)]
        public double[] Time_Accuracy_BinLow;
        [XmlArray(Order = 121)]
        public double[] Time_Accuracy_BinHigh;
        [XmlArray(Order = 122)]
        public double[] Counts_Value;

        IDataCube[] ICharacterization.DataCubes
        {
            get
            {
                return new IDataCube[]
				{
					data,
                    modelParameters
				};
            }
            set { }
        }

        #region Constructors
        public Spectrum()
        {
        }

        public Spectrum(bool initialize)
        {
            if (initialize)
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
        }

        public Spectrum(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Spectrum(Spectrum old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        public void UpdateCharacterization()
        {
            //****
            /*
            if (data != null && data.SpectralAxis != null && data.SpectralAxis.Value != null && data.SpectralAxis.Value.Value != null && data.SpectralAxis.Coverage != null && data.SpectralAxis.Coverage.Bounds != null)
            {
                data.SpectralAxis.Coverage.Bounds.Start = new DoubleParam(data.SpectralAxis.Value.Value[0], data.SpectralAxis.Value.Unit);
                data.SpectralAxis.Coverage.Bounds.Start.Ucd = data.SpectralAxis.Value.Ucd + ";stat.min";

                data.SpectralAxis.Coverage.Bounds.Stop = new DoubleParam(data.SpectralAxis.Value.Value[data.SpectralAxis.Value.Value.Length - 1], data.SpectralAxis.Value.Unit);
                data.SpectralAxis.Coverage.Bounds.Stop.Ucd = data.SpectralAxis.Value.Ucd + ";stat.max";

                data.SpectralAxis.Coverage.Bounds.Extent = new DoubleParam(data.SpectralAxis.Value.Value[data.SpectralAxis.Value.Value.Length - 1] - data.SpectralAxis.Value.Value[0], data.SpectralAxis.Value.Unit);
                data.SpectralAxis.Coverage.Bounds.Extent.Ucd = "instr.bandwidth";
            }
             * */
        }

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Spectrum Clone(Spectrum old)
        {
            if (old != null)
            {
                return new Spectrum(old);
            }
            else
                return null;
        }
        #endregion

        #region Utility static functions

        public static object GetField(object obj, string fieldName, int dataType)
        {
            return GetField(obj, fieldName, dataType, true);
        }

        /// <summary>
        /// Returns a member field of any object identified by a string where the elements of the object
        /// hierarchy are separated by dots. If the string identifies a custom field, i.e. a member
        /// of the collection with a type that cannot determined by reflection, the dataType
        /// property must be provided.
        /// </summary>
        /// <param name="obj">Root of the object hierarchy</param>
        /// <param name="var"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static object GetField(object obj, string fieldName, int dataType, bool autoCreate)
        {
            // populating object hierarchy
            string[] names = fieldName.Split('.');
            object tmp = obj, tmp2 = null;
            // start with 1 because 0 is the sed itself
            for (int i = 0; i < names.Length; i++)
            {
                if (tmp.GetType() == typeof(ParamCollection))
                {
                    object res = null;
                    switch (dataType)
                    {
                        case 0:
                            res = new Jhu.SpecSvc.Schema.TextParam(true);
                            break;
                        case 1:
                            res = new Jhu.SpecSvc.Schema.IntParam(true);
                            break;
                        case 2:
                            res = new Jhu.SpecSvc.Schema.DoubleParam(true);
                            break;
                        case 3:
                            res = new Jhu.SpecSvc.Schema.TimeParam(true);
                            break;
                        case 4:
                            res = new Jhu.SpecSvc.Schema.PositionParam(true);
                            break;
                        case 5:
                            res = new Jhu.SpecSvc.Schema.BoolParam(true);
                            break;
                    }

                    ((ParamCollection)tmp).Add(names[i], (ParamBase)res);
                    return res;
                }
                else
                {
                    MemberInfo[] mems = tmp.GetType().GetMember(names[i]);
                    if (mems != null && mems.Length > 0)
                    {
                        MemberInfo mem = mems[0];
                        object[] attr = mem.GetCustomAttributes(typeof(FieldAttribute), false);
                        if (attr != null && attr.Length > 0 && ((((FieldAttribute)attr[0]).Required & (ParamRequired.Dummy | ParamRequired.Derived)) == 0))
                        {

                            if (mem.MemberType == MemberTypes.Property)
                            {
                                PropertyInfo prop = (PropertyInfo)mem;

                                tmp2 = prop.GetValue(tmp, null);
                                if (tmp2 == null && autoCreate)
                                {
                                    tmp2 = prop.PropertyType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                                    prop.SetValue(tmp, tmp2, null);
                                }
                            }
                            else if (mem.MemberType == MemberTypes.Field)
                            {
                                FieldInfo fld = (FieldInfo)mem;
                                tmp2 = fld.GetValue(tmp);
                                if (tmp2 == null && autoCreate)
                                {
                                    tmp2 = fld.FieldType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                                    fld.SetValue(tmp, tmp2);
                                }
                            }

                            tmp = tmp2;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                        return null;
                }
            }

            return tmp;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Spectrum.cs,v $
        Revision 1.3  2008/10/25 18:26:23  dobos
        *** empty log message ***

        Revision 1.2  2008/10/14 13:29:23  dobos
        Environment data added to schema.

        Revision 1.1  2008/01/08 22:26:53  dobos
        Initial checkin


*/
#endregion
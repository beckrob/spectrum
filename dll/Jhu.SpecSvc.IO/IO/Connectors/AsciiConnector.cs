#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: AsciiConnector.cs,v 1.2 2008/09/11 10:44:59 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/09/11 10:44:59 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using Jhu.SpecSvc.Schema;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    /// <summary>
    /// Summary description for AsciiConnector.
    /// </summary>
    public class AsciiConnector : ConnectorBase, IDisposable
    {
        public static readonly string[] ColumnNames =
            {
         "Spectral_Value",
         "Spectral_Accuracy_StatError",
         "Spectral_Accuracy_StatErrLow",
         "Spectral_Accuracy_StatErrHigh",
         "Spectral_Accuracy_BinSize",
         "Spectral_Accuracy_BinLow",
         "Spectral_Accuracy_BinHigh",
         "Flux_Value",
         "Flux_Accuracy_StatError",
         "Flux_Accuracy_StatErrLow",
         "Flux_Accuracy_StatErrHigh",
         "Flux_Accuracy_Quality",
         "BackgroundModel_Value",
         "BackgroundModel_Accuracy_StatError",
         "BackgroundModel_Accuracy_StatErrLow",
         "BackgroundModel_Accuracy_StatErrHigh",
         "BackgroundModel_Accuracy_Quality",
         "Time_Value",
         "Time_Accuracy_StatError",
         "Time_Accuracy_BinSize",
         "Time_Accuracy_BinLow",
         "Time_Accuracy_BinHigh"
            };

        #region Member variables

        private AsciiFormat format;
        private bool writeFields;
        private string[] columns;
        private string fileName;
        private TextWriter outputStream;
        private TextReader inputStream;

        #endregion
        #region Constructors

        public AsciiConnector()
        {
            InitializeMembers();
        }

        public AsciiConnector(string fileName)
        {
            InitializeMembers();

            this.fileName = fileName;
        }

        #endregion
        #region Properties

        public AsciiFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        public bool WriteFields
        {
            get { return writeFields; }
            set { writeFields = value; }
        }

        public string[] Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public TextWriter OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
        }

        public TextReader InputStream
        {
            get { return inputStream; }
            set { inputStream = value; }
        }

        #endregion

        private void InitializeMembers()
        {
            format = AsciiFormat.Tabular;
            writeFields = true;
            columns = null;
            fileName = string.Empty;
            inputStream = null;
            outputStream = null;
        }

        #region Search functions

        public override Jhu.SpecSvc.SpectrumLib.Spectrum GetSpectrum(Guid userGuid, string spectrumId, bool loadPoints, string[] pointsMask, bool loadDetails)
        {
            bool closefile = false;

            Jhu.SpecSvc.SpectrumLib.Spectrum spec = new Jhu.SpecSvc.SpectrumLib.Spectrum(false);
            spec.BasicInitialize();

            if (inputStream == null)
            {
                inputStream = new StreamReader(fileName, System.Text.Encoding.ASCII);
                closefile = true;
            }

            if (loadPoints)
            {
                List<string[]> lines = new List<string[]>();
                string line;
                while ((line = inputStream.ReadLine()) != null)
                {
                    // skip comment rows
                    if (line.StartsWith("#"))
                        continue;

                    lines.Add(line.Split(" \t,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                }


                List<Array> arrays = new List<Array>(columns.Length);
                List<Type> types = new List<Type>(columns.Length);
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != null && columns[i] != string.Empty)
                    {
                        Array a = null;
                        Type t = spec.GetType().GetField(columns[i]).FieldType;
                        if (t == typeof(double[]))
                        {
                            a = new double[lines.Count];
                        }
                        else if (t == typeof(long[]))
                        {
                            a = new long[lines.Count];
                        }
                        arrays.Add(a);
                        types.Add(t);
                        spec.GetType().GetField(columns[i]).SetValue(spec, a);
                    }
                }
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != null && columns[i] != string.Empty)
                    {
                        if (types[i] == typeof(double[]))
                        {
                            for (int l = 0; l < lines.Count; l++)
                            {
                                arrays[i].SetValue(double.Parse(lines[l][i]), l);
                            }
                        }
                        else if (types[i] == typeof(long[]))
                        {
                            for (int l = 0; l < lines.Count; l++)
                            {
                                arrays[i].SetValue(long.Parse(lines[l][i]), l);
                            }
                        }
                    }
                }
            }

            if (closefile)
            {
                inputStream.Close();
                inputStream.Dispose();
                inputStream = null;
            }

            spec.UserGuid = userGuid;
            spec.Curation.PublisherDID.Value = spectrumId;

            return spec;
        }
        #endregion

        #region Spectrum save functions

        public override long SaveSpectrum(Jhu.SpecSvc.SpectrumLib.Spectrum spec, Guid userGuid)
        {
            return SaveSpectrum(spec, userGuid, false);
        }

        public long SaveSpectrum(Jhu.SpecSvc.SpectrumLib.Spectrum spec, Guid userGuid, bool mask)
        {
            bool closefile = false;

            spec.CalculateHtmId();
            spec.UserGuid = userGuid;

            if (outputStream == null)
            {
                outputStream = new StreamWriter(fileName);
                closefile = true;
            }

            CreateSpectrum(spec, userGuid);
            SaveSpectrumFields(spec, userGuid);
            SaveSpectrumData(spec, userGuid, mask);

            outputStream.Flush();

            if (closefile)
            {
                outputStream.Close();
                outputStream.Dispose();
                outputStream = null;
            }

            //return spec.Id;
            return 0;
        }

        private void CreateSpectrum(Jhu.SpecSvc.SpectrumLib.Spectrum spec, Guid userGuid)
        {
            if (format == AsciiFormat.Tabular)
                outputStream.WriteLine("# Exported from Spectrum Service: http://voservices.net/spectrum");
        }

        private void SaveSpectrumFields(Jhu.SpecSvc.SpectrumLib.Spectrum spec, Guid userGuid)
        {
            if (format == AsciiFormat.Tabular && writeFields)
                SaveSpectrumFields_Group(spec, "Spectrum");
        }

        private void SaveSpectrumFields_Group(object obj, string name)
        {
            // adding parameters and groups
            if (obj != null)
            {
                foreach (FieldInfo field in obj.GetType().GetFields())
                {
                    SaveSpectrumField(field.FieldType, field.GetValue(obj), name + "." + field.Name);
                }

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    SaveSpectrumField(prop.PropertyType, prop.GetValue(obj, Type.EmptyTypes), name + "." + prop.Name);
                }
            }
        }

        private void SaveSpectrumField(Type type, object value, string name)
        {
            if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.ParamBase)))
            {
                SaveSpectrumFields_Param(value, name);
            }
            else if (type.IsSubclassOf(typeof(Jhu.SpecSvc.Schema.Group)))
            {
                SaveSpectrumFields_Group(value, name);
            }
            else if (type == typeof(Jhu.SpecSvc.Schema.ParamCollection))
            {
                SaveSpectrumFields_ParamCollection((ParamCollection)value, name);
            }
        }

        private void SaveSpectrumFields_ParamCollection(ParamCollection coll, string name)
        {
            if (coll != null)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    ParamBase par = (ParamBase)coll[i];
                    SaveSpectrumFields_Param(par, name + "." + par.Key);
                }
            }
        }

        private void SaveSpectrumFields_Param(object par, string name)
        {
            outputStream.Write("# $" + name + "=");
            ParamBase param = (ParamBase)par;

            if (param != null)
            {
                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.TextParam))
                {
                    outputStream.Write(((Jhu.SpecSvc.Schema.TextParam)param).Value);
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.IntParam))
                {
                    outputStream.Write(((Jhu.SpecSvc.Schema.IntParam)param).Value);
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.DoubleParam))
                {
                    outputStream.Write(((Jhu.SpecSvc.Schema.DoubleParam)param).Value);
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.TimeParam))
                {
                    outputStream.Write(((Jhu.SpecSvc.Schema.TimeParam)param).Value);
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.PositionParam))
                {
                    outputStream.Write("{0} {1}", ((Jhu.SpecSvc.Schema.PositionParam)param).Value.Ra, ((Jhu.SpecSvc.Schema.PositionParam)param).Value.Dec);
                }

                if (par.GetType() == typeof(Jhu.SpecSvc.Schema.BoolParam))
                {
                    outputStream.Write(((Jhu.SpecSvc.Schema.TimeParam)param).Value);
                }
            }

            outputStream.WriteLine();
        }

        private void SaveSpectrumData(Jhu.SpecSvc.SpectrumLib.Spectrum spec, Guid userGuid, bool mask)
        {
            SaveSpectrumData_Segment(spec, "Spectrum", mask);
        }

        private void SaveSpectrumData_Segment(Jhu.SpecSvc.Schema.Spectrum.Spectrum spec, string name, bool mask)
        {
            if (columns != null)
            {
                string separator = string.Empty;
                switch (format)
                {
                    case AsciiFormat.Tabular:
                        separator = "\t";
                        break;
                    case AsciiFormat.CommaSeparated:
                        separator = ",";
                        break;
                }

                // fields
                Array[] data = new Array[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != null && columns[i] != string.Empty)
                        data[i] = (Array)spec.GetType().GetField(columns[i]).GetValue(spec);
                    else
                        data[i] = null;
                }

                // write header
                outputStream.Write("# ");
                for (int i = 0; i < columns.Length; i++)
                {
                    if (i > 0 && columns[i] != string.Empty && data[i] != null) outputStream.Write(separator);
                    outputStream.Write(columns[i]);
                }
                outputStream.WriteLine();

                if (spec.Spectral_Value != null)
                {
                    for (int i = 0; i < spec.Spectral_Value.Length; i++)
                    {
                        if (!mask || spec.Flux_Accuracy_Quality[i] == 0)
                        {
                            for (int j = 0; j < columns.Length; j++)
                            {
                                if (data[j] != null)
                                {
                                    if (j > 0) outputStream.Write(separator);
                                    outputStream.Write(((Array)data[j]).GetValue(i).ToString());
                                }
                            }
                            outputStream.WriteLine();
                        }
                    }
                }
                else
                {
                    outputStream.WriteLine("Error in spectrum, cannot write spectral data points.");
                }
            }
            else
            {
                // Write data
                outputStream.WriteLine("# Wavelength Flux ErrorLo ErrorHi Mask");
                for (int i = 0; i < spec.Spectral_Value.Length; i++)
                {

                    if (!mask || spec.Flux_Accuracy_Quality[i] == 0)
                    {
                        outputStream.WriteLine("{0} {1} {2} {3}",
                            spec.Spectral_Value == null ? 0 : spec.Spectral_Value[i],
                             spec.Flux_Value == null ? 0 : spec.Flux_Value[i],
                             spec.Flux_Accuracy_StatErrLow == null ? 0 : spec.Flux_Accuracy_StatErrLow[i],
                             spec.Flux_Accuracy_StatErrHigh == null ? 0 : spec.Flux_Accuracy_StatErrHigh[i]
                            //seg.Flux_Accuracy_Quality == null ? 0 : seg.Flux_Accuracy_Quality[i]
                             );
                    }
                }
            }
        }

        public enum AsciiFormat
        {
            Tabular,
            CommaSeparated
        }

        #endregion

        #region Utility functions

        // static util methods
        protected static System.Type GetBaseType(System.Type type)
        {
            System.Type nt = type;
            while (nt != typeof(System.Object))
            {
                if (nt.BaseType == typeof(System.Object) ||
                    nt.BaseType == typeof(System.Collections.CollectionBase) ||
                    nt.BaseType == typeof(System.Collections.Specialized.NameObjectCollectionBase))
                    return nt;
                nt = nt.BaseType;
            }
            return nt;
        }

        #endregion
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: AsciiConnector.cs,v $
        Revision 1.2  2008/09/11 10:44:59  dobos
        Bugfixes and parallel execution added to PortalConnector

        Revision 1.1  2008/01/08 22:01:34  dobos
        Initial checkin


*/
#endregion
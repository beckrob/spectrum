#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: VectorConverter.cs,v 1.1 2008/01/08 22:01:33 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:33 $
 */
#endregion
//#define unsafe
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Collections.Generic;

namespace Jhu.SpecSvc.IO
{
    public partial class VectorConverter : Converter
    {

        #region double list to binary converters (1 to 10 parameters)

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_1(SqlDouble v1)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_2(SqlDouble v1, SqlDouble v2)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_3(SqlDouble v1, SqlDouble v2, SqlDouble v3)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_4(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_5(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_6(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5, SqlDouble v6)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value, v6.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_7(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5, SqlDouble v6, SqlDouble v7)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value, v6.Value, v7.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_8(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5, SqlDouble v6, SqlDouble v7, SqlDouble v8)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value, v6.Value, v7.Value, v8.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_9(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5, SqlDouble v6, SqlDouble v7, SqlDouble v8, SqlDouble v9)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value, v6.Value, v7.Value, v8.Value, v9.Value }));
        }

        [Microsoft.SqlServer.Server.SqlFunction(IsDeterministic = true, IsPrecise = true)]
        public static SqlBinary Vector_10(SqlDouble v1, SqlDouble v2, SqlDouble v3, SqlDouble v4, SqlDouble v5, SqlDouble v6, SqlDouble v7, SqlDouble v8, SqlDouble v9, SqlDouble v10)
        {
            return new SqlBinary(ToBinary(new double[] { v1.Value, v2.Value, v3.Value, v4.Value, v5.Value, v6.Value, v7.Value, v8.Value, v9.Value, v10.Value }));
        }

        #endregion

        
#if unsafe
        [Microsoft.SqlServer.Server.SqlFunction]
        unsafe public static SqlDouble VectorItem(SqlBinary v, SqlInt32 index)
        {
            double res;

            fixed (byte* b = v.Value)
            {
                double* f = (double*)b;
                res = *(f + index.Value);
            }

            return res;
        }
#else
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble VectorItem(SqlBinary v, SqlInt32 index)
        {
            return BitConverter.ToDouble(v.Value, sizeof(int) + sizeof(int) + index.Value * sizeof(double));
        }
#endif

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 VectorSize(SqlBinary v)
        {
            return BitConverter.ToInt32(v.Value, sizeof(int));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlString VectorToString(SqlBinary v)
        {
            double[] data = ToArray(v.Value);
            string res = "";

            for (int i = 0; i < data.Length; i++)
                res += " " + data[i].ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            return new SqlString("{" + res.Substring(1) + "}");
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary VectorFromString(SqlString str)
        {
            int p = 0;
            return ToBinary(ReadVector(str.Value, ref p));
        }

#if unsafe
        unsafe internal static byte[] ToBinary(double[] data)
        {
            byte[] buffer = new byte[data.Length << 2];

            fixed (byte* b = buffer)
            {
                double* f = (double*)b;
                for (int i = 0; i < data.Length; i++)
                    *f++ = data[i];
            }

            return buffer;
        }
#else
        internal static byte[] ToBinary(double[] data)
        {
            byte[] buffer = new byte[data.Length * sizeof(double) + sizeof(int) + sizeof(int)];

            Array.Copy(BitConverter.GetBytes((int)1), 0, buffer, 0, sizeof(int));
            Array.Copy(BitConverter.GetBytes((int)data.Length), 0, buffer, sizeof(int), sizeof(int));

            int q = sizeof(int) + sizeof(int);
            for (int i = 0; i < data.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(data[i]), 0, buffer, q, sizeof(double));
                q += sizeof(double);
            }

            return buffer;
        }
#endif

        internal static double[] ToArray(SqlBinary data)
        {
            return ToArray(data.Value);
        }

#if unsafe
        unsafe internal static double[] ToArray(byte[] data)
        {
            double[] array = new double[data.Length >> 2];

            fixed (byte* b = data)
            {
                double* f = (double*)b;
                for (int i = 0; i < array.Length; i++)
                    array[i] = *f++;
            }

            return array;
        }
#else
        internal static double[] ToArray(byte[] data)
        {
            double[] array = new double[BitConverter.ToInt32(data, sizeof(int))];

            int q = sizeof(int) + sizeof(int);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToDouble(data, q);
                q += sizeof(double);
            }

            return array;
        }
#endif
    }
}
#region Revision History
/* Revision History

        $Log: VectorConverter.cs,v $
        Revision 1.1  2008/01/08 22:01:33  dobos
        Initial checkin


*/
#endregion
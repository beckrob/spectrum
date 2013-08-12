#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: MatrixConverter.cs,v 1.1 2008/01/08 22:01:33 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:33 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

namespace Jhu.SpecSvc.IO
{
    public partial class MatrixConverter :Converter
    {
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_1(SqlBinary v1)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_2(SqlBinary v1, SqlBinary v2)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_3(SqlBinary v1, SqlBinary v2, SqlBinary v3)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_4(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_5(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_6(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_7(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_8(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_9(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8, SqlBinary v9)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value), VectorConverter.ToArray(v9.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow_10(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8, SqlBinary v9, SqlBinary v10)
        {
            return new SqlBinary(ToBinary(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value), VectorConverter.ToArray(v9.Value), VectorConverter.ToArray(v10.Value) }));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_1(SqlBinary v1)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_2(SqlBinary v1, SqlBinary v2)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_3(SqlBinary v1, SqlBinary v2, SqlBinary v3)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_4(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_5(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_6(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_7(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_8(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_9(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8, SqlBinary v9)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value), VectorConverter.ToArray(v9.Value) });
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol_10(SqlBinary v1, SqlBinary v2, SqlBinary v3, SqlBinary v4, SqlBinary v5, SqlBinary v6, SqlBinary v7, SqlBinary v8, SqlBinary v9, SqlBinary v10)
        {
            return MatrixCol_n(new double[][] { VectorConverter.ToArray(v1.Value), VectorConverter.ToArray(v2.Value), VectorConverter.ToArray(v3.Value), VectorConverter.ToArray(v4.Value), VectorConverter.ToArray(v5.Value), VectorConverter.ToArray(v6.Value), VectorConverter.ToArray(v7.Value), VectorConverter.ToArray(v8.Value), VectorConverter.ToArray(v9.Value), VectorConverter.ToArray(v10.Value) });
        }

        private static SqlBinary MatrixCol_n(double[][] vectors)
        {
            double[][] mat = new double[vectors[0].Length][];

            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new double[vectors.Length];
                for (int j = 0; j < mat[i].Length; j++)
                    mat[i][j] = vectors[j][i];
            }

            return new SqlBinary(ToBinary(mat));
        }

#if unsafe
        [Microsoft.SqlServer.Server.SqlFunction]
        unsafe public static SqlSingle MatrixItem(SqlBinary v, SqlInt32 i, SqlInt32 j)
        {
            float res;
            int imax, jmax;

            fixed (byte* b = v.Value)
            {
                int* ii = (int*)b;
                imax = *ii++;
                jmax = *ii++;

                float* f = (float*)ii;
                res = *(f + i.Value * imax + j.Value);
            }

            return res;
        }
#else
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble MatrixItem(SqlBinary v, SqlInt32 i, SqlInt32 j)
        {
            int cols = BitConverter.ToInt32(v.Value, 0);

            return new SqlDouble(BitConverter.ToDouble(v.Value, sizeof(int) + sizeof(int) + sizeof(double) * (i.Value * cols + j.Value)));
        }
#endif

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 MatrixCols(SqlBinary v)
        {
            return BitConverter.ToInt32(v.Value, sizeof(int));
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 MatrixRows(SqlBinary v)
        {
            return BitConverter.ToInt32(v.Value, 0);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixRow(SqlBinary matrix, SqlInt32 i)
        {
            return VectorConverter.ToBinary(MatrixConverter.ToArray(matrix.Value)[i.Value]);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixCol(SqlBinary matrix, SqlInt32 j)
        {
            double[][] m = MatrixConverter.ToArray(matrix.Value);
            double[] v = new double[m.Length];
            for (int i = 0; i < v.Length; i++)
                v[i] = m[i][j.Value];
            
            return VectorConverter.ToBinary(v);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlString MatrixToString(SqlBinary v)
        {
            double[][] m = ToArray(v.Value);
            string res = "";
            
            for (int i = 0; i < m.Length; i++)
            {
                string line = "";
                for (int j = 0; j < m[i].Length; j++)
                    line += " " + m[i][j].ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

                res += "{" + line.Substring(1) + "}";
            }

            return new SqlString("{" + res + "}");
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixFromString(SqlString str)
        {
            int p = 0;
            return ToBinary(ReadMatrix(str.Value, ref p));
        }

        /*
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBinary MatrixFromInequalities(SqlString expression, SqlString variableNames)
        {
            int dim = 1;
            for (int i = 0; i < variableNames.Value.Length; i++)
                if (variableNames.Value[i] == ',') dim++;
            
            Parser.Parser p = new Parser.Parser(new Parser.Tokenizer(), new Parser.VariableMapper(variableNames.Value));
            InequalitySystem ls = p.Parse(expression.Value, dim);

            return ToBinary(ls.GetInhomogenMatrix());
        }
         * */

#if unsafe
        unsafe internal static byte[] ToBinary(float[][] data)
        {
            int size = data.Length * data[0].Length;
            byte[] buffer = new byte[(size << 2) + 8];

            fixed (byte* b = buffer)
            {
                int* ii = (int*)b;
                *(ii++) = data.Length;
                *(ii++) = data[0].Length;

                float* f = (float*)ii;
                for (int i = 0; i < data.Length; i++)
                    for (int j = 0; j < data[0].Length; j++)
                        *(f++) = data[i][j];
            }

            return buffer;
        }
#else
        internal static byte[] ToBinary(double[][] data)
        {
            int size = data.Length * data[0].Length;
            byte[] buffer = new byte[size * sizeof(double) + sizeof(int) + sizeof(int) ];

            Array.Copy(BitConverter.GetBytes(data.Length), 0, buffer, 0, sizeof(int));
            Array.Copy(BitConverter.GetBytes(data[0].Length), 0, buffer, sizeof(int), sizeof(int));

            int q = sizeof(int) + sizeof(int);
            for (int i = 0; i < data.Length; i ++)
                for (int j = 0; j < data[i].Length; j++)
                {
                    Array.Copy(BitConverter.GetBytes(data[i][j]), 0, buffer, q, sizeof(double));
                    q += sizeof(double);
                }

            return buffer;
        }
#endif

        internal static double[][] ToArray(SqlBinary data)
        {
            return ToArray(data.Value);
        }

#if unsafe
        unsafe internal static float[][] ToArray(byte[] data)
        {
            float[][] array;
            int imax, jmax;

            fixed (byte* b = data)
            {
                int* ii = (int*)b;
                imax = *(ii++);
                jmax = *(ii++);

                float* f = (float*)ii;
                array = new float[imax][];
                for (int i = 0; i < imax; i++)
                {
                    array[i] = new float[jmax];
                    for (int j = 0; j < jmax; j++)
                        array[i][j] = *(f++);
                }
            }

            return array;
        }
#else
        internal static double[][] ToArray(byte[] data)
        {
            double[][] array;
            int rows, cols;

            rows = BitConverter.ToInt32(data, 0);
            cols = BitConverter.ToInt32(data, sizeof(int));

            int q = sizeof(int) + sizeof(int);
            array = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    array[i][j] = BitConverter.ToDouble(data, q);
                    q += sizeof(double);
                }
            }

            return array;
        }
#endif
    }
}
#region Revision History
/* Revision History

        $Log: MatrixConverter.cs,v $
        Revision 1.1  2008/01/08 22:01:33  dobos
        Initial checkin


*/
#endregion
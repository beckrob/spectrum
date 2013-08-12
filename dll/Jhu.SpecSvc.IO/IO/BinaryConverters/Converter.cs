#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Converter.cs,v 1.1 2008/01/08 22:01:32 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:32 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class Converter
    {
        public static double[] ReadVector(string s, ref int p)
        {
            List<double> res = new List<double>();

            SkipWhitespaces(s, ref p);
            if (s[p] != '{')
                throw new FormatException(p, "Vector must begin with '{'.");
            p++;
            SkipWhitespaces(s, ref p);

            while (s[p] != '}')
            {
                res.Add(ReadDouble(s, ref p));
                SkipWhitespaces(s, ref p);
            }
            p++;

            return res.ToArray();
        }

        public static double[][] ReadMatrix(string s, ref int p)
        {
            List<double[]> res = new List<double[]>();

            SkipWhitespaces(s, ref p);
            if (s[p] != '{')
                throw new FormatException(p, "Matrix must begin with '{'.");
            p++;
            SkipWhitespaces(s, ref p);

            while (s[p] != '}')
            {
                res.Add(ReadVector(s, ref p));
                SkipWhitespaces(s, ref p);
            }
            p++;

            return res.ToArray();
        }

        protected static double ReadDouble(string s, ref int p)
        {
            // skipping whitespaces done in caller function
            string num = "";

            while (p < s.Length)
            {
                if (s[p] == '-' || s[p] == '+')
                {
                    num += s[p];
                    p++;
                    SkipWhitespaces(s, ref p);

                }

                if (!char.IsWhiteSpace(s[p]) && s[p] != '}')
                {
                    num += s[p];
                    p++;
                }
                else
                    break;
            }

            return double.Parse(num, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        protected static void SkipWhitespaces(string s, ref int p)
        {
            while (p < s.Length && char.IsWhiteSpace(s[p])) p++;
            if (p >= s.Length)
                throw new FormatException(p, "Unexpected end of string.");
        }
    }
}
#region Revision History
/* Revision History

        $Log: Converter.cs,v $
        Revision 1.1  2008/01/08 22:01:32  dobos
        Initial checkin


*/
#endregion
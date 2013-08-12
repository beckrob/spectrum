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
 *   ID:          $Id: Unit.cs,v 1.1 2008/01/08 22:27:00 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:27:00 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jhu.SpecSvc.Schema.Units
{
    public class Unit
    {
        private Multiplier multiplier;
        private List<UnitPart> parts;

        public Multiplier Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        public List<UnitPart> Parts
        {
            get { return parts; }
        }

        public Unit()
        {
            InitializeMembers();
        }

        public Unit(Unit old)
        {
            CopyMembers(old);
        }

        public Unit(string unit)
        {
            InitializeMembers();
            Parse(unit);
        }

        private void InitializeMembers()
        {
            this.multiplier = new Multiplier(1.0);
            this.parts = new List<UnitPart>();
        }

        private void CopyMembers(Unit old)
        {
            this.multiplier = old.multiplier;
            this.parts = new List<UnitPart>(old.parts);
        }

        public void Parse(string unit)
        {
            string[] unitparts = unit.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (unitparts.Length == 0)
                throw new ArgumentException("Invalid unit format: no parts to parse.");

            int part = 0;

            // try parsing the 0th element, which may be a number
            string[] numparts = unitparts[0].Split('e', 'E');
            if (numparts.Length == 1)
            {
                // simple notation
                double m;
                if (double.TryParse(numparts[0], out m))
                {
                    multiplier = new Multiplier(m);
                    part++;
                }
            }
            else
            {
                // exponential notation
                double m;
                int e;
                if (double.TryParse(numparts[0], out m) && int.TryParse(numparts[1], out e))
                {
                    multiplier = new Multiplier(m, e);
                    part++;
                }
            }

            // if no multiplier found, it will be set to 1 with 1 exponent
            if (part == 0)
            {
                multiplier = new Multiplier(1.0);
            }

            // adding parts
            while (part < unitparts.Length)
            {
                UnitPart up;

                int pos = unitparts[part].IndexOf('+');
                int neg = unitparts[part].IndexOf('-');

                if (pos < 0 && neg < 0)
                {
                    // symbol only, no exponent
                    parts.Add(new UnitPart(unitparts[part]));
                }
                else if (pos >= 0)
                {
                    parts.Add(new UnitPart(unitparts[part].Substring(0, pos), int.Parse(unitparts[part].Substring(pos + 1))));
                }
                else if (neg >= 0)
                {
                    parts.Add(new UnitPart(unitparts[part].Substring(0, neg), -int.Parse(unitparts[part].Substring(neg + 1))));
                }

                part++;
            }
        }

        public override string ToString()
        {
            string res = string.Empty;

            if (multiplier.Mantissa != 1.0)
            {
                if (multiplier.Exponent == 1)
                {
                    res += " " + multiplier.Mantissa.ToString();
                }
                else
                {
                    res += " " + multiplier.Mantissa.ToString() + "e" + (multiplier.Exponent < 0 ? "-" : "+") + Math.Abs(multiplier.Exponent).ToString();
                }
            }

            foreach (UnitPart u in parts)
            {
                if (u.Exponent == 1)
                {
                    res += " " + u.Symbol;
                }
                else
                {
                    res += " " + u.Symbol + (u.Exponent < 0 ? "-" : "+") + Math.Abs(u.Exponent).ToString();
                }
            }

            return res.Substring(1);
        }

        public string FormatHtml()
        {
            string res = string.Empty;

            if (multiplier.Mantissa != 1.0)
            {
                if (multiplier.Exponent == 1)
                {
                    res += " " + multiplier.Mantissa.ToString();
                }
                else
                {
                    res += " " + multiplier.Mantissa.ToString() + "<sup>" + (multiplier.Exponent < 0 ? "-" : "") + Math.Abs(multiplier.Exponent).ToString() + "</sup>";
                }
            }

            foreach (UnitPart u in parts)
            {
                if (u.Exponent == 1)
                {
                    res += " " + u.Symbol;
                }
                else
                {
                    res += " " + u.Symbol + "<sup>" + (u.Exponent < 0 ? "-" : "") + Math.Abs(u.Exponent).ToString() + "</sup>";
                }
            }

            return res.Substring(1);
        }

        public string FormatVOTable()
        {
            string res = string.Empty;

            if (multiplier.Mantissa != 1.0)
            {
                if (multiplier.Exponent == 1)
                {
                    res += " " + multiplier.Mantissa.ToString();
                }
                else
                {
                    res += " " + multiplier.Mantissa.ToString() + "**(" + (multiplier.Exponent < 0 ? "-" : "+") + Math.Abs(multiplier.Exponent).ToString() + ")";
                }
            }

            foreach (UnitPart u in parts)
            {
                if (u.Exponent == 1)
                {
                    res += " " + u.Symbol;
                }
                else
                {
                    res += " " + u.Symbol + "**(" + (u.Exponent < 0 ? "-" : "+") + Math.Abs(u.Exponent).ToString() + ")";
                }
            }

            return res.Substring(1);
        }
    }
}
#region Revision History
/* Revision History

        $Log: Unit.cs,v $
        Revision 1.1  2008/01/08 22:27:00  dobos
        Initial checkin


*/
#endregion
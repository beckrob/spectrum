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
 *   ID:          $Id: Position.cs,v 1.1 2008/01/08 22:26:20 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:20 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema
{
    [Serializable]
    public struct Position
    {
        private double ra;
        private double dec;

        public Position(double ra, double dec)
        {
            this.ra = ra;
            this.dec = dec;
        }

        public double Ra
        {
            get { return ra; }
            set { ra = value; }
        }

        public double Dec
        {
            get { return dec; }
            set { dec = value; }
        }

        public override string ToString()
        {
            return this.ra.ToString("##0.000000") + " " + this.dec.ToString("##0.000000"); ;
        }

        public void ParseSelf(string value)
        {
            // parsing two decimal values separated by a whitespace
            string[] vals = value.Split(new char[] { ' ', ',', '\t', '\r', '\n' });
            if (vals.Length != 2)
            {
                throw new FormatException("Two values needed");
            }

            try
            {
                this.ra = double.Parse(vals[0]);
                this.dec = double.Parse(vals[1]);
            }
            catch
            {
                throw new FormatException("Invalid numeric format.");
            }
        }

        public static Position Parse(string value)
        {
            Position p = new Position();
            p.ParseSelf(value);

            return p;
        }
    }
}
#region Revision History
/* Revision History

        $Log: Position.cs,v $
        Revision 1.1  2008/01/08 22:26:20  dobos
        Initial checkin


*/
#endregion
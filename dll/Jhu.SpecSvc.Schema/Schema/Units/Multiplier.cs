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
 *   ID:          $Id: Multiplier.cs,v 1.1 2008/01/08 22:26:59 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:59 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema.Units
{
    public struct Multiplier
    {
        public double Mantissa;
        public int Exponent;

        public Multiplier(double mantissa)
        {
            Mantissa = mantissa;
            Exponent = 1;
        }

        public Multiplier(double mantissa, int exponent)
        {
            Mantissa = mantissa;
            Exponent = exponent;
        }
    }
}
#region Revision History
/* Revision History

        $Log: Multiplier.cs,v $
        Revision 1.1  2008/01/08 22:26:59  dobos
        Initial checkin


*/
#endregion
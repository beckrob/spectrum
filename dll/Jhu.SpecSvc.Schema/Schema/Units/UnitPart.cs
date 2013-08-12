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
 *   ID:          $Id: UnitPart.cs,v 1.1 2008/01/08 22:27:00 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:27:00 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema.Units
{
    public struct UnitPart
    {
        public string Symbol;
        public int Exponent;

        public UnitPart(string symbol)
        {
            Symbol = symbol;
            Exponent = 1;
        }

        public UnitPart(string symbol, int exponent)
        {
            Symbol = symbol;
            Exponent = exponent;
        }
    }
}
#region Revision History
/* Revision History

        $Log: UnitPart.cs,v $
        Revision 1.1  2008/01/08 22:27:00  dobos
        Initial checkin


*/
#endregion
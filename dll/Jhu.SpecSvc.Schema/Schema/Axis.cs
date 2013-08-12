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
 *   ID:          $Id: Axis.cs,v 1.1 2008/01/08 22:26:15 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:15 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema
{
    public class Axis : Group
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string CALIBRATED = "CALIBRATED";
        public const string ARBITRARY = "ARBITRARY";
        public const string NORMALIZED = "NORMALIZED";

        public Axis()
        {
        }
    }
}
#region Revision History
/* Revision History

        $Log: Axis.cs,v $
        Revision 1.1  2008/01/08 22:26:15  dobos
        Initial checkin


*/
#endregion
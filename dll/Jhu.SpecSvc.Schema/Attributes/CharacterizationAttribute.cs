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
 *   ID:          $Id: CharacterizationAttribute.cs,v 1.1 2008/01/08 22:26:14 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:14 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class CharacterizationAttribute : System.Attribute
    {
        public CharacterizationAttribute()
        {
        }
    }
}
#region Revision History
/* Revision History

        $Log: CharacterizationAttribute.cs,v $
        Revision 1.1  2008/01/08 22:26:14  dobos
        Initial checkin


*/
#endregion
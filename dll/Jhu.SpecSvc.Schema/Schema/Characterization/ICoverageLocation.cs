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
 *   ID:          $Id: ICoverageLocation.cs,v 1.1 2008/01/08 22:26:36 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:36 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema.Characterization
{
	[Characterization]
	public interface ICoverageLocation
	{
		ParamBase Value { get;set;}
	}
}
#region Revision History
/* Revision History

        $Log: ICoverageLocation.cs,v $
        Revision 1.1  2008/01/08 22:26:36  dobos
        Initial checkin


*/
#endregion
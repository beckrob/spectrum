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
 *   ID:          $Id: ISamplingPrecisionRefVal.cs,v 1.1 2008/01/08 22:26:39 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:39 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema.Characterization
{
	[Characterization]
	public interface ISamplingPrecisionRefVal
	{
		DoubleParam FillFactor { get; set; }
	}
}
#region Revision History
/* Revision History

        $Log: ISamplingPrecisionRefVal.cs,v $
        Revision 1.1  2008/01/08 22:26:39  dobos
        Initial checkin


*/
#endregion
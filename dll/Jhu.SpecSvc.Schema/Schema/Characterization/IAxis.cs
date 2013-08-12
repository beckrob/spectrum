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
 *   ID:          $Id: IAxis.cs,v 1.1 2008/01/08 22:26:33 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:33 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Schema.Characterization
{
	[Characterization]
	public interface IAxis
	{
        TextParam Name { get; set; }
        TextParam Ucd { get; set; }
        TextParam Unit { get; set; }

        TextParam Calibration { get; set; }
        DoubleParam Resolution { get; set; }
        DoubleParam ResPower { get; set; }

		//DoubleArrayParam Value { get; set; }
		IAccuracy Accuracy { get; set; }
		ICoverage Coverage { get; set; }
		ISamplingPrecision SamplingPrecision { get; set; }
	}
}
#region Revision History
/* Revision History

        $Log: IAxis.cs,v $
        Revision 1.1  2008/01/08 22:26:33  dobos
        Initial checkin


*/
#endregion
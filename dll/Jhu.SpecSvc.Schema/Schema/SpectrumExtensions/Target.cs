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
 *   ID:          $Id: Target.cs,v 1.1 2008/01/08 22:26:42 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:42 $
 */
#endregion
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public partial class Target : Group, ICloneable
	{
		private DoubleParam galacticExtinction;
		
        /// <summary>
        /// Galactic extinction at the coordinates of the target
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Custom)]
		public DoubleParam GalacticExtinction
		{
			get { return galacticExtinction; }
			set { galacticExtinction = value; }
		}
		
	}
}
#region Revision History
/* Revision History

        $Log: Target.cs,v $
        Revision 1.1  2008/01/08 22:26:42  dobos
        Initial checkin


*/
#endregion
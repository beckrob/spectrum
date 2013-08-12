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
 *   ID:          $Id: SpatialCoverage.cs,v 1.1 2008/01/08 22:26:48 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:48 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
	public class SpatialCoverage : Group, ICloneable, ICoverage
	{
		private SpatialCoverageLocation location;
		private SpatialCoverageBounds bounds;
		private SpatialCoverageSupport support;

		[XmlElement]
		[Field(Required = ParamRequired.Mandatory)]
		public SpatialCoverageLocation Location
		{
			get { return location; }
			set { location = value; }
		}

		ICoverageLocation ICoverage.Location
		{
			get { return location; }
			set { location = (SpatialCoverageLocation)value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Mandatory)]
		public SpatialCoverageBounds Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		ICoverageBounds ICoverage.Bounds
		{
			get { return bounds; }
			set { bounds = (SpatialCoverageBounds)value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Recommended)]
		public SpatialCoverageSupport Support
		{
			get { return support; }
			set { support = value; }
		}

		ICoverageSupport ICoverage.Support
		{
			get { return support; }
			set { support = (SpatialCoverageSupport)value; }
		}

		#region Constructors
		public SpatialCoverage()
		{
		}

		public SpatialCoverage(ParamRequired initializationLevel)
		{
			SchemaUtil.InitializeMembers(this, initializationLevel, true);
		}

		public SpatialCoverage(SpatialCoverage old)
		{
			SchemaUtil.CopyMembers(this, old);
		}
		#endregion

		#region Clone functions
		public object Clone()
		{
			return Clone(this);
		}

		public static SpatialCoverage Clone(SpatialCoverage old)
		{
			if (old != null)
			{
				return new SpatialCoverage(old);
			}
			else
				return null;
		}
		#endregion
	}
}
#region Revision History
/* Revision History

        $Log: SpatialCoverage.cs,v $
        Revision 1.1  2008/01/08 22:26:48  dobos
        Initial checkin


*/
#endregion
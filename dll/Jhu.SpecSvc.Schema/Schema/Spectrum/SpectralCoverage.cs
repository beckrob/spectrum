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
 *   ID:          $Id: SpectralCoverage.cs,v 1.1 2008/01/08 22:26:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:50 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpectralCoverage : Group, ICloneable, ICoverage
    {
		private SpectralCoverageLocation location;
		private SpectralCoverageBounds bounds;
		private SpectralCoverageSupport support;

		[XmlElement]
		
		[Field(Required = ParamRequired.Mandatory)]
		public SpectralCoverageLocation Location
		{
			get { return location; }
			set { location = value; }
		}

		ICoverageLocation ICoverage.Location
		{
			get { return location; }
			set { location = (SpectralCoverageLocation)value; }
		}

		[XmlElement]
		
		[Field(Required = ParamRequired.Mandatory)]
		public SpectralCoverageBounds Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		ICoverageBounds ICoverage.Bounds
		{
			get { return bounds; }
			set { bounds = (SpectralCoverageBounds) value; }
		}

		[XmlElement]
		
		[Field(Required = ParamRequired.Optional)]
		public SpectralCoverageSupport Support
		{
			get { return support; }
			set { support = value; }
		}

		ICoverageSupport ICoverage.Support
		{
			get { return support; }
			set { support = (SpectralCoverageSupport) value; }
		}

        #region Constructors
        public SpectralCoverage()
        {
        }

        public SpectralCoverage(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralCoverage(SpectralCoverage old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralCoverage Clone(SpectralCoverage old)
        {
            if (old != null)
            {
                return new SpectralCoverage(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralCoverage.cs,v $
        Revision 1.1  2008/01/08 22:26:50  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: TimeCoverage.cs,v 1.1 2008/01/08 22:26:55 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:55 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class TimeCoverage : Group, ICloneable, ICoverage
    {
		private TimeCoverageLocation location;
		private TimeCoverageBounds bounds;
		private TimeCoverageSupport support;

		[XmlElement]
		[Field(Required = ParamRequired.Mandatory)]
		public TimeCoverageLocation Location
		{
			get { return location; }
			set { location = value; }
		}

		ICoverageLocation ICoverage.Location
		{
			get { return location; }
			set { location = (TimeCoverageLocation)value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Mandatory)]
		public TimeCoverageBounds Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		ICoverageBounds ICoverage.Bounds
		{
			get { return bounds; }
			set { bounds = (TimeCoverageBounds)value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TimeCoverageSupport Support
		{
			get { return support; }
			set { support = value; }
		}

		ICoverageSupport ICoverage.Support
		{
			get { return support; }
			set { support = (TimeCoverageSupport)value; }
		}

        #region Constructors
        public TimeCoverage()
        {
        }

        public TimeCoverage(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeCoverage(TimeCoverage old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeCoverage Clone(TimeCoverage old)
        {
            if (old != null)
            {
                return new TimeCoverage(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeCoverage.cs,v $
        Revision 1.1  2008/01/08 22:26:55  dobos
        Initial checkin


*/
#endregion
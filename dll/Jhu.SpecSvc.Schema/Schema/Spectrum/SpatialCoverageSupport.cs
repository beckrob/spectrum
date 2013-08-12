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
 *   ID:          $Id: SpatialCoverageSupport.cs,v 1.1 2008/01/08 22:26:49 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:49 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpatialCoverageSupport : Group, ICloneable, ICoverageSupport
    {
		private TextParam area;
		private DoubleParam extent;

        /// <summary>
        /// Aperture region, STC string
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Recommended, Ucd = "stat.fill;pos.eq")]
		public TextParam Area
		{
			get { return area; }
			set { area = value; }
		}

        /// <summary>
        /// Aperture area
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "instr.fov", DefaultUnit = "deg+2")]
		public DoubleParam Extent
		{
			get { return extent; }
			set { extent = value; }
		}

        #region Constructors
        public SpatialCoverageSupport()
        {
        }

        public SpatialCoverageSupport(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpatialCoverageSupport(SpatialCoverageSupport old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpatialCoverageSupport Clone(SpatialCoverageSupport old)
        {
            if (old != null)
            {
                return new SpatialCoverageSupport(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpatialCoverageSupport.cs,v $
        Revision 1.1  2008/01/08 22:26:49  dobos
        Initial checkin


*/
#endregion
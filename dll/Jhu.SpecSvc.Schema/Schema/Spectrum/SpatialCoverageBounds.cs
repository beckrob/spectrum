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
 *   ID:          $Id: SpatialCoverageBounds.cs,v 1.1 2008/01/08 22:26:48 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:48 $
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
    public class SpatialCoverageBounds : Group, ICloneable, ICoverageBounds
    {
		private DoubleParam extent;

        /// <summary>
        /// Aperture angular diameter
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Mandatory, Ucd = "instr.fov", DefaultUnit = SpatialAxis.COMMONUNIT)]
		public DoubleParam Extent
		{
			get { return extent; }
			set { extent = value; }
		}

		DoubleParam ICoverageBounds.Start
		{
			get { return null; }
			set { }
		}

		DoubleParam ICoverageBounds.Stop
		{
			get { return null; }
			set { }
		}

        #region Constructors
        public SpatialCoverageBounds()
        {
        }

        public SpatialCoverageBounds(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpatialCoverageBounds(SpatialCoverageBounds old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpatialCoverageBounds Clone(SpatialCoverageBounds old)
        {
            if (old != null)
            {
                return new SpatialCoverageBounds(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpatialCoverageBounds.cs,v $
        Revision 1.1  2008/01/08 22:26:48  dobos
        Initial checkin


*/
#endregion
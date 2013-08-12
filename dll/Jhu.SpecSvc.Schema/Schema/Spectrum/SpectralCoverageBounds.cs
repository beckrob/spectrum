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
 *   ID:          $Id: SpectralCoverageBounds.cs,v 1.1 2008/01/08 22:26:51 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:51 $
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
    public class SpectralCoverageBounds : Group, ICloneable, ICoverageBounds
    {
		private DoubleParam extent;
		private DoubleParam start;
		private DoubleParam stop;

        /// <summary>
        /// Width of the spectrum in A or other spec coord
        /// </summary>
		[XmlElement]
		
		[Field(Required = ParamRequired.Mandatory, Ucd = "instr.bandpass", DefaultUnit = SpectralAxis.COMMONUNIT)]
		public DoubleParam Extent
		{
			get { return extent; }
			set { extent = value; }
		}

        /// <summary>
        /// Start in spectral coordinate
        /// </summary>
		[XmlElement]
		
		[Field(Required = ParamRequired.Mandatory, Ucd = "em.wl;stat.min", DefaultUnit = SpectralAxis.COMMONUNIT)]
		public DoubleParam Start
		{
			get { return start; }
			set { start = value; }
		}

        /// <summary>
        /// Stop in spectral coordinate
        /// </summary>
        [XmlElement]
        
        [Field(Required = ParamRequired.Mandatory, Ucd = "em.wl;stat.max", DefaultUnit = SpectralAxis.COMMONUNIT)]
        public DoubleParam Stop
		{
			get { return stop; }
			set { stop = value; }
		}

        #region Constructors
        public SpectralCoverageBounds()
        {
        }

        public SpectralCoverageBounds(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralCoverageBounds(SpectralCoverageBounds old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralCoverageBounds Clone(SpectralCoverageBounds old)
        {
            if (old != null)
            {
                return new SpectralCoverageBounds(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralCoverageBounds.cs,v $
        Revision 1.1  2008/01/08 22:26:51  dobos
        Initial checkin


*/
#endregion
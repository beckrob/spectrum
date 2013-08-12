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
 *   ID:          $Id: TimeCoverageBounds.cs,v 1.1 2008/01/08 22:26:55 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:55 $
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
    public class TimeCoverageBounds : Group, ICloneable, ICoverageBounds
    {
		private DoubleParam extent;
		private DoubleParam start;
		private DoubleParam stop;

        /// <summary>
        /// Total exposure time
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Mandatory, Ucd = "time.duration", DefaultUnit = TimeAxis.COMMONUNIT)]
		public DoubleParam Extent
		{
			get { return extent; }
			set { extent = value; }
		}

        /// <summary>
        /// Start in spectral coordinate
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Recommended, Ucd = "time.start;obs.exposure")]
		public DoubleParam Start
		{
			get { return start; }
			set { start = value; }
		}

        /// <summary>
        /// Stop in spectral coordinate
        /// </summary>
		[XmlElement]

		[Field(Required = ParamRequired.Recommended, Ucd = "time.stop;obs.exposure")]
		public DoubleParam Stop
		{
			get { return stop; }
			set { stop = value; }
		}

        #region Constructors
        public TimeCoverageBounds()
        {
        }

        public TimeCoverageBounds(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeCoverageBounds(TimeCoverageBounds old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeCoverageBounds Clone(TimeCoverageBounds old)
        {
            if (old != null)
            {
                return new TimeCoverageBounds(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeCoverageBounds.cs,v $
        Revision 1.1  2008/01/08 22:26:55  dobos
        Initial checkin


*/
#endregion
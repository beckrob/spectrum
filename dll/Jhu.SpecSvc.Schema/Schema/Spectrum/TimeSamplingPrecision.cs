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
 *   ID:          $Id: TimeSamplingPrecision.cs,v 1.1 2008/01/08 22:26:56 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:56 $
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
    public class TimeSamplingPrecision : Group, ICloneable, ISamplingPrecision
    {
		private DoubleParam sampleExtent;
        private TimeSamplingPrecisionRefVal samplingPrecisionRefVal;

        /// <summary>
		/// Wavelength bin size
        /// </summary>
        [XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "time.interval", DefaultUnit = TimeAxis.COMMONUNIT)]
        public DoubleParam SampleExtent
		{
			get { return sampleExtent; }
			set { sampleExtent = value; }
		}

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode = SerializationMode.Characterization)]
        public TimeSamplingPrecisionRefVal SamplingPrecisionRefVal
        {
            get { return samplingPrecisionRefVal; }
            set { samplingPrecisionRefVal = value; }
        }

        [XmlIgnore]
        ISamplingPrecisionRefVal ISamplingPrecision.SamplingPrecisionRefVal
        {
            get { return samplingPrecisionRefVal; }
            set { samplingPrecisionRefVal = (TimeSamplingPrecisionRefVal)value; }
        }

        #region Constructors
        public TimeSamplingPrecision()
        {
        }

        public TimeSamplingPrecision(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeSamplingPrecision(TimeSamplingPrecision old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeSamplingPrecision Clone(TimeSamplingPrecision old)
        {
            if (old != null)
            {
                return new TimeSamplingPrecision(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeSamplingPrecision.cs,v $
        Revision 1.1  2008/01/08 22:26:56  dobos
        Initial checkin


*/
#endregion
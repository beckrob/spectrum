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
 *   ID:          $Id: SpectralSamplingPrecision.cs,v 1.1 2008/01/08 22:26:53 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:53 $
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
    public class SpectralSamplingPrecision : Group, ICloneable, ISamplingPrecision
    {
		private DoubleParam sampleExtent;
        private SpectralSamplingPrecisionRefVal samplingPrecisionRefVal;

        /// <summary>
		/// Wavelength bin size
        /// </summary>
        [XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "em.wl;spect.binSize", DefaultUnit = SpectralAxis.COMMONUNIT)]
        public DoubleParam SampleExtent
		{
			get { return sampleExtent; }
			set { sampleExtent = value; }
		}

        [XmlElement]
        [Field(Required = ParamRequired.Optional, SerializationMode = SerializationMode.Characterization)]
        public SpectralSamplingPrecisionRefVal SamplingPrecisionRefVal
        {
            get { return samplingPrecisionRefVal; }
            set { samplingPrecisionRefVal = value; }
        }

        [XmlIgnore]
        ISamplingPrecisionRefVal ISamplingPrecision.SamplingPrecisionRefVal
        {
            get { return samplingPrecisionRefVal; }
            set { samplingPrecisionRefVal = (SpectralSamplingPrecisionRefVal)value; }
        }

        #region Constructors
        public SpectralSamplingPrecision()
        {
        }

        public SpectralSamplingPrecision(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

		public SpectralSamplingPrecision(SpectralSamplingPrecision old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

		public static SpectralSamplingPrecision Clone(SpectralSamplingPrecision old)
        {
            if (old != null)
            {
				return new SpectralSamplingPrecision(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralSamplingPrecision.cs,v $
        Revision 1.1  2008/01/08 22:26:53  dobos
        Initial checkin


*/
#endregion
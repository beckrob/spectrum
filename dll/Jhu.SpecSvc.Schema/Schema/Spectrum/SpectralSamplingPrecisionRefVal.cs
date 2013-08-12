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
 *   ID:          $Id: SpectralSamplingPrecisionRefVal.cs,v 1.1 2008/01/08 22:26:53 dobos Exp $
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
    public class SpectralSamplingPrecisionRefVal : Group, ICloneable, ISamplingPrecisionRefVal
    {
		private DoubleParam fillFactor;

        /// <summary>
		/// Wavelength fill factor
        /// </summary>
        [XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "stat.fill;em.wl", DefaultValue = 1.0)]
        public DoubleParam FillFactor
		{
			get { return fillFactor; }
			set { fillFactor = value; }
		}

        #region Constructors
        public SpectralSamplingPrecisionRefVal()
        {
        }

        public SpectralSamplingPrecisionRefVal(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralSamplingPrecisionRefVal(SpectralSamplingPrecisionRefVal old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralSamplingPrecisionRefVal Clone(SpectralSamplingPrecisionRefVal old)
        {
            if (old != null)
            {
                return new SpectralSamplingPrecisionRefVal(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralSamplingPrecisionRefVal.cs,v $
        Revision 1.1  2008/01/08 22:26:53  dobos
        Initial checkin


*/
#endregion
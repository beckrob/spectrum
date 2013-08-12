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
 *   ID:          $Id: SpatialSamplingPrecisionRefVal.cs,v 1.1 2008/01/08 22:26:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:50 $
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
    public class SpatialSamplingPrecisionRefVal : Group, ICloneable, ISamplingPrecisionRefVal
    {
		private DoubleParam fillFactor;

        /// <summary>
		/// Wavelength bin size
        /// </summary>
        [XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "stat.fill;pos.eq", DefaultValue = 1.0)]
        public DoubleParam FillFactor
		{
			get { return fillFactor; }
			set { fillFactor = value; }
		}

        #region Constructors
        public SpatialSamplingPrecisionRefVal()
        {
        }

        public SpatialSamplingPrecisionRefVal(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpatialSamplingPrecisionRefVal(SpatialSamplingPrecisionRefVal old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpatialSamplingPrecisionRefVal Clone(SpatialSamplingPrecisionRefVal old)
        {
            if (old != null)
            {
                return new SpatialSamplingPrecisionRefVal(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpatialSamplingPrecisionRefVal.cs,v $
        Revision 1.1  2008/01/08 22:26:50  dobos
        Initial checkin


*/
#endregion
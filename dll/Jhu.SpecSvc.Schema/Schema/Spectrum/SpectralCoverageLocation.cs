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
 *   ID:          $Id: SpectralCoverageLocation.cs,v 1.1 2008/01/08 22:26:51 dobos Exp $
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
    public class SpectralCoverageLocation : Group, ICloneable, ICoverageLocation
    {
		private TextParam value;

        /// <summary>
        /// Spectral coord value
        /// </summary>
		[XmlElement]
		
		[Field(Required = ParamRequired.Mandatory, Ucd = "instr.bandpass", DefaultUnit = SpectralAxis.COMMONUNIT)]
		public TextParam Value
		{
			get { return value; }
			set { this.value = value; }
		}

        [XmlIgnore]
        ParamBase ICoverageLocation.Value
        {
            get { return this.value; }
            set { this.value = (TextParam)value; }
        }

        #region Constructors
        public SpectralCoverageLocation()
        {
        }

        public SpectralCoverageLocation(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralCoverageLocation(SpectralCoverageLocation old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralCoverageLocation Clone(SpectralCoverageLocation old)
        {
            if (old != null)
            {
                return new SpectralCoverageLocation(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralCoverageLocation.cs,v $
        Revision 1.1  2008/01/08 22:26:51  dobos
        Initial checkin


*/
#endregion
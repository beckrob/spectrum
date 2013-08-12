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
 *   ID:          $Id: SpectralCoverageSupport.cs,v 1.1 2008/01/08 22:26:52 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:52 $
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
    public class SpectralCoverageSupport : Group, ICloneable, ICoverageSupport
    {
		private DoubleParam extent;

        [XmlIgnore]
        TextParam ICoverageSupport.Area
        {
            get { return null; }
            set {  }
        }

        /// <summary>
        /// Effective width of spectrum in A
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "instr.bandwidth", DefaultUnit = "A")]
		public DoubleParam Extent
		{
			get { return extent; }
			set { extent = value; }
		}

        #region Constructors
        public SpectralCoverageSupport()
        {
        }

        public SpectralCoverageSupport(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralCoverageSupport(SpectralCoverageSupport old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralCoverageSupport Clone(SpectralCoverageSupport old)
        {
            if (old != null)
            {
                return new SpectralCoverageSupport(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralCoverageSupport.cs,v $
        Revision 1.1  2008/01/08 22:26:52  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: SpectralFrame.cs,v 1.1 2008/01/08 22:26:52 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:52 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpectralFrame : Group, ICloneable
    {
		private TextParam name;
		private TextParam ucd;
		private TextParam refPos;
		private DoubleParam redshift;

        /// <summary>
        /// Spectral frame name
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TextParam Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// Spectral frame UCD
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TextParam UCD
		{
			get { return ucd; }
			set { ucd = value; }
		}

        /// <summary>
        /// Spectral frame origin
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, DefaultValue = CoordSys.TOPOCENTER)]
		public TextParam RefPos
		{
			get { return refPos; }
			set { refPos = value; }
		}

        /// <summary>
        /// If restframe corrected, provide only when Name=CUSTOM
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, DefaultValue = 0.0)]
		public DoubleParam Redshift
		{
			get { return redshift; }
			set { redshift = value; }
		}


#region Constructors
        public SpectralFrame()
        {
        }

        public SpectralFrame(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralFrame(SpectralFrame old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralFrame Clone(SpectralFrame old)
        {
            if (old != null)
            {
                return new SpectralFrame(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralFrame.cs,v $
        Revision 1.1  2008/01/08 22:26:52  dobos
        Initial checkin


*/
#endregion
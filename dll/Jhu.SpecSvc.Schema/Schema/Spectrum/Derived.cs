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
 *   ID:          $Id: Derived.cs,v 1.1 2008/01/08 22:26:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:45 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class Derived : Group, ICloneable
	{
		private DoubleParam snr;
		private Redshift redshift;
        private VelocityDispersion velocityDispersion;
		private DoubleParam varAmpl;
        private DoubleParam eigenvalue;

        /// <summary>
        /// Signal-to-noise for spectrum
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.SpecService, Ucd = "stat.snr")]
		public DoubleParam SNR
		{
			get { return snr; }
			set { snr = value; }
		}

        /// <summary>
        /// Frame of the spectrum (not the target object)
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public Redshift Redshift
		{
			get { return redshift; }
			set { redshift = value; }
		}

        [XmlElement]
        [Field(Required = ParamRequired.Optional)]
        public VelocityDispersion VelocityDispersion
        {
            get { return velocityDispersion; }
            set { velocityDispersion = value; }
        }

        /// <summary>
        /// Variability amplitude as fraction of mean
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.SpecService, Ucd = "src.var.amplitude;arith.ratio")]
		public DoubleParam VarAmpl
		{
			get { return varAmpl; }
			set { varAmpl = value; }
		}

        /// <summary>
        /// Eigenvalue, used for PCA components
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Derived, Ucd = "")]
        public DoubleParam Eigenvalue
        {
            get { return eigenvalue; }
            set { eigenvalue = value; }
        }

#region Constructors
        public Derived()
        {
        }

        public Derived(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Derived(Derived old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Derived Clone(Derived old)
        {
            if (old != null)
            {
                return new Derived(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Derived.cs,v $
        Revision 1.1  2008/01/08 22:26:45  dobos
        Initial checkin


*/
#endregion
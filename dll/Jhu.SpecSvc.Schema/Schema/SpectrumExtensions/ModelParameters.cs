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
 *   ID:          $Id: ModelParameters.cs,v 1.1 2008/01/08 22:26:40 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:40 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class ModelParameters : Group, ICloneable, IDataCube
    {
        private AgeAxis ageAxis;
        private SfrAxis sfrAxis;
        private StellarMetAxis stellarMetAxis;
        private GasMetAxis gasMetAxis;

        private DoubleParam z_met;
        private DoubleParam t_eff;
        private DoubleParam log_g;
        private DoubleParam tau_V0;
        private DoubleParam mu;
        private DoubleParam t_form;
        private DoubleParam gamma;
        private IntParam n_bursts;
        private DoubleParam age;
        private DoubleParam age_lastBurst;

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public AgeAxis AgeAxis
        {
            get { return ageAxis; }
            set { ageAxis = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public SfrAxis SfrAxis
        {
            get { return sfrAxis; }
            set { sfrAxis = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public StellarMetAxis StellarMetAxis
        {
            get { return stellarMetAxis; }
            set { stellarMetAxis = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public GasMetAxis GasMetAxis
        {
            get { return gasMetAxis; }
            set { gasMetAxis = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Z_met
        {
            get { return z_met; }
            set { z_met = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam T_eff
        {
            get { return t_eff; }
            set { t_eff = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Log_g
        {
            get { return log_g; }
            set { log_g = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Tau_V0
        {
            get { return tau_V0; }
            set { tau_V0 = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Mu
        {
            get { return mu; }
            set { mu = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam T_form
        {
            get { return t_form; }
            set { t_form = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public IntParam N_bursts
        {
            get { return n_bursts; }
            set { n_bursts = value; }
        }

        [XmlElement]
        [Field(Required = ParamRequired.Custom)]
        public DoubleParam Age_lastBurst
        {
            get { return age_lastBurst; }
            set { age_lastBurst = value; }
        }


        IAxis[] IDataCube.Axes
		{
			get
			{
				return new IAxis[]
				{
					(IAxis) ageAxis,
					(IAxis) sfrAxis,
                    (IAxis) stellarMetAxis,
                    (IAxis) gasMetAxis
				};
			}
			set { }
		}


        #region Constructors
        public ModelParameters()
        {
        }

        public ModelParameters(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public ModelParameters(ModelParameters old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static ModelParameters Clone(ModelParameters old)
        {
            if (old != null)
            {
                return new ModelParameters(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: ModelParameters.cs,v $
        Revision 1.1  2008/01/08 22:26:40  dobos
        Initial checkin


*/
#endregion
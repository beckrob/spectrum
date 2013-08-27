#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ModelSearchParameters.cs,v 1.1 2008/01/08 22:01:44 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:44 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
    public class ModelSearchParameters : SearchParametersBase
    {
        #region Member variables

        private DoubleInterval z_met;
        private DoubleInterval t_eff;
        private DoubleInterval log_g;
        private DoubleInterval tau_V0;
        private DoubleInterval mu;
        private DoubleInterval t_form;
        private DoubleInterval gamma;
        private DoubleInterval n_bursts;
        private DoubleInterval age;
        private DoubleInterval age_lastBurst;

        #endregion

        [XmlElement]
        public DoubleInterval Z_met
        {
            get { return z_met; }
            set { z_met = value; }
        }

        [XmlElement]
        public DoubleInterval T_eff
        {
            get { return t_eff; }
            set { t_eff = value; }
        }

        [XmlElement]
        public DoubleInterval Log_g
        {
            get { return log_g; }
            set { log_g = value; }
        }

        [XmlElement]
        public DoubleInterval Tau_V0
        {
            get { return tau_V0; }
            set { tau_V0 = value; }
        }

        [XmlElement]
        public DoubleInterval Mu
        {
            get { return mu; }
            set { mu = value; }
        }

        [XmlElement]
        public DoubleInterval T_form
        {
            get { return t_form; }
            set { t_form = value; }
        }

        [XmlElement]
        public DoubleInterval Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        [XmlElement]
        public DoubleInterval N_bursts
        {
            get { return n_bursts; }
            set { n_bursts = value; }
        }

        [XmlElement]
        public DoubleInterval Age
        {
            get { return age; }
            set { age = value; }
        }

        [XmlElement]
        public DoubleInterval Age_lastBurst
        {
            get { return age_lastBurst; }
            set { age_lastBurst = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Model; }
        }

        #region Constructors

        public ModelSearchParameters()
        {
        }

        public ModelSearchParameters(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public ModelSearchParameters(ModelSearchParameters old)
        {
            CopyMembers(old);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.z_met = new DoubleInterval(ParamRequired.Optional);
            this.t_eff = new DoubleInterval(ParamRequired.Optional);
            this.log_g = new DoubleInterval(ParamRequired.Optional);
            this.tau_V0 = new DoubleInterval(ParamRequired.Optional);
            this.mu = new DoubleInterval(ParamRequired.Optional);
            this.t_form = new DoubleInterval(ParamRequired.Optional);
            this.gamma = new DoubleInterval(ParamRequired.Optional);
            this.n_bursts = new DoubleInterval(ParamRequired.Optional);
            this.age = new DoubleInterval(ParamRequired.Optional);
            this.age_lastBurst = new DoubleInterval(ParamRequired.Optional);
        }

        private void CopyMembers(ModelSearchParameters old)
        {
            base.CopyMembers(old);

            this.z_met = (old.z_met == null) ? null : new DoubleInterval(old.z_met);
            this.t_eff = (old.t_eff == null) ? null : new DoubleInterval(old.t_eff);
            this.log_g = (old.log_g == null) ? null : new DoubleInterval(old.log_g);
            this.tau_V0 = (old.tau_V0 == null) ? null : new DoubleInterval(old.tau_V0);
            this.mu = (old.mu == null) ? null : new DoubleInterval(old.mu);
            this.t_form = (old.t_form == null) ? null : new DoubleInterval(old.t_form);
            this.gamma = (old.gamma == null) ? null : new DoubleInterval(old.gamma);
            this.n_bursts = (old.n_bursts == null) ? null : new DoubleInterval(old.n_bursts);
            this.age = (old.age == null) ? null : new DoubleInterval(old.age);
            this.age_lastBurst = (old.age_lastBurst == null) ? null : new DoubleInterval(old.age_lastBurst);
        }

        public ModelSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: ModelSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:44  dobos
        Initial checkin


*/
#endregion
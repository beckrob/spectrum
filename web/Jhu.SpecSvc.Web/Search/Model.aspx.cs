using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.Web.Search
{
    public partial class Model : SearchPageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Model.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                
                var par = new ModelSearchParameters(true);

                if (Z_metFrom.Text != string.Empty)
                    par.Z_met.Min.Value = double.Parse(Z_metFrom.Text);
                else
                    par.Z_met.Min = null;
                if (Z_metTo.Text != string.Empty)
                    par.Z_met.Max.Value = double.Parse(Z_metTo.Text);
                else
                    par.Z_met.Max = null;

                if (T_effFrom.Text != string.Empty)
                    par.T_eff.Min.Value = double.Parse(T_effFrom.Text);
                else
                    par.T_eff.Min = null;
                if (T_effTo.Text != string.Empty)
                    par.T_eff.Max.Value = double.Parse(T_effTo.Text);
                else
                    par.T_eff.Max = null;

                if (Log_gFrom.Text != string.Empty)
                    par.Log_g.Min.Value = double.Parse(Log_gFrom.Text);
                else
                    par.Log_g.Min = null;
                if (Log_gTo.Text != string.Empty)
                    par.Log_g.Max.Value = double.Parse(Log_gTo.Text);
                else
                    par.Log_g.Max = null;

                if (Tau_V0From.Text != string.Empty)
                    par.Tau_V0.Min.Value = double.Parse(Tau_V0From.Text);
                else
                    par.Tau_V0.Min = null;
                if (Tau_V0To.Text != string.Empty)
                    par.Tau_V0.Max.Value = double.Parse(Tau_V0To.Text);
                else
                    par.Tau_V0.Max = null;

                if (MuFrom.Text != string.Empty)
                    par.Mu.Min.Value = double.Parse(MuFrom.Text);
                else
                    par.Mu.Min = null;
                if (MuTo.Text != string.Empty)
                    par.Mu.Max.Value = double.Parse(MuTo.Text);
                else
                    par.Mu.Max = null;

                if (T_formFrom.Text != string.Empty)
                    par.T_form.Min.Value = double.Parse(T_formFrom.Text) * 1000000000;
                else
                    par.T_form.Min = null;
                if (T_formTo.Text != string.Empty)
                    par.T_form.Max.Value = double.Parse(T_formTo.Text) * 1000000000;
                else
                    par.T_form.Max = null;

                if (GammaFrom.Text != string.Empty)
                    par.Gamma.Min.Value = double.Parse(GammaFrom.Text);
                else
                    par.Gamma.Min = null;
                if (GammaTo.Text != string.Empty)
                    par.Gamma.Max.Value = double.Parse(GammaTo.Text);
                else
                    par.Gamma.Max = null;

                if (N_burstsFrom.Text != string.Empty)
                    par.N_bursts.Min.Value = double.Parse(N_burstsFrom.Text);
                else
                    par.N_bursts.Min = null;
                if (N_burstsTo.Text != string.Empty)
                    par.N_bursts.Max.Value = double.Parse(N_burstsTo.Text);
                else
                    par.N_bursts.Max = null;

                if (AgeFrom.Text != string.Empty)
                    par.Age.Min.Value = double.Parse(AgeFrom.Text) * 1000000000;
                else
                    par.Age.Min = null;
                if (AgeTo.Text != string.Empty)
                    par.Age.Max.Value = double.Parse(AgeTo.Text) * 100000000;
                else
                    par.Age.Max = null;

                if (Age_lastBurstFrom.Text != string.Empty)
                    par.Age_lastBurst.Min.Value = double.Parse(Age_lastBurstFrom.Text) * 1000000000;
                else
                    par.Age_lastBurst.Min = null;
                if (Age_lastBurstTo.Text != string.Empty)
                    par.Age_lastBurst.Max.Value = double.Parse(Age_lastBurstTo.Text) * 1000000000;
                else
                    par.Age_lastBurst.Max = null;

                par.Collections = Collections.SelectedKeys;

                SearchParameters = par;
                Execute();
            }
        }
    }
}
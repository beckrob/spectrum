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
    public partial class Advanced : PageBase
    {
        public static string GetUrl()
        {
            return "~/Search/Advanced.aspx";
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                var par = new AdvancedSearchParameters(true);

                par.Keyword = Keyword.Text;
                par.Name = Name.Text;

                if (TargetClass.SelectedValue != "0")
                    par.TargetClass = new string[] { TargetClass.SelectedValue };

                if (SpectralClass.Text != string.Empty)
                    par.SpectralClass = SpectralClass.Text.Split(',', ' ');

                if (CreationType.SelectedValue != "0")
                    par.CreationType = new string[] { CreationType.SelectedValue };

                if (DateFrom.Text != string.Empty)
                    par.Date.Start.Value = DateTime.Parse(DateFrom.Text);
                else
                    par.Date.Start = null;
                if (DateTo.Text != string.Empty)
                    par.Date.Stop.Value = DateTime.Parse(DateTo.Text);
                else
                    par.Date.Stop = null;

                par.Version = Version.Text;

                if (Ra.Text != string.Empty && Dec.Text != string.Empty && Sr.Text != string.Empty)
                {
                    par.Pos.Value = new Jhu.SpecSvc.Schema.Position(
                        AstroUtil.rastring2deg(Ra.Text), AstroUtil.decstring2deg(Dec.Text));
                    par.Sr.Value = double.Parse(Sr.Text);
                }
                else
                {
                    par.Pos = null;
                    par.Sr = null;
                }

                if (SnrFrom.Text != string.Empty)
                    par.Snr.Min.Value = double.Parse(SnrFrom.Text);
                else
                    par.Snr.Min = null;
                if (SnrTo.Text != string.Empty)
                    par.Snr.Max.Value = double.Parse(SnrTo.Text);
                else
                    par.Snr.Max = null;

                if (VarAmplFrom.Text != string.Empty)
                    par.VarAmpl.Min.Value = double.Parse(SnrFrom.Text);
                else
                    par.VarAmpl.Min = null;
                if (VarAmplTo.Text != string.Empty)
                    par.VarAmpl.Max.Value = double.Parse(SnrTo.Text);
                else
                    par.VarAmpl.Max = null;

                if (RedshiftFrom.Text != string.Empty)
                    par.Redshift.Min.Value = double.Parse(RedshiftFrom.Text);
                else
                    par.Redshift.Min = null;
                if (RedshiftTo.Text != string.Empty)
                    par.Redshift.Max.Value = double.Parse(RedshiftTo.Text);
                else
                    par.Redshift.Max = null;
                if (RedshiftStatErrorFrom.Text != string.Empty)
                    par.RedshiftStatError.Min.Value = double.Parse(RedshiftStatErrorFrom.Text);
                else
                    par.RedshiftStatError.Min = null;
                if (RedshiftStatErrorTo.Text != string.Empty)
                    par.RedshiftStatError.Max.Value = double.Parse(RedshiftStatErrorTo.Text);
                else
                    par.RedshiftStatError.Max = null;
                if (RedshiftConfidenceFrom.Text != string.Empty)
                    par.RedshiftConfidence.Min.Value = double.Parse(RedshiftConfidenceFrom.Text);
                else
                    par.RedshiftConfidence.Min = null;
                if (RedshiftConfidenceTo.Text != string.Empty)
                    par.RedshiftConfidence.Max.Value = double.Parse(RedshiftConfidenceTo.Text);
                else
                    par.RedshiftConfidence.Max = null;


                if (SpectralCoverageFrom.Text != string.Empty)
                    par.SpectralCoverage.Min.Value = double.Parse(SpectralCoverageFrom.Text);
                else
                    par.SpectralCoverage.Min = null;
                if (SpectralCoverageTo.Text != string.Empty)
                    par.SpectralCoverage.Max.Value = double.Parse(SpectralCoverageTo.Text);
                else
                    par.SpectralCoverage.Max = null;

                if (SpectralResPowerFrom.Text != string.Empty)
                    par.SpectralResPower.Min.Value = double.Parse(SpectralResPowerFrom.Text);
                else
                    par.SpectralResPower.Min = null;
                if (SpectralResPowerTo.Text != string.Empty)
                    par.SpectralResPower.Max.Value = double.Parse(SpectralResPowerTo.Text);
                else
                    par.SpectralResPower.Max = null;

                par.Flux = null;    // temporary

                if (FluxCalibration.SelectedValue != "0")
                    par.FluxCalibration = new string[] { FluxCalibration.SelectedValue };

                par.Collections = PortalConnector.LoadCollections(Collections.SelectedKeys, UserGuid);

                SearchParameters = par;
                ExecuteSearch();
            }
        }
    }
}
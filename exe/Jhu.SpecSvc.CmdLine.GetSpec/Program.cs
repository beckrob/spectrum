using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Steps;

namespace Jhu.SpecSvc.CommandLine.GetSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            Spectrum s, ss;

            List<string> aa = new List<string>(args);

            bool restframe = false;
            bool normalize = false;
            bool mask = false;
            string subtract = null;
            double vdisp = 0;

            int i;

            if ((i = aa.IndexOf("-r")) != -1)
            {
                aa.RemoveAt(i);
                restframe = true;
            }

            if ((i = aa.IndexOf("-n")) != -1)
            {
                aa.RemoveAt(i);
                normalize = true;
            }

            if ((i = aa.IndexOf("-m")) != -1)
            {
                aa.RemoveAt(i);
                mask = true;
            }

            if ((i = aa.IndexOf("-s")) != -1)
            {
                subtract = aa[i + 1];
                aa.RemoveAt(i);
                aa.RemoveAt(i);
            }

            if ((i = aa.IndexOf("-vd")) != -1)
            {
                vdisp = double.Parse(aa[i + 1]);
                aa.RemoveAt(i);
                aa.RemoveAt(i);
            }


            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[aa[0]].ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SqlConnector cc = new SqlConnector(cn, tn);

                    s = cc.GetSpectrum(Guid.Empty, "#" + aa[1], true, null, false);

                    if (subtract != null)
                    {
                        ss = cc.GetSpectrum(Guid.Empty, "#" + subtract, true, null, false);
                    }
                    else
                    {
                        ss = null;
                    }

                    tn.Commit();
                }
            }

            if (restframe)
            {
                s.Restframe();

                if (ss != null) ss.Restframe();
            }

            if (normalize)
            {
                s.Normalize_Median(NormalizeStep.limitsStart_galaxy, NormalizeStep.limitsEnd_galaxy, 1.0, false, 0, 0);

                if (ss != null) ss.Normalize_Median(NormalizeStep.limitsStart_galaxy, NormalizeStep.limitsEnd_galaxy, 1.0, false, 0, 0);
            }

            if (mask && s.Flux_Accuracy_Quality != null)
            {
                for (int w = 0; w < s.Spectral_Value.Length; w++)
                {
                    if (s.Flux_Value[w] <= 0)
                    {
                        s.Flux_Accuracy_Quality[w] = (long)PointMask.NoData;
                    }
                }
            }

            if (vdisp != 0)
            {
                s.ConvolveWithVelocityDisp(vdisp);
            }

            if (subtract != null)
            {
                Jhu.SpecSvc.Util.Vector.Subtract(s.Flux_Value, ss.Flux_Value, out s.Flux_Value);
            }


            AsciiConnector asc = new AsciiConnector();
            asc.OutputStream = Console.Out;

            List<string> cols = new List<string>();

            if (s.Spectral_Value != null) cols.Add("Spectral_Value");
            if (s.Flux_Value != null) cols.Add("Flux_Value");
            if (s.Flux_Accuracy_StatError != null) cols.Add("Flux_Accuracy_StatError");
            if (s.Flux_Accuracy_StatErrLow != null) cols.Add("Flux_Accuracy_StatErrLow");
            if (s.Flux_Accuracy_StatErrHigh != null) cols.Add("Flux_Accuracy_StatErrHigh");
            if (s.Flux_Accuracy_Quality != null) cols.Add("Flux_Accuracy_Quality");
            if (s.Flux_Continuum != null) cols.Add("Flux_Continuum");
            if (s.Flux_Lines != null) cols.Add("Flux_Lines");
            if (s.Model_Continuum != null) cols.Add("Model_Continuum");
            if (s.Model_Lines != null) cols.Add("Model_Lines");
            if (s.Counts_Value != null) cols.Add("Counts_Value");
            if (s.BackgroundModel_Value != null) cols.Add("BackgroundModel_Value");

            asc.Columns = cols.ToArray();

            asc.SaveSpectrum(s, Guid.Empty, mask);
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Schema;

namespace Jhu.SpecSvc.Ssa
{
    public partial class Default : PageBase
    {
        protected List<Collection> collections;

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var cn = CreatePortalConnector())
            {
                if (Request["request"] != null && Request["request"].ToLower() == "querydata")
                {
                    AdvancedSearchParameters asp = new AdvancedSearchParameters();
                    asp.LoadPoints = false;
                    asp.LoadDetails = true;
                    asp.UserGuid = Guid.Empty;

                    // Parsing collection id
                    asp.Collections = new Collection[] { cn.LoadCollection((string)Request["collection"]) };

                    // Parsing position
                    if (Request["pos"] != null && Request["pos"] != string.Empty &&
                        Request["size"] != null && Request["size"] != string.Empty)
                    {
                        string[] parts = Request["pos"].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                        asp.Pos = new PositionParam(new Position(double.Parse(parts[0]), double.Parse(parts[1])), "deg");
                        asp.Sr = new DoubleParam(double.Parse(Request["size"]) * 60.0, "arcmin");
                    }
                    if (Request["time"] != null && Request["time"] != string.Empty)
                    {
                        string[] parts = Request["time"].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                        asp.Date = new TimeInterval(ParamRequired.Optional);
                        if (parts.Length > 0)
                            asp.Date.Start.Value = DateTime.Parse(parts[0]);
                        if (parts.Length > 1)
                            asp.Date.Stop.Value = DateTime.Parse(parts[1]);
                    }

                    asp.SpectralResPower = ParseParameter("specrp");
                    asp.Snr = ParseParameter("snr");
                    asp.Redshift = ParseParameter("redshift");
                    asp.VarAmpl = ParseParameter("varampl");
                    asp.Name = Request["targetname"];

                    if (Request["targetclass"] != null && Request["targetclass"] != string.Empty)
                        asp.TargetClass = new string[] { Request["targetclass"] };

                    if (Request["fluxcalib"] != null && Request["fluxcalib"].ToLower() == "true")
                        asp.FluxCalibration = new string[] { Jhu.SpecSvc.Schema.Spectrum.FluxAxis.CALIBRATED };

                    var ssa = new Jhu.SpecSvc.Schema.Ssa.Ssa(true);
                    ssa.Spectra = cn.FindSpectrum(asp);
                    //ssa.Spectra = Connector.FindSpectrum(asp);
                    ssa.Format = Request["format"];
                    ssa.GetUrl = String.Format("http://{0}{1}", LocalhostName, VirtualPathUtility.ToAbsolute("~/Get.aspx"));

                    var vt = Jhu.SpecSvc.IO.Mappers.Ssa2VoTable.MapSsa2VoTable(ssa);

                    Response.ContentType = "text/xml";
                    XmlSerializer ser = new XmlSerializer(typeof(VOTABLE.VOTABLE));
                    ser.Serialize(Response.OutputStream, vt);

                    Response.End();
                }
                else
                {
                    collections = new List<Collection>(cn.QueryCollections(Guid.Empty, SearchMethod.Unknown));

                    foreach (var coll in collections)
                    {
                        Collection.Items.Add(new ListItem(coll.Name, coll.Id));
                    }
                }
            }
        }

        private DoubleInterval ParseParameter(string param)
        {
            if (Request[param] != null && Request[param] != string.Empty)
            {
                string[] parts = Request[param].Split(new char[] { ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries);

                DoubleInterval interval = new DoubleInterval(ParamRequired.Optional);
                if (parts.Length > 0)
                    interval.Min.Value = double.Parse(parts[0]);
                if (parts.Length > 1)
                    interval.Max.Value = double.Parse(parts[1]);

                return interval;
            }
            else
                return null;
        }

        public string LocalhostName
        {
            get
            {
                string url = Request.Url.Scheme + "://" + Request.Url.Host;
                if (Request.Url.Port != 80 && Request.Url.Port != 443)
                    url += ":" + Request.Url.Port.ToString();

                return url;
            }
        }
    }
}
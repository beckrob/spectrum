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
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.Ssa
{
    public partial class get : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;

            string format = Request["format"].ToLower();
            string id = Request["spectrumid"];

            // Handle format alternates
            switch (format)
            {
                case "application/x-votable+xml":
                    format = "votable";
                    break;
                case "application/xml":
                    format = "xml";
                    break;
                case "text/plain":
                    format = "text";
                    break;
                case "image/gif":
                case "image/jpeg":
                case "image/png":
                    format = "graph";
                    break;
            }

            using (var cn = CreatePortalConnector())
            {
                // Load Spectrum
                var spec = cn.GetSpectrum(Guid.Empty, id);

                switch (format)
                {
                    case "xml":
                        {
                            Response.ContentType = "text/xml";
                            XmlSerializer ser = new XmlSerializer(typeof(Spectrum));
                            ser.Serialize(Response.OutputStream, spec);
                        }
                        break;
                    case "votable":
                        {
                            Response.ContentType = "text/xml";

                            var vt = Jhu.SpecSvc.IO.Mappers.Spectrum2VoTable.MapSpectrum2VoTable(spec);

                            XmlSerializer ser = new XmlSerializer(typeof(VOTABLE.VOTABLE));
                            ser.Serialize(Response.OutputStream, vt);
                        }
                        break;
                    case "ascii":
                        {
                        }
                        break;
                    case "graph":
                        {
                            // Parsing request parameters
                            var par = new Jhu.SpecSvc.Visualizer.SpectrumPlotParameters();

                            par.Width = (Request.QueryString["width"] != null) ? int.Parse(Request.QueryString["width"]) : 640;
                            par.Height = (Request.QueryString["height"] != null) ? int.Parse(Request.QueryString["height"]) : 480;

                            par.Labels = (Request.QueryString["title"] != "no");

                            par.XMax = (Request.QueryString["xmin"] != null) ? float.Parse(Request.QueryString["xmin"]) : -1.0f;
                            par.XMax = (Request.QueryString["xmax"] != null) ? float.Parse(Request.QueryString["xmax"]) : -1.0f;
                            par.YMin = (Request.QueryString["ymin"] != null) ? float.Parse(Request.QueryString["ymin"]) : -1.0f;
                            par.YMax = (Request.QueryString["ymax"] != null) ? float.Parse(Request.QueryString["ymax"]) : -1.0f;

                            par.XLogScale = (Request.QueryString["xlog"] != null) ? (Request.QueryString["xlog"] == "yes") : false;
                            par.YLogScale = (Request.QueryString["ylog"] != null) ? (Request.QueryString["ylog"] == "yes") : false;

                            par.PlotSpectralLines = (Request.QueryString["Emlines"] == "yes");

                            // Generating graph with the visualizer and sending to the client
                            Response.ContentType = "image/gif";

                            var vis = new Jhu.SpecSvc.Visualizer.SpectrumVisualizer();
                            vis.PlotSpectraGraph(par, spec).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                        }
                        break;
                }
            }

            Response.End();

        }
    }
}
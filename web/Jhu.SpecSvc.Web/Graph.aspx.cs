using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.Visualizer;

namespace Jhu.SpecSvc.Web
{
    public partial class Graph : PageBase
    {
        public static string GetUrl(string spectrumId, int width, int height)
        {
            return String.Format("~/Graph.aspx?spectrumID={0}&width={1}&height={2}&labels=no", HttpContext.Current.Server.UrlEncode(spectrumId), width, height);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Parsing request parameters
            var par = new SpectrumPlotParameters();

            par.Width = (Request.QueryString["width"] != null) ? int.Parse(Request.QueryString["width"]) : 640;
            par.Height = (Request.QueryString["height"] != null) ? int.Parse(Request.QueryString["height"]) : 480;

            par.Legend = (Request.QueryString["legend"] != "no");
            par.Labels = (Request.QueryString["labels"] != "no");

            par.XMin = (Request.QueryString["xmin"] != null) ? float.Parse(Request.QueryString["xmin"]) : 3000;
            par.XMax = (Request.QueryString["xmax"] != null) ? float.Parse(Request.QueryString["xmax"]) : 10000;
            par.YMin = (Request.QueryString["ymin"] != null) ? float.Parse(Request.QueryString["ymin"]) : -1.0f;
            par.YMax = (Request.QueryString["ymax"] != null) ? float.Parse(Request.QueryString["ymax"]) : -1.0f;

            par.XLogScale = (Request.QueryString["xlog"] != null) ? (Request.QueryString["xlog"] == "yes") : false;
            par.YLogScale = (Request.QueryString["ylog"] != null) ? (Request.QueryString["ylog"] == "yes") : false;

            par.PlotSpectralLines = (Request.QueryString["Emlines"] == "yes");

            // Loading spectra
            var idpar = new IdSearchParameters(true);
            idpar.LoadDetails = false;
            idpar.LoadPoints = true;
            idpar.UserGuid = UserGuid;
            idpar.PointsMask = new string[] { "Spectral_Value", "Flux_Value" };

            if (Request.QueryString["SpectrumID"] == "list")
            {
                idpar.Ids = ((string)Session["SpectrumList"]).Replace("\r", "").Replace("\n", "").Split(',');
            }
            else
            {
                idpar.Ids = Request.QueryString["SpectrumID"].Split(',');
            }

            //***********************
            var spectra = Connector.FindSpectrum(idpar).ToArray();

            // Generating graph with the visualizer and sending to the client
            Response.Expires = -1; // TODO: check if it's necessary
            Response.ContentType = "image/png";

            var vis = new SpectrumVisualizer();
            vis.PlotSpectraGraph(par, spectra).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

            Response.End();
        }
    }
}
#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Spectrum.cs,v 1.3 2008/10/25 18:26:22 dobos Exp $
 *   Revision:    $Revision: 1.3 $
 *   Date:        $Date: 2008/10/25 18:26:22 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;
using System.Reflection;

namespace Jhu.SpecSvc.SpectrumLib
{
    [XmlType("Spectrum")]
    [XmlInclude(typeof(Jhu.SpecSvc.Schema.Spectrum.Spectrum))]
    public class Spectrum : Jhu.SpecSvc.Schema.Spectrum.Spectrum
    {
        // members hidden from the web service, used internally
        private string publisherId;
        private Guid userGuid;
        private int userFolderId;
        private int @public;
        private long htmId;
        private string url;

        //****** make property
        [XmlIgnore]
        public ContinuumFit ContinuumFitParameters;

        [XmlIgnore]
        public int ContinuumFitBest;

        [XmlIgnore]
        public LineFit LineFit;

        [XmlIgnore]
        public SpectralIndices SpectralIndices;

        [XmlIgnore]
        public List<Magnitudes> Magnitudes;

        [XmlIgnore]
        public long GroupByHash;

        //private DoubleParam originalRedshift;

        // fields used for spectrum service portal
        [XmlIgnore]
        public long ResultId;
        [XmlIgnore]
        public bool ResultSelected;

        /// <summary>
        /// Prefix for ivo IDs, used when assigning numerical value to the ID property
        /// </summary>
        [XmlIgnore]
        public string PublisherId
        {
            get { return publisherId; }
            set { publisherId = value; }
        }

        [XmlIgnore]
        public Guid UserGuid
        {
            get { return this.userGuid; }
            set { this.userGuid = value; }
        }

        [XmlIgnore]
        public int UserFolderId
        {
            get { return this.userFolderId; }
            set { this.userFolderId = value; }
        }

        [XmlIgnore]
        public int Public
        {
            get { return this.@public; }
            set { this.@public = value; }
        }

        [XmlIgnore]
        public long HtmId
        {
            get { return this.htmId; }
            set { this.htmId = value; }
        }

        [XmlIgnore]
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        [XmlIgnore]
        public long Id
        {
            get
            {
                try
                {
                    string uri = this.Curation.PublisherDID.Value;
                    if (uri == null)
                        return 0;
                    else
                        return GetId(uri);
                }
                catch (System.Exception)
                {
                    return -1;
                }
            }
            set
            {
                this.Curation.PublisherDID.Value = PublisherId + "#" + value.ToString();
            }
        }

        public Spectrum()
        {
        }

        public Spectrum(bool initialize)
            : base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public Spectrum(Jhu.SpecSvc.Schema.Spectrum.Spectrum old)
            : base(old)
        {
            InitializeMembers();
        }

        public Spectrum(Spectrum old)
            : base((Jhu.SpecSvc.Schema.Spectrum.Spectrum)old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.publisherId = string.Empty;
            this.userGuid = Guid.Empty;
            this.userFolderId = 0;
            this.@public = 0;
            this.htmId = 0;
            this.url = string.Empty;

            this.GroupByHash = 0;
        }

        public void CopyMembers(Spectrum old)
        {
            // members hidden from the web service
            this.publisherId = old.publisherId;
            this.userGuid = old.userGuid;
            this.userFolderId = old.userFolderId;
            this.@public = old.@public;
            this.htmId = old.htmId;
            this.url = old.url;
        }

        public void BasicInitialize()
        {
            InitializeMembers();

            this.Target = new Target();
            this.Target.Name = new TextParam();
            this.Target.Name.Value = string.Empty;
            this.Target.Class = new TextParam();
            this.Target.Class.Value = string.Empty;
            this.Target.SpectralClass = new TextParam();
            this.Target.SpectralClass.Value = string.Empty;
            this.Target.Pos = new PositionParam();
            this.Target.Redshift = new DoubleParam();
            this.Target.VarAmpl = new DoubleParam();

            this.DataId = new DataId();
            this.DataId.CreatorDID = new TextParam();
            this.DataId.CreatorDID.Value = string.Empty;
            this.DataId.CreationType = new TextParam();
            this.DataId.CreationType.Value = string.Empty;
            this.DataId.Date = new TimeParam();
            this.DataId.Date.Value = DateTime.Now;
            this.DataId.Version = new TextParam();
            this.DataId.Version.Value = string.Empty;


            this.Data = new Data();
            this.Data.SpatialAxis = new SpatialAxis();
            this.Data.SpatialAxis.Coverage = new SpatialCoverage();
            this.Data.SpatialAxis.Coverage.Location = new SpatialCoverageLocation();
            this.Data.SpatialAxis.Coverage.Location.Value = new PositionParam();
            this.Data.SpectralAxis = new SpectralAxis();
            this.Data.SpectralAxis.Coverage = new SpectralCoverage();
            this.Data.SpectralAxis.Coverage.Bounds = new SpectralCoverageBounds();
            this.Data.SpectralAxis.Coverage.Bounds.Start = new DoubleParam();
            this.Data.SpectralAxis.Coverage.Bounds.Stop = new DoubleParam();
            this.Data.SpectralAxis.Value = new DoubleParam(0, "A");
            this.Data.SpectralAxis.ResPower = new DoubleParam();
            this.Data.FluxAxis = new FluxAxis();
            this.Data.FluxAxis.Value = new DoubleParam(0, "ADU");
            this.Data.FluxAxis.Calibration = new TextParam();
            this.Data.FluxAxis.Calibration.Value = Jhu.SpecSvc.Schema.Spectrum.FluxAxis.UNKNOWN;

            this.Derived = new Derived();
            this.Derived.SNR = new DoubleParam();
            this.Derived.Redshift = new Redshift();
            this.Derived.Redshift.Value = new DoubleParam();
            this.Derived.Redshift.StatError = new DoubleParam();
            this.Derived.Redshift.Confidence = new DoubleParam();
            this.Derived.VarAmpl = new DoubleParam();

            this.Curation = new Curation();
            this.Curation.PublisherDID = new TextParam();
            this.Curation.PublisherDID.Value = string.Empty;
        }

        #region Utility member functions

        public string GetCollectionId()
        {
            return GetCollectionId(this.Curation.PublisherDID.Value);
        }

        public static string GetCollectionId(string id)
        {
            int hashpos = id.IndexOf("#");
            if (hashpos >= 0)
                return id.Substring(0, hashpos);
            else
                return "";
        }

        public void CalculateHtmId()
        {
            try
            {
                if (Target.Pos.Value.Ra == 0 && Target.Pos.Value.Dec == 0)
                    htmId = Spherical.Htm.Sql.fHtmEq(
                        Data.SpatialAxis.Coverage.Location.Value.Value.Ra,
                        Data.SpatialAxis.Coverage.Location.Value.Value.Dec);
                else
                    HtmId = Spherical.Htm.Sql.fHtmEq(Target.Pos.Value.Ra, Target.Pos.Value.Dec);
            }
            catch (System.Exception)
            {
                htmId = 0;
            }
        }

        private bool Validate(bool throwError)
        {
            // check empty spectra
            if (this.Spectral_Value == null ||
                this.Flux_Value == null ||
                this.Spectral_Value.Length == 0 ||
                this.Flux_Value.Length == 0)
            {
                if (throwError)
                    throw new SpectrumException("Spectrum must contain points");
                else
                    return false;
            }

            return true;
        }

        #endregion

        #region Utility static functions

        public static long GetId(string uri)
        {
            try
            {
                int hashmark = uri.LastIndexOf("#");
                if (hashmark < 0)	// not found
                    return long.Parse(uri);
                else
                    return long.Parse(uri.Substring(hashmark + 1));
            }
            catch (System.Exception)
            {
                return -1;
            }
        }

        #endregion

        #region Processing functions

        public void FindBins()
        {
            Validate(true);

            Util.Grid.FindBins(this.Spectral_Value,
                out this.Spectral_Accuracy_BinLow,
                out this.Spectral_Accuracy_BinHigh,
                out this.Spectral_Accuracy_BinSize);
        }

        public void Rebin(double[] coord, double[] binlow, double[] binhigh)
        {
            Validate(true);

            double[][] res;
            long[] resmask;

            Util.Grid.Rebin(this.Spectral_Accuracy_BinLow, this.Spectral_Accuracy_BinHigh,
                GetFluxArrays(true),
                this.Flux_Accuracy_Quality,
                binlow, binhigh,
                out res,
                out resmask);

            SetFluxArrays(res, true);
            this.Flux_Accuracy_Quality = resmask;

            this.Spectral_Value = coord;
            this.Spectral_Accuracy_BinLow = binlow;
            this.Spectral_Accuracy_BinHigh = binhigh;

            this.Data.SpectralAxis.Coverage.Bounds.Start.Value = this.Spectral_Value[0];
            this.Data.SpectralAxis.Coverage.Bounds.Stop.Value = this.Spectral_Value[this.Spectral_Value.Length - 1];
        }

        /// <summary>
        /// Convolves spectrum with a kernel function
        /// </summary>
        /// <param name="supMin">Kernel support lower limit</param>
        /// <param name="supMax">Kernel support upper limit</param>
        /// <param name="oversampling">Oversampling in bins for narrow kernels</param>
        /// <param name="kernel">Kernel function</param>
        public void Convolve(double supMin, double supMax, Util.Integral.ConvolutionFunction kernel)
        {
            Validate(true);

            double[][] res;

            Util.Integral.Convolve(
                Spectral_Value, Spectral_Accuracy_BinLow, Spectral_Accuracy_BinHigh,
                GetFluxArrays(false),
                supMin, supMax, 5, kernel, out res);

            SetFluxArrays(res, false);
        }

        public void ConvolveWithVelocityDisp(double vdisp)
        {
            /*
             * old code, remove
             * double vdpc = vdisp * 1000.0 / Constants.LightSpeed;
            double sq2piinv = 1 / Math.Sqrt(2 * Math.PI);

            Convolve(-150, 150, 4,
                delegate(double dx, double x)
                {
                    double invsigma = 1 / (x * vdpc);
                    double xpersigma = dx * invsigma;
                    return sq2piinv * Math.Exp(-xpersigma * xpersigma / 2) * invsigma;
                });*/

            double c = Constants.LightSpeed / 1000.0;
            double sq2piinv = 1 / Math.Sqrt(2 * Math.PI);
            double a = sq2piinv / vdisp;
            double b = 2 * vdisp * vdisp;

            Convolve(-20, 20,
                delegate(double dx, double x)
                {
                    double v = c * ((x + dx) / x - 1);
                    double q = a * Math.Exp(-v * v / b);
                    return q;
                });
        }

        /// <summary>
        /// Multiplies all value and error of the spectrum by the multiplier parameter
        /// </summary>
        /// <param name="multiplier">Multiplier</param>
        public void Multiply(double multiplier)
        {
            Validate(true);

            double[][] res;

            Util.Vector.Multiply(multiplier, GetFluxArrays(true), out res);

            SetFluxArrays(res, true);
        }

        /// <summary>
        /// Adds the flux values from the passed spectrum to this one, multiplied by multiplier. Computes error but assumes
        /// spectra resampled to the same grid
        /// </summary>
        /// <param name="other">The spectrum to add</param>
        /// <param name="multiplier">The spectrum to add</param>
        public void Add(Spectrum other, double multiplier)
        {
            Validate(true);
            other.Validate(true);

            if (this.Spectral_Value.Length != other.Spectral_Value.Length)
            {
                throw new SpectrumException("Number of points must match in the two spectra.");
            }

            double[][] res;

            Util.Vector.Sum(this.GetFluxArrays(false), other.GetFluxArrays(false), 1, multiplier, out res);

            this.SetFluxArrays(res, false);
        }

        /// <summary>
        /// Weights the values of the spectrum using the value * factor * pow(wavelength, power) formula
        /// </summary>
        /// <param name="power"></param>
        /// <param name="factor"></param>
        public void Weight(double power, double factor)
        {
            if ((factor == 1.0 || factor == -1) && power == 1.0) return;

            double[][] res;

            Util.Vector.WeightFunction f = null;

            if (power == 0)
            {
                f = delegate(double x, double y)
                {
                    return factor * y;
                };
            }
            else if (power == 1.0)
            {
                f = delegate(double x, double y)
                {
                    return factor * x * y;
                };
            }
            else if (power == -1.0)
            {
                f = delegate(double x, double y)
                {
                    return factor / x * y;
                };
            }
            else
            {
                f = delegate(double x, double y)
                {
                    return factor * Math.Pow(x, power) * y;
                };
            }

            Util.Vector.Weight(Spectral_Value, GetFluxArrays(true), f, out res);

            SetFluxArrays(res, true);
        }

        public void Restframe()
        {
            Redshift(0);
        }

        public void Redshift(double z)
        {
            if (this.Derived.Redshift.Value > -9 && z > -9)
            {
                double zcoeff = 1;

                if (z == (double)this.Derived.Redshift.Value.Value)
                {
                    // nothing to do
                    return;
                }
                else
                {
                    zcoeff = (1 + z) / (1 + this.Derived.Redshift.Value.Value);
                }

                double[][] ny;

                Util.Vector.Multiply(zcoeff,
                    new double[][] { this.Spectral_Value, this.Spectral_Accuracy_BinLow, this.Spectral_Accuracy_BinHigh },
                    out ny);

                this.Spectral_Value = ny[0];
                this.Spectral_Accuracy_BinLow = ny[1];
                this.Spectral_Accuracy_BinHigh = ny[2];

                try
                {
                    // *** just to get this work with votable upload -> implement update characterization instead
                    this.Data.SpectralAxis.Coverage.Bounds.Start.Value *= zcoeff;
                    this.Data.SpectralAxis.Coverage.Bounds.Stop.Value *= zcoeff;
                }
                catch (Exception)
                {
                }

                this.Derived.Redshift.Value.Value = z;
            }
        }

        public double[][] GetFluxArrays(bool withError)
        {
            if (withError)
            {
                return new double[][] { 
                    this.Flux_Value, 
                    this.Flux_Accuracy_StatErrLow, 
                    this.Flux_Accuracy_StatErrHigh, 
                    this.Flux_Accuracy_StatError,
                    this.Flux_Continuum, 
                    this.Flux_Lines, 
                    this.Model_Continuum, 
                    this.Model_Lines,
                    this.Counts_Value
                };
            }
            else
            {
                return new double[][] { 
                    this.Flux_Value, 
                    this.Flux_Continuum, 
                    this.Flux_Lines, 
                    this.Model_Continuum, 
                    this.Model_Lines,
                    this.Counts_Value
                };
            }
        }

        public void SetFluxArrays(double[][] data, bool withError)
        {
            if (withError)
            {
                this.Flux_Value = data[0];
                this.Flux_Accuracy_StatErrLow = data[1];
                this.Flux_Accuracy_StatErrHigh = data[2];
                this.Flux_Accuracy_StatError = data[3];
                this.Flux_Continuum = data[4];
                this.Flux_Lines = data[5];
                this.Model_Continuum = data[6];
                this.Model_Lines = data[7];
                this.Counts_Value = data[8];
            }
            else
            {
                this.Flux_Value = data[0];
                this.Flux_Continuum = data[1];
                this.Flux_Lines = data[2];
                this.Model_Continuum = data[3];
                this.Model_Lines = data[4];
                this.Counts_Value = data[5];
            }
        }

        #region Normalizer functions

        public void Normalize(double wavelength, double flux)
        {
            // find closest wavelength
            int wl = -1; // will point to the closest wl
            double dist = double.MaxValue;
            for (int i = 0; i < Spectral_Value.Length; i++)
            {
                double d = Math.Abs(Spectral_Value[i] - wavelength);
                if (d < dist)
                {
                    dist = d;
                    wl = i;
                }
            }

            // 
            double factor = flux / Flux_Value[wl];
            Multiply(factor);
        }

        public void Normalize_Median(double[] limitsStart, double[] limitsEnd, double flux, bool weight, double power, double factor)
        {
            // Blueshift to 0
            //double oldz = (double)this.Derived.Redshift.Value.Value;
            //this.Redshift(0);

            double[] oldwl = this.Spectral_Value;

            if (0.0 != (double)this.Derived.Redshift.Value.Value)
            {
                double zcoeff = 1 / (1 + this.Derived.Redshift.Value.Value);
                Util.Vector.Multiply(zcoeff, this.Spectral_Value, out this.Spectral_Value);
            }

            // Weighting the spectrum values first
            if (weight)
            {
                this.Weight(power, factor);
            }

            // The scalefactor in the given lambda ranges will be calculated from the median
            // of the spectrum value in the range
            double scale = 0;
            int rangecount = 0;
            for (int i = 0; i < limitsStart.Length; i++)
            {
                double a = Util.Integral.Median(this.Spectral_Value, this.Flux_Value, null, limitsStart[i], limitsEnd[i]);
                if (!double.IsNaN(a))
                {
                    scale += a;
                    rangecount++;
                }
            }
            scale /= rangecount;

            if (scale != 0 && !double.IsNaN(scale) && !double.IsInfinity(scale))
            {
                this.Multiply(flux / scale);
            }
            else
                throw new Exception("Cannot normalize with median flux of 0.");

            if (weight)
            {
                this.Weight(-power, 1 / factor);
            }

            this.Spectral_Value = oldwl;

            // *** debug code
            /*
            lock (AppDomain.CurrentDomain)
            {
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter("norm.txt"))
                {
                    for (int i = 0; i < this.Spectral_Value.Length; i++)
                    {
                        outfile.WriteLine("{0}\t{1}", this.Spectral_Value[i], this.Flux_Value[i]);
                    }
                }
                Console.Write("");
            }*/

            //this.Redshift(oldz);
            SetNormalized();
        }

        /// <summary>
        /// Normalizes spectrum by the integral on the given interval
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void Normalize_Integral(double[] limitsStart, double[] limitsEnd, double flux)
        {
            // Blueshift to 0
            double oldz = (double)this.Derived.Redshift.Value.Value;
            this.Redshift(0);

            double integral = 0;
            for (int i = 0; i < limitsStart.Length; i++)
            {
                integral += Util.Integral.Integrate(this.Spectral_Accuracy_BinLow, this.Spectral_Accuracy_BinHigh, this.Flux_Value, limitsStart[i], limitsEnd[i]);
            }

            if (integral != 0)
            {
                this.Multiply(flux / integral);
            }
            else
            {
                throw new System.Exception("Cannot normalize with integral flux of 0.");
            }

            this.Redshift(oldz);
            SetNormalized();
        }

        public void Normalize_Vivienne(double minWl, double maxWl)
        {
            /*
            double minflux = double.MaxValue;
            double maxflux = double.MinValue;

            for (int i = 0; i < this.Flux_Value.Length; i++)
            {
                //if ((this.Flux_Accuracy_Quality == null) || this.Flux_Accuracy_Quality[i] == 0)
                //{
                if (this.Spectral_Value[i] >= minWl && this.Spectral_Value[i] <= maxWl
                    && this.Flux_Value[i] != 0)
                {
                    minflux = Math.Min(minflux, this.Flux_Value[i]);
                    maxflux = Math.Max(maxflux, this.Flux_Value[i]);
                }
                //}
            }

            double norminv = 2 / (minflux + maxflux);
             * */

            double totalflux = 0;
            int count = 0;
            for (int i = 0; i < this.Flux_Value.Length; i++)
                if (this.Spectral_Value[i] >= minWl && this.Spectral_Value[i] <= maxWl
                        && this.Flux_Value[i] != 0)
                {
                    totalflux += this.Flux_Value[i];
                    count++;
                }

            double norminv = (double)count / totalflux;

            for (int i = 0; i < this.Flux_Value.Length; i++)
            {
                this.Flux_Value[i] *= norminv;
                if (this.Flux_Accuracy_StatErrHigh != null) this.Flux_Accuracy_StatErrHigh[i] *= norminv;
                if (this.Flux_Accuracy_StatErrLow != null) this.Flux_Accuracy_StatErrLow[i] *= norminv;
            }
        }

        private void SetNormalized()
        {
            this.Data.FluxAxis.Calibration.Value = Jhu.SpecSvc.Schema.Spectrum.FluxAxis.NORMALIZED;
            this.Data.FluxAxis.Value.Unit = "ADU";
        }

        #endregion

        // ----------------------------

        public void Deredden()
        {
            Deredden(this.Target.GalacticExtinction.Value);
        }

        public void Deredden(double eB_V)
        {
            // Query database for extinction
            //double ext = 1.0;

            double a_V = eB_V * 3.1;


            // Applying extinction curve
            for (int i = 0; i < this.Flux_Value.Length; i++)
            {
                double a_lambda = a_V * Extinction_Odonell(this.Spectral_Value[i]);
                double e_tau = Math.Exp(a_lambda / 1.086);
                this.Flux_Value[i] *= e_tau;
            }
        }

        protected double Extinction_Odonell(double wavelength)
        {
            // Extinction curve from O'Donnell et al. (1994)

            // Returns A(x)/A(V)

            const double r_V = 3.1;

            double yy = 10000.0 / wavelength - 1.82;

            double yy2 = yy * yy;
            double yy3 = yy2 * yy;
            double yy4 = yy3 * yy;
            double yy5 = yy4 * yy;
            double yy6 = yy5 * yy;
            double yy7 = yy6 * yy;
            double yy8 = yy7 * yy;

            double a = 1.0 + 0.104 * yy - 0.609 * yy2 + 0.701 * yy3 + 1.137 * yy4 - 1.718 * yy5 - 0.827 * yy6 + 1.647 * yy7 - 0.505 * yy8;
            double b = 1.952 * yy + 2.908 * yy2 - 3.989 * yy3 - 7.985 * yy4 + 11.102 * yy5 + 5.491 * yy6 - 10.805 * yy7 + 3.347 * yy8;

            return a + b / r_V;
        }

        public void Vac2Air()
        {
            Vac2Air(Spectral_Value);
            if (Spectral_Accuracy_BinLow != null) Vac2Air(Spectral_Accuracy_BinLow);
            if (Spectral_Accuracy_BinHigh != null) Vac2Air(Spectral_Accuracy_BinHigh);
        }

        private static void Vac2Air(double[] array)
        {
            // coeffs from SDSS website: http://www.sdss.org/dr5/products/spectra/vacwavelength.html
            for (int i = 0; i < array.Length; i++)
            {
                if (1000 <= array[i] && array[i] < 10000)
                {
                    double vac = array[i];
                    double air = vac / (1.0 + 2.735182E-4 + 131.4182 / Math.Pow(vac, 2) + 2.76249E8 / Math.Pow(vac, 4));
                    array[i] = air;
                }
            }
        }

        public void Air2Vac()
        {
            Air2Vac(Spectral_Value);
            if (Spectral_Accuracy_BinLow != null) Air2Vac(Spectral_Accuracy_BinLow);
            if (Spectral_Accuracy_BinHigh != null) Air2Vac(Spectral_Accuracy_BinHigh);
        }

        private static void Air2Vac(double[] array)
        {
            double last = 0;

            // coeffs from Morton (1991, ApJS, 77, 119). 
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= 2000 && array[i] <= 10000)
                {
                    double air = array[i];
                    double sigma2 = 1.0e+8 / (air * air);
                    double n = 1 + 6.4328e-5 + 2.94981e-2 / (146.0 - sigma2) + 2.5540e-4 / (41.0 - sigma2);
                    double vac = air * n;
                    array[i] = vac;

                    if (vac <= last)
                    {
                        throw new Exception();
                    }

                    last = vac;
                }
            }

            // air * (1 + 0.000064328 + 0.0294981 / (146.0 - 10.0e+8 / (air * air)) + 0.0002554 / (41.0 - 10.0e+8 / (air * air)))
        }

        /*
        public void SubstituteLinesWithModelContinuum(double width)
        {
            for (int wl = 0; wl < Spectral_Value.Length; wl++)
            {
                bool replace = false;

                for (int l = 0; l < LineFit.Wavelength.Length; l++)
                {
                    if (LineFit.LineDetected[l] &&
                        Math.Abs(LineFit.Wavelength[l] - Spectral_Value[wl]) <= width)
                    {
                        replace = true;
                        break;
                    }
                }

                if (replace)
                {
                    Flux_Continuum[wl] = Model_Continuum[wl];
                }
            }
        }
         * */

        #endregion

        #region Continuum and Line Fitting

        /*
        public FitResults Fit(Spectrum[] templates, FitParameters par)
        {
            // --- Initialize results object
            FitResults res = new FitResults();
            res.Spectrum = this;
            res.LineNames = par.LineNames;

            FitContinuum(templates, par, res);

            if (par.FitLines)
            {
                FitLines(par, res);
            }

            return res;
        }*/

#if false
        

        public void FitLines(FitParameters par, FitResults res)
        {
            int n = par.Lines.Value.Length;
            res.LineDetected = new bool[n];
            res.LineWavelength = new double[n];
            res.LineWavelengthError = new double[n];
            res.LineSigma = new double[n];
            res.LineSigmaError = new double[n];
            res.LineAmplitude = new double[n];
            res.LineAmplitudeError = new double[n];
            res.LineEW = new double[n];
            res.LineEWError = new double[n];


            // Group lines if they overlap
            double sigma = par.VDisp / Constants.LightSpeed * 4000.0;

            List<double> linelist = new List<double>();
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                if (linelist.Count == 0)
                {
                    linelist.Add(par.Lines.Value[i]);
                }
                else
                {
                    // Check if lines should be fitted together
                    if (Math.Abs(par.Lines.Value[i] - linelist[linelist.Count - 1]) < 10 * sigma)
                    {
                        linelist.Add(par.Lines.Value[i]);
                    }
                    else
                    {
                        // run fitting
                        FitLineBlock(linelist, sigma, res, i);

                        linelist.Clear();
                        linelist.Add(par.Lines.Value[i]);
                    }
                }
            }
            if (linelist.Count > 0)
            {
                FitLineBlock(linelist, sigma, res, par.Lines.Value.Length);
            }

            // --- Compute median flux in the residual in order to constraint line heights
            double median = Util.Vector.Median(this.Flux_Value);
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                // Constraints for valid line fits
                res.LineDetected[i] &=
                       Math.Abs(res.LineAmplitude[i] / median) < 200
                    && (1.0 < res.LineSigma[i] && res.LineSigma[i] < 10.0)
                    && Math.Abs(res.LineSigmaError[i] / res.LineSigma[i]) < 1.0
                    && Math.Abs(res.LineAmplitudeError[i] / res.LineAmplitude[i]) < 20.0
                    && Math.Abs(res.LineWavelengthError[i] / res.LineWavelength[i]) < 0.05
                    && res.LineAmplitudeError[i] != 0.0;

            }

            // --- Reconstructs lines vector
            double sq2pi = 1 / Math.Sqrt(2 * Math.PI);

            this.Model_Lines = new double[this.Spectral_Value.Length];
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                if (res.LineDetected[i])
                {
                    double s2 = 2 * res.LineSigma[i] * res.LineSigma[i];

                    for (int j = 0; j < this.Spectral_Value.Length; j++)
                    {
                        double xmms = (this.Spectral_Value[j] - res.LineWavelength[i]); xmms *= xmms;
                        double exp = Math.Exp(-xmms / s2);

                        // evaluate gauss
                        this.Model_Lines[j] += res.LineAmplitude[i] * sq2pi / res.LineSigma[i] * exp;
                    }
                }
            }

            Util.Vector.Subtract(this.Flux_Value, this.Model_Lines, out this.Flux_Continuum);

            res.LineChiSquare = Util.Vector.ChiSquared(this.Flux_Lines, this.Model_Lines, null, out res.ContinuumNdf);
            res.LineNdf = this.Flux_Lines.Length;

            // --- Calculate velocity dispersion
            int count = 0;
            double avg = 0, stdev = 0;
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                if (res.LineDetected[i])
                {
                    double v = res.LineSigma[i] * Constants.LightSpeed / res.LineWavelength[i];
                    avg += v;
                    stdev += v*v;
                    count++;
                }
            }
            avg /= count;
            stdev = stdev / count - avg * avg;

            res.VDisp = avg;
            res.VDispError = Math.Sqrt(stdev);

            // --- Calculate equivalent width
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                CalculateEquivalentWidth(res.LineWavelength[i], res.LineAmplitude[i], res.LineSigma[i], out res.LineEW[i]);
                res.LineEWError[i] = res.LineAmplitudeError[i] / res.LineAmplitude[i] * res.LineEW[i];
            }
        }

        private void FitLineBlock(List<double> linelist, double sigma, FitResults res, int i)
        {
            double[] nwavelength = linelist.ToArray();
            double[] namplitude = new double[linelist.Count];
            double[] nsigma = new double[linelist.Count];
            double[] nwavelengtherror, namplitudeerror, nsigmaerror;

            // shift line wavelengths to obs-frame and set sigma
            for (int j = 0; j < linelist.Count; j++)
            {
                nwavelength[j] *= (1 + this.Derived.Redshift.Value.Value);
                nsigma[j] = sigma;
            }

            bool ok = FitMultipleGaussianLine(ref nwavelength, out nwavelengtherror, ref namplitude, out namplitudeerror, ref nsigma, out nsigmaerror);

            // copy results
            for (int j = 0; j < linelist.Count; j++)
            {
                int ni = i - linelist.Count + j;

                res.LineDetected[ni] = ok;
                res.LineWavelength[ni] = nwavelength[j];
                res.LineWavelengthError[ni] = nwavelengtherror[j];
                res.LineSigma[ni] = nsigma[j];
                res.LineSigmaError[ni] = nsigmaerror[j];
                res.LineAmplitude[ni] = namplitude[j];
                res.LineAmplitudeError[ni] = namplitudeerror[j];
            }
        }

        public void CalculateEquivalentWidth(double wavelenght, double amplitude, double sigma, out double ew)
        {
            //ew = 0;
            //return;

            double wwdstep = 8;
            double wd, ci;

            while (true)
            {
                double wdstep = wwdstep;
                ci = 0;
                wd = 0;

                double[] res;

                for (int st = 0; st < 128; st++)
                {
                    if (Math.Abs(Math.Abs(amplitude) - ci) < 1e-5)
                    {
                        break;
                    }
                    else if (ci < Math.Abs(amplitude))
                    {
                        wd += wdstep;
                    }
                    else
                    {
                        wd -= wdstep;
                    }

                    Util.Integral.Integrate(this.Spectral_Accuracy_BinLow,
                        this.Spectral_Accuracy_BinHigh,
                        new double[][] { this.Model_Continuum },
                        wavelenght - wd, wavelenght + wd, out res);

                    ci = res[0];

                    wdstep /= 2.0;
                }

                if (Math.Abs(Math.Abs(amplitude) - ci) < 1e-5)
                {
                    break;
                }
                else if (double.IsNaN(ci))
                {
                    wd = double.NaN;
                    break;
                }
                else if (wwdstep > 512)
                {
                    wd = double.NaN;
                    break;
                }
                else
                {
                    wwdstep *= 2;
                }
            }

            ew = 2.0 * wd * Math.Sign(amplitude);
        }

        /* NumRec - doesn't work
        private bool FitSingleGaussianLine(ref double wavelength, ref double amplitude, ref double sigma)
        {
            double[] wl;        // wavelength grid around the line
            double[] fl;        // flux values around the line
            double[] ss;

            double[][] oo;

            Util.Grid.GetRange(this.Spectral_Value, new double[][] { this.Flux_Lines, this.Flux_Value, this.Flux_Accuracy_StatErrLow, this.Flux_Accuracy_StatErrHigh }, wavelength - 2 * sigma, wavelength + 2 * sigma, out wl, out oo);
            fl = oo[1];
            
            // compute errors
            ss = new double[wl.Length];
            for (int i = 0; i < ss.Length; i++)
            {
                
                //double err1 = (oo[2][i] - oo[1][i]) / oo[1][i]; err1 *= err1;
                //double err2 = (oo[3][i] - oo[1][i]) / oo[1][i]; err2 *= err2;

                //ss[i] = Math.Abs(fl[i]) * Math.Sqrt(err1 + err2);
                //if (ss[i] == 0.0) ss[i] = fl[i] * 2;

                ss[i] = 1;
            }


            int N = 3;                  // number of fit parameters
            int M = wl.Length;      // number of functions (ie. number of points to fit)

            if (M <= N)
            {
                amplitude = sigma = wavelength = 0;
                return false;
            }

            // Single gaussing function and derivative evaluation
            NumericalRecipes.Delegates.FunctionDoubleDoubleAToDoubleA f =
                delegate(double x, double[] a)
                {
                    double[] y = new double[a.Length + 1];

                    double sq2pi = 1 / Math.Sqrt(2 * Math.PI);

                    double A = a[0];
                    double s = a[1];
                    double m = a[2];

                    double s2 = s * s;
                    double s3 = s2 * s;

                    double xmms = (x - m); xmms *= xmms;
                    double exp = Math.Exp(-xmms / s2 / 2);

                    // Jacobian
                    y[0] = sq2pi / s * exp;                                    // dG/dA
                    y[1] = A * sq2pi / s2 * exp * (xmms / s2 - 1.0);           // dG/ds
                    y[2] = A * sq2pi / s3 * exp * (x - m);                     // dG/dm

                    // Evaluate
                    y[3] = A * sq2pi / s * exp;

                    return y;
                };

            // find min/max to estimate line height
            double ymin = double.MaxValue, ymax = double.MinValue;
            for (int i = 1; i < fl.Length; i++)
            {
                ymin = Math.Min(ymin, fl[i]);
                ymax = Math.Max(ymax, fl[i]);
            }

            amplitude = 0;
            if (Math.Abs(ymin) > Math.Abs(ymax))
            {
                amplitude = ymin;
            }
            else
            {
                amplitude = ymax;
            }
            amplitude *= Math.Sqrt(2 * Math.PI) * sigma;


            double[] aa = { amplitude, sigma, wavelength };     // Array for free parameters
            bool[] ia = { true, true, true };

            // Execute LM fitting
            NumericalRecipes.ModelingOfData.Mrqmin mrq = new NumericalRecipes.ModelingOfData.Mrqmin(aa.Length);
            for (int i = 0; i < 10; i++)
            {
                mrq.mrqmin(wl, fl, ss, wl.Length, aa, ia, aa.Length, f);
                Console.WriteLine(mrq.Chi2);
            }

            mrq.Lambda = 0.0;
            mrq.mrqmin(wl, fl, ss, wl.Length, aa, ia, aa.Length, f);

            // xx now contains parameters
            amplitude = aa[0];
            sigma = Math.Abs(aa[1]);
            wavelength = aa[2];

            bool res = (sigma < 10);

            return res;
        }*/


        /*
        private bool FitSingleGaussianLine(ref double wavelength, out double wavelengthError, ref double amplitude, out double amplitudeError, ref double sigma, out double sigmaError)
        {
            double[] wl;        // wavelength grid around the line
            double[] fl;        // flux values around the line

            Util.Grid.GetRange(this.Spectral_Value, this.Flux_Lines, wavelength - 5 * sigma, wavelength + 5 * sigma, out wl, out fl, true);
            int N = 3;                  // number of fit parameters
            int M = wl.Length - 1;      // number of functions (ie. number of points to fit)

            if (M <= N)
            {
                amplitude = sigma = wavelength = 0;
                amplitudeError = sigmaError = wavelengthError = 0;
                return false;
            }

            // Single gaussing function and derivative evaluation
            Util.LevenbergMarquardt.FuncVecJac f =
                delegate(ref double[] x, ref double[] fvec, ref double[,] fjac, ref int iflag)
                {
                    double sq2pi = 1 / Math.Sqrt(2 * Math.PI);

                    double A = x[1];
                    double s = x[2];
                    double m = x[3];

                    double s2 = s * s;
                    double s3 = s2 * s;

                    for (int j = 1; j <= M; j++)
                    {
                        double xmms = (wl[j] - m); xmms *= xmms;
                        double exp = Math.Exp(-xmms / s2 / 2);
                        if (iflag == 1)
                        {
                            // Evaluate
                            fvec[j] = fl[j] - A * sq2pi / s * exp;
                        }
                        else
                        {
                            // Jacobian
                            fjac[j, 1] = -sq2pi / s * exp;                                    // dG/dA
                            fjac[j, 2] = -A * sq2pi / s2 * exp * (xmms / s2 - 1.0);           // dG/ds
                            fjac[j, 3] = -A * sq2pi / s3 * exp * (wl[j] - m);                 // dG/dm
                        }
                    }
                };

            // find min/max to estimate line height
            double ymin = double.MaxValue, ymax = double.MinValue;
            for (int i = 1; i < fl.Length; i++)
            {
                ymin = Math.Min(ymin, fl[i]);
                ymax = Math.Max(ymax, fl[i]);
            }

            amplitude = 0;
            if (Math.Abs(ymin) > Math.Abs(ymax))
            {
                amplitude = ymin;
            }
            else
            {
                amplitude = ymax;
            }
            amplitude *= Math.Sqrt(2 * Math.PI) * sigma;


            double[] xx = { 0, amplitude, sigma, wavelength };     // Array for free parameters
            int info = 0;

            // Execute LM fitting
            Util.LevenbergMarquardt.Minimize(f, N, M, ref xx, 1e-10, 1e-10, 1e-10, 1000, ref info);

            amplitude = xx[1];
            sigma = Math.Abs(xx[2]);
            wavelength = xx[3];

            // --- Calculate covariance matrix
            double[] yy = new double[N + 1];
            double[,] dyda = new double[M + 1, N + 1];
            int infflag = 0;
            f(ref xx, ref yy, ref dyda, ref infflag);

            Lapack.Matrix cov = new Lapack.Matrix(N, N);

            for (int k = 0; k < M; k++)
            {
                for (int i = 0; i < cov.Rows; i++)
                {
                    for (int j = 0; j < cov.Columns; j++)
                    {
                        cov[i, j] += dyda[k + 1, i + 1] * dyda[k + 1, j + 1];
                    }
                }
            }

            bool res = true;
            res &= (1 <= info && info <= 5);

            try
            {
                cov = cov.Inverse;

                // xx now contains parameters

                amplitudeError = Math.Abs(cov[0, 0]);
                sigmaError = Math.Abs(cov[1, 1]);
                wavelengthError = Math.Abs(cov[2, 2]);

                //res &= (amplitudeError / amplitude) < 1;
                //res &= (wavelengthError / wavelength) < 0.01;
                //res &= (sigmaError / sigma) < 0.1;

                res &= (sigma < 10);
            }
            catch (System.Exception)
            {
                amplitudeError = sigmaError = wavelengthError = 0;

                //res = false;
                res = true;     //**** remove this line
            }

            return res;
        }
         * */

        private bool FitMultipleGaussianLine(ref double[] wavelength, out double[] wavelengthError, ref double[] amplitude, out double[] amplitudeError, ref double[] sigma, out double[] sigmaError)
        {
            bool res = true;

            // --- Get points for fitting
            int ng = wavelength.Length;  // number of gaussians

            double[] wl;        // wavelength grid around the line
            double[] fl;        // flux values around the line

            // determine wavelength range
            double minwave = double.MaxValue, maxwave = double.MinValue;
            double minsigma = double.MaxValue, maxsigma = double.MinValue;
            for (int i = 0; i < wavelength.Length; i++)
            {
                minwave = Math.Min(wavelength[i], minwave);
                maxwave = Math.Max(wavelength[i], maxwave);
                minsigma = Math.Min(sigma[i], minsigma);
                maxsigma = Math.Max(sigma[i], maxsigma);
            }

            Util.Grid.GetRange(this.Spectral_Value, this.Flux_Lines, minwave - 3 * maxsigma, maxwave + 3 * maxsigma, out wl, out fl, true);

            int N = ng * 3;                  // number of fit parameters
            int M = wl.Length - 1;           // number of functions (ie. number of points to fit) (-1 is due to the leadning zero)

            // --- initialize return arrays
            wavelengthError = new double[ng];
            amplitudeError = new double[ng];
            sigmaError = new double[ng];


            if (M <= N)
            {
                for (int i = 0; i < ng; i++)
                {
                    amplitude[i] = sigma[i] = wavelength[i] = 0;
                    amplitudeError[i] = sigmaError[i] = wavelengthError[i] = 0;
                }

                return false;
            }

            // Single gaussing function and derivative evaluation
            Util.LevenbergMarquardt.FuncVecJac f =
                delegate(ref double[] x, ref double[] fvec, ref double[,] fjac, ref int iflag)
                {
                    double sq2pi = 1 / Math.Sqrt(2 * Math.PI);

                    if (iflag == 1)
                    {
                        for (int j = 0; j < fvec.Length; j++)
                        {
                            fvec[j] = fl[j];
                        }
                    }

                    for (int i = 0; i < ng; i++)
                    {
                        double A = x[i * 3 + 1];
                        double s = x[i * 3 + 2];
                        double m = x[i * 3 + 3];

                        double s2 = s * s;
                        double s3 = s2 * s;

                        for (int j = 1; j <= M; j++)
                        {
                            double xmms = (wl[j] - m); xmms *= xmms;
                            double exp = Math.Exp(-xmms / s2 / 2);
                            if (iflag == 1)
                            {
                                // Evaluate
                                fvec[j] += -A * sq2pi / s * exp;
                            }
                            else
                            {
                                // Jacobian
                                fjac[j, i * 3 + 1] = -sq2pi / s * exp;                                    // dG/dA
                                fjac[j, i * 3 + 2] = -A * sq2pi / s2 * exp * (xmms / s2 - 1.0);           // dG/ds
                                fjac[j, i * 3 + 3] = -A * sq2pi / s3 * exp * (wl[j] - m);                 // dG/dm
                            }
                        }
                    }
                };

            // find min/max to estimate line height
            for (int i = 0; i < ng; i++)
            {
                double ymin = double.MaxValue, ymax = double.MinValue;
                for (int j = 1; j < fl.Length; j++)
                {
                    if (Math.Abs(wl[j] - wavelength[i]) < 1 * sigma[i])
                    {
                        ymin = Math.Min(ymin, fl[j]);
                        ymax = Math.Max(ymax, fl[j]);
                    }
                }

                amplitude[i] = 0;
                if (Math.Abs(ymin) > Math.Abs(ymax))
                {
                    amplitude[i] = ymin;
                }
                else
                {
                    amplitude[i] = ymax;
                }
                amplitude[i] *= Math.Sqrt(2 * Math.PI) * sigma[i];
            }

            // Array of free parameters
            double[] xx = new double[1 + 3 * ng];
            for (int i = 0; i < ng; i++)
            {
                xx[3 * i + 1] = amplitude[i];
                xx[3 * i + 2] = sigma[i];
                xx[3 * i + 3] = wavelength[i];
            }

            int info = 0;

            // Execute LM fitting
            Util.LevenbergMarquardt.Minimize(f, N, M, ref xx, 1e-7, 1e-7, 1e-7, 1000, ref info);
            res &= (1 <= info && info <= 5);

            for (int i = 0; i < ng; i++)
            {
                amplitude[i] = xx[3 * i + 1];
                sigma[i] = Math.Abs(xx[3 * i + 2]);
                wavelength[i] = xx[3 * i + 3];
            }

            // --- Calculate covariance matrix
            double[] yy = new double[N + 1];
            double[,] dyda = new double[M + 1, N + 1];
            int infflag = 0;
            f(ref xx, ref yy, ref dyda, ref infflag);


            Lapack.Matrix cov = new Lapack.Matrix(N, N);

            for (int k = 0; k < M; k++)
            {
                for (int i = 0; i < cov.Rows; i++)
                {
                    for (int j = 0; j < cov.Columns; j++)
                    {
                        cov[i, j] += dyda[k + 1, i + 1] * dyda[k + 1, j + 1];
                    }
                }
            }

            cov = cov.Inverse;

            // xx now contains parameters
            for (int i = 0; i < ng; i++)
            {
                amplitudeError[i] = Math.Abs(cov[3 * i, 3 * i]);
                sigmaError[i] = Math.Abs(cov[3 * i + 1, 3 * i + 1]);
                wavelengthError[i] = Math.Abs(cov[3 * i + 2, 3 * i + 2]);
            }

            return res;
        }

        /* Uses Brent's algorithm, slow
         * */
        /*
        private void FitLines(FitParameters par, FitResults res)
        {
            double[][] C;
            double rcoeff;
            double chi2;
            double vdisp = par.VDisp;

            Util.Fit.BrentFunction f =
                delegate(double lp, out double[] lerror, out double lchi2)
                {
                    double[] la;
                    double[][] lC;
                    double lrcoeff;
                    double[] lcc;
                    FitLines(par.Lines, lp, out la, out lC, out lrcoeff, out lcc, out lchi2);

                    lerror = new double[lC.Length];
                    for (int i = 0; i < lerror.Length; i++)
                    {
                        lerror[i] = Math.Sqrt(lC[i][i]);
                    }

                    return la;
                };

            res.LineAmplitude = Util.Fit.FitBrent(f, ref vdisp, 50, par.Lines.Value.Length, out res.LineChiSquare, out res.LineAmplitudeError);
            
            // Do the fit with the optimal vdisp
            FitLines(par.Lines, vdisp, out res.LineAmplitude, out C, out rcoeff, out this.Model_Lines, out chi2);
            Util.Vector.Subtract(this.Flux_Value, this.Model_Lines, out this.Flux_Continuum);

            res.VDisp = vdisp;
        }
         * */
#endif

#if false
        private void FitLines(FitParameters par, FitResults res)
        {

            // --- Intialize results object
            res.LineNames = par.LineNames;

            // Offset used for local minimum finding
            double vdipsoff = par.VDisp * 0.1;

            //res.LineAmplitude = SpectrumFit.Fit_Lines(spectrum, par.Lines, ref res.VDisp, out cs0, out res.LineAmplitudeError);
            //res.Linefit = SpectrumFit.Combine_Lines(spectrum, par.Lines, res.LineAmplitude, res.VDisp);


            // calculating error of velocity dispersion
            // have to calculate chi-square at to different positions around the minimum and fit a parabola on them
            double[] a;
            double[][] C;
            double rcoeff;
            double[] linemodel;
            double cs0, csa, csb;
            double[] erd;	// dummy, we don't need it
            double[] cfd;

            FitLines(par.Lines, par.VDisp, out a, out C, out rcoeff, out linemodel, out cs0);
            FitLines(par.Lines, par.VDisp - vdipsoff, out a, out C, out rcoeff, out linemodel, out csa);
            FitLines(par.Lines, par.VDisp + vdipsoff, out a, out C, out rcoeff, out linemodel, out csb);

            /*
            cfd = SpectrumFit.Fit_Lines_Linear(spectrum, par.Lines, res.VDisp - 10, out erd);
            csa = SpectrumFit.Fit_Lines_ChiSquare(spectrum, par.Lines, cfd, res.VDisp - 10);

            cfd = SpectrumFit.Fit_Lines_Linear(spectrum, par.Lines, res.VDisp + 10, out erd);
            csb = SpectrumFit.Fit_Lines_ChiSquare(spectrum, par.Lines, cfd, res.VDisp + 10);
             * */

            // --- Fit second order polinomial
            // chi2 = (x - x0) / sigma2 + c : should be fitted to csa, cs0 and csb
            //Matrix
            Lapack.Matrix A = new Lapack.Matrix(3, 3);
            A[0, 0] = (par.VDisp - vdipsoff) * (par.VDisp - vdipsoff); A[0, 1] = (par.VDisp - vdipsoff); A[0, 2] = 1.0;
            A[1, 0] = (par.VDisp) * (par.VDisp); A[1, 1] = (par.VDisp); A[1, 2] = 1.0;
            A[2, 0] = (par.VDisp + vdipsoff) * (par.VDisp + vdipsoff); A[2, 1] = (par.VDisp + vdipsoff); A[2, 2] = 1.0;

            //Matrix
            Lapack.Matrix b = new Lapack.Matrix(3, 1);
            b[0, 0] = csa;
            b[1, 0] = cs0;
            b[2, 0] = csb;

            Lapack.Svd svd = new Lapack.Svd(A, Lapack.Implementation.Managed);
            svd.Solve(b);
            Lapack.Matrix sol = svd.x;

            res.VDisp = -sol[1, 0] / sol[0, 0] / 2.0;
            res.VDispError = 1 / Math.Sqrt(Math.Abs(sol[0, 0]));

            // --- DEBUG

            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(string.Format("vdisp{0}.txt", Id)))
            {
                for (int i = -10; i < 10; i++)
                {
                    double vd = par.VDisp + i * par.VDisp / 100.0;
                    FitLines(par.Lines, vd, out a, out C, out rcoeff, out linemodel, out cs0);

                    outfile.WriteLine("{0}\t{1}", vd, cs0);
                }
            }

            // --- END DEBUG

            // --- Do the fit with estimated vdisp

            FitLines(par.Lines, res.VDisp, out res.LineAmplitude, out C, out rcoeff, out this.Model_Lines, out res.LineChiSquare);

            Util.Vector.Subtract(this.Flux_Value, this.Model_Lines, out this.Flux_Continuum);

            /*
            Combine_Lines(spectrum, par.Lines, res.LineAmplitude, res.VDisp);
            spectrum.Flux_Continuum = new double[spectrum.Spectral_Value.Length];
            for (int i = 0; i < spectrum.Spectral_Value.Length; i++)
            {
                if (spectrum.Flux_Accuracy_Quality[i] != 0)
                {
                    // Substitute with continuum fit
                    spectrum.Flux_Continuum[i] = spectrum.Model_Continuum[i];
                }
                else
                {
                    spectrum.Flux_Continuum[i] = spectrum.Flux_Value[i] - spectrum.Model_Lines[i];
                }
            }*/

            // amplitudes
            res.LineSigma = new double[par.Lines.Value.Length];
            res.LineSigmaError = new double[par.Lines.Value.Length];
            //_amplitudes = new double[_lines.Length];
            double zz = res.VDisp / Constants.LightSpeed;
            double zze = res.VDispError / Constants.LightSpeed;
            double sqrt2pi = Math.Sqrt(2 * Math.PI);
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                res.LineSigma[i] = par.Lines.Value[i] * zz;
                res.LineSigmaError[i] = par.Lines.Value[i] * zze;
                res.LineAmplitude[i] *= res.LineSigma[i] * sqrt2pi;
            }

            //**** check this
            res.LineNdf = this.Spectral_Value.Length - par.Lines.Value.Length - 1;



            // equivalent widths
            res.LineEqWidth = new double[par.Lines.Value.Length];

#if false
            for (int i = 0; i < par.Lines.Value.Length; i++)
            {
                try
                {
                    // step with the pointer to the closest point in the spectrum
                    int pointer = 0;
                    while (res.Continuum.Spectral_Value[pointer] <= par.Lines.Value[i])
                    {
                        pointer++;
                        if (pointer >= res.Continuum.Spectral_Value.Length) break;
                    }


                    // now pointer is located right after the required wavelength
                    // integrating continuum until the integral exceeds the amplitude (integral of the Gauss-curve)

                    // probably equivalent width is larger than the resolution of grid, so making the first step
                    // won't cause any problem
                    int pleft = pointer - 1;
                    int pright = pointer;
                    double integral = (res.Continuum.Flux_Value[pleft] + res.Continuum.Flux_Value[pright]) / 2 * (res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft]);
                    double nintegral = 0.0;

                    while (integral + nintegral < Math.Abs(res.LineAmplitude[i]))
                    {
                        // suppose that integral is less than the amplitude
                        // make a single step and check whether we exceeded amplitude
                        int npleft = pleft - 1;
                        int npright = pright + 1;

                        nintegral += (res.Continuum.Flux_Value[npleft] + res.Continuum.Flux_Value[pleft]) / 2 * (res.Continuum.Spectral_Value[pleft] - res.Continuum.Spectral_Value[npleft]);
                        nintegral += (res.Continuum.Flux_Value[pright] + res.Continuum.Flux_Value[npright]) / 2 * (res.Continuum.Spectral_Value[npright] - res.Continuum.Spectral_Value[pright]);

                        pleft = npleft;
                        pright = npright;
                    }

                    // now the bracketed area slightly larger than the required
                    // but we may accept it as an assumption and interpolate between this and the previous integral
                    if (nintegral != 0.0)
                        res.LineEqWidth[i] = res.LineAmplitude[i] * (res.Continuum.Spectral_Value[pleft + 1] - res.Continuum.Spectral_Value[pleft] +
                            res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft - 1]) / nintegral;
                    else
                    {

                        res.LineEqWidth[i] = res.Continuum.Spectral_Value[pright] - res.Continuum.Spectral_Value[pleft];

                        // *** eq with can be negative?
                        //if (res.LineAmplitude[i] < 0) res.LineEqWidth[i] = - res.LineEqWidth[i];
                    }
                    res.LineEqWidth[i] = Math.Abs(res.LineEqWidth[i]);
                }
                catch (System.Exception)
                {
                    res.LineEqWidth[i] = double.NaN;
                }
            }
#endif
        }
#endif

        /*
        private void FitLines(double[] lines, double vdisp, out double[] a, out double[][] C, out double rcoeff, out double[] combined, out double chi2)
        {
            /* Delete this
            double z = vdisp / Constants.LightSpeed;	// required to calculate line width

            double[,] limits = new double[lines.Length, 2];
            for (int i = 0; i < lines.Length; i++)
            {
                limits[i, 0] = lines[i] * (1 - z);
                limits[i, 1] = lines[i] * (1 + z);
            //}

            // Generate templates for line fitting

            double s = 1 / Math.Sqrt(2 * Math.PI) / vdisp;
            double z = -(Constants.LightSpeed * Constants.LightSpeed) / (vdisp * vdisp) / 2.0;

            double[][] temp = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                temp[i] = new double[this.Spectral_Value.Length];
                for (int wl = 0; wl < this.Spectral_Value.Length; wl++)
                {
                    if (Math.Abs(this.Spectral_Value[wl] - lines[i]) < 10)
                    {
                        double l = (this.Spectral_Value[wl] - lines[i]) / lines[i];

                        temp[i][wl] = s * Math.Exp(z * (l * l));
                    }
                    else
                    {
                        temp[i][wl] = 0.0;
                    }
                }
            }

            // Execute fitting
            Util.Fit.FitLeastSquares(this.Flux_Lines, null, null, temp, out a, out C, out rcoeff);
            Util.Vector.Sum(temp, a, out combined);

            int ndf;
            chi2 = Util.Vector.ChiSquared(this.Flux_Lines, combined, null, out ndf);

            /*
            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(String.Format("lines{0}.txt", Id)))
            {
                for (int wl = 0; wl < temp[0].Length; wl++)
                {
                    outfile.Write("{0}", this.Spectral_Value[wl]);
                    for (int i = 0; i < temp.Length; i++)
                    {
                        outfile.Write("\t{0}", temp[i][wl]);
                    }
                    outfile.WriteLine();
                }
            //}
        }
    */

        #endregion

    }
}
#region Revision History
/* Revision History

        $Log: Spectrum.cs,v $
        Revision 1.3  2008/10/25 18:26:22  dobos
        *** empty log message ***

        Revision 1.2  2008/09/11 10:45:37  dobos
        Bugfixes to rebinning code

        Revision 1.1  2008/01/08 21:36:57  dobos
        Initial checkin


*/
#endregion
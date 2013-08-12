#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: PreprocessParameters.cs,v 1.2 2008/10/15 20:05:40 dobos Exp $
 *   Revision:    $Revision: 1.2 $
 *   Date:        $Date: 2008/10/15 20:05:40 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using VoServices.Schema;

namespace VoServices.Spectrum.Lib
{
    /// <summary>
    /// Summary description for PreprocessParameters.
    /// </summary>
    [Serializable]
    public class PreprocessParameters
    {
        public static readonly double[] limitsStart_galaxy = { 4250, 4600, 5400, 5600 };
        public static readonly double[] limitsEnd_galaxy =   { 4300, 4800, 5500, 5800 };
        public static readonly double[] limitsStart_qso = { 1430, 2150, 3020, 4150 };
        public static readonly double[] limitsEnd_qso =   { 1480, 2230, 3100, 4250 };
        public const double power_galaxy = 1.0;
        public const double factor_galaxy = 1.0;
        public const double power_qso = 1.4;
        public const double factor_qso = 1e-4;

        //
        private bool deredden;
        private bool restframe;
        private WavelengthConversion wavelengthConversion;

        private NormalizeMethods normalizeMethod;
        private DoubleParam normalizeWavelength;
        private DoubleParam normalizeFlux;
        private NormalizeTemplates normalizeTemplate;
        private DoubleArrayParam normalizeLimitsStart;
        private DoubleArrayParam normalizeLimitsEnd;
        private double normalizeFactor;
        private double normalizePower;

        private bool rebin;
        private DoubleInterval rebinLimits;
        private DoubleParam rebinBinSize;
        private DoubleArrayParam rebinValue;
        private DoubleArrayParam rebinBinLow;
        private DoubleArrayParam rebinBinHigh;

        private bool convolve;
        private DoubleParam convolveVelocityDispersion;

        #region Properties

        public bool Deredden
        {
            get { return this.deredden; }
            set { this.deredden = value; }
        }

        public bool Restframe
        {
            get { return this.restframe; }
            set { this.restframe = value; }
        }

        public WavelengthConversion WavelengthConversion
        {
            get { return this.wavelengthConversion; }
            set { this.wavelengthConversion = value; }
        }

        public NormalizeMethods NormalizeMethod
        {
            get { return this.normalizeMethod; }
            set { this.normalizeMethod = value; }
        }

        public DoubleParam NormalizeWavelength
        {
            get { return this.normalizeWavelength; }
            set { this.normalizeWavelength = value; }
        }

        public DoubleParam NormalizeFlux
        {
            get { return this.normalizeFlux; }
            set { this.normalizeFlux = value; }
        }

        [XmlIgnore]
        public NormalizeTemplates NormalizeTemplate
        {
            get { return this.normalizeTemplate; }
            set
            {
                this.normalizeTemplate = value;

                switch (this.normalizeTemplate)
                {
                    case NormalizeTemplates.Galaxy:
                        this.normalizeLimitsStart = (DoubleArrayParam)limitsStart_galaxy;
                        this.normalizeLimitsEnd = (DoubleArrayParam)limitsEnd_galaxy;
                        this.normalizePower = power_galaxy;
                        this.normalizeFactor = factor_galaxy;
                        break;
                    case NormalizeTemplates.Qso:
                        this.normalizeLimitsStart = (DoubleArrayParam)limitsStart_qso;
                        this.normalizeLimitsEnd = (DoubleArrayParam)limitsEnd_qso;
                        this.normalizePower = power_qso;
                        this.normalizeFactor = factor_qso;
                        break;
                }
            }
        }

        public DoubleArrayParam NormalizeLimitsStart
        {
            get { return this.normalizeLimitsStart; }
            set { this.normalizeLimitsStart = value; }
        }

        public DoubleArrayParam NormalizeLimitsEnd
        {
            get { return this.normalizeLimitsEnd; }
            set { this.normalizeLimitsEnd = value; }
        }

        public double NormalizeFactor
        {
            get { return this.normalizeFactor; }
            set { this.normalizeFactor = value; }
        }

        public double NormalizePower
        {
            get { return this.normalizePower; }
            set { this.normalizePower = value; }
        }

        public bool Rebin
        {
            get { return rebin; }
            set { rebin = value; }
        }

        public DoubleInterval RebinLimits
        {
            get { return rebinLimits; }
            set { rebinLimits = value; }
        }

        public DoubleParam RebinBinSize
        {
            get { return rebinBinSize; }
            set { rebinBinSize = value; }
        }

        public DoubleArrayParam RebinValue
        {
            get { return rebinValue; }
            set { rebinValue = value; }
        }

        public DoubleArrayParam RebinBinLow
        {
            get { return rebinBinLow; }
            set { rebinBinLow = value; }
        }

        public DoubleArrayParam RebinBinHigh
        {
            get { return rebinBinHigh; }
            set { rebinBinHigh = value; }
        }

        public bool Convolve
        {
            get { return convolve; }
            set { convolve = value; }
        }

        public DoubleParam ConvolveVelocityDispersion
        {
            get { return convolveVelocityDispersion; }
            set { convolveVelocityDispersion = value; }
        }


        #endregion
        #region Constructors

        public PreprocessParameters()
            :this (true)
        {
        }

        public PreprocessParameters(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public PreprocessParameters(PreprocessParameters old)
        {
            CopyMembers(old);
        }

        #endregion

        private void InitializeMembers()
        {
            this.deredden = true;
            this.restframe = false;
            this.wavelengthConversion = WavelengthConversion.None;

            this.normalizeMethod = NormalizeMethods.None;
            this.normalizeWavelength = new DoubleParam(4000.0, "A");
            this.normalizeFlux = new DoubleParam(1.0, "ADU");
            this.normalizeTemplate = NormalizeTemplates.Unknown;
            this.normalizeLimitsStart = new DoubleArrayParam(true);
            this.normalizeLimitsEnd = new DoubleArrayParam(true);
            this.normalizeFactor = 1.0;
            this.normalizePower = 1.0;

            this.rebin = false;
            this.rebinLimits = new DoubleInterval(ParamRequired.Optional);
            this.rebinBinSize = new DoubleParam(true);
            this.rebinValue = new DoubleArrayParam(true);
            this.rebinBinLow = new DoubleArrayParam(true);
            this.rebinBinHigh = new DoubleArrayParam(true);

            this.convolve = false;
            this.convolveVelocityDispersion = new DoubleParam(150.0, "");
        }

        private void CopyMembers(PreprocessParameters old)
        {
            this.deredden = old.deredden;
            this.restframe = old.restframe;
            this.wavelengthConversion = old.wavelengthConversion;

            this.normalizeMethod = old.normalizeMethod;
            this.normalizeTemplate = old.normalizeTemplate;
            this.normalizeLimitsStart = old.normalizeLimitsStart == null ? null : new DoubleArrayParam(old.normalizeLimitsStart);
            this.normalizeLimitsEnd = old.normalizeLimitsEnd == null ? null : new DoubleArrayParam(old.normalizeLimitsEnd);
            this.normalizeFactor = old.normalizeFactor;
            this.normalizePower = old.normalizePower;

            this.rebin = old.rebin;
            this.rebinLimits = new DoubleInterval(old.rebinLimits);
            this.rebinBinSize = new DoubleParam(old.rebinBinSize);
            this.rebinValue = new DoubleArrayParam(old.rebinValue);
            this.rebinBinLow = new DoubleArrayParam(old.rebinBinLow);
            this.rebinBinHigh = new DoubleArrayParam(old.rebinBinHigh);

            this.convolve = old.convolve;
            this.convolveVelocityDispersion = new DoubleParam(old.convolveVelocityDispersion);
        }

        public PreprocessParameters GetStandardUnits()
        {
            return this;
        }

        public void InitializeFor(string op)
        {
            rebinLimits = new DoubleInterval(ParamRequired.Optional);
            rebinLimits.Min.Value = 3000;
            rebinLimits.Max.Value = 10000;
            rebinBinSize = new DoubleParam(1.0, "");

            switch (op)
            {
                case "download":
                case "ws":
                case "graph":
                case "convolve":
                    wavelengthConversion = WavelengthConversion.None;
                    deredden = false;
                    restframe = false;
                    break;
                case "composite":
                    wavelengthConversion = WavelengthConversion.None;
                    normalizeMethod = NormalizeMethods.FluxMedianInRanges;
                    NormalizeTemplate = NormalizeTemplates.Galaxy;
                    normalizeFlux.Value = 10000.0;
                    deredden = true;
                    restframe = true;
                    rebinLimits.Min.Value = 4000;
                    rebinLimits.Max.Value = 8000;
                    rebinBinSize = new DoubleParam(5.0, "");
                    rebin = true;
                    break;
                case "fit":
                    wavelengthConversion = WavelengthConversion.Vac2Air;
                    deredden = true;
                    restframe = true;
                    normalizeMethod = NormalizeMethods.None;
                    break;
                case "pca":
                    wavelengthConversion = WavelengthConversion.Vac2Air;
                    deredden = true;
                    restframe = true;
                    normalizeMethod = NormalizeMethods.FluxMedianInRanges;
                    NormalizeTemplate = NormalizeTemplates.Galaxy;
                    normalizeFlux.Value = 10000.0;
                    rebinLimits.Min.Value = 4000;
                    rebinLimits.Max.Value = 8000;
                    rebinBinSize = new DoubleParam(5.0, "");
                    rebin = true;
                    break;
            }

            CalculateRebinGrid();
        }

        public void CalculateRebinGrid()
        {
            int points = (int)Math.Floor((rebinLimits.Max.Value - rebinLimits.Min.Value) / rebinBinSize) + 1;

            rebinValue = new DoubleArrayParam(new double[points], "");
            rebinBinLow = new DoubleArrayParam(new double[points], "");
            rebinBinHigh = new DoubleArrayParam(new double[points], "");

            for (int i = 0; i < points; i++)
            {
                rebinValue.Value[i] = rebinLimits.Min.Value + i * rebinBinSize;
                rebinBinLow.Value[i] = rebinValue.Value[i] - rebinBinSize / 2;
                rebinBinHigh.Value[i] = rebinValue.Value[i] + rebinBinSize / 2;
            }
        }
    }
}
#region Revision History
/* Revision History

        $Log: PreprocessParameters.cs,v $
        Revision 1.2  2008/10/15 20:05:40  dobos
        *** empty log message ***

        Revision 1.1  2008/01/08 21:36:56  dobos
        Initial checkin


*/
#endregion
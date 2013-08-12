#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: FitParameters.cs,v 1.1 2008/01/08 21:36:55 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:55 $
 */
#endregion
using System;
using VoServices.Schema;

namespace VoServices.Spectrum.Lib
{
    /// <summary>
    /// Summary description for FitParameters.
    /// </summary>
    [Serializable]
    public class FitParameters
    {
        private FitMethods method;
        private DoubleArrayParam maskMin;
        private DoubleArrayParam maskMax;
        private DoubleArrayParam lines;
        private string[] lineNames;
        private bool maskLines;
        private DoubleArrayParam maskSkyMin;
        private DoubleArrayParam maskSkyMax;
        private bool maskSkyLines;
        private bool maskFromSpectra;
        private bool maskZeroError;
        private bool weightWithError;
        private double errorSoftening;
        private bool fitLines;
        private DoubleParam vDisp;
        private string templateSet;
        private string[] templateList;

        public FitMethods Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public DoubleArrayParam MaskMin
        {
            get { return this.maskMin; }
            set { this.maskMin = value; }
        }

        public DoubleArrayParam MaskMax
        {
            get { return this.maskMax; }
            set { this.maskMax = value; }
        }

        public DoubleArrayParam Lines
        {
            get { return this.lines; }
            set { this.lines = value; }
        }

        public string[] LineNames
        {
            get { return this.lineNames; }
            set { this.lineNames = value; }
        }

        public bool MaskLines
        {
            get { return this.maskLines; }
            set { this.maskLines = value; }
        }

        public DoubleArrayParam MaskSkyMin
        {
            get { return this.maskSkyMin; }
            set { this.maskSkyMin = value; }
        }

        public DoubleArrayParam MaskSkyMax
        {
            get { return this.maskSkyMax; }
            set { this.maskSkyMax = value; }
        }

        public bool MaskSkyLines
        {
            get { return this.maskSkyLines; }
            set { this.maskSkyLines = value; }
        }

        public bool MaskFromSpectra
        {
            get { return this.maskFromSpectra; }
            set { this.maskFromSpectra = value; }
        }

        public bool MaskZeroError
        {
            get { return this.maskZeroError; }
            set { this.maskZeroError = value; }
        }

        public bool WeightWithError
        {
            get { return this.weightWithError; }
            set { this.weightWithError = value; }
        }

        public double ErrorSoftening
        {
            get { return this.errorSoftening; }
            set { this.errorSoftening = value; }
        }

        public bool FitLines
        {
            get { return this.fitLines; }
            set { this.fitLines = value; }
        }

        public DoubleParam VDisp
        {
            get { return this.vDisp; }
            set { this.vDisp = value; }
        }

        public string TemplateSet
        {
            get { return this.templateSet; }
            set { this.templateSet = value; }
        }

        public string[] TemplateList
        {
            get { return this.templateList; }
            set { this.templateList = value; }
        }

        public FitParameters()
            : this(true)
        {
        }

        public FitParameters(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public FitParameters(FitParameters old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = FitMethods.NonNegativeLeastSquare;
            this.maskMin = new DoubleArrayParam(true);
            this.maskMax = new DoubleArrayParam(true);
            this.lines = new DoubleArrayParam(true);
            this.maskLines = true;
            this.maskSkyMin = new DoubleArrayParam(true);
            this.maskSkyMax = new DoubleArrayParam(true);
            this.maskSkyLines = true;
            this.maskFromSpectra = true;
            this.maskZeroError = true;
            this.weightWithError = true;
            this.errorSoftening = 0.0;
            this.fitLines = true;
            this.vDisp = new DoubleParam(200.0, "km s-1");
            this.templateSet = string.Empty;
            this.templateList = null;
        }

        private void CopyMembers(FitParameters old)
        {
            this.method = old.method;
            this.maskMin = old.maskMin == null ? null : new DoubleArrayParam(old.maskMin);
            this.maskMax = old.maskMax == null ? null : new DoubleArrayParam(old.maskMax);
            this.lines = old.lines == null ? null : new DoubleArrayParam(old.lines);
            this.maskLines = old.maskLines;
            this.maskSkyMin = old.maskSkyMin == null ? null : new DoubleArrayParam(old.maskSkyMin);
            this.maskSkyMax = old.maskSkyMax == null ? null : new DoubleArrayParam(old.maskSkyMax);
            this.maskSkyLines = old.maskSkyLines;
            this.maskFromSpectra = old.maskFromSpectra;
            this.maskZeroError = old.maskZeroError;
            this.weightWithError = old.weightWithError;
            this.errorSoftening = old.errorSoftening;
            //this.interpolation = old.interpolation;
            this.fitLines = old.fitLines;
            this.vDisp = old.vDisp == null ? null : new DoubleParam(old.vDisp);
            this.templateSet = old.templateSet;

            if (old.templateList != null)
            {
                this.templateList = new string[old.templateList.Length];
                Array.Copy(old.templateList, this.templateList, old.templateList.Length);
            }
            else
                this.templateList = null;
        }

        public FitParameters GetStandardUnits()
        {
            return this;
        }

        public void AddSdssLines()
        {
            this.Lines = (DoubleArrayParam)new double[]
				{
                    1033.82, // OVI_1033
                    1215.67, // Lya_1216
                    1240.81, // NV_1241
                    1305.53, // OI_1306
                    1335.31, // CII_1335
                    1397.61, // SiIV_1398
                    1399.80, // SiIV_OIV_1400
                    1549.48, // CIV_1549
                    1640.40, // HeII_1640
                    1665.85, // OIII_1666
                    1857.40, // AlIII_1857
                    1908.73, // CIII_1909
                    2326.00, // CII_2326
                    2439.50, // NeIV_2439
                    2799.12, // MgII_2799
                    3346.79, // NeV_3347
                    3426.85, // NeV_3427
                    3727.09, // OII_3727
                    3729.88, // OII_3730
                    3798.98, // Hh_3799
                    3836.47, // Oy_3836
                    3889.00, // HeI_3889
                    3934.78, // K_3935
                    3969.59, // H_3970
                    3971.19, // He_3971
                    4072.30, // SII_4072
                    4102.89, // Hd_4103
                    4305.61, // G_4306
                    4341.68, // Hg_4342
                    4364.44, // OIII_4364
                    4862.68, // Hb_4863
                    4932.60, // OIII_4933
                    4960.30, // OIII_4960
                    5008.24, // OIII_5008
                    5176.70, // Mg_5177
                    5895.60, // Na_5896
                    6302.05, // OI_6302
                    6365.54, // OI_6366
                    6529.03, // NI_6529
                    6549.86, // NII_6550
                    6564.61, // Ha_6565
                    6585.27, // NII_6585
                    6707.89, // Li_6708
                    6718.29, // SII_6718
                    6732.67, // SII_6733
                    8500.36, // CaII_8500
                    8544.44, // CaII_8544
                    8664.52, // CaII_8665
				};

            this.lineNames = new string[]
				{
                    "OVI_1033",
                    "Lya_1216",
                    "NV_1241",
                    "OI_1306",
                    "CII_1335",
                    "SiIV_1398",
                    "SiIV_OIV_1400",
                    "CIV_1549",
                    "HeII_1640",
                    "OIII_1666",
                    "AlIII_1857",
                    "CIII_1909",
                    "CII_2326",
                    "NeIV_2439",
                    "MgII_2799",
                    "NeV_3347",
                    "NeV_3427",
                    "OII_3727",
                    "OII_3730",
                    "Hh_3799",
                    "Oy_3836",
                    "HeI_3889",
                    "K_3935",
                    "H_3970",
                    "He_3971",
                    "SII_4072",
                    "Hd_4103",
                    "G_4306",
                    "Hg_4342",
                    "OIII_4364",
                    "Hb_4863",
                    "OIII_4933",
                    "OIII_4960",
                    "OIII_5008",
                    "Mg_5177",
                    "Na_5896",
                    "OI_6302",
                    "OI_6366",
                    "NI_6529",
                    "NII_6550",
                    "Ha_6565",
                    "NII_6585",
                    "Li_6708",
                    "SII_6718",
                    "SII_6733",
                    "CaII_8500",
                    "CaII_8544",
                    "CaII_8665",
                };

            GenerateLineMask();
        }

        public void AddStandardLines()
        {
            this.Lines = (DoubleArrayParam)new double[]
				{
                    1033.82, //OVI_1033         0
                    1215.67, //Lya_1216
                    1240.81, //NV_1241
                    1305.53, //OI_1306
                    1335.31, //CII_1335
                    1397.61, //SiIV_1398
                    1399.80, //SiIV_OIV_1400
                    1549.48, //CIV_1549
                    1640.40, //HeII_1640
                    1665.85, //OIII_1666
                    1857.40, //AlIII_1857       10
                    1908.73, //CIII_1909
                    2326.00, //CII_2326
                    2439.50, //NeIV_2439
                    2799.12, //MgII_2799
                    3346.79, //NeV_3347
                    3426.85, //NeV_3427
                    3705.56, //HeI_3705
                    3727.09, //OII_3727
                    3729.88, //OII_3730
                    3751.84, //H12_3751         20
                    3772.31, //H11_3772
                    3798.98, //Hh_3799
                    3821.30, //He I_3821
                    3836.47, //Oy_3836
                    3837.04, //H9_3837
                    3870.40, //NeIII_3870
                    3889.00, //HeI_3889
                    3890.70, //H8_3890
                    3934.78, //K_3935
                    3969.59, //H_3970           30
                    3971.19, //He_3971
                    4027.83, //HeI_4027
                    4070.21, //SII_4070
                    4072.30, //SII_4072
                    4102.89, //Hd_4103
                    4305.61, //G_4306
                    4341.68, //Hg_4342
                    4364.44, //OIII_4364
                    4473.04, //HeI_4473
                    4862.68, //Hb_4863          40
                    4932.60, //OIII_4933
                    4960.30, //OIII_4960
                    4965.23, //OIII_4965
                    5008.24, //OIII_5008
                    5176.70, //Mg_5177
                    5202.33, //NI_5202
                    5877.65, //HeI_5877
                    5891.99, //NaD_5891
                    5895.60, //Na_5896
                    5897.99, //NaD_5897         50
                    6302.05, //OI_6302
                    6314.45, //SIII_6314
                    6365.54, //OI_6366
                    6529.03, //NI_6529
                    6549.86, //NII_6550
                    6564.61, //Ha_6565
                    6585.27, //NII_6585
                    6680.27, //HeI_6680
                    6707.89, //Li_6708
                    6718.29, //SII_6718         60
                    6732.67, //SII_6733
                    7067.48, //HeI_7067
                    7137.99, //ArIII_7137
                    7321.90, //OII_7321
                    7332.41, //OII_7332
                    7753.47, //ArIII_7753
                    8500.36, //CaII_8500
                    8544.44, //CaII_8544
                    8664.52, //CaII_8665
        };

            this.lineNames = new string[]
				{
                    "OVI_1033",
                    "Lya_1216",
                    "NV_1241",
                    "OI_1306",
                    "CII_1335",
                    "SiIV_1398",
                    "SiIV_OIV_1400",
                    "CIV_1549",
                    "HeII_1640",
                    "OIII_1666",
                    "AlIII_1857",
                    "CIII_1909",
                    "CII_2326",
                    "NeIV_2439",
                    "MgII_2799",
                    "NeV_3347",
                    "NeV_3427",
                    "HeI_3705",
                    "OII_3727",
                    "OII_3730",
                    "H12_3751",
                    "H11_3772",
                    "Hh_3799",
                    "He I_3821",
                    "Oy_3836",
                    "H9_3837",
                    "NeIII_3870",
                    "HeI_3889",
                    "H8_3890",
                    "K_3935",
                    "H_3970",
                    "He_3971",
                    "HeI_4027",
                    "SII_4070",
                    "SII_4072",
                    "Hd_4103",
                    "G_4306",
                    "Hg_4342",
                    "OIII_4364",
                    "HeI_4473",
                    "Hb_4863",
                    "OIII_4933",
                    "OIII_4960",
                    "OIII_4965",
                    "OIII_5008",
                    "Mg_5177",
                    "NI_5202",
                    "HeI_5877",
                    "NaD_5891",
                    "Na_5896",
                    "NaD_5897",
                    "OI_6302",
                    "SIII_6314",
                    "OI_6366",
                    "NI_6529",
                    "NII_6550",
                    "Ha_6565",
                    "NII_6585",
                    "HeI_6680",
                    "Li_6708",
                    "SII_6718",
                    "SII_6733",
                    "HeI_7067",
                    "ArIII_7137",
                    "OII_7321",
                    "OII_7332",
                    "ArIII_7753",
                    "CaII_8500",
                    "CaII_8544",
                    "CaII_8665"
                    				};

            GenerateLineMask();
        }

        /*
        public void AddStandardLines()
        {
            this.Lines = (DoubleArrayParam)new double[]
				{
					3703.86, // ; He I
					3726.03, // ; [O II]*
					3728.82, // ; [O II]*
					3750.15, // ; H12
					3770.63, // ; H11
					3797.90, // ; H10
					3819.64, // ; He I
					3835.38, // ; H9
					3868.75, // ; [Ne III]*
					3889.05, // ; H8
					3970.07, // ; H-episilon
					4101.73, // ; H-delta*
					4026.21, // ; He I
					4068.60, // ; [S II]
					4340.46, // ; H-gamma*
					4363.21, // ; [O III]
					4471.50, // ; He I
					4861.33, // ; H-beta*
					4959.91, // ; [O III]*
					5006.84, // ; [O III]*
					5200.26, // ; [N I]
					5875.67, // ; He I
					5890.0 , // ; Na D (abs)*
					5896.0 , // ; Na D (abs)*
					6300.30, // ; [O I]*
					6312.40, // ; [S III]
					6363.78, // ; [O I]
					6548.04, // ; [N II]*
					6562.82, // ; H-alpha*
					6583.41, // ; [N II]*
					6678.15, // ; He I
					6716.44, // ; [S II]*
					6730.81, // ; [S II]*
					7065.28, // ; He I
					7135.78, // ; [Ar III]
					7319.65, // ; [O II]
					7330.16, // ; [O II]
					7751.12  //   [Ar III]
				};

            this.lineNames = new string[]
				{
					"He I",
					"[O II]*",
					"[O II]*",
					"H12",
					"H11",
					"H10",
					"He I",
					"H9",
					"[Ne III]*",
					"H8",
					"H-episilon",
					"H-delta*",
					"He I",
					"[S II]",
					"H-gamma*",
					"[O III]",
					"He I",
					"H-beta*",
					"[O III]*",
					"[O III]*",
					"[N I]",
					"He I",
					"Na D (abs)*",
					"Na D (abs)*",
					"[O I]*",
					"[S III]",
					"[O I]",
					"[N II]*",
					"H-alpha*",
					"[N II]*",
					"He I",
					"[S II]*",
					"[S II]*",
					"He I",
					"[Ar III]",
					"[O II]",
					"[O II]",
					"[Ar III]"
				};

            GenerateLineMask();
        }
         * */

        public void GenerateLineMask()
        {
            if (this.maskLines)
            {
                // mask
                // calculating mask with a given velocity dispersion
                // z = dl / l
                // z = 1 - sqrt((c-vdisp)/(c+vdisp)
                double c = Constants.LightSpeed;		// vacuum, km/s

                //double z = 1 - Math.Sqrt((c - vdisp) / (c + vdisp));
                double z = this.VDisp / c;

                int maskcount = 0;
                if (this.maskLines) maskcount += this.lines.Value.Length;
                this.maskMin.Value = new double[maskcount];
                this.maskMax.Value = new double[maskcount];

                if (this.maskLines)
                    for (int i = 0; i < this.Lines.Value.Length; i++)
                    {
                        this.maskMin.Value[i] = this.lines.Value[i] * (1 - z);
                        this.maskMax.Value[i] = this.lines.Value[i] * (1 + z);
                    }
            }
            else
            {
                this.maskMin.Value = new double[0];
                this.maskMax.Value = new double[0];
            }

            if (this.maskSkyLines)
            {
                this.maskSkyMin.Value = new double[] { 5574, 4276, 6297, 6364 };
                this.maskSkyMax.Value = new double[] { 5590, 4282, 6305, 6368 };
            }
            else
            {
                this.maskSkyMin.Value = new double[0];
                this.maskSkyMax.Value = new double[0];
            }
        }
    }
}
#region Revision History
/* Revision History

        $Log: FitParameters.cs,v $
        Revision 1.1  2008/01/08 21:36:55  dobos
        Initial checkin


*/
#endregion
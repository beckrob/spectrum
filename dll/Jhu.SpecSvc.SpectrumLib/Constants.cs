#region Written by L�szl� Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Constants.cs,v 1.1 2008/01/08 21:36:43 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:43 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SpecSvc.Schema;

namespace Jhu.SpecSvc.SpectrumLib
{
    public static class Constants
    {
        public const double LightSpeed = 299792458;   // m s-1;
        public const double NightSkyLine = 5577.0;

        public static readonly SpectralLineDefinition[] SdssLines = 
				{
                    // 0
                    new SpectralLineDefinition("OVI_1033", "\\ion{O}{vi}", 1033.82, true),
                    new SpectralLineDefinition("Lya_1216", "Ly$\\alpha$",1215.67, true),
                    new SpectralLineDefinition("NV_1241", "\\ion{N}{v}", 1240.81, true), 
                    new SpectralLineDefinition("OI_1306", "\\ion{O}{i}", 1305.53, true),
                    new SpectralLineDefinition("CII_1335", "\\ion{C}{ii}", 1335.31),
                    // 5
                    new SpectralLineDefinition("SiIV_1398", "\\ion{Si}{iv}", 1397.61), 
                    new SpectralLineDefinition("SiIV_OIV_1400", "\\ion{Si}{iv}+\\ion{O}{iv}",   1399.80),
                    new SpectralLineDefinition("CIV_1549", "\\ion{C}{iv}", 1549.48),
                    new SpectralLineDefinition("HeII_1640", "\\ion{He}{ii}", 1640.40, true), 
                    new SpectralLineDefinition("OIII_1666", "\\ion{O}{iii}", 1665.85, true),
                    // 10
                    new SpectralLineDefinition("AlIII_1857", "\\ion{Al}{iii}", 1857.40), 
                    new SpectralLineDefinition("CIII_1909", "\\ion{C}{iii}", 1908.73),
                    new SpectralLineDefinition("CII_2326", "\\ion{C}{ii}", 2326.00),
                    new SpectralLineDefinition("NeIV_2439", "\\ion{Ne}{iv}", 2439.50), 
                    new SpectralLineDefinition("MgII_2799", "\\ion{Mg}{ii}", 2799.12),
                    // 15
                    new SpectralLineDefinition("NeV_3347", "\\ion{Ne}{v}", 3346.79),
                    new SpectralLineDefinition("NeV_3427", "\\ion{Ne}{v}", 3426.85), 
                    new SpectralLineDefinition("OII_3727", "\\ion{O}{ii}", 3727.09, true), 
                    new SpectralLineDefinition("OII_3730", "\\ion{O}{ii}", 3729.88, true), 
                    new SpectralLineDefinition("Hh_3799", "H$\\theta$", 3798.98, true),
                    // 20
                    new SpectralLineDefinition("Oy_3836", "H$\\eta$", 3836.47, true),
                    new SpectralLineDefinition("HeI_3889", "\\ion{He}{i}", 3889.00, true), 
                    new SpectralLineDefinition("K_3935", "K", 3934.78),
                    new SpectralLineDefinition("H_3970", "H", 3969.59),
                    new SpectralLineDefinition("SII_4072", "\\ion{S}{ii}", 4072.30, true), 
                    // 25
                    new SpectralLineDefinition("Hd_4103", "H$\\delta$", 4102.89, true),
                    new SpectralLineDefinition("G_4306", "G", 4305.61),
                    new SpectralLineDefinition("Hg_4342", "H$\\gamma$", 4341.68, true), 
                    new SpectralLineDefinition("OIII_4364", "\\ion{O}{iii}", 4364.44, true),
                    new SpectralLineDefinition("Hb_4863", "H$\\beta$", 4862.68, true),
                    // 30
                    new SpectralLineDefinition("OIII_4933", "\\ion{O}{iii}", 4932.60, true), 
                    new SpectralLineDefinition("OIII_4960", "\\ion{O}{iii}", 4960.30, true),
                    new SpectralLineDefinition("OIII_5008", "\\ion{O}{iii}", 5008.24, true),
                    new SpectralLineDefinition("Mg_5177", "Mg", 5176.70),
                    new SpectralLineDefinition("Na_5896", "Na", 5895.60),
                    // 35
                    new SpectralLineDefinition("OI_6302", "\\ion{O}{i}", 6302.05, true),
                    new SpectralLineDefinition("OI_6366", "\\ion{O}{i}", 6365.54, true),
                    new SpectralLineDefinition("NI_6529", "\\ion{N}{i}", 6529.03, true),
                    new SpectralLineDefinition("NII_6550", "\\ion{N}{ii}", 6549.86, true), 
                    new SpectralLineDefinition("Ha_6565", "H$\\alpha$", 6564.61, true),
                    // 40
                    new SpectralLineDefinition("NII_6585", "\\ion{N}{ii}", 6585.27, true), 
                    new SpectralLineDefinition("Li_6708", "Li", 6707.89),
                    new SpectralLineDefinition("SII_6718", "\\ion{S}{ii}", 6718.29, true), 
                    new SpectralLineDefinition("SII_6733", "\\ion{S}{ii}", 6732.67, true),
                    new SpectralLineDefinition("CaII_8500", "\\ion{Ca}{ii}", 8500.36), 
                    new SpectralLineDefinition("CaII_8544", "\\ion{Ca}{ii}", 8544.44),
                    new SpectralLineDefinition("CaII_8665", "\\ion{Ca}{ii}", 8664.52), 
				};

        public static readonly SpectralLineDefinition[] SdssEmissionLines =
            SdssLines.Where(l => l.Emission).ToArray();

        public static readonly SpectralLineDefinition[] StandardLines =
            SdssLines;

        public static readonly SpectralLineDefinition[] UpdatedEmissionLines = 
        {
            // 0
            new SpectralLineDefinition("OII_3727", "\\ion{O}{ii}", 3727.09, true),
            new SpectralLineDefinition("OII_3730", "\\ion{O}{ii}", 3729.88, true),
            new SpectralLineDefinition("Hh_3799", "H$\\theta$", 3798.98, true),
            new SpectralLineDefinition("Heta_3836", "H$\\eta$", 3836.47, true),
            //new SpectralLineDefinition("HeI_3889", "\\ion{He}{i}", 3889.75, true), 
            new SpectralLineDefinition("Hz_3890", "H$\\zeta$", 3890.16, true),
            // 5
            new SpectralLineDefinition("Heps_3971", "H$\\epsilon$", 3971.20, true), 
            new SpectralLineDefinition("SII_4072", "\\ion{S}{ii}", 4072.30, true), 
            new SpectralLineDefinition("Hd_4103", "H$\\delta$", 4102.89, true),
            new SpectralLineDefinition("Hg_4342", "H$\\gamma$", 4341.68, true),
            new SpectralLineDefinition("OIII_4364", "\\ion{O}{iii}", 4364.44, true),
            // 10
            new SpectralLineDefinition("Hb_4863", "H$\\beta$", 4862.68, true),
            new SpectralLineDefinition("OIII_4933", "\\ion{O}{iii}", 4932.60, true), 
            new SpectralLineDefinition("OIII_4960", "\\ion{O}{iii}", 4960.30, true),
            new SpectralLineDefinition("OIII_5008", "\\ion{O}{iii}", 5008.24, true),
            new SpectralLineDefinition("HeI_5877", "\\ion{He}{i}", 5877.65, true),
            // 15
            new SpectralLineDefinition("OI_6302", "\\ion{O}{i}", 6302.05, true),
            new SpectralLineDefinition("OI_6366", "\\ion{O}{i}", 6365.54, true),
            new SpectralLineDefinition("NI_6529", "\\ion{N}{i}", 6529.03, true),
            new SpectralLineDefinition("NII_6550", "\\ion{N}{ii}", 6549.86, true), 
            new SpectralLineDefinition("Ha_6565", "H$\\alpha$", 6564.61, true),
            // 20
            new SpectralLineDefinition("NII_6585", "\\ion{N}{ii}", 6585.27, true), 
            new SpectralLineDefinition("SII_6718", "\\ion{S}{ii}", 6718.29, true), 
            new SpectralLineDefinition("SII_6733", "\\ion{S}{ii}", 6732.67, true),
        };

        /*
        public static readonly DoubleArrayParam SdssEmissionLines = (DoubleArrayParam)new double[]
				{
                    1033.82, // OVI_1033
                    1215.67, // Lya_1216
                    1240.81, // NV_1241
                    1305.53, // OI_1306
                    1640.40, // HeII_1640
                    1665.85, // OIII_1666
                    3727.09, // OII_3727
                    3729.88, // OII_3730
                    3798.98, // Hh_3799
                    3836.47, // Oy_3836
                    3889.00, // HeI_3889
                    3971.19, // He_3971
                    4072.30, // SII_4072
                    4102.89, // Hd_4103
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
                    6718.29, // SII_6718
                    6732.67, // SII_6733
				};

        public static readonly string[] SdssEmissionLineNames = new string[]
				{
                    "OVI_1033", //0
                    "Lya_1216",
                    "NV_1241",
                    "OI_1306",
                    "HeII_1640",
                    "OIII_1666",
                    "OII_3727",
                    "OII_3730",
                    "Hh_3799",
                    "Oy_3836",
                    "HeI_3889", //10
                    "He_3971",
                    "SII_4072",
                    "Hd_4103",
                    "Hg_4342",
                    "OIII_4364",
                    "Hb_4863",
                    "OIII_4933",
                    "OIII_4960",
                    "OIII_5008",
                    "Mg_5177",  //20
                    "Na_5896",
                    "OI_6302",
                    "OI_6366",
                    "NI_6529",
                    "NII_6550",
                    "Ha_6565",
                    "NII_6585",
                    "SII_6718",
                    "SII_6733",
				};

        public static readonly DoubleArrayParam StandardLines = (DoubleArrayParam)new double[]
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
                    5897.99, //NaD_5897         
                    6302.05, //OI_6302          50
                    6314.45, //SIII_6314
                    6365.54, //OI_6366
                    6529.03, //NI_6529
                    6549.86, //NII_6550
                    6564.61, //Ha_6565
                    6585.27, //NII_6585
                    6680.27, //HeI_6680
                    6707.89, //Li_6708
                    6718.29, //SII_6718 
                    6732.67, //SII_6733         60
                    7067.48, //HeI_7067
                    7137.99, //ArIII_7137
                    7321.90, //OII_7321
                    7332.41, //OII_7332
                    7753.47, //ArIII_7753
                    8500.36, //CaII_8500
                    8544.44, //CaII_8544
                    8664.52, //CaII_8665
        };


        public static readonly string[] StandardLineNames = new string[]
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

        public static readonly int[] StandardLinesIds = new int[]
				{
                    1033     , //OVI_1033         0
                    1216, //Lya_1216
                    1241, //NV_1241
                    1306, //OI_1306
                    1335, //CII_1335
                    1398, //SiIV_1398
                    1400, //SiIV_OIV_1400
                    1549, //CIV_1549
                    1640, //HeII_1640
                    1666, //OIII_1666
                    1857   , //AlIII_1857       10
                    1909, //CIII_1909
                    2326, //CII_2326
                    2439, //NeIV_2439
                    2799, //MgII_2799
                    3347, //NeV_3347
                    3427, //NeV_3427
                    3705, //HeI_3705
                    3727, //OII_3727
                    3730, //OII_3730
                    3751     , //H12_3751         20
                    3772, //H11_3772
                    3799, //Hh_3799
                    3821, //He I_3821
                    3836, //Oy_3836
                    3837, //H9_3837
                    3870, //NeIII_3870
                    3889, //HeI_3889
                    3890, //H8_3890
                    3935, //K_3935
                    3970       , //H_3970           30
                    3971, //He_3971
                    4027, //HeI_4027
                    4070, //SII_4070
                    4072, //SII_4072
                    4103, //Hd_4103
                    4306, //G_4306
                    4342, //Hg_4342
                    4364, //OIII_4364
                    4473, //HeI_4473
                    4863      , //Hb_4863          40
                    4933, //OIII_4933
                    4960, //OIII_4960
                    4965, //OIII_4965
                    5008, //OIII_5008
                    5177, //Mg_5177
                    5202, //NI_5202
                    5877, //HeI_5877
                    5891, //NaD_5891
                    5896, //Na_5896
                    5897     , //NaD_5897         50
                    6302, //OI_6302
                    6314, //SIII_6314
                    6366, //OI_6366
                    6529, //NI_6529
                    6550, //NII_6550
                    6565, //Ha_6565
                    6585, //NII_6585
                    6680, //HeI_6680
                    6708, //Li_6708
                    6718     , //SII_6718         60
                    6733, //SII_6733
                    7067, //HeI_7067
                    7137, //ArIII_7137
                    7321, //OII_7321
                    7332, //OII_7332
                    7753, //ArIII_7753
                    8500, //CaII_8500
                    8544, //CaII_8544
                    8665, //CaII_8665
        };*/

        public static readonly SpectralIndexDefinition[] SpectralIndexDefinitions = 
		{
            // 0
			new SpectralIndexDefinition("Lick_CN1", "CN$_1$", 4142.125, 4177.125, 4080.125, 4117.625, 4244.125, 4284.125, SpectralIndexUnit.Mag),
			new SpectralIndexDefinition("Lick_CN2", "CN$_2$", 4142.125, 4177.125, 4083.875, 4096.375, 4244.125, 4284.125, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("Lick_Ca4227", "Ca4227", 4222.250, 4234.750, 4211.000, 4219.750, 4241.000, 4251.000, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_G4300", "G4300", 4281.375, 4316.375, 4266.375, 4282.625, 4318.875, 4335.125, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe4383", "Fe4383", 4369.125, 4420.375, 4359.125, 4370.375, 4442.875, 4455.375, SpectralIndexUnit.EW ),
			// 5
            new SpectralIndexDefinition("Lick_Ca4455", "Ca4455", 4452.125, 4474.625, 4445.875, 4454.625, 4477.125, 4492.125, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe4531", "Fe4531", 4514.250, 4559.250, 4504.250, 4514.250, 4560.500, 4579.250, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_C4668", "C4668", 4634.000, 4720.250, 4611.500, 4630.250, 4742.750, 4756.500, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Hb", "H$\\beta$", 4847.875, 4876.625, 4827.875, 4847.875, 4876.625, 4891.625, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe5015", "Fe5015", 4977.750, 5054.000, 4946.500, 4977.750, 5054.000, 5065.250, SpectralIndexUnit.EW ),
			// 10
            new SpectralIndexDefinition("Lick_Mg1", "Mg$_1$", 5069.125, 5134.125, 4895.125, 4957.625, 5301.125, 5366.125, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("Lick_Mg2", "Mg$_2$", 5154.125, 5196.625, 4895.125, 4957.625, 5301.125, 5366.125, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("Lick_Mgb", "Mg $b$", 5160.125, 5192.625, 5142.625, 5161.375, 5191.375, 5206.375, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe5270", "Fe5270", 5245.650, 5285.650, 5233.150, 5248.150, 5285.650, 5318.150, SpectralIndexUnit.EW), 
			new SpectralIndexDefinition("Lick_Fe5335", "Fe5335", 5312.125, 5352.125, 5304.625, 5315.875, 5353.375, 5363.375, SpectralIndexUnit.EW ),
			// 15
            new SpectralIndexDefinition("Lick_Fe5406", "Fe5406", 5387.500, 5415.000, 5376.250, 5387.500, 5415.000, 5425.000, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe5709", "Fe5709", 5696.625, 5720.375, 5672.875, 5696.625, 5722.875, 5736.625, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Fe5782", "Fe5782", 5776.625, 5796.625, 5765.375, 5775.375, 5797.875, 5811.625, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_NaD", "Na D", 5876.875, 5909.375, 5860.625, 5875.625, 5922.125, 5948.125, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_TiO1", "TiO$_1$", 5936.625, 5994.125, 5816.625, 5849.125, 6038.625, 6103.625, SpectralIndexUnit.Mag ), 
			// 20
            new SpectralIndexDefinition("Lick_TiO2", "TiO$_2$", 6189.625, 6272.125, 6066.625, 6141.625, 6372.625, 6415.125, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("Lick_Hd_A", "H$\\delta_A$", 4083.500, 4122.250, 4041.600, 4079.750, 4128.500, 4161.000, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Hg_A", "H$\\gamma_A$", 4319.750, 4363.500, 4283.500, 4319.750, 4367.250, 4419.750, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Hd_F", "H$\\delta_F$", 4091.000, 4112.250, 4057.250, 4088.500, 4114.750, 4137.250, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("Lick_Hg_F", "H$\\gamma_F$", 4331.250, 4352.250, 4283.500, 4319.750, 4354.750, 4384.750, SpectralIndexUnit.EW ),
			// 25
            new SpectralIndexDefinition("DTT_CaII8498", "\\ion{Ca}{ii} $\\lambda8498$", 8483.000, 8513.000, 8447.500, 8462.500, 8842.500, 8857.500, SpectralIndexUnit.EW), 
			new SpectralIndexDefinition("DTT_CaII8542", "\\ion{Ca}{ii} $\\lambda8542$", 8527.000, 8557.000, 8447.500, 8462.500, 8842.500, 8857.500, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("DTT_CaII8662", "\\ion{Ca}{ii} $\\lambda8662$", 8647.000, 8677.000, 8447.500, 8462.500, 8842.500, 8857.500, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("DTT_MgI8807", "\\ion{Mg}{i} $\\lambda8807$", 8799.500, 8814.500, 8775.000, 8787.000, 8845.000, 8855.000, SpectralIndexUnit.EW ),
			new SpectralIndexDefinition("BH_CNB", "CNB", 3810.000, 3910.000, 3785.000, 3810.000, 3910.000, 3925.000, SpectralIndexUnit.Mag ),
			// 30
            new SpectralIndexDefinition("BH_HK", "H + K", 3925.000, 3995.000, 3910.000, 3925.000, 3995.000, 4010.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_CaI", "\\ion{Ca}{i}", 4215.000, 4245.000, 4200.000, 4215.000, 4245.000, 4260.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_G", "G", 4285.000, 4315.000, 4275.000, 4285.000, 4315.000, 4325.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_Hb", "Hb", 4830.000, 4890.000, 4800.000, 4830.000, 4890.000, 4920.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_MgG", "MgG", 5150.000, 5195.000, 5125.000, 5150.000, 5195.000, 5220.000, SpectralIndexUnit.Mag ),
			// 35
            new SpectralIndexDefinition("BH_MH", "MH", 4940.000, 5350.000, 4740.000, 4940.000, 5350.000, 5550.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_FC", "FC", 5250.000, 5280.000, 5225.000, 5250.000, 5280.000, 5305.000, SpectralIndexUnit.Mag ),
			new SpectralIndexDefinition("BH_NaD", "NaD", 5865.000, 5920.000, 5835.000, 5865.000, 5920.000, 5950.000, SpectralIndexUnit.Mag ),
            new SpectralIndexDefinition("D4000", "D$4000$", 0, 0, 3750.000, 3950.000, 4050.000, 4250.000, SpectralIndexUnit.Ratio),
            new SpectralIndexDefinition("D4000_n", "D$4000_n$", 0, 0, 3850.000, 3950.000, 4000.000, 4100.000, SpectralIndexUnit.Ratio),
		};

    }
}
#region Revision History
/* Revision History

        $Log: Constants.cs,v $
        Revision 1.1  2008/01/08 21:36:43  dobos
        Initial checkin


*/
#endregion
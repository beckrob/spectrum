#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Enum.cs,v 1.1 2008/01/08 21:36:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:54 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.SpectrumLib
{
    /*
    public enum WavelengthConversion
    {
        None,
        Vac2Air,
        Air2Vac
    }

    public enum NormalizeMethods
    {
        None,
        FluxAtWavelength,
        FluxMedianInRanges,
        FluxIntegralInRanges,
    }

    public enum NormalizeTemplates
    {
        Unknown,
        Galaxy,
        Qso
    }
     * */

    /*
    public enum CompositeMethods
    {
        Average,
        Median,
        Sum
    }
     * */

    /*
    public enum MagnitudeSystems
    {
        Flux,
        Magnitude
    }*/

    public enum SpectrumWavelengthScale
    {
        Any = -1,
        Linear = 0,
        Logarithmic = 1,
        Other = 2
    }

    public enum SpectrumOrigin
    {
        Any = -1,
        Observed = 0,
        Simulated = 1,
        Composite = 2
    }

    /*
    public enum FitMethods
    {
        LeastSquare,
        NonNegativeLeastSquare
    }
     * */

    // sloan specific

    public enum SpectrumZStatus
    {
        Not_Measured = 0,   /* Not yet measured                                            */
        Failed = 1,   /* Redshift measurement failed                                 */
        Inconsistent = 2,   /* Xcorr & emz redshifts both high-confidence but inconsistent */
        Xcorr_Emline = 3,   /* Xcorr plus consistent emz redshift measurement              */
        Xcorr_Hic = 4,   /* z determined from x-corr with high confidence               */
        Xcorr_Loc = 5,   /* z determined from x-corr with low confidence                */
        Emline_Xcorr = 6,   /* Emz plus consistent xcorr redshift measurement              */
        Emline_Hic = 7,   /* z determined from em-lines with high confidence             */
        Emline_Loc = 8,   /* z determined from em-lines with low confidence              */
        Manual_Hic = 9,   /* z determined "by hand" with high confidence                 */
        Manual_Loc = 10,   /* z determined "by hand" with low confidence                  */
        Xcorr_4000break = 11    /* Xcorr redshift determined when EW(4000break) > 0.95         */
    }

    public enum SpectrumZWarning
    {
        Ok = 0x000,    /* no warnings                                      */
        No_Spec = 0x001,    /* no spec                                          */
        No_Blue = 0x002,    /* no blueside                                      */
        No_Red = 0x004,    /* no redside                                       */
        Not_Gal = 0x010,    /* classification does not match galaxy target      */
        Not_Qso = 0x020,    /* classification does not match qso target         */
        Not_Star = 0x040,    /* classification does not match star target        */
        Gal_Coef = 0x080,    /* strange galaxy coefficients                      */
        EmAb_Inc = 0x100,    /* emmission and absorbtion redshifts inconsistent  */
        Ab_Inc = 0x200,    /* absorbtion redshifts inconsistent ,multiple peaks*/
        Em_Inc = 0x400,    /* emmission redshifts inconsistent                 */
        Hiz = 0x800,    /* redshift is   high                               */
        Loc = 0x1000,   /* confidence is low                                */
        Low_Sng = 0x2000,   /* signal to noise is low in g'                     */
        Low_Snr = 0x4000,   /* signal to noise is low in r'                     */
        Low_Sni = 0x8000,   /* signal to noise is low in i'                     */
        Ew4000Break = 0x10000   /* EW(4000break) > 0.95                             */
    }

    public enum SpectrumClass
    {
        Any = -1,						// Not used in DB, only for search
        Unknown = 0x0000000000000000,
        Star = 0x0000000000000001,
        Galaxy = 0x0000000000000002,
        Qso = 0x0000000000000003,
        Hiz_Qso = 0x0000000000000004,
        Sky = 0x0000000000000005,
        Star_Late = 0x0000000000000006,
        Gal_Em = 0x0000000000000007,
        Merger = 101,
        BrightStarHalo = 102
    }

    [Flags]
    public enum PointMask : long
    {
        Ok = 0x000,
        NoPlug = 0x001,      /*  Fiber not listed in plugmap file                     */
        BadTrace = 0x002,      /*  Bad trace from routine TRACE320CRUDE                 */
        BadFlat = 0x004,      /*  Low counts in fiberflat                              */
        BadArc = 0x008,      /*  Bad arc solution                                     */
        Manybadcol = 0x010,      /*  More than 10% pixels are bad columns                 */
        ManyReject = 0x020,      /*  More than 10% pixels are rejected in extraction      */
        LargeShift = 0x040,      /*  Large spatial shift between flat and object position */
        NearBadPix = 0x10000,    /*  Bad pixel within 3 pixels of trace                   */
        LowFlat = 0x20000,    /*  Flat field less than 0.5                             */
        FullReject = 0x40000,    /*  Pixel fully rejected in extraction                   */
        PartialRej = 0x80000,    /*  Some pixels rejected in extraction                   */
        ScatLight = 0x100000,   /*  Scattered light significant                          */
        CrossTalk = 0x200000,   /*  Cross-talk significant                               */
        NoSky = 0x400000,   /*  Sky level unknown at this. wavelength                 */
        BrightSky = 0x800000,   /*  Sky level > flux + 10*(flux error)                   */
        NoData = 0x1000000,  /*  No data available in combine B-spline                */
        CombineRej = 0x2000000,  /*  Rejected in combine B-spline                         */
        BadFluxFactor = 0x4000000,  /*  Low flux-calibration or flux-correction factor       */
        BadSkyChi = 0x8000000,  /*  Chi^2 > 4 in sky residuals at this. wavelength        */
        RedMonster = 0x10000000, /*  Contiguous region of bad chi^2 in sky residuals      */
        EmLine = 0x40000000, /*  Emmission line detected here                         */

        SDSSBadValue = (PointMask.NoPlug |
                PointMask.BadTrace |
                PointMask.BadFlat |
                PointMask.BadArc |
                PointMask.Manybadcol |
                PointMask.ManyReject |
                PointMask.LowFlat |
                PointMask.FullReject |
                PointMask.ScatLight |
                PointMask.CrossTalk |
                PointMask.NoSky |
                PointMask.NoData |
                PointMask.CombineRej |
                PointMask.BadFluxFactor |
                PointMask.BadSkyChi |
                PointMask.RedMonster)
    }
}
#region Revision History
/* Revision History

        $Log: Enum.cs,v $
        Revision 1.1  2008/01/08 21:36:54  dobos
        Initial checkin


*/
#endregion
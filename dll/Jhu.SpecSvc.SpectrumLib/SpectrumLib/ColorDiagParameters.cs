#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ColorDiagParameters.cs,v 1.1 2008/01/08 21:36:53 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:53 $
 */
#endregion
using System;
using VoServices.Schema;

namespace VoServices.SpecSvc.Lib
{
    /// <summary>
    /// Summary description for SynthMagParameters.
    /// </summary>
    [Serializable]
    public class ColorDiagParameters
    {
        //***
        public int[] XFilters;
        public int[] YFilters;
        public DoubleInterval Redshift;
        public int Steps;

        public ColorDiagParameters()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            XFilters = new int[2];
            YFilters = new int[2];
            Redshift = new DoubleInterval(ParamRequired.Optional);
            Redshift.Min.Value = 0.0;
            Redshift.Max.Value = 1.0;
            Steps = 100;
        }
    }
}
#region Revision History
/* Revision History

        $Log: ColorDiagParameters.cs,v $
        Revision 1.1  2008/01/08 21:36:53  dobos
        Initial checkin


*/
#endregion
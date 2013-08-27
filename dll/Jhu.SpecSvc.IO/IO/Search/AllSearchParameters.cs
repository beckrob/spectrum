#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: AllSearchParameters.cs,v 1.1 2008/01/08 22:01:43 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:43 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class AllSearchParameters : SearchParametersBase
    {

        private float sampleFraction = 1.0f;

        public float SampleFraction
        {
            get { return sampleFraction; }
            set { sampleFraction = value; }
        }

        public AllSearchParameters()
        {
        }

        public AllSearchParameters(bool initialize)
            : base(initialize)
        {
        }

        public AllSearchParameters(AllSearchParameters old)
            : base(old)
        {
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.All; }
        }

        public AllSearchParameters GetStandardUnits()
        {
            return this;
        }

    }
}
#region Revision History
/* Revision History

        $Log: AllSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:43  dobos
        Initial checkin


*/
#endregion
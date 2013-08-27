#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SimilarSearchParameters.cs,v 1.1 2008/01/08 22:01:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:45 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.IO
{
    public class SimilarSearchParameters : SearchParametersBase
    {
        #region Member variables

        private Jhu.SpecSvc.SpectrumLib.Spectrum spectrum;
        private double[] coeffs;
        private string basisId;
        private int resultsCount;

        #endregion

        public override SearchMethod Type
        {
            get { return SearchMethod.Similar; }
        }

        public Jhu.SpecSvc.SpectrumLib.Spectrum Spectrum
        {
            get { return this.spectrum; }
            set { this.spectrum = value; }
        }

        public double[] Coeffs
        {
            get { return coeffs; }
            set { coeffs = value; }
        }

        public string BasisId
        {
            get { return basisId; }
            set { basisId = value; }
        }

        public int ResultsCount
        {
            get { return this.resultsCount; }
            set { this.resultsCount = value; }
        }

        #region Constructors

        public SimilarSearchParameters()
        {
        }

        public SimilarSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public SimilarSearchParameters(IdSearchParameters old)
        {
            CopyMembers(old);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.spectrum = null;
            this.resultsCount = 0;
        }

        private void CopyMembers(SimilarSearchParameters old)
        {
            base.CopyMembers(old);

            this.spectrum = new Jhu.SpecSvc.SpectrumLib.Spectrum(old.spectrum);
            this.resultsCount = old.resultsCount;
        }

        public SimilarSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SimilarSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:45  dobos
        Initial checkin


*/
#endregion
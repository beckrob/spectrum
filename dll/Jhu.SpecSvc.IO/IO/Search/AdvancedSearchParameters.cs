#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: AdvancedSearchParameters.cs,v 1.1 2008/01/08 22:01:43 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:43 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Spectrum;

namespace Jhu.SpecSvc.IO
{
    public class AdvancedSearchParameters : SearchParametersBase
    {
        #region Member variables

        private string keyword;
        private string name;
        private string[] targetClass;
        private string[] spectralClass;
        private string[] creationType;
        private TimeInterval date;
        private string version;
        private PositionParam pos;
        private DoubleParam sr;
        private DoubleInterval snr;
        private DoubleInterval redshift;
        private DoubleInterval redshiftStatError;
        private DoubleInterval redshiftConfidence;
        private DoubleInterval varAmpl;
        private DoubleInterval spectralCoverage;
        private DoubleInterval spectralResPower;
        private DoubleParam flux;
        private string[] fluxCalibration;

        #endregion

        public string Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string[] TargetClass
        {
            get { return this.targetClass; }
            set { this.targetClass = value; }
        }

        public string[] SpectralClass
        {
            get { return this.spectralClass; }
            set { this.spectralClass = value; }
        }

        public string[] CreationType
        {
            get { return this.creationType; }
            set { this.creationType = value; }
        }

        public TimeInterval Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public PositionParam Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        public DoubleParam Sr
        {
            get { return this.sr; }
            set { this.sr = value; }
        }

        public DoubleInterval Snr
        {
            get { return this.snr; }
            set { this.snr = value; }
        }

        public DoubleInterval Redshift
        {
            get { return this.redshift; }
            set { this.redshift = value; }
        }

        public DoubleInterval RedshiftStatError
        {
            get { return this.redshiftStatError; }
            set { this.redshiftStatError = value; }
        }

        public DoubleInterval RedshiftConfidence
        {
            get { return this.redshiftConfidence; }
            set { this.redshiftConfidence = value; }
        }

        public DoubleInterval VarAmpl
        {
            get { return this.varAmpl; }
            set { this.varAmpl = value; }
        }

        public DoubleInterval SpectralCoverage
        {
            get { return this.spectralCoverage; }
            set { this.spectralCoverage = value; }
        }

        public DoubleInterval SpectralResPower
        {
            get { return this.spectralResPower; }
            set { this.spectralResPower = value; }
        }

        public DoubleParam Flux
        {
            get { return this.flux; }
            set { this.flux = value; }
        }

        public string[] FluxCalibration
        {
            get { return this.fluxCalibration; }
            set { this.fluxCalibration = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Advanced; }
        }

        #region Constructors

        public AdvancedSearchParameters()
        {
        }

        public AdvancedSearchParameters(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public AdvancedSearchParameters(AdvancedSearchParameters old)
        {
            CopyMembers(old);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.keyword = string.Empty;
            this.name = string.Empty;
            this.targetClass = null;
            this.spectralClass = null;
            this.creationType = null;
            this.date = new TimeInterval(ParamRequired.Optional);
            this.version = string.Empty;
            this.pos = new PositionParam(true);
            this.sr = new DoubleParam(true);
            this.snr = new DoubleInterval(ParamRequired.Optional);
            this.redshift = new DoubleInterval(ParamRequired.Optional);
            this.redshiftStatError = new DoubleInterval(ParamRequired.Optional);
            this.redshiftConfidence = new DoubleInterval(ParamRequired.Optional);
            this.varAmpl = new DoubleInterval(ParamRequired.Optional);
            this.spectralCoverage = new DoubleInterval(ParamRequired.Optional);
            this.spectralResPower = new DoubleInterval(ParamRequired.Optional);
            this.flux = new DoubleParam(true);
            this.fluxCalibration = null;
        }

        private void CopyMembers(AdvancedSearchParameters old)
        {
            base.CopyMembers(old);

            this.keyword = old.keyword;
            this.name = old.name;

            CopyArray<string>(old.targetClass, out this.targetClass);
            CopyArray<string>(old.spectralClass, out this.spectralClass);
            CopyArray<string>(old.creationType, out this.creationType);

            this.date = old.date == null ? null : new TimeInterval(old.date);
            this.version = old.version == null ? null : old.version;
            this.pos = old.pos == null ? null : new PositionParam(old.pos);
            this.sr = old.sr == null ? null : new DoubleParam(old.sr);
            this.snr = old.snr == null ? null : new DoubleInterval(old.snr);
            this.redshift = old.redshift == null ? null : new DoubleInterval(old.redshift);
            this.redshiftStatError = old.redshiftStatError == null ? null : new DoubleInterval(old.redshiftStatError);
            this.redshiftConfidence = old.redshiftConfidence == null ? null : new DoubleInterval(old.redshiftConfidence);
            this.varAmpl = old.varAmpl == null ? null : new DoubleInterval(old.varAmpl);
            this.spectralCoverage = old.spectralCoverage == null ? null : new DoubleInterval(old.spectralCoverage);
            this.spectralResPower = old.spectralResPower == null ? null : new DoubleInterval(old.spectralResPower);
            this.flux = old.flux == null ? null : new DoubleParam(old.flux);

            CopyArray<string>(old.fluxCalibration, out this.fluxCalibration);
        }

        private void CopyArray<T>(T[] oldArray, out T[] newArray)
        {
            if (oldArray == null)
                newArray = null;
            else
            {
                newArray = new T[oldArray.Length];
                oldArray.CopyTo(newArray, 0);
            }
        }

        public AdvancedSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: AdvancedSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:43  dobos
        Initial checkin


*/
#endregion
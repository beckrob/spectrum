#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * Jhu.SpecSvc.Schema classes support the implementation
 * of Virtual Observatory Data Models.
 * Jhu.SpecSvc.Schema.Spectrum implements the spectrum data model
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectralAccuracy.cs,v 1.1 2008/01/08 22:26:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:50 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class SpectralAccuracy : Group, IAccuracy, ICloneable
    {
        private DoubleParam binSize;
        private DoubleParam binLow;
        private DoubleParam binHigh;
        private DoubleParam statError;
        private DoubleParam sysError;
        private DoubleParam statErrLow;
        private DoubleParam statErrHigh;
        //private IntParam quality;

        // ----- Characterization

        [XmlIgnore]
        [Field(Required = ParamRequired.Recommended, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.BinSize
        {
            get { return binSize; }
            set { binSize = value; }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Recommended, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.StatError
        {
            get { return statError; }
            set { statError = value; }
        }
        // -----------------------

        /// <summary>
        /// Wavelength bin size
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;spect.binSize", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_BinSize", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinSize
        {
            get { return binSize; }
            set { binSize = value; }
        }

        /// <summary>
        /// Spectral coord bin lower end
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.min", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_BinLow", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinLow
        {
            get { return binLow; }
            set { binLow = value; }
        }

        /// <summary>
        /// Spectral coord bin upper end
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.max", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_BinHigh", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam BinHigh
        {
            get { return binHigh; }
            set { binHigh = value; }
        }

        /// <summary>
        /// Spectral coord measurement error (symetric, plus/minus)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.error", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_StatError", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatError
        {
            get { return statError; }
            set { statError = value; }
        }

        /// <summary>
        /// Spectral coord measurement error (lower bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.error;stat.min", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_StatErrLow", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrLow
        {
            get { return statErrLow; }
            set { statErrLow = value; }
        }

        /// <summary>
        /// Spectral coord measurement error (upper bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.error;stat.max", DefaultUnit = SpectralAxis.COMMONUNIT, RefMember = "Spectral_Accuracy_StatErrHigh", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrHigh
        {
            get { return statErrHigh; }
            set { statErrHigh = value; }
        }

        /// <summary>
        /// Spectral coord systematic error
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "em.wl;stat.error.sys", DefaultUnit = SpectralAxis.COMMONUNIT)]
        public DoubleParam SysError
        {
            get { return sysError; }
            set { sysError = value; }
        }

        public IntParam Quality
        {
            get { return null; }
            set { }
        }

        #region Constructors
        public SpectralAccuracy()
        {
        }

        public SpectralAccuracy(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public SpectralAccuracy(SpectralAccuracy old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static SpectralAccuracy Clone(SpectralAccuracy old)
        {
            if (old != null)
            {
                return new SpectralAccuracy(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: SpectralAccuracy.cs,v $
        Revision 1.1  2008/01/08 22:26:50  dobos
        Initial checkin


*/
#endregion
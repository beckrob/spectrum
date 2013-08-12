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
 *   ID:          $Id: FluxAccuracy.cs,v 1.1 2008/01/08 22:26:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:45 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class FluxAccuracy : Group, ICloneable, IAccuracy
    {
        private DoubleParam statError;
        private DoubleParam sysError;
        private DoubleParam statErrLow;
        private DoubleParam statErrHigh;
        private IntParam quality;

        // ----- Characterization

        [XmlIgnore]
        [Field(Required = ParamRequired.Derived, SerializationMode = SerializationMode.Characterization)]
        DoubleParam IAccuracy.BinSize
        {
            get
            {
                return null;
            }
            set { }
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
        /// Statistical error (symmetric, plus/minus)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Recommended, Ucd = "phot.flux.density;em.wl;stat.error", DefaultUnit = FluxAxis.COMMONUNIT, RefMember = "Flux_Accuracy_StatError", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatError
        {
            get { return statError; }
            set { statError = value; }
        }

        /// <summary>
        /// Systematic error (symmetric, plus/minus)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Recommended, Ucd = "phot.flux.density;em.wl;stat.error.sys", DefaultUnit = FluxAxis.COMMONUNIT)]
        public DoubleParam SysError
        {
            get { return sysError; }
            set { sysError = value; }
        }

        /// <summary>
        /// Statistical error (lower bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "phot.flux.density;em.wl;stat.error;stat.min", DefaultUnit = FluxAxis.COMMONUNIT, RefMember = "Flux_Accuracy_StatErrLow", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrLow
        {
            get { return statErrLow; }
            set { statErrLow = value; }
        }

        /// <summary>
        /// Statistical error (upper bound, absolute)
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "phot.flux.density;em.wl;stat.error;stat.max", DefaultUnit = FluxAxis.COMMONUNIT, RefMember = "Flux_Accuracy_StatErrHigh", ReferenceMode = ReferenceMode.Item)]
        public DoubleParam StatErrHigh
        {
            get { return statErrHigh; }
            set { statErrHigh = value; }
        }

        /// <summary>
        /// Quality mask
        /// </summary>
        [XmlElement]
        [Field(Required = ParamRequired.Optional, Ucd = "meta.code.qual;phot.flux;em.wl", RefMember = "Flux_Accuracy_Quality", ReferenceMode = ReferenceMode.Item)]
        public IntParam Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        #region Constructors
        public FluxAccuracy()
        {
        }

        public FluxAccuracy(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public FluxAccuracy(FluxAccuracy old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static FluxAccuracy Clone(FluxAccuracy old)
        {
            if (old != null)
            {
                return new FluxAccuracy(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: FluxAccuracy.cs,v $
        Revision 1.1  2008/01/08 22:26:45  dobos
        Initial checkin


*/
#endregion
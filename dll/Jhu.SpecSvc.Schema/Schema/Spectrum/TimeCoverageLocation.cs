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
 *   ID:          $Id: TimeCoverageLocation.cs,v 1.1 2008/01/08 22:26:55 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:55 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class TimeCoverageLocation : Group, ICloneable, ICoverageLocation
    {
		private DoubleParam value;

        /// <summary>
        /// Spectral coord value
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Mandatory, Ucd = "time.epoch", DefaultUnit = "d")]
		public DoubleParam Value
		{
			get { return value; }
			set { this.value = value; }
		}

        [XmlIgnore]
        ParamBase ICoverageLocation.Value
        {
            get { return this.value; }
            set { this.value = (DoubleParam)value; }
        }


        #region Constructors
        public TimeCoverageLocation()
        {
        }

        public TimeCoverageLocation(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public TimeCoverageLocation(TimeCoverageLocation old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static TimeCoverageLocation Clone(TimeCoverageLocation old)
        {
            if (old != null)
            {
                return new TimeCoverageLocation(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: TimeCoverageLocation.cs,v $
        Revision 1.1  2008/01/08 22:26:55  dobos
        Initial checkin


*/
#endregion
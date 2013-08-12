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
 *   ID:          $Id: Spectrum.cs,v 1.1 2008/01/08 22:26:41 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:41 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public partial class Spectrum
    {
        private ModelParameters modelParameters;

        [XmlArray(Order = 150)]
        public double[] Flux_Continuum;
        [XmlArray(Order = 151)]
        public double[] Flux_Lines;
        [XmlArray(Order = 152)]
        public double[] Model_Continuum;
        [XmlArray(Order = 153)]
        public double[] Model_Lines;

        [XmlArray(Order = 180)]
        public double[] Age_Value;
        [XmlArray(Order = 181)]
        public double[] Sfr_Value;

        [XmlElement(Order=75)]
        [Field(Required = ParamRequired.Custom)]
        public ModelParameters ModelParameters
        {
            get { return modelParameters; }
            set { modelParameters = value; }
        }
    }
}
#region Revision History
/* Revision History

        $Log: Spectrum.cs,v $
        Revision 1.1  2008/01/08 22:26:41  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: BackgroundModel.cs,v 1.1 2008/01/08 22:26:42 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:42 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
	public class BackgroundModel : Axis, ICloneable, IAxis
	{
		public const string COMMONUNIT = "10e-17 erg s-1 cm-2 A-1";
		public const string SIUNIT = "W m-2 m-1";

		private DoubleParam value;
		private BackgroundModelAccuracy accuracy;

        [XmlIgnore]
        TextParam IAxis.Name
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = "BackgroundModel";
                res.Ucd = "meta.id";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        public TextParam Ucd
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = value.Ucd;
                res.Ucd = "meta.ucd";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        [Field(Required = ParamRequired.Dummy)]
        public TextParam Unit
        {
            get
            {
                TextParam res = new TextParam();
                res.Value = value.Unit;
                res.Ucd = "meta.unit";
                return res;
            }
            set { }
        }

        [XmlIgnore]
        DoubleParam IAxis.Resolution
        {
            get { return null; }
            set { }
        }

        [XmlIgnore]
        DoubleParam IAxis.ResPower
        {
            get { return null; }
            set { }
        }

        [XmlIgnore]
        TextParam IAxis.Calibration
        {
            get { return null; }
            set {  }
        }

		/// <summary>
		/// Flux values for points
		/// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional, Ucd = "phot.flux.density;em.wl", DefaultUnit = BackgroundModel.COMMONUNIT, RefMember = "BackgroundModel_Value", ReferenceMode=ReferenceMode.Item )]
		public DoubleParam Value
		{
			get { return value; }
			set { this.value = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public BackgroundModelAccuracy Accuracy
		{
			get { return accuracy; }
			set { this.accuracy = value; }
		}

        IAccuracy IAxis.Accuracy
        {
            get { return accuracy; }
            set { accuracy = (BackgroundModelAccuracy)value; }
        }

        ICoverage IAxis.Coverage
        {
            get { return null; }
            set { }
        }

        ISamplingPrecision IAxis.SamplingPrecision
        {
            get { return null; }
            set { }
        }

		#region Constructors
		public BackgroundModel()
		{
		}

		public BackgroundModel(ParamRequired initializationLevel)
		{
			SchemaUtil.InitializeMembers(this, initializationLevel, true);
		}

		public BackgroundModel(BackgroundModel old)
		{
			SchemaUtil.CopyMembers(this, old);
		}
		#endregion

		#region Clone functions
		public object Clone()
		{
			return Clone(this);
		}

		public static BackgroundModel Clone(BackgroundModel old)
		{
			if (old != null)
			{
				return new BackgroundModel(old);
			}
			else
				return null;
		}
		#endregion
	}
}
#region Revision History
/* Revision History

        $Log: BackgroundModel.cs,v $
        Revision 1.1  2008/01/08 22:26:42  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: Data.cs,v 1.1 2008/01/08 22:26:44 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:44 $
 */
#endregion
using System;
using System.Xml.Serialization;
using Jhu.SpecSvc.Schema.Characterization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class Data : Group, ICloneable, IDataCube
	{
		private SpectralAxis spectralAxis;
		private FluxAxis fluxAxis;
		private TimeAxis timeAxis;
		private SpatialAxis spatialAxis;
		private BackgroundModel backgroundModel;

		[XmlElement(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="char")]
		[Field(Required = ParamRequired.Mandatory)]
		public SpectralAxis SpectralAxis
		{
			get { return spectralAxis; }
			set { spectralAxis = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Mandatory)]
		public FluxAxis FluxAxis
		{
			get { return fluxAxis; }
			set { fluxAxis = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TimeAxis TimeAxis
		{
			get { return timeAxis; }
			set { timeAxis = value; }
		}

		[XmlElement]
        [Field(Required = ParamRequired.SpecService, SerializationMode = SerializationMode.Characterization)]
		public SpatialAxis SpatialAxis
		{
			get { return spatialAxis; }
			set { spatialAxis = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public BackgroundModel BackgroundModel
		{
			get { return backgroundModel; }
			set { backgroundModel = value; }
		}

		IAxis[] IDataCube.Axes
		{
			get
			{
				return new IAxis[]
				{
					(IAxis) spectralAxis,
					(IAxis) fluxAxis,
					(IAxis) timeAxis,
					(IAxis) spatialAxis,
					(IAxis) backgroundModel
				};
			}
			set { }
		}

		#region Constructors
        public Data()
        {
        }

        public Data(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Data(Data old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Data Clone(Data old)
        {
            if (old != null)
            {
                return new Data(old);
            }
            else
                return null;
        }
        #endregion
	}
}
#region Revision History
/* Revision History

        $Log: Data.cs,v $
        Revision 1.1  2008/01/08 22:26:44  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: RedshiftFrame.cs,v 1.1 2008/01/08 22:26:47 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:47 $
 */
#endregion
using System;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.Schema.Spectrum
{
    public class RedshiftFrame : Group, ICloneable
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string OPT = "OPT";
        public const string RADIO = "RADIO";
        public const string REL = "REL";

		private TextParam name;
		private TextParam dopplerDefinition;
		private TextParam refPos;

        /// <summary>
        /// Redshift frame name
        /// </summary>
		[XmlElement]
		[Field(Required = ParamRequired.Optional)]
		public TextParam Name
		{
			get { return name; }
			set { name = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional, DefaultValue = RedshiftFrame.UNKNOWN)]
		public TextParam DopplerDefinition
		{
			get { return dopplerDefinition; }
			set { dopplerDefinition = value; }
		}

		[XmlElement]
		[Field(Required = ParamRequired.Optional, DefaultValue = CoordSys.UNKNOWN)]
		public TextParam RefPos
		{
			get { return refPos; }
			set { refPos = value; }
		}

#region Constructors
        public RedshiftFrame()
        {
        }

        public RedshiftFrame(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public RedshiftFrame(RedshiftFrame old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static RedshiftFrame Clone(RedshiftFrame old)
        {
            if (old != null)
            {
                return new RedshiftFrame(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: RedshiftFrame.cs,v $
        Revision 1.1  2008/01/08 22:26:47  dobos
        Initial checkin


*/
#endregion
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
 *   ID:          $Id: Dataset.cs,v 1.1 2008/01/08 22:26:58 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:26:58 $
 */
#endregion
using System;

namespace Jhu.SpecSvc.Schema.Ssa
{
    /// <summary>
    /// Summary description for Dataset.
    /// </summary>
    public class Dataset : Group, ICloneable
    {
        public TextParam dataModel;
        public TextParam type;
        public IntParam length;
        public TextParam timeSI;
        public TextParam spectralSI;
        public TextParam fluxSI;

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataModel", ReferenceMode = ReferenceMode.Item)]
        public TextParam DataModel
        {
            get { return dataModel; }
            set { dataModel = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataType", ReferenceMode = ReferenceMode.Item)]
        public TextParam Type
        {
            get { return type; }
            set { type = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataLength", ReferenceMode = ReferenceMode.Item)]
        public IntParam Length
        {
            get { return length; }
            set { length = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataTimeSI", ReferenceMode = ReferenceMode.Item)]
        public TextParam TimeSI
        {
            get { return timeSI; }
            set { timeSI = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataSpectralSI", ReferenceMode = ReferenceMode.Item)]
        public TextParam SpectralSI
        {
            get { return spectralSI; }
            set { spectralSI = value; }
        }

        [Field(Required = ParamRequired.Mandatory, RefMember = "DataFluxSI", ReferenceMode = ReferenceMode.Item)]
        public TextParam FluxSI
        {
            get { return fluxSI; }
            set { fluxSI = value; }
        }

        #region Constructors
        public Dataset()
        {
        }

        public Dataset(bool initialize)
        {
            if (initialize)
            {
                SchemaUtil.InitializeMembers(this, ParamRequired.Custom, true);
            }
        }

        public Dataset(ParamRequired initializationLevel)
        {
            SchemaUtil.InitializeMembers(this, initializationLevel, true);
        }

        public Dataset(Dataset old)
        {
            SchemaUtil.CopyMembers(this, old);
        }
        #endregion

        #region Clone functions
        public object Clone()
        {
            return Clone(this);
        }

        public static Dataset Clone(Dataset old)
        {
            if (old != null)
            {
                return new Dataset(old);
            }
            else
                return null;
        }
        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: Dataset.cs,v $
        Revision 1.1  2008/01/08 22:26:58  dobos
        Initial checkin


*/
#endregion
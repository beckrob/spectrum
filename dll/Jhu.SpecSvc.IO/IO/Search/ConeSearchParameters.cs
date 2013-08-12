#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ConeSearchParameters.cs,v 1.1 2008/01/08 22:01:43 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:43 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;

namespace Jhu.SpecSvc.IO
{
    public class ConeSearchParameters : SearchParametersBase
    {

        #region Member variables

        private PositionParam pos; 
        private DoubleParam sr;
        
        #endregion
        #region Properties

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

        public override SearchMethods Type
        {
            get { return SearchMethods.Cone; }
        }

        #endregion
        #region Constructors

        public ConeSearchParameters()
        {
        }

        public ConeSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public ConeSearchParameters(ConeSearchParameters old)
        {
            CopyMembers(old);
        }

        public ConeSearchParameters(SearchParametersBase old)
            : base(old)
        {
            InitializeMembers();
        }

        public ConeSearchParameters(Position pos, double sr)
        {
            InitializeMembers();

            this.pos.Value = pos;
            this.sr.Value = sr;
        }

        public ConeSearchParameters(PositionParam pos, DoubleParam sr)
        {
            InitializeMembers();

            this.pos = new PositionParam(pos);
            this.sr = new DoubleParam(sr);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.pos = new PositionParam("pos.eq");
            this.pos.Value = new Position(0, 0);
            this.pos.Unit = "deg";

            this.sr = new DoubleParam("instr.fov");
            this.sr.Value = 10;
            this.sr.Unit = "arcmin";
        }

        private void CopyMembers(ConeSearchParameters old)
        {
            base.CopyMembers(old);

            this.pos = old.pos == null ? null : new PositionParam(old.pos);
            this.sr = old.sr == null ? null : new DoubleParam(old.sr);
        }

        public ConeSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: ConeSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:43  dobos
        Initial checkin


*/
#endregion
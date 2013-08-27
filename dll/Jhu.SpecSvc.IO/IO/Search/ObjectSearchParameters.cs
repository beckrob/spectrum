#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ObjectSearchParameters.cs,v 1.1 2008/01/08 22:01:45 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:01:45 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.Schema;
using Jhu.SpecSvc.IO;

namespace Jhu.SpecSvc.IO
{
	public class ObjectSearchParameters : SearchParametersBase
    {

        #region Member variables

        private string[] ids;
        private string[] objects;
        private DoubleParam sr;		// arc sec

        #endregion
        #region Properties

        public string[] Ids
        {
            get { return this.ids; }
            set { this.ids = value; }
        }

        public string[] Objects
        {
            get { return this.objects; }
            set { this.objects = value; }
        }

        public DoubleParam Sr
        {
            get { return this.sr; }
            set { this.sr = value; }
        }

        public override SearchMethod Type
        {
            get { return SearchMethod.Object; }
        }

        #endregion

        #region Constructors

        public ObjectSearchParameters()
        {
        }

        public ObjectSearchParameters(bool initialize)
            :base(initialize)
        {
            if (initialize) InitializeMembers();
        }

        public ObjectSearchParameters(ObjectSearchParameters old)
        {
            CopyMembers(old);
        }

        #endregion
        #region Member functions

        private void InitializeMembers()
        {
            this.objects = null;

            this.sr = new DoubleParam("instr.fov");
            this.sr.Value = 0.5;
            this.sr.Unit = "arcmin";
        }

        private void CopyMembers(ObjectSearchParameters old)
        {
            base.CopyMembers(old);

            if (old.objects != null)
            {
                this.objects = new string[old.objects.Length];
                Array.Copy(old.objects, this.objects, old.objects.Length);
            }
            else
                this.objects = null;

            this.sr = old.sr == null ? null : new DoubleParam(old.sr);
        }

        public ObjectSearchParameters GetStandardUnits()
        {
            return this;
        }

        #endregion
    }
}
#region Revision History
/* Revision History

        $Log: ObjectSearchParameters.cs,v $
        Revision 1.1  2008/01/08 22:01:45  dobos
        Initial checkin


*/
#endregion
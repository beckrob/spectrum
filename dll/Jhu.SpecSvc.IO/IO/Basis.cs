#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: Basis.cs,v 1.1 2008/01/08 22:00:48 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:00:48 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    public class Basis
    {
        private string id;
        private Guid userGuid;
        private int @public;
        private string name;
        private string description;
        // ***private PreprocessParameters preprocessParameters;
        // ***private FitParameters fitParameters;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public int Public
        {
            get { return @public; }
            set { @public = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /*public PreprocessParameters PreprocessParameters
        {
            get { return preprocessParameters; }
            set { preprocessParameters = value; }
        }*/

        /*public FitParameters FitParameters
        {
            get { return fitParameters; }
            set { fitParameters = value; }
        }*/

        public Basis()
        {
            InitializeMembers();
        }

        public Basis(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.id = string.Empty;
            this.name = string.Empty;
            this.description = string.Empty;
            //this.preprocessParameters = null;
            //this.fitParameters = null;
        }
    }
}
#region Revision History
/* Revision History

        $Log: Basis.cs,v $
        Revision 1.1  2008/01/08 22:00:48  dobos
        Initial checkin


*/
#endregion
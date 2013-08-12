#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.IO classes are designed for persisting
 * astonomical spectra in different storage systems
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: TemplateSet.cs,v 1.1 2008/01/08 22:00:50 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:00:50 $
 */
#endregion
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace Jhu.SpecSvc.IO
{
	/// <summary>
	/// Summary description for Template.
	/// </summary>
	public class TemplateSet
	{
		private int id;
		private string name;

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public TemplateSet(bool initialize)
		{
            if (initialize) InitializeMembers();
		}

        private void InitializeMembers()
        {
            this.id = 0;
            this.name = "";
        }
	}
}
#region Revision History
/* Revision History

        $Log: TemplateSet.cs,v $
        Revision 1.1  2008/01/08 22:00:50  dobos
        Initial checkin


*/
#endregion
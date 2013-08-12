#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: CompositeParameters.cs,v 1.1 2008/01/08 21:36:54 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:36:54 $
 */
#endregion
using System;

namespace VoServices.Spectrum.Lib
{
	/// <summary>
	/// Summary description for CompositeParameters.
	/// </summary>
    [Serializable]
    public class CompositeParameters
    {
        private CompositeMethods method;

        public CompositeMethods Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public CompositeParameters(bool initialize)
        {
            if (initialize) InitializeMembers();
        }

        public CompositeParameters(CompositeParameters old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.method = CompositeMethods.Average;
        }

        private void CopyMembers(CompositeParameters old)
        {
            this.method = old.method;
        }
    }
}
#region Revision History
/* Revision History

        $Log: CompositeParameters.cs,v $
        Revision 1.1  2008/01/08 21:36:54  dobos
        Initial checkin


*/
#endregion
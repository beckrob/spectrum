#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Visualizer classes are for plotting spectra on
 * the webpage
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ColorDiagParameters.cs,v 1.1 2008/01/08 22:53:48 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:53:48 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Visualizer
{
    public class ColorPlotParameters
    {
        public int width = 640;
        public int height = 480;
        public float xMin = -1;
        public float xMax = -1;
        public float yMin = -1;
        public float yMax = -1;

        public float zMin = -1;
        public float zMax = -1;

        public bool titles = true;
        public bool autoLoad = true;
    }
}
#region Revision History
/* Revision History

        $Log: ColorDiagParameters.cs,v $
        Revision 1.1  2008/01/08 22:53:48  dobos
        Initial checkin


*/
#endregion
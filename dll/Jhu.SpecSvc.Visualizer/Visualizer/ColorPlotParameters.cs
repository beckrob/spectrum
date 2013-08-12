#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Visualizer classes are for plotting spectra on
 * the webpage
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: SpectrumGraphParameters.cs,v 1.1 2008/01/08 22:53:59 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 22:53:59 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SpecSvc.Visualizer
{
    [Serializable]
    public class ColorPlotParameters : PlotParameters
    {
        private int xFilter1;
        private int xFilter2;
        private int yFilter1;
        private int yFilter2;

        public int XFilter1
        {
            get { return xFilter1; }
            set { xFilter1 = value; }
        }

        public int XFilter2
        {
            get { return xFilter2; }
            set { xFilter2 = value; }
        }

        public int YFilter1
        {
            get { return yFilter1; }
            set { yFilter1 = value; }
        }

        public int YFilter2
        {
            get { return yFilter2; }
            set { yFilter2 = value; }
        }

        public ColorPlotParameters()
            :base()
        {
            InitializeMembers();
        }

        public ColorPlotParameters(ColorPlotParameters old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.xFilter1 = -1;
            this.xFilter2 = -1;
            this.yFilter1 = -1;
            this.yFilter2 = -1;
        }

        private void CopyMembers(ColorPlotParameters old)
        {
            this.xFilter1 = old.xFilter1;
            this.xFilter2 = old.xFilter2;
            this.yFilter1 = old.yFilter1;
            this.yFilter2 = old.yFilter2;
        }
    }
}
#region Revision History
/* Revision History

        $Log: SpectrumGraphParameters.cs,v $
        Revision 1.1  2008/01/08 22:53:59  dobos
        Initial checkin


*/
#endregion
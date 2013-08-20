using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Jhu.SpecSvc.SpectrumLib;

namespace Jhu.SpecSvc.IO
{
    public class MySpectrumTarget : OutputTarget
    {
        private string url;
        private Guid userGuid;
        private int userFolderId;
        private string namePrefix;
        private bool @public;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public int UserFolderId
        {
            get { return userFolderId; }
            set { userFolderId = value; }
        }

        public string NamePrefix
        {
            get { return namePrefix; }
            set { namePrefix = value; }
        }

        public bool Public
        {
            get { return @public; }
            set { @public = value; }
        }

        public MySpectrumTarget()
        {
            InitializeMembers();
        }

        public MySpectrumTarget(MySpectrumTarget old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.url = string.Empty;
            this.userGuid = Guid.Empty;
            this.userFolderId = 0;
            this.namePrefix = string.Empty;
            this.@public = false;
            this.log = null;
        }

        private void CopyMembers(MySpectrumTarget old)
        {
            this.url = old.url;
            this.userGuid = old.userGuid;
            this.userFolderId = old.userFolderId;
            this.namePrefix = old.namePrefix;
            this.@public = old.@public;
            this.log = old.log;
        }



        public void Execute(IEnumerable<Jhu.SpecSvc.SpectrumLib.Spectrum> spectra)
        {
            iteration = 0;
            WsConnector wsc = new WsConnector();
            wsc.AdminUrl = url;

            foreach (Spectrum spectrum in spectra)
            {
                iteration++;

                if (!string.IsNullOrEmpty(namePrefix))
                    spectrum.Target.Name.Value = namePrefix + " - " + spectrum.Target.Name.Value;

                spectrum.UserFolderId = userFolderId;
                spectrum.Id = 0;
                spectrum.Public = @public ? 1 : 0;

                long newid = wsc.SaveSpectrum(spectrum, userGuid);

                if (log != null)
                {
                    log.WriteLine("{0}\t{1}", spectrum.Target.Name, newid);
                }
            }
        }

    }
}

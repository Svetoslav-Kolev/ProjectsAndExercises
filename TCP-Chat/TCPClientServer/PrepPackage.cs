using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class PrepPackage : Package
    {
        public string targetUsername { get; set; }
        public bool isPersonal { get; set; }
        public long fileSizeInBytes { get; set; }
        public string sender { get; set; }
    }
}

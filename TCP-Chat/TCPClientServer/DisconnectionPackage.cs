using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class DisconnectionPackage
    {
        public string username { get; set; }
        public string reason { get; set; }
        public DisconnectionPackage(string username,string reason)
        {
            this.username = username;
            this.reason = reason;
        }
    }
}

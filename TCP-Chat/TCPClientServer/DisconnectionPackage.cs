    using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class DisconnectionPackage : Package
    {
        public string sender { get; set; }
        public string reason { get; set; }
        public DisconnectionPackage(string sender,string reason)
        {
            this.sender = sender;
            this.reason = reason;
        }
    }
}

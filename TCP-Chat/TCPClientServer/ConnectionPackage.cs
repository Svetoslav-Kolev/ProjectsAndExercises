using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class ConnectionPackage : Package
    {
        public string sender { get;  set; }
        public Guid userId { get; set; }

        public string message { get; set; }
        public ConnectionPackage(Guid id,string username)
        {
            this.userId = id;
            this.sender = username;
        }
        public ConnectionPackage(string username)
        {
            this.sender = username;
        }
    }
}

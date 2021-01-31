using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class ConnectionPackage
    {
        public string clientUsername { get;  set; }
        public Guid userId { get; set; }
        public ConnectionPackage(Guid id,string username)
        {
            this.userId = id;
            this.clientUsername = username;
        }
    }
}

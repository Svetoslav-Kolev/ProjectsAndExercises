using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
     
    [Serializable]
    class UsersPacket:Package
    {
       public List<string> Usernames;
        public string sender { get;  set ; }
        public UsersPacket(List<string> usernames)
        {
            this.Usernames = usernames;
        }

       
    }
}

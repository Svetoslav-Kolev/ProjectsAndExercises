using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
     
    [Serializable]
    class UsersPacket
    {
       public List<string> Usernames;
        public UsersPacket(List<string> usernames)
        {
            this.Usernames = usernames;
        }
    }
}

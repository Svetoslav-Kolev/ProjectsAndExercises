using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
   public class MessagePacket
    {
        public string targetUsername { get; set; }
        public string message { get; set; }
        public bool isPersonal { get; set; }
        public string sender { get; set; }
        public MessagePacket(string message , string targetName , bool personal)
        {
            this.isPersonal = personal;
            this.message = message;
            this.targetUsername = targetName;
        }
        public MessagePacket(string message)
        {
            this.message = message;
            this.isPersonal = false;
            this.targetUsername = "No target";
        }
    }
}

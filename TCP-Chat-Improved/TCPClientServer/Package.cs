using System;
using System.Collections.Generic;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public abstract class Package
    {
       public string sender { get; set; }
    }
}

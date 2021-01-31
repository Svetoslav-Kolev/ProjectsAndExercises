using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TCPClientServer
{
    [Serializable]
    public class ImagePacket
    {
        public string targetUsername { get; set; }
        public string sender { get; set; }
        public bool isPersonal { get; set; }
        public Bitmap Imagebmp { get; set; }
    }
}

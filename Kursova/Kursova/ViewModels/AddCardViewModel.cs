using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class AddCardViewModel
    {
        public string CardType { get; set; } //Visa ,MasterCard etc
        public string CardNumber { get; set; }
        public byte ExpiryMonth { get; set; }
        public ushort ExpiryYear { get; set; }
    }
}

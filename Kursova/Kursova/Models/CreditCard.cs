using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models
{
    public class CreditCard
    {
        [Key]
        public int CreditCardID { get; set; }
        public string CardType { get; set; } //Visa ,MasterCard etc
        public string CardNumber { get; set; }
        public byte ExpiryMonth { get; set; }
        public ushort ExpiryYear { get; set; }

        public List<Transaction> transactions { get; set; }
        public ICollection<User> Users { get; set; }

    }
}

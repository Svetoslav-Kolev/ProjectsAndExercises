using Kursova.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public string TransactionType { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int? CreditCardID { get; set; }
        public virtual CreditCard CreditCard { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models.Context
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public string DeliveryAddress { get; set; }
        public Decimal TotalPrice { get; set; }

        public List<OrderDetails> Details { get; set; }
    }
}

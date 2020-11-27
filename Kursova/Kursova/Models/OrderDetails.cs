using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models.Context
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsID { get; set; }
        public int Quantity { get; set; }
        public int ClothingID { get; set; }
        public Clothing Clothing { get; set; }
        public Decimal Price { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}

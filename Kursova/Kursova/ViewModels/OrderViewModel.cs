using Kursova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class OrderViewModel
    {
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public string DeliveryAddress { get; set; }
        public int CreditCardId { get; set; }
        public int ClothingID { get; set; }
        public List<CreditCard> yourCards { get; set; }
        public OrderViewModel()
        {
            yourCards = new List<CreditCard>();
        }
    
    }
}

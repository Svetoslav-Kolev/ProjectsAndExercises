using Kursova.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class OrderHistoryViewModel
    {
        public ICollection<Order> myOrders { get; set; }
        public OrderHistoryViewModel()
        {
            myOrders = new List<Order>();
        }
    }
}

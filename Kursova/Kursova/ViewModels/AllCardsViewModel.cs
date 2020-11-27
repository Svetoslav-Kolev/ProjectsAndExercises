using Kursova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class AllCardsViewModel
    {
        public List<CreditCard> userCards { get; set; }
        public AllCardsViewModel()
        {
            userCards = new List<CreditCard>();
        }
    }
}

using Kursova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class ShopViewModel
    {
       public List<Clothing> AllClothing { get; set; }
        public int selectedClothingID { get; set; }
        public ShopViewModel()
        {
            AllClothing = new List<Clothing>();
        }
    }
}

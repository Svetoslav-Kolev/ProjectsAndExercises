using Kursova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class OutfitsViewModel
    {
        public List<Outfits> AllOutfits { get; set; }
        public OutfitsViewModel()
        {
            AllOutfits = new List<Outfits>();
        }
        
    }
}

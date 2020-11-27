using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models
{
    public class Outfits
    {
        [Key]
        public int OutfitID { get; set; }
        public string OutfitName { get; set; }
        public ICollection<Clothing> Clothes { get; set; }
        public Outfits()
        {
            Clothes = new List<Clothing>();
        }
    }
}

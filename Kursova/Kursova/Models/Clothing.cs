using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models
{
    public class Clothing
    {
        [Key]
        public int ClothingID { get; set; }
        public string ClothingName { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public Decimal Price { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<Outfits> Outfits { get; set; }
        public Clothing()
        {
            Outfits = new List<Outfits>();
        }
    }
}

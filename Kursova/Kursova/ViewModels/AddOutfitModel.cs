using Kursova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.ViewModels
{
    public class AddOutfitModel
    {
        public string OutfitName { get; set; }
        public int pieceOneId { get; set; }
        public int  pieceTwoId { get; set; }
        public int pieceThreeId { get; set; }
        public ICollection<Clothing> AllClothes { get; set; }
        public AddOutfitModel()
        {
            AllClothes = new List<Clothing>();
        }
    }
}

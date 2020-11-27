using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

    }
}

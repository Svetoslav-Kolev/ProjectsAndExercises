using Kursova.Models;
using Kursova.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Kursova
{
    public class OutfitsContext:DbContext
    {
        public OutfitsContext(DbContextOptions<OutfitsContext> options) 
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Clothing> Clothing { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Outfits> Outfits { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}

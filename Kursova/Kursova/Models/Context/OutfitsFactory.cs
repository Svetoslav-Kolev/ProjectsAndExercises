using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Models.Context
{
    public class OutfitsFactory : IDesignTimeDbContextFactory<OutfitsContext>
    {
        public OutfitsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OutfitsContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-1ADSAV1\\SQLEXPRESS01;Database=OutfitsDB;Trusted_Connection=True;");

            return new OutfitsContext(optionsBuilder.Options);
        }
    }
}

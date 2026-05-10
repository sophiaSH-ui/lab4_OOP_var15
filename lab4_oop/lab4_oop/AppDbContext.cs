using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace lab4_oop
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RentalCompany> RentalCompanies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoDB.mdf");
            optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;AttachDbFilename={dbPath};Database=AutoDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
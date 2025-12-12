using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }  


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        modelBuilder.Entity<Car>()
            .Property(c => c.Price)
            .HasPrecision(18,2); // 18 digitos en total, 2 decimales
            base.OnModelCreating(modelBuilder);
        }      

    }
}

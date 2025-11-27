using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class CarNestDBContext : IdentityDbContext
    {
        public CarNestDBContext(DbContextOptions<CarNestDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Make> Makes { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<BodyType> BodyTypes { get; set; }
        public virtual DbSet<FuelType> FuelTypes { get; set; }
        public virtual DbSet<LocationCity> LocationCities { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Car> Cars { get; set; }

        public virtual DbSet<Vendor> Buyers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Car>()
        .HasOne(c => c.Vendor)
        .WithMany(b => b.Cars)
        .HasForeignKey(c => c.BuyerId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>()
                .HasOne(c => c.Admin)
                .WithMany(a => a.Cars)
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}

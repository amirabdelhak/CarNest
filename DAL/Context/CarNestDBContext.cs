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

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Model belongs to Make
            builder.Entity<Model>()
                .HasOne(m => m.Make)
                .WithMany(mk => mk.Models)
                .HasForeignKey(m => m.MakeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint: (MakeId, ModelName) to prevent duplicate model names per make
            builder.Entity<Model>()
                .HasIndex(m => new { m.MakeId, m.ModelName })
                .IsUnique();

            // Car belongs to Model (Make is derived via Model)
            builder.Entity<Car>()
                .HasOne(c => c.Model)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>()
                .HasOne(c => c.Vendor)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>()
                .HasOne(c => c.Admin)
                .WithMany(a => a.Cars)
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

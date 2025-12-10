using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider, IConfiguration config)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var roleName = "Admin";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Get admin configuration from appsettings.json
            var email = config["Admin:Email"];
            var password = config["Admin:Password"];
            var username = config["Admin:Username"];
            var firstName = config["Admin:FirstName"];
            var lastName = config["Admin:LastName"];
            var address = config["Admin:Address"];
            var nationalId = config["Admin:NationalId"];

            // Check if admin user already exists
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                // Create new admin user
                user = new Admin
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    NationalId = nationalId,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(" | ", result.Errors.Select(e => e.Description))
                    );
                }
            }

            // Assign Admin role to the user if not already assigned
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }

        public static async Task SeedTestDataAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<CarNestDBContext>();

            // Ensure database is created / migrated
            await context.Database.MigrateAsync();

            // -------- Lookup tables --------

            if (!await context.BodyTypes.AnyAsync())
            {
                context.BodyTypes.AddRange(
                    new BodyType { BodyId = 1, Name = "Sedan" },
                    new BodyType { BodyId = 2, Name = "SUV" },
                    new BodyType { BodyId = 3, Name = "Hatchback" }
                );
            }

            if (!await context.FuelTypes.AnyAsync())
            {
                context.FuelTypes.AddRange(
                    new FuelType { FuelId = 1, Name = "Gasoline" },
                    new FuelType { FuelId = 2, Name = "Diesel" },
                    new FuelType { FuelId = 3, Name = "Electric" }
                );
            }

            if (!await context.LocationCities.AnyAsync())
            {
                context.LocationCities.AddRange(
                    new LocationCity { LocId = 1, Name = "Cairo" },
                    new LocationCity { LocId = 2, Name = "Alexandria" },
                    new LocationCity { LocId = 3, Name = "Giza" }
                );
            }

            if (!await context.Makes.AnyAsync())
            {
                context.Makes.AddRange(
                    new Make { MakeId = 1, MakeName = "Toyota" },
                    new Make { MakeId = 2, MakeName = "Honda" }
                );
            }

            if (!await context.Models.AnyAsync())
            {
                context.Models.AddRange(
                    new Model { ModelId = 1, ModelName = "Corolla", MakeId = 1 },
                    new Model { ModelId = 2, ModelName = "Camry", MakeId = 1 },
                    new Model { ModelId = 3, ModelName = "Civic", MakeId = 2 }
                );
            }

            await context.SaveChangesAsync();

            // -------- Test users (Vendor) --------

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            const string vendorRole = "Vendor";

            if (!await roleManager.RoleExistsAsync(vendorRole))
            {
                await roleManager.CreateAsync(new IdentityRole(vendorRole));
            }

            const string vendorUserName = "vendor@test.com";
            var vendorUser = await userManager.FindByNameAsync(vendorUserName);

            if (vendorUser == null)
            {
                vendorUser = new Vendor
                {
                    UserName = vendorUserName,
                    Email = vendorUserName,
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = "Vendor",
                    Address = "Cairo",
                    NationalId = "11111111111111",
                    CreatedDate = DateTime.UtcNow
                };

                var vendorCreateResult = await userManager.CreateAsync(vendorUser, "Vendor@123");
                if (!vendorCreateResult.Succeeded)
                {
                    throw new Exception(string.Join(" | ", vendorCreateResult.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(vendorUser, vendorRole))
            {
                await userManager.AddToRoleAsync(vendorUser, vendorRole);
            }

            // -------- Seed cars for testing Update --------

            if (!await context.Cars.AnyAsync())
            {
                // Car owned by Admin
                var admin = await userManager.FindByNameAsync("admin");
                var adminId = admin?.Id;

                context.Cars.Add(new Car
                {
                    CarId = Guid.NewGuid().ToString(),
                    Year = 2020,
                    Price = 250000,
                    Description = "Admin Corolla Sedan Cairo",
                    ModelId = 1,        // Corolla
                    BodyTypeId = 1,     // Sedan
                    FuelId = 1,         // Gasoline
                    LocId = 1,          // Cairo
                    AdminId = adminId,
                    VendorId = null,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrls = null
                });

                // Car owned by Vendor
                context.Cars.Add(new Car
                {
                    CarId = Guid.NewGuid().ToString(),
                    Year = 2022,
                    Price = 350000,
                    Description = "Vendor Civic Hatchback Alexandria",
                    ModelId = 3,        // Civic
                    BodyTypeId = 3,     // Hatchback
                    FuelId = 2,         // Diesel
                    LocId = 2,          // Alexandria
                    AdminId = null,
                    VendorId = vendorUser.Id,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrls = null
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
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
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created / migrated
            await context.Database.MigrateAsync();

            // -------- Lookup tables --------

            if (!await context.BodyTypes.AnyAsync())
            {
                context.BodyTypes.AddRange(
                    new BodyType { Name = "Sedan" },
                    new BodyType { Name = "SUV" },
                    new BodyType { Name = "Hatchback" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.FuelTypes.AnyAsync())
            {
                context.FuelTypes.AddRange(
                    new FuelType { Name = "Gasoline" },
                    new FuelType { Name = "Diesel" },
                    new FuelType { Name = "Electric" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.LocationCities.AnyAsync())
            {
                context.LocationCities.AddRange(
                    new LocationCity { Name = "Cairo" },
                    new LocationCity { Name = "Alexandria" },
                    new LocationCity { Name = "Giza" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Makes.AnyAsync())
            {
                context.Makes.AddRange(
                    new Make { MakeName = "Toyota" },
                    new Make { MakeName = "Honda" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Models.AnyAsync())
            {
                // Get the saved Makes to reference their IDs
                var toyota = await context.Makes.FirstOrDefaultAsync(m => m.MakeName == "Toyota");
                var honda = await context.Makes.FirstOrDefaultAsync(m => m.MakeName == "Honda");

                if (toyota != null && honda != null)
                {
                    context.Models.AddRange(
                        new Model { ModelName = "Corolla", MakeId = toyota.MakeId },
                        new Model { ModelName = "Camry", MakeId = toyota.MakeId },
                        new Model { ModelName = "Civic", MakeId = honda.MakeId }
                    );
                    await context.SaveChangesAsync();
                }
            }

            // -------- Test users (Vendor) --------

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
                // Get the saved entities to reference their IDs
                var corolla = await context.Models.FirstOrDefaultAsync(m => m.ModelName == "Corolla");
                var camry = await context.Models.FirstOrDefaultAsync(m => m.ModelName == "Camry");
                var civic = await context.Models.FirstOrDefaultAsync(m => m.ModelName == "Civic");
                var sedan = await context.BodyTypes.FirstOrDefaultAsync(b => b.Name == "Sedan");
                var hatchback = await context.BodyTypes.FirstOrDefaultAsync(b => b.Name == "Hatchback");
                var gasoline = await context.FuelTypes.FirstOrDefaultAsync(f => f.Name == "Gasoline");
                var diesel = await context.FuelTypes.FirstOrDefaultAsync(f => f.Name == "Diesel");
                var cairo = await context.LocationCities.FirstOrDefaultAsync(l => l.Name == "Cairo");
                var alexandria = await context.LocationCities.FirstOrDefaultAsync(l => l.Name == "Alexandria");

                // Car owned by Admin
                var admin = await userManager.FindByNameAsync("admin");
                var adminId = admin?.Id;

                // New car (Admin)
                if (corolla != null && sedan != null && gasoline != null && cairo != null)
                {
                    context.Cars.Add(new Car
                    {
                        CarId = Guid.NewGuid().ToString(),
                        Year = 2024,
                        Price = 450000,
                        Description = "Brand new Toyota Corolla 2024",
                        ModelId = corolla.ModelId,
                        BodyTypeId = sedan.BodyId,
                        FuelId = gasoline.FuelId,
                        LocId = cairo.LocId,
                        AdminId = adminId,
                        VendorId = null,
                        CreatedDate = DateTime.UtcNow,
                        ImageUrls = null,
                        Condition = CarCondition.New,
                        Mileage = null,
                        LastInspectionDate = null
                    });
                }

                // Used car (Admin)
                if (camry != null && sedan != null && gasoline != null && cairo != null)
                {
                    context.Cars.Add(new Car
                    {
                        CarId = Guid.NewGuid().ToString(),
                        Year = 2020,
                        Price = 320000,
                        Description = "Well-maintained Toyota Camry, single owner",
                        ModelId = camry.ModelId,
                        BodyTypeId = sedan.BodyId,
                        FuelId = gasoline.FuelId,
                        LocId = cairo.LocId,
                        AdminId = adminId,
                        VendorId = null,
                        CreatedDate = DateTime.UtcNow,
                        ImageUrls = null,
                        Condition = CarCondition.Used,
                        Mileage = 45000,
                        LastInspectionDate = null
                    });
                }

                // Certified Pre-Owned car (Vendor)
                if (civic != null && hatchback != null && diesel != null && alexandria != null)
                {
                    context.Cars.Add(new Car
                    {
                        CarId = Guid.NewGuid().ToString(),
                        Year = 2022,
                        Price = 380000,
                        Description = "Honda Civic with full inspection report",
                        ModelId = civic.ModelId,
                        BodyTypeId = hatchback.BodyId,
                        FuelId = diesel.FuelId,
                        LocId = alexandria.LocId,
                        AdminId = null,
                        VendorId = vendorUser.Id,
                        CreatedDate = DateTime.UtcNow,
                        ImageUrls = null,
                        Condition = CarCondition.Used,  // Changed from CertifiedPreOwned
                        Mileage = 25000,
                        LastInspectionDate = DateTime.UtcNow.AddDays(-30)
                    });
                }

                // Used car (Vendor)
                if (corolla != null && sedan != null && gasoline != null && alexandria != null)
                {
                    context.Cars.Add(new Car
                    {
                        CarId = Guid.NewGuid().ToString(),
                        Year = 2019,
                        Price = 220000,
                        Description = "Used Toyota Corolla, good condition",
                        ModelId = corolla.ModelId,
                        BodyTypeId = sedan.BodyId,
                        FuelId = gasoline.FuelId,
                        LocId = alexandria.LocId,
                        AdminId = null,
                        VendorId = vendorUser.Id,
                        CreatedDate = DateTime.UtcNow,
                        ImageUrls = null,
                        Condition = CarCondition.Used,
                        Mileage = 78000,
                        LastInspectionDate = null
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
using DAL.Entity;
using Microsoft.AspNetCore.Identity;

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
    }
}

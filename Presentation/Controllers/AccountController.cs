using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminRequest request)
        {
            var admin = request.ToEntity();

            var result = await userManager.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await userManager.AddToRoleAsync(admin, "Admin");

            return Ok(new { Message = "Admin registered successfully", UserId = admin.Id });
        }

        [HttpPost("register/vendor")]
        public async Task<IActionResult> RegisterVendor(RegisterVendorRequest request)
        {
            var vendor = request.ToEntity();

            var result = await userManager.CreateAsync(vendor, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Ensure Vendor role exists
            if (!await roleManager.RoleExistsAsync("Vendor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Vendor"));
            }

            await userManager.AddToRoleAsync(vendor, "Vendor");

            return Ok(new { Message = "Vendor registered successfully", UserId = vendor.Id });
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequest request)
        {
            var customer = request.ToEntity();

            var result = await userManager.CreateAsync(customer, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Ensure Customer role exists
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            await userManager.AddToRoleAsync(customer, "Customer");

            return Ok(new { Message = "Customer registered successfully", UserId = customer.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.Count > 0 ? roles[0] : "User";

            var token = GenerateJwtToken(user, role);
            var expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
        }

        private string GenerateJwtToken(IdentityUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

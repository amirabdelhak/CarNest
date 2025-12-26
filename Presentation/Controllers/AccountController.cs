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
using BLL.Manager.AccountManager;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IAccountManager accountManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IAccountManager accountManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.accountManager = accountManager;
        }

        // ... Register methods unchanged ...
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

            // Auto-login response
            var token = GenerateJwtToken(admin, "Admin");
            var expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            // Since we just created it, we can return the response directly or fetch it to be safe.
            // Fetching ensures we get the exact same structure as Login.
            var response = await accountManager.GetFullUserResponseAsync(admin.Id, "Admin", token, expiryMinutes);
            
            if (response != null) return Ok(response);

            // Fallback (should not happen for newly created user)
            return Ok(new AuthResponse
            {
                Token = token,
                UserId = admin.Id,
                Email = admin.Email,
                Role = "Admin",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
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

            // Auto-login response
            var token = GenerateJwtToken(vendor, "Vendor");
            var expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            var response = await accountManager.GetFullUserResponseAsync(vendor.Id, "Vendor", token, expiryMinutes);
            
            if (response != null) return Ok(response);

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = vendor.Id,
                Email = vendor.Email,
                Role = "Vendor",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
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

            // Auto-login response
            var token = GenerateJwtToken(customer, "Customer");
            var expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            var response = await accountManager.GetFullUserResponseAsync(customer.Id, "Customer", token, expiryMinutes);
            
            if (response != null) return Ok(response);

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = customer.Id,
                Email = customer.Email,
                Role = "Customer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
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

            var response = await accountManager.GetFullUserResponseAsync(user.Id, role, token, expiryMinutes);
            if (response != null)
            {
                return Ok(response);
            }

            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });

        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null || role == null)
            {
                return Unauthorized();
            }

            var error = await accountManager.UpdateProfileAsync(userId, role, request);
            if (error != null)
            {
                return BadRequest(new { Message = error });
            }

            return Ok(new { Message = "Profile updated successfully" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var error = await accountManager.ChangePasswordAsync(userId, request);
            if (error != null)
            {
                return BadRequest(new { Message = error });
            }

            return Ok(new { Message = "Password changed successfully" });
        }

        private string GenerateJwtToken(IdentityUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
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

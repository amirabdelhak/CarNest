using DAL.Entity;
using DAL.UnitOfWork;
using Presentation.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace BLL.Manager.AccountManager
{
    public class AccountManager : IAccountManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> userManager;

        public AccountManager(IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<AuthResponse?> GetFullUserResponseAsync(string userId, string role, string token, int expiryMinutes)
        {
            if (role == "Admin")
            {
                var admin = await unitOfWork.AdminRepo.GetByIdAsync(userId);
                return admin != null ? ToResponse(admin, token, role, expiryMinutes) : null;
            }
            else if (role == "Vendor")
            {
                var vendor = await unitOfWork.VendorRepo.GetByIdAsync(userId);
                return vendor != null ? ToResponse(vendor, token, role, expiryMinutes) : null;
            }
            else if (role == "Customer")
            {
                var customer = await unitOfWork.CustomerRepo.GetByIdAsync(userId);
                return customer != null ? ToResponse(customer, token, role, expiryMinutes) : null;
            }

            return null;
        }

        public async Task<string?> UpdateProfileAsync(string userId, string role, Presentation.DTOs.Requests.UpdateProfileRequest request)
        {
            if (role == "Admin")
            {
                var admin = await unitOfWork.AdminRepo.GetByIdAsync(userId);
                if (admin == null) return "User not found";

                admin.FirstName = request.FirstName;
                admin.LastName = request.LastName;
                admin.Address = request.Address;
                admin.NationalId = request.NationalId;
                admin.PhoneNumber = request.PhoneNumber;
                
                if (admin.Email != request.Email)
                {
                    // Check if email is already taken by another user
                    var existingUser = await userManager.FindByEmailAsync(request.Email);
                    if (existingUser != null)
                    {
                        return "Email is already in use by another user";
                    }

                    var token = await userManager.GenerateChangeEmailTokenAsync(admin, request.Email);
                    var result = await userManager.ChangeEmailAsync(admin, request.Email, token);
                    
                    if (!result.Succeeded)
                    {
                        return string.Join(", ", result.Errors.Select(e => e.Description));
                    }
                    
                    admin.UserName = request.Email; 
                    await userManager.UpdateNormalizedUserNameAsync(admin);
                }
            }
            else if (role == "Vendor")
            {
                var vendor = await unitOfWork.VendorRepo.GetByIdAsync(userId);
                if (vendor == null) return "User not found";

                vendor.FirstName = request.FirstName;
                vendor.LastName = request.LastName;
                vendor.Address = request.Address;
                vendor.NationalId = request.NationalId;
                vendor.PhoneNumber = request.PhoneNumber;

                if (vendor.Email != request.Email)
                {
                    // Check if email is already taken by another user
                    var existingUser = await userManager.FindByEmailAsync(request.Email);
                    if (existingUser != null)
                    {
                        return "Email is already in use by another user";
                    }

                    var token = await userManager.GenerateChangeEmailTokenAsync(vendor, request.Email);
                    var result = await userManager.ChangeEmailAsync(vendor, request.Email, token);
                    
                    if (!result.Succeeded)
                    {
                        return string.Join(", ", result.Errors.Select(e => e.Description));
                    }
                    
                    vendor.UserName = request.Email;
                    await userManager.UpdateNormalizedUserNameAsync(vendor);
                }
            }
            else if (role == "Customer")
            {
                var customer = await unitOfWork.CustomerRepo.GetByIdAsync(userId);
                if (customer == null) return "User not found";

                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.Address = request.Address;
                customer.NationalId = request.NationalId;
                customer.PhoneNumber = request.PhoneNumber;

                if (customer.Email != request.Email)
                {
                    // Check if email is already taken by another user
                    var existingUser = await userManager.FindByEmailAsync(request.Email);
                    if (existingUser != null)
                    {
                        return "Email is already in use by another user";
                    }

                    var token = await userManager.GenerateChangeEmailTokenAsync(customer, request.Email);
                    var result = await userManager.ChangeEmailAsync(customer, request.Email, token);
                    
                    if (!result.Succeeded)
                    {
                        return string.Join(", ", result.Errors.Select(e => e.Description));
                    }
                    
                    customer.UserName = request.Email;
                    await userManager.UpdateNormalizedUserNameAsync(customer);
                }
            }
            else
            {
                return "Invalid role";
            }

            await unitOfWork.SaveAsync();
            return null; // Success
        }

        public async Task<string?> ChangePasswordAsync(string userId, Presentation.DTOs.Requests.ChangePasswordRequest request)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return null; // Success
        }

        private AuthResponse ToResponse(Admin a, string token, string role, int expiryMinutes) =>
            new AuthResponse
            {
                Token = token,
                UserId = a.Id,
                Email = a.Email,
                Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                FirstName = a.FirstName,
                LastName = a.LastName,
                Address = a.Address,
                NationalId = a.NationalId,
                PhoneNumber = a.PhoneNumber
            };

        private AuthResponse ToResponse(Vendor v, string token, string role, int expiryMinutes) =>
            new AuthResponse
            {
                Token = token,
                UserId = v.Id,
                Email = v.Email,
                Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                FirstName = v.FirstName,
                LastName = v.LastName,
                Address = v.Address,
                NationalId = v.NationalId,
                PhoneNumber = v.PhoneNumber
            };

        private AuthResponse ToResponse(Customer c, string token, string role, int expiryMinutes) =>
            new AuthResponse
            {
                Token = token,
                UserId = c.Id,
                Email = c.Email,
                Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Address = c.Address,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber
            };
    }
}

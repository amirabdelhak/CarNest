using DAL.Entity;
using Presentation.DTOs.Requests;
using System;

namespace Presentation.Mappings
{
    public static class UserMapping
    {
        public static Admin ToEntity(this RegisterAdminRequest r) =>
            new Admin
            {
                UserName = r.Email,
                Email = r.Email,
                FirstName = r.FirstName,
                LastName = r.LastName,
                NationalId = r.NationalId,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

        public static Vendor ToEntity(this RegisterVendorRequest r) =>
            new Vendor
            {
                UserName = r.Email,
                Email = r.Email,
                FirstName = r.FirstName,
                LastName = r.LastName,
                NationalId = r.NationalId,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

        public static Customer ToEntity(this RegisterCustomerRequest r) =>
            new Customer
            {
                UserName = r.Email,
                Email = r.Email,
                FirstName = r.FirstName,
                LastName = r.LastName,
                NationalId = r.NationalId,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };
    }
}

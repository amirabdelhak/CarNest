using System;
using System.Linq;
using System.Text.Json;
using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class CarMapping
    {
        public static CarResponse ToResponse(this Car e) =>
            new CarResponse
            {
                CarId = e.CarId,
                Year = e.Year,
                Price = e.Price,
                Description = e.Description,
                MakeName = e.Model?.Make?.MakeName,
                ModelName = e.Model?.ModelName,
                BodyTypeName = e.BodyType?.Name,
                FuelName = e.FuelType?.Name,
                LocationName = e.LocationCity?.Name,
                CreatedDate = e.CreatedDate,
                ImageUrls = string.IsNullOrEmpty(e.ImageUrls)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(e.ImageUrls) ?? new List<string>(),
                Condition = e.Condition,
                Mileage = e.Mileage,
                LastInspectionDate = e.LastInspectionDate,

                GearType = e.GearType,
                ExteriorColor = e.ExteriorColor,
                InteriorColor = e.InteriorColor,
                EngineCapacity = e.EngineCapacity,
                Horsepower = e.Horsepower,
                DrivetrainType = e.DrivetrainType,
                Status = e.Status,
                CarLicenseUrl = e.CarLicenseUrl
            };

        public static CarDetailResponse ToDetailResponse(this Car e) =>
            new CarDetailResponse
            {
                CarId = e.CarId,
                Year = e.Year,
                Price = e.Price,
                Description = e.Description,
                MakeId = e.Model?.MakeId ?? 0,
                MakeName = e.Model?.Make?.MakeName,
                ModelId = e.ModelId,
                ModelName = e.Model?.ModelName,
                BodyTypeId = e.BodyTypeId,
                BodyTypeName = e.BodyType?.Name,
                FuelId = e.FuelId,
                FuelName = e.FuelType?.Name,
                LocId = e.LocId,
                LocationName = e.LocationCity?.Name,
                CreatedDate = e.CreatedDate,
                ImageUrls = string.IsNullOrEmpty(e.ImageUrls)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(e.ImageUrls) ?? new List<string>(),
                PublisherName = e.Admin != null ? $"{e.Admin.FirstName} {e.Admin.LastName}" : (e.Vendor != null ? $"{e.Vendor.FirstName} {e.Vendor.LastName}" : null),
                PublisherPhone = e.Admin != null ? e.Admin.PhoneNumber : (e.Vendor != null ? e.Vendor.PhoneNumber : null),
                PublisherEmail = e.Admin != null ? e.Admin.Email : (e.Vendor != null ? e.Vendor.Email : null),
                OwnerId = e.VendorId ?? e.AdminId,
                Condition = e.Condition,
                Mileage = e.Mileage,
                LastInspectionDate = e.LastInspectionDate,

                GearType = e.GearType,
                ExteriorColor = e.ExteriorColor,
                InteriorColor = e.InteriorColor,
                EngineCapacity = e.EngineCapacity,
                Horsepower = e.Horsepower,
                DrivetrainType = e.DrivetrainType,
                Status = e.Status,
                CarLicenseUrl = e.CarLicenseUrl
            };

        public static Car ToEntity(this CarRequest r, string? adminId, string? vendorId) =>
            new Car
            {
                CarId = Guid.NewGuid().ToString(),
                Year = r.Year,
                Price = r.Price,
                Description = r.Description,
                ModelId = r.ModelId,
                BodyTypeId = r.BodyTypeId,
                FuelId = r.FuelId,
                LocId = r.LocId,
                AdminId = adminId,
                VendorId = vendorId,
                CreatedDate = DateTime.UtcNow,
                ImageUrls = null,
                Condition = r.Condition,
                Mileage = r.Mileage,
                LastInspectionDate = r.LastInspectionDate,

                GearType = r.GearType,
                ExteriorColor = r.ExteriorColor,
                InteriorColor = r.InteriorColor,
                EngineCapacity = r.EngineCapacity,
                Horsepower = r.Horsepower,
                DrivetrainType = r.DrivetrainType
            };
    }
}

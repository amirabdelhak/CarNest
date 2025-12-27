using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;
using Presentation.Mappings;

namespace BLL.Manager.CarManager
{
    public class CarManager : ICarManager
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly string webRootPath;

        public CarManager(IUnitOfWork UnitOfWork, string webRootPath)
        {
            this.UnitOfWork = UnitOfWork;
            this.webRootPath = webRootPath;
        }

        public async Task<PagedResponse<CarResponse>> GetAllAsync(PaginationRequest request)
        {
            // Start with base query including all necessary relationships
            var dataList = await UnitOfWork.CarRepo.GetAllAsync(q =>
                q.Include(c => c.Model).ThenInclude(m => m.Make)
                 .Include(c => c.BodyType)
                 .Include(c => c.FuelType)
                 .Include(c => c.LocationCity)
            );

            var query = dataList.AsQueryable();

            // Apply filters
            query = ApplyFilters(query, request);

            // Get total count AFTER filtering
            var count = query.Count();

            // Apply pagination
            var data = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<CarResponse>(
                data.Select(c => c.ToResponse()),
                request.PageNumber,
                request.PageSize,
                count
            );
        }

        public async Task<PagedResponse<CarResponse>> GetCarsByVendorAsync(PaginationRequest request, string vendorId)
        {
            // Start with vendor-specific query
            var dataList = await UnitOfWork.CarRepo.GetAllAsync(q =>
                q.Where(c => c.VendorId == vendorId)
                 .Include(c => c.Model).ThenInclude(m => m.Make)
                 .Include(c => c.BodyType)
                 .Include(c => c.FuelType)
                 .Include(c => c.LocationCity)
            );

            var query = dataList.AsQueryable();

            // Apply filters
            query = ApplyFilters(query, request);

            // Get total count AFTER filtering
            var count = query.Count();

            // Apply pagination
            var data = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<CarResponse>(
                data.Select(c => c.ToResponse()),
                request.PageNumber,
                request.PageSize,
                count
            );
        }

        public async Task<CarDetailResponse?> GetByIdAsync(string id)
        {
            var dataList = await UnitOfWork.CarRepo.GetAllAsync(query =>
                query.Where(c => c.CarId == id)
                     .Include(c => c.Model).ThenInclude(m => m.Make)
                     .Include(c => c.BodyType)
                     .Include(c => c.FuelType)
                     .Include(c => c.LocationCity)
                     .Include(c => c.Admin)
                     .Include(c => c.Vendor)
            );

            var data = dataList.FirstOrDefault();

            return data?.ToDetailResponse();
        }

        public async Task<CarResponse> AddAsync(CarRequest request, string? adminId, string? vendorId, IFormFileCollection? images)
        {
            // Validate condition-specific rules
            ValidateConditionRules(request);

            // Validate ModelId exists
            var model = await UnitOfWork.ModelRepo.GetByIdAsync(request.ModelId);
            if (model == null)
            {
                throw new ArgumentException($"Model with ID {request.ModelId} not found");
            }

            // Validate BodyTypeId exists
            if (!await UnitOfWork.BodyTypeRepo.AnyAsync(b => b.BodyId == request.BodyTypeId))
            {
                throw new ArgumentException($"Body Type with ID {request.BodyTypeId} not found");
            }

            // Validate FuelId exists
            if (!await UnitOfWork.FuelTypeRepo.AnyAsync(f => f.FuelId == request.FuelId))
            {
                throw new ArgumentException($"Fuel Type with ID {request.FuelId} not found");
            }

            // Validate LocId exists
            if (!await UnitOfWork.LocationCityRepo.AnyAsync(l => l.LocId == request.LocId))
            {
                throw new ArgumentException($"Location with ID {request.LocId} not found");
            }

            var entity = request.ToEntity(adminId, vendorId);

            // Save images if provided
            if (images != null && images.Count > 0)
            {
                var imageUrls = SaveImages(images);
                entity.ImageUrls = JsonSerializer.Serialize(imageUrls);
            }

            UnitOfWork.CarRepo.Add(entity);
            await UnitOfWork.SaveAsync();

            // Reload entity with includes to get related names
            var reloadedList = await UnitOfWork.CarRepo.GetAllAsync(query =>
                query.Where(c => c.CarId == entity.CarId)
                     .Include(c => c.Model).ThenInclude(m => m.Make)
                     .Include(c => c.BodyType)
                     .Include(c => c.FuelType)
                     .Include(c => c.LocationCity)
            );

            var reloadedEntity = reloadedList.FirstOrDefault();

            return reloadedEntity?.ToResponse() ?? entity.ToResponse();
        }

        public async Task<CarResponse> UpdateAsync(string id, CarRequest request, string userId, string userRole, IFormFileCollection? newImages, List<string>? imagesToDelete)
        {
            var existingCar = await UnitOfWork.CarRepo.GetByIdAsync(id);
            if (existingCar == null)
            {
                throw new Exception($"Car with ID {id} not found");
            }

            // Authorization check: Vendor can only update their own cars
            if (userRole == "Vendor" && existingCar.VendorId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own cars");
            }

            // Validate condition-specific rules
            ValidateConditionRules(request);

            // Validate ModelId exists
            var model = await UnitOfWork.ModelRepo.GetByIdAsync(request.ModelId);
            if (model == null)
            {
                throw new ArgumentException($"Model with ID {request.ModelId} not found");
            }

            // Validate BodyTypeId exists
            if (!await UnitOfWork.BodyTypeRepo.AnyAsync(b => b.BodyId == request.BodyTypeId))
            {
                throw new ArgumentException($"Body Type with ID {request.BodyTypeId} not found");
            }

            // Validate FuelId exists
            if (!await UnitOfWork.FuelTypeRepo.AnyAsync(f => f.FuelId == request.FuelId))
            {
                throw new ArgumentException($"Fuel Type with ID {request.FuelId} not found");
            }

            // Validate LocId exists
            if (!await UnitOfWork.LocationCityRepo.AnyAsync(l => l.LocId == request.LocId))
            {
                throw new ArgumentException($"Location with ID {request.LocId} not found");
            }

            // Manual mapping update logic
            existingCar.Year = request.Year;
            existingCar.Price = request.Price;
            existingCar.Description = request.Description;
            existingCar.ModelId = request.ModelId;
            existingCar.BodyTypeId = request.BodyTypeId;
            existingCar.FuelId = request.FuelId;
            existingCar.LocId = request.LocId;
            existingCar.Condition = request.Condition;
            existingCar.Mileage = request.Mileage;
            existingCar.LastInspectionDate = request.LastInspectionDate;

            existingCar.GearType = request.GearType;
            existingCar.ExteriorColor = request.ExteriorColor;
            existingCar.InteriorColor = request.InteriorColor;
            existingCar.EngineCapacity = request.EngineCapacity;
            existingCar.Horsepower = request.Horsepower;
            existingCar.DrivetrainType = request.DrivetrainType;

            // Handle images
            var currentImages = string.IsNullOrEmpty(existingCar.ImageUrls)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingCar.ImageUrls) ?? new List<string>();

            // Delete specified images
            if (imagesToDelete != null && imagesToDelete.Count > 0)
            {
                DeleteImages(imagesToDelete);
                currentImages = currentImages.Where(img => !imagesToDelete.Contains(img)).ToList();
            }

            // Add new images
            if (newImages != null && newImages.Count > 0)
            {
                var newImageUrls = SaveImages(newImages);
                currentImages.AddRange(newImageUrls);
            }

            existingCar.ImageUrls = currentImages.Count > 0
                ? JsonSerializer.Serialize(currentImages)
                : null;

            UnitOfWork.CarRepo.Update(existingCar);
            await UnitOfWork.SaveAsync();

            // Reload entity with includes to get related names
            var reloadedList = await UnitOfWork.CarRepo.GetAllAsync(query =>
                query.Where(c => c.CarId == existingCar.CarId)
                     .Include(c => c.Model).ThenInclude(m => m.Make)
                     .Include(c => c.BodyType)
                     .Include(c => c.FuelType)
                     .Include(c => c.LocationCity)
            );

            var reloadedEntity = reloadedList.FirstOrDefault();

            return reloadedEntity?.ToResponse() ?? existingCar.ToResponse();
        }

        public async Task DeleteAsync(string id, string userId, string userRole)
        {
            var item = await UnitOfWork.CarRepo.GetByIdAsync(id);
            if (item == null)
            {
                throw new Exception($"Car with ID {id} not found");
            }

            // Authorization check: Vendor can only delete their own cars
            if (userRole == "Vendor" && item.VendorId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own cars");
            }

            // Delete all associated images
            if (!string.IsNullOrEmpty(item.ImageUrls))
            {
                var imageUrls = JsonSerializer.Deserialize<List<string>>(item.ImageUrls);
                if (imageUrls != null && imageUrls.Count > 0)
                {
                    DeleteImages(imageUrls);
                }
            }

            UnitOfWork.CarRepo.Delete(item);
            await UnitOfWork.SaveAsync();
        }

        #region Private Helper Methods

        /// <summary>
        /// Validates condition-specific business rules
        /// </summary>
        private void ValidateConditionRules(CarRequest request)
        {
            // Used cars must have mileage
            if (request.Condition == CarCondition.Used && request.Mileage.HasValue)
            {
                if (request.Mileage.Value == 0)
                    throw new ArgumentException("Mileage is required for used cars");
            }

            // New cars should not have mileage (or should be 0)
            if (request.Condition == CarCondition.New && request.Mileage.HasValue && request.Mileage > 0)
            {
                throw new ArgumentException("New cars should not have mileage greater than 0");
            }

            // Inspection date cannot be in the future
            if (request.LastInspectionDate.HasValue && (request.LastInspectionDate.Value > DateTime.UtcNow
                || request.LastInspectionDate.Value.Year < request.Year))
            {
                throw new ArgumentException("Last inspection date cannot be in the future or prior its release year.");
            }
        }

        /// <summary>
        /// Applies filtering logic to the car query based on the request parameters
        /// </summary>
        private IQueryable<Car> ApplyFilters(IQueryable<Car> query, PaginationRequest request)
        {
            // Filter by Make (through Model relationship)
            if (request.MakeId.HasValue)
            {
                query = query.Where(c => c.Model.MakeId == request.MakeId.Value);
            }

            // Filter by Model
            if (request.ModelId.HasValue)
            {
                query = query.Where(c => c.ModelId == request.ModelId.Value);
            }

            // Filter by Body Type
            if (request.BodyTypeId.HasValue)
            {
                query = query.Where(c => c.BodyTypeId == request.BodyTypeId.Value);
            }

            // Filter by Fuel Type
            if (request.FuelId.HasValue)
            {
                query = query.Where(c => c.FuelId == request.FuelId.Value);
            }

            // Filter by Location
            if (request.LocId.HasValue)
            {
                query = query.Where(c => c.LocId == request.LocId.Value);
            }

            // Filter by Price Range
            if (request.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= request.MaxPrice.Value);
            }

            // Filter by Year
            if (request.Year.HasValue)
            {
                query = query.Where(c => c.Year == request.Year.Value);
            }

            // Filter by Condition
            if (request.Condition.HasValue)
            {
                query = query.Where(c => c.Condition == request.Condition.Value);
            }

            // Filter by Mileage Range
            if (request.MinMileage.HasValue)
            {
                query = query.Where(c => c.Mileage >= request.MinMileage.Value);
            }

            if (request.MaxMileage.HasValue)
            {
                query = query.Where(c => c.Mileage <= request.MaxMileage.Value);
            }

            // Filter by Gear Type
            if (request.GearType.HasValue)
            {
                query = query.Where(c => c.GearType == request.GearType.Value);
            }

            // Spec Filters

            if (request.DrivetrainType.HasValue)
            {
                query = query.Where(c => c.DrivetrainType == request.DrivetrainType.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.ExteriorColor))
            {
                query = query.Where(c => c.ExteriorColor.ToLower() == request.ExteriorColor.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(request.InteriorColor))
            {
                query = query.Where(c => c.InteriorColor != null && c.InteriorColor.ToLower() == request.InteriorColor.ToLower());
            }

            //these filters are unlikely to be used
            if (request.EngineCapacity.HasValue)
            {
                query = query.Where(c => c.EngineCapacity <= request.EngineCapacity.Value); //all cars less than the searched target
            }

            if (request.Horsepower.HasValue)
            {
                query = query.Where(c => c.Horsepower <= request.Horsepower.Value); //all cars less than the searched target
            }


            // Optional: Search term filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(c =>
                    c.Model.Make.MakeName.ToLower().Contains(term) ||
                    c.Model.ModelName.ToLower().Contains(term) ||
                    (c.Description != null && c.Description.ToLower().Contains(term))
                );
            }

            return query;
        }

        private List<string> SaveImages(IFormFileCollection images)
        {
            var imageUrls = new List<string>();
            var uploadsFolder = Path.Combine(webRootPath, "images", "cars");

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var image in images)
            {
                // Validate image
                if (!ValidateImageFile(image))
                {
                    continue; // Skip invalid images
                }

                // Generate unique filename
                var fileExtension = Path.GetExtension(image.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                // Store relative path
                imageUrls.Add($"images/cars/{uniqueFileName}");
            }

            return imageUrls;
        }

        private void DeleteImages(List<string> imageUrls)
        {
            foreach (var imageUrl in imageUrls)
            {
                try
                {
                    var filePath = Path.Combine(webRootPath, imageUrl.Replace("/", "\\"));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't throw - continue deleting other images
                    Console.WriteLine($"Error deleting image {imageUrl}: {ex.Message}");
                }
            }
        }

        private bool ValidateImageFile(IFormFile file)
        {
            // Check file size (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return false;
            }

            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }

            // Check MIME type
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}

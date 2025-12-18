using System.Text.Json;
using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class CarMappings
    {
        public static Car ToEntity(this CarRequest request, string? adminId, string? vendorId)
        {
            return new Car
            {
                CarId = Guid.NewGuid().ToString("N"),
                Year = request.Year,
                Price = request.Price,
                Description = request.Description,
                ModelId = request.ModelId,
                BodyTypeId = request.BodyTypeId,
                FuelId = request.FuelId,
                LocId = request.LocId,
                AdminId = adminId,
                VendorId = vendorId,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static CarResponse ToResponse(this Car car)
        {
            return new CarResponse
            {
                CarId = car.CarId,
                Year = car.Year,
                Price = car.Price,
                Description = car.Description,
                CreatedDate = car.CreatedDate,
                ImageUrls = string.IsNullOrEmpty(car.ImageUrls)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(car.ImageUrls),
                MakeName = car.Model?.Make?.MakeName,
                ModelName = car.Model?.ModelName,
                BodyTypeName = car.BodyType?.Name,
                FuelName = car.FuelType?.Name,
                LocationName = car.LocationCity?.Name
            };
        }
    }
}
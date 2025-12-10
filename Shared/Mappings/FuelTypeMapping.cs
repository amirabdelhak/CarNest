using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class FuelTypeMapping
    {
        public static FuelTypeResponse ToResponse(this FuelType e) =>
            new FuelTypeResponse
            {
                FuelId = e.FuelId,
                Name = e.Name
            };

        public static FuelType ToEntity(this FuelTypeRequest r) =>
            new FuelType
            {
                Name = r.Name
            };
    }
}

using DAL.Entity;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

namespace Presentation.Mappings
{
    public static class LocationCityMapping
    {
        public static LocationCityResponse ToResponse(this LocationCity e) =>
            new LocationCityResponse
            {
                LocId = e.LocId,
                Name = e.Name
            };

        public static LocationCity ToEntity(this LocationCityRequest r) =>
            new LocationCity
            {
                Name = r.Name
            };
    }
}

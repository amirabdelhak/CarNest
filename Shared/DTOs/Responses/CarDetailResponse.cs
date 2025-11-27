using Presentation.DTOs.Requests;

namespace Presentation.DTOs.Responses
{
    public class CarDetailResponse : CarResponse
    {
        public string? MakeName { get; set; }
        public string? ModelName { get; set; }
        public string? BodyTypeName { get; set; }
        public string? FuelName { get; set; }
        public string? LocationName { get; set; }
    }
}

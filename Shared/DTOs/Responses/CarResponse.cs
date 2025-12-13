using DAL.Entity;

namespace Presentation.DTOs.Responses
{
    public class CarResponse
    {
        public string CarId { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? ImageUrls { get; set; }

        public string? MakeName { get; set; }
        public string? ModelName { get; set; }
        public string? BodyTypeName { get; set; }
        public string? FuelName { get; set; }
        public string? LocationName { get; set; }

        public CarCondition Condition { get; set; }
        public int? Mileage { get; set; }
        public DateTime? LastInspectionDate { get; set; }
    }
}

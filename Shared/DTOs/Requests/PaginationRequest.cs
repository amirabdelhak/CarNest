using DAL.Entity;

namespace Presentation.DTOs.Requests
{
    public class PaginationRequest
    {
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Filter by dropdowns (IDs)
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? BodyTypeId { get; set; }
        public int? FuelId { get; set; }
        public int? LocId { get; set; }

        // Filter by ranges
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Year { get; set; }

        // Condition filters
        public CarCondition? Condition { get; set; }
        public int? MinMileage { get; set; }
        public int? MaxMileage { get; set; }

        // Optional: Search term for text search
        public string? SearchTerm { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using DAL.Entity;

namespace Presentation.DTOs.Requests
{
    public class CarRequest
    {
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2026, ErrorMessage = "Year must be between 1900 and 2026")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [MaxLength(2048, ErrorMessage = "Description cannot exceed 2048 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Body Type is required")]
        public int BodyTypeId { get; set; }

        [Required(ErrorMessage = "Fuel Type is required")]
        public int FuelId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public int LocId { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        public CarCondition Condition { get; set; } = CarCondition.New;

        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
        public int? Mileage { get; set; }

        public DateTime? LastInspectionDate { get; set; }

        [Required(ErrorMessage = "Gear Type is required")]
        public GearType GearType { get; set; } = GearType.Manual;
    }
}

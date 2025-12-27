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
        [Range(10000, double.MaxValue, ErrorMessage = "Price must be greater than 10,000")]
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


        //specs
        [Required(ErrorMessage = "Gear Type is required")]
        public GearType GearType { get; set; } = GearType.Manual;

        [Required(ErrorMessage = "Exterior color is required")]
        [MaxLength(50, ErrorMessage = "Exterior color cannot exceed 50 characters")]
        public string ExteriorColor { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Interior color cannot exceed 50 characters")]
        public string? InteriorColor { get; set; }

        [Required(ErrorMessage = "Engine capacity is required")]
        [Range(0.1, 99.99, ErrorMessage = "Engine capacity must be between 0.1 and 99.99 liters")]
        public decimal EngineCapacity { get; set; }

        [Required(ErrorMessage = "Horsepower is required")]
        [Range(1, 2000, ErrorMessage = "Horsepower must be between 1 and 2000")]
        public int Horsepower { get; set; }

        [Required(ErrorMessage = "Drivetrain type is required")]
        public DrivetrainType DrivetrainType { get; set; } = DrivetrainType.FWD;

    }
}

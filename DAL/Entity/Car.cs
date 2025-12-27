using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entity
{
    public class Car
    {
        [Key]
        [MaxLength(36)]
        public string CarId { get; set; }

        [Required]
        [Range(1900, 2026, ErrorMessage = "Year must be between 1900 and 2026")]
        public int Year { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(10.000, double.MaxValue, ErrorMessage = "Price must be greater than 10,000")]
        public decimal Price { get; set; }

        [MaxLength(2048)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// Image URLs stored as JSON array (e.g., ["images/cars/guid1.jpg", "images/cars/guid2.jpg"])
        [Column(TypeName = "nvarchar(max)")]
        public string? ImageUrls { get; set; }


        //SPECS
        [Required]
        [MaxLength(50)]
        public string ExteriorColor { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? InteriorColor { get; set; }

        [Required]
        [Precision(5, 2)]
        [Range(0.1, 99.99, ErrorMessage = "Engine capacity must be between 0.1 and 99.99 liters")]
        public decimal EngineCapacity { get; set; }

        [Required]
        [Range(1, 2000, ErrorMessage = "Horsepower must be between 1 and 2000")]
        public int Horsepower { get; set; }

        [Required]
        public DrivetrainType DrivetrainType { get; set; } = DrivetrainType.FWD;

        [Required]
        public GearType GearType { get; set; } = GearType.Manual;

        // Used car properties
        [Required]
        public CarCondition Condition { get; set; } = CarCondition.New;

        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
        public int? Mileage { get; set; }
        public DateTime? LastInspectionDate { get; set; }

        [Required]
        [ForeignKey("Model")]
        public int ModelId { get; set; }
        public virtual Model Model { get; set; }

        [Required]
        [ForeignKey("BodyType")]
        public int BodyTypeId { get; set; }
        public virtual BodyType BodyType { get; set; }

        [Required]
        [ForeignKey("FuelType")]
        public int FuelId { get; set; }
        public virtual FuelType FuelType { get; set; }

        [Required]
        [ForeignKey("LocationCity")]
        public int LocId { get; set; }
        public virtual LocationCity LocationCity { get; set; }


        [MaxLength(450)]
        [ForeignKey("Admin")]
        public string? AdminId { get; set; }
        public virtual Admin Admin { get; set; }

        [MaxLength(450)]
        [ForeignKey("Vendor")]
        public string? VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
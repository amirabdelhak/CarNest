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
        public int Year { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [MaxLength(2048)]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Make")]
        public int MakeId { get; set; }
        public virtual Make Make { get; set; }

        [ForeignKey("Model")]
        public int ModelId { get; set; }
        public virtual Model Model { get; set; }

        public virtual ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();

        [ForeignKey("BodyType")]
        public int BodyTypeId { get; set; }
        public virtual BodyType BodyType { get; set; }

        [ForeignKey("FuelType")]
        public int FuelId { get; set; }
        public virtual FuelType FuelType { get; set; }

        [ForeignKey("LocationCity")]
        public int LocId { get; set; }
        public virtual LocationCity LocationCity { get; set; }

        [ForeignKey("Admin")]
        public string AdminId { get; set; }
        public virtual Admin Admin { get; set; }


        [ForeignKey("Vendor")]
        public string BuyerId { get; set; }
        public virtual Vendor Vendor { get; set; }


        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>(); //customer relation

    }
}
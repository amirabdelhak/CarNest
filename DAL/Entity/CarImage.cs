using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class CarImage
    {
        [Key]
        public int ImageId { get; set; }
        [Required, MaxLength(2048)]
        public string ImageUrl { get; set; }
        [Required, ForeignKey("Car")]
        [MaxLength(36)]
        public string CarId { get; set; }
        public virtual Car? Car { get; set; }

    }
}

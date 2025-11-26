using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entity
{
    [PrimaryKey(nameof(CustomerId), nameof(CarId))]
    public class Favorite
    {
        [ForeignKey("AppUser")]
        public string CustomerId { get; set; }
        [ForeignKey("Car")]
        public string CarId { get; set; }
        public DateTime SavedAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Car Car { get; set; }

    }
}

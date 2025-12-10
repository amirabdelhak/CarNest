using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entity
{
    [Table("Customers")]

    public class Customer : IdentityUser
    {
        [Required, MaxLength(128)]
        public string FirstName { get; set; } = null!;

        [MaxLength(128)]
        public string? LastName { get; set; }

        [MaxLength(512)]
        public string? Address { get; set; }
        public string NationalId { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Responses
{
    public class CarImageResponse
    {
        [Required]
        public int ImageId { get; set; }

        [Required]
        [MaxLength(36)]
        public string CarId { get; set; } = string.Empty;

        [Required]
        [MaxLength(2048)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}

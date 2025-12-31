using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Presentation.DTOs.Requests
{
    public class CarCreateRequest : CarRequest
    {
        [Required(ErrorMessage = "At least one car image is required")]
        public new IFormFileCollection Images { get; set; }

        [Required(ErrorMessage = "Car license image is required")]
        public new IFormFile LicenseImage { get; set; }
    }
}

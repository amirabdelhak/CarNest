using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class FuelTypeRequest
    {
        [Required, MaxLength(128)]
        public string Name { get; set; } 
    }
}
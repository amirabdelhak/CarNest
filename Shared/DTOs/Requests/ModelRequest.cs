using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class ModelRequest
    {
        [Required, MaxLength(128)]
        public string ModelName { get; set; }
    }
}
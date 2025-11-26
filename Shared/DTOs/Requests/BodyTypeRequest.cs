using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class BodyTypeRequest
    {
        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}
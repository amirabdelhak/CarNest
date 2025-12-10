using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class MakeRequest
    {
        [Required, MaxLength(128)]
        public string MakeName { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class FavoriteRequest
    {
        [Required]
        public string CarId { get; set; }
    }
}

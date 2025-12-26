using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class UpdateProfileRequest
    {
        [Required, MaxLength(128)]
        public string FirstName { get; set; } = null!;

        [MaxLength(128)]
        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
        public string NationalId { get; set; } = null!;

        [MaxLength(512)]
        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}

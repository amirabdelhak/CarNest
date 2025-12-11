using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class RegisterVendorRequest
    {
        [Required, MaxLength(128)]
        public string FirstName { get; set; }

        [MaxLength(128)]
        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
        public string NationalId { get; set; }

        [MaxLength(512)]
        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}

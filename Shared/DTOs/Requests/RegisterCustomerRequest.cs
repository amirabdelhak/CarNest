using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.Requests
{
    public class RegisterCustomerRequest
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
        public string NationalId { get; set; }

        [MaxLength(512)]
        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}

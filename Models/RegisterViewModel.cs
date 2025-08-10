using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Role { get; set; } // Admin or Customer

        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}

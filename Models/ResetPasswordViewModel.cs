using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}

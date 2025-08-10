using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}

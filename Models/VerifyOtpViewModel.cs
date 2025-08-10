using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class VerifyOtpViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }

       
    }
}

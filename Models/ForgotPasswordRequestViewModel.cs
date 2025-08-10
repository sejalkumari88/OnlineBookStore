using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class ForgotPasswordRequestViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}

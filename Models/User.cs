using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // ✅ OTP Properties
        public string? OTP { get; set; }
        public DateTime? OTPGeneratedAt { get; set; }

        // Relationships
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

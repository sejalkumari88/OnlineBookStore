using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Author { get; set; }

        public string? Genre { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? Description { get; set; }

        public string? CoverImage { get; set; }

        public string? Publisher { get; set; }

        public string? Language { get; set; }

        public DateTime? PublishedDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

        public ICollection<Cart> Cart { get; set; } = new List<Cart>();

        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

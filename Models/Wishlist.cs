namespace OnlineBookStore.Models
{
    public class Wishlist
    {
        public int WishlistID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = default!;

        public int BookID { get; set; }
        public Book Book { get; set; } = default!;

        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}

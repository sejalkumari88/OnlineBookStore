namespace OnlineBookStore.Models
{
    public class BookCategory
    {
        public int BookID { get; set; }
        public Book Book { get; set; } = default!;
        public int CategoryID { get; set; }
        public Category Category { get; set; } = default!;
    }
}

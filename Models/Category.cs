namespace OnlineBookStore.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = default!;
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}

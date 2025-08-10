using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;

namespace OnlineBookStore.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderID, od.BookID });

            // BookCategory many-to-many
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookID, bc.CategoryID });

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookID);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryID);

            // Cart relations
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Cart)
                .HasForeignKey(c => c.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Wishlist relation
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Order relation
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Review relation
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookID);

            // Precision settings for price
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasPrecision(18, 2);
        }
    }
}

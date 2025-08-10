using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class WishlistController : Controller
    {
        private readonly BookStoreDbContext _context;

        public WishlistController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ✅ Show all wishlist items for logged-in user
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlist = _context.Wishlists
                                   .Include(w => w.Book)
                                   .Where(w => w.UserID == userId)
                                   .ToList();

            return View(wishlist);
        }

        // ✅ Add to wishlist
        public IActionResult Add(int bookId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool alreadyExists = _context.Wishlists
                                         .Any(w => w.BookID == bookId && w.UserID == userId);
            if (!alreadyExists)
            {
                var wishlistItem = new Wishlist
                {
                    BookID = bookId,
                    UserID = userId.Value
                };
                _context.Wishlists.Add(wishlistItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ✅ Remove from wishlist
        public IActionResult Remove(int id)
        {
            var item = _context.Wishlists.Find(id);
            if (item != null)
            {
                _context.Wishlists.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ✅ Move to Cart and remove from wishlist
        public IActionResult MoveToCart(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlistItem = _context.Wishlists
                                       .Include(w => w.Book)
                                       .FirstOrDefault(w => w.WishlistID == id && w.UserID == userId);

            if (wishlistItem != null)
            {
                var cartItem = _context.Carts.FirstOrDefault(c => c.UserID == userId && c.BookID == wishlistItem.BookID);
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                }
                else
                {
                    _context.Carts.Add(new Cart
                    {
                        BookID = wishlistItem.BookID,
                        UserID = userId.Value,
                        Quantity = 1
                    });
                }

                _context.Wishlists.Remove(wishlistItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}

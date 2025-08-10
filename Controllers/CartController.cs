using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly BookStoreDbContext _context;

        public CartController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ✅ View Cart Items
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = _context.Carts
                .Include(c => c.Book)
                .Where(c => c.UserID == userId)
                .ToList();

            return View(cartItems);
        }

        // ✅ Add to Cart (POST)
        [HttpPost]
        public IActionResult Add(int bookId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account"); // ✅ Correct controller name

            var cartItem = _context.Carts
                .FirstOrDefault(c => c.BookID == bookId && c.UserID == userId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var newItem = new Cart
                {
                    BookID = bookId,
                    UserID = userId.Value,
                    Quantity = quantity
                };
                _context.Carts.Add(newItem);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // ✅ Remove item from cart
        public IActionResult Remove(int id)
        {
            var item = _context.Carts.Find(id);
            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ✅ Clear all items from cart
        public IActionResult Clear()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var cartItems = _context.Carts.Where(c => c.UserID == userId).ToList();
            if (cartItems.Any())
            {
                _context.Carts.RemoveRange(cartItems);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BookStoreDbContext _context;

        public OrdersController(BookStoreDbContext context)
        {
            _context = context;
        }

        // Admin: View all orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .ToListAsync();

            return View(orders);
        }

        // User: View their own orders
        public async Task<IActionResult> MyOrders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var orders = await _context.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .ToListAsync();

            return View("Index",orders);
        }

        // View order details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}

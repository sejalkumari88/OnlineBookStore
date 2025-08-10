using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OnlineBookStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BookStoreDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public CheckoutController(BookStoreDbContext context, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        // ✅ Display Checkout page
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var cartItems = _context.Carts
                .Include(c => c.Book)
                .Where(c => c.UserID == userId)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            return View(cartItems);
        }

        // ✅ Place Order (COD or Online)
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string shippingAddress, string paymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var cartItems = _context.Carts
                .Include(c => c.Book)
                .Where(c => c.UserID == userId)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var totalAmount = cartItems.Sum(c => c.Quantity * c.Book.Price);

            if (paymentMethod == "Online")
            {
                // 🔐 Razorpay API credentials
                var key = _config["Razorpay:KeyId"];
                var secret = _config["Razorpay:KeySecret"];

                var client = _httpClientFactory.CreateClient();
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{key}:{secret}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                var orderData = new
                {
                    amount = (int)(totalAmount * 100), // in paise
                    currency = "INR",
                    receipt = $"rcpt_{Guid.NewGuid()}",
                    payment_capture = 1
                };

                var json = new StringContent(JsonSerializer.Serialize(orderData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.razorpay.com/v1/orders", json);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Failed to create Razorpay order.");

                var orderJson = JsonDocument.Parse(result);
                var razorpayOrderId = orderJson.RootElement.GetProperty("id").GetString();

                // 📝 Save order with pending status
                var order = new Order
                {
                    UserID = userId.Value,
                    ShippingAddress = shippingAddress,
                    PaymentMethod = "Online",
                    Status = "Pending",
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    RazorpayOrderId = razorpayOrderId,
                    OrderDetails = cartItems.Select(c => new OrderDetail
                    {
                        BookID = c.BookID,
                        Quantity = c.Quantity,
                        Price = c.Book.Price
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // 📧 Fetch user email & phone (update this logic as needed)
                var user = await _context.Users.FindAsync(userId);

                // Prepare Razorpay Checkout ViewBag
                ViewBag.RazorpayOrderId = razorpayOrderId;
                ViewBag.Amount = (int)(totalAmount * 100);
                ViewBag.Key = key;
                ViewBag.Currency = "INR";
                ViewBag.OrderId = order.OrderID;
                ViewBag.Name = "Online Book Store";
                ViewBag.Email = user?.Email ?? "customer@example.com"; // Replace if null
                ViewBag.Contact = user?.PhoneNumber ?? "9876543210";    // Replace if null

                return View("RazorpayPayment");
            }

            // ✅ COD Flow
            var codOrder = new Order
            {
                UserID = userId.Value,
                ShippingAddress = shippingAddress,
                PaymentMethod = "COD",
                Status = "Pending",
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    BookID = c.BookID,
                    Quantity = c.Quantity,
                    Price = c.Book.Price
                }).ToList()
            };

            _context.Orders.Add(codOrder);
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderConfirmation", new { orderId = codOrder.OrderID });
        }

        // ✅ Razorpay Payment Verification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyPayment(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int orderId)
        {
            var secret = _config["Razorpay:KeySecret"];
            var data = $"{razorpay_order_id}|{razorpay_payment_id}";

            string expectedSignature = GenerateSignature(data, secret);

            if (expectedSignature != razorpay_signature)
                return BadRequest("Payment verification failed.");

            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null)
                return NotFound();

            order.Status = "Paid";
            order.RazorpayPaymentId = razorpay_payment_id;

            var userId = HttpContext.Session.GetInt32("UserId");
            var userCart = _context.Carts.Where(c => c.UserID == userId);
            _context.Carts.RemoveRange(userCart);

            _context.SaveChanges();

            ViewBag.PaymentId = razorpay_payment_id;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = order.TotalAmount;

            return View("PaymentSuccess");
        }

        // ✅ Generate Signature
        private string GenerateSignature(string data, string key)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] dataBytes = encoding.GetBytes(data);
            using var hmac = new HMACSHA256(keyBytes);
            byte[] hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        // ✅ Final Order Summary Page
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Book)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}

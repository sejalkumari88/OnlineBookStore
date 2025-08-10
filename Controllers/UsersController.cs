using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly BookStoreDbContext _context;

        public UserController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ========== ADMIN ==========

        // GET: /User/All
        public IActionResult All()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return Unauthorized();

            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: /User/Delete/5
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return Unauthorized();

            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("All");
        }

        // ========== CUSTOMER ==========

        // GET: /User/Profile
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /User/EditProfile
        [HttpPost]
        public IActionResult EditProfile(User updatedUser)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId != updatedUser.UserID)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(updatedUser.UserID);
                if (user == null)
                    return NotFound();

                user.FullName = updatedUser.FullName;
                user.Address = updatedUser.Address;
                user.PhoneNumber = updatedUser.PhoneNumber;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Profile updated!";
                return RedirectToAction("Profile");
            }

            return View("Profile", updatedUser);
        }
    }
}

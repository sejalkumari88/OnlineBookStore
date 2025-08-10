using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Services;
using System;
using OnlineBookStore.Filters;

namespace OnlineBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookStoreDbContext _context;
        private readonly EmailService _emailService;
        public AccountController(BookStoreDbContext context,EmailService emailService)
        {
            _emailService = emailService;
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password, // ✅ Consider hashing
                    Role = model.Role ?? "Customer",
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u =>
                    u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserID);
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid credentials.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


        // 1. ForgotPassword GET + POST
        [HttpGet]
public IActionResult ForgotPassword() => View();

[HttpPost]
public IActionResult ForgotPassword(ForgotPasswordViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            user.OTP = otp;
            user.OTPGeneratedAt = DateTime.Now;

            _context.SaveChanges();

                    // For now, show OTP on screen or console. In production, send email/SMS.
                    _emailService.SendEmail(
            user.Email,
            "Your OTP for Password Reset",
            $"Your OTP is: {otp}. It is valid for 10 minutes."
        );
                    TempData["OTPMessage"] = "OTP sent to your email.";

                    return RedirectToAction("VerifyOTP", new { email = user.Email });
        }
        ModelState.AddModelError("", "Email not found.");
    }

    return View(new ForgotPasswordRequestViewModel());
}

// 2. Verify OTP GET + POST
[HttpGet]
public IActionResult VerifyOTP(string email)
{
    var model = new VerifyOtpViewModel { Email = email };
    return View(model);
}

[HttpPost]
public IActionResult VerifyOTP(VerifyOtpViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null && user.OTP == model.OTP &&
            user.OTPGeneratedAt != null &&
            (DateTime.Now - user.OTPGeneratedAt.Value).TotalMinutes <= 10)
        {
            return RedirectToAction("ResetPassword", new { email = user.Email });
        }

        ModelState.AddModelError("", "Invalid or expired OTP.");
    }

    return View(model);
}

// 3. Reset Password GET + POST
[HttpGet]
public IActionResult ResetPassword(string email)
{
    return View(new ResetPasswordViewModel { Email = email });
}

[HttpPost]
public IActionResult ResetPassword(ResetPasswordViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            user.Password = model.NewPassword;
            user.OTP = null;
            user.OTPGeneratedAt = null;

            _context.SaveChanges();

            TempData["Success"] = "Password reset successful!";
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "User not found.");
    }

    return View(model);
}

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoreDbContext _context;

        public HomeController(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string genre)
        {
            var books = from b in _context.Books
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                books = books.Where(b => b.Genre == genre);
            }

            var genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .ToListAsync();

            ViewBag.Genres = genres;

            return View(await books.ToListAsync());
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.Categories = _context.Categories.ToList();
            base.OnActionExecuting(context);
        }


    }


}

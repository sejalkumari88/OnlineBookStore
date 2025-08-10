using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Filters;

namespace OnlineBookStore.Controllers
{
    [AdminOnly]
    public class BooksController : Controller
    {
        private readonly BookStoreDbContext _context;

        public BooksController(BookStoreDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Books with optional Category filter
        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            var books = _context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                books = books.Where(b => b.BookCategories.Any(bc => bc.CategoryID == categoryId));
            }

            return View(await books.ToListAsync());
        }

        // ✅ GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(m => m.BookID == id);

            if (book == null) return NotFound();

            return View(book);
        }
        [AdminOnly]
        // ✅ GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // ✅ POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile imageFile, int[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    book.CoverImage = "/images/" + fileName;
                }

                _context.Add(book);
                await _context.SaveChangesAsync();

                // Save selected categories
                if (selectedCategories != null && selectedCategories.Any())
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        _context.BookCategories.Add(new BookCategory
                        {
                            BookID = book.BookID,
                            CategoryID = categoryId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }
        [AdminOnly]
        // ✅ GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.BookID == id);

            if (book == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategories = book.BookCategories.Select(bc => bc.CategoryID).ToList();

            return View(book);
        }

        // ✅ POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile imageFile, int[] selectedCategories)
        {
            if (id != book.BookID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        book.CoverImage = "/images/" + fileName;
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    // Remove old category mappings
                    var existingCategories = _context.BookCategories.Where(bc => bc.BookID == id);
                    _context.BookCategories.RemoveRange(existingCategories);
                    await _context.SaveChangesAsync();

                    // Add new category mappings
                    if (selectedCategories != null && selectedCategories.Any())
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            _context.BookCategories.Add(new BookCategory
                            {
                                BookID = book.BookID,
                                CategoryID = categoryId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }

        // ✅ GET: Books/Delete/5
        [AdminOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null) return NotFound();

            return View(book);
        }

        // ✅ POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}

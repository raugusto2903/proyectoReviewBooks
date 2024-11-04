using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReviewBooks.Data;
using ReviewBooks.Models;

namespace ReviewBooks.Controllers
{
    public class BookReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookReviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookReview.Include(b => b.Book).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> IndexSearch(string author, string genre, string title)
        {
            var reviews = _context.BookReview.Include(br => br.Book).Include(br => br.User);
            List<BookReview> unifiedList = new List<BookReview>();

            if (!string.IsNullOrEmpty(author))
            {
                var reviewsAuthor = reviews.Where(r => r.Book.Author.Contains(author));
                unifiedList.AddRange(reviewsAuthor.ToList());
            }

            if (!string.IsNullOrEmpty(genre))
            {
                var reviewsGenre = reviews.Where(r => r.Book.Gender.Contains(genre));
                unifiedList.AddRange(reviewsGenre.ToList());
            }

            if (!string.IsNullOrEmpty(title))
            {
                var reviewsName = reviews.Where(r => r.Book.Name.Contains(title));
                unifiedList.AddRange(reviewsName.ToList());
            }

            return View(unifiedList);
        }

        // GET: BookReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReview = await _context.BookReview
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookReview == null)
            {
                return NotFound();
            }

            return View(bookReview);
        }

        // GET: BookReviews/Create
        public IActionResult Create()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Esto obtiene el ID del usuario logueado
            //ViewBag.UserId = userId;

            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BookReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReviewText,ReviewDate,BookId,UserId,Qualifying")] BookReview bookReview)
        {
            bookReview.Id = 0;
            bookReview.ReviewDate = DateTime.SpecifyKind(bookReview.ReviewDate, DateTimeKind.Utc);

            ModelState.Remove("Book");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                bookReview.Book = await _context.Book.FindAsync(bookReview.BookId);
                bookReview.User = await _context.Users.FindAsync(bookReview.UserId);
                _context.Add(bookReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", bookReview.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookReview.UserId);
            return View(bookReview);
        }

        // GET: BookReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReview = await _context.BookReview.FindAsync(id);
            if (bookReview == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", bookReview.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookReview.UserId);
            return View(bookReview);
        }

        // POST: BookReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReviewText,ReviewDate,BookId,UserId")] BookReview bookReview)
        {
            if (id != bookReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookReviewExists(bookReview.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", bookReview.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookReview.UserId);
            return View(bookReview);
        }

        // GET: BookReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReview = await _context.BookReview
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookReview == null)
            {
                return NotFound();
            }

            return View(bookReview);
        }

        // POST: BookReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookReview = await _context.BookReview.FindAsync(id);
            if (bookReview != null)
            {
                _context.BookReview.Remove(bookReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookReviewExists(int id)
        {
            return _context.BookReview.Any(e => e.Id == id);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly LibraryProjectContext _context;

        public BooksController(LibraryProjectContext context)
        {
            _context = context;
        }
        // GET: Books
        //get distinct books
        public IActionResult ShowAllBooks(string sTitle, string sAuthor, string sGenre)
        {
            /*var b = _context.Book.GroupBy(p => p.Title)
                .Select(g => new BookCatalog
                {
                    Title = g.Key,
                }).ToList();*/
            List<Book> books = new List<Book>();
            IQueryable<string> Titles = _context.Book.Select(s => s.Title).Distinct();
            IQueryable<string> MovieGenre = _context.Book.Select(s => s.Genre).Distinct();
            foreach (var n in Titles)
            {
                Book book = _context.Book.Where(s => s.Title == n).FirstOrDefault();
                books.Add(book);
            }
            if (!string.IsNullOrEmpty(sTitle) || !string.IsNullOrEmpty(sAuthor) || !string.IsNullOrEmpty(sGenre)) {
                foreach (Book b in books.ToList())
                {
                    if (!string.IsNullOrEmpty(sTitle))
                    {
                        if (!b.Title.ToLower().Contains(sTitle.ToLower()))
                            books.Remove(b);
                    }
                    if (!string.IsNullOrEmpty(sAuthor))
                    {
                        if (!b.Author.ToLower().Contains(sAuthor.ToLower()))
                            books.Remove(b);
                    }
                    if (!string.IsNullOrEmpty(sGenre))
                    {
                        if (!(b.Genre==sGenre))
                            books.Remove(b);
                    }
                }
            }
            var sBooks = new SearchBooks
            {
                Books = books,
                Genre= new SelectList(MovieGenre.ToList())
            };
            return View(sBooks);            
        }
        public async Task<IActionResult> Show(int id)
        {
            var book =  _context.Book.FirstOrDefault(m => m.Id == id);
            var bookTitle = book.Title;
            ViewData["NewId"] = id;
            IQueryable<Book> Books = _context.Book.Where(s => s.Title == bookTitle);
            return View(await Books.ToListAsync());
        }
        public IActionResult AddNew()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew(int id, int ISNB) 
        {
            if (ModelState.IsValid)
            {
                Book book = new Book();
                var OldBook = _context.Book.FirstOrDefault(m => m.Id == id);
                book.Title = OldBook.Title;
                book.ShortContent = OldBook.ShortContent;
                book.PageNumber = OldBook.PageNumber;
                book.PublicationYear = OldBook.PublicationYear;
                book.PublishingHouse = OldBook.PublishingHouse;
                book.BookImage = OldBook.BookImage;
                book.Author = OldBook.Author;
                book.Genre = OldBook.Genre;
                book.Status = "available";
                book.ISBN = ISNB;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Show",new { id = id });
                //return RedirectToAction("Show", new { id = id })
                //redturn View(Show,{ new id =id});
            }
            return View();
            
            //return View(newbook);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,ISBN,PageNumber,ShortContent,PublicationYear,PublishingHouse,Genre,Status,BookImage")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowAllBooks));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,PageNumber,ShortContent,PublicationYear,PublishingHouse,Genre,Status,BookImage")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Show", new { id = id });
            }
            return View(book);
        }

        public async Task<IActionResult> NewEditBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewEditBook(int id, [Bind("Id,Title,Author,ISBN,PageNumber,ShortContent,PublicationYear,PublishingHouse,Genre,Status,BookImage")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                var booknew = _context.Book.FirstOrDefault(m => m.Id == id);
                var bookTitle = booknew.Title;
                IQueryable<Book> ha =_context.Book.Where(s => s.Title == bookTitle);
                List<Book> books = ha.ToList();
                //return View(await Books.ToListAsync());
                try
                {
                    foreach (Book b in books)
                    {

                        b.Id = b.Id;
                        b.ISBN = b.ISBN;
                        b.Title = book.Title;
                        b.Author = book.Author;
                        b.ISBN = b.ISBN;
                        b.PageNumber = book.PageNumber;
                        b.ShortContent = book.ShortContent;
                        b.PublicationYear = book.PublicationYear;
                        b.PublishingHouse = book.PublishingHouse;
                        b.Genre = book.Genre;
                        b.Status = book.Status;
                        b.BookImage = book.BookImage;
                        _context.Update(b);
                        await _context.SaveChangesAsync();
                    }
                    //_context.Update(book);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ShowAllBooks");
            }
              
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowAllBooks");
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly LibraryProjectContext _context;

        public LibrarianController(LibraryProjectContext context)
        {
            _context = context;
        }
        public IActionResult ShowAllBooks(string sTitle, string sAuthor, string sGenre)
        {

            List<Book> books = new List<Book>();
            IQueryable<string> Titles = _context.Book.Select(s => s.Title).Distinct();
            IQueryable<string> MovieGenre = _context.Book.Select(s => s.Genre).Distinct();
            foreach (var n in Titles)
            {
                Book book = _context.Book.Where(s => s.Title == n).FirstOrDefault();
                books.Add(book);
            }
            if (!string.IsNullOrEmpty(sTitle) || !string.IsNullOrEmpty(sAuthor) || !string.IsNullOrEmpty(sGenre))
            {
                foreach (Book b in books.ToList())
                {
                    if (!string.IsNullOrEmpty(sTitle))
                    {
                        if (!b.Title.ToLower().Contains(sTitle.ToLower()))
                            books.Remove(b);
                    }
                    if (!string.IsNullOrEmpty(sAuthor))
                    {
                        if (!b.Title.ToLower().Contains(sAuthor.ToLower()))
                            books.Remove(b);
                    }
                    if (!string.IsNullOrEmpty(sGenre))
                    {
                        if (!(b.Genre == sGenre))
                            books.Remove(b);
                    }
                }
            }
            var sBooks = new SearchBooks
            {
                Books = books,
                Genre = new SelectList(MovieGenre.ToList())
            };
            return View(sBooks);
        }
        public async Task<IActionResult> Show(int id)
        {
            var book = _context.Book.FirstOrDefault(m => m.Id == id);
            var bookTitle = book.Title;
            IQueryable<Book> Books = _context.Book.Where(s => s.Title == bookTitle);
            Books = Books.Include(s => s.ChecksOut);
            return View(await Books.ToListAsync());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class CheckOutsController : Controller
    {
       
        private readonly LibraryProjectContext _context;
        private readonly UserManager<AppUser> userManager;
        public CheckOutsController(LibraryProjectContext context,UserManager<AppUser> userMgr)
        {
            _context = context;
            userManager = userMgr;
        }

        // GET: CheckOuts
        public async Task<IActionResult> Index()
        {
            var libraryProjectContext = _context.CheckOut.Include(c => c.Book).Include(c => c.Customer).Include(c => c.Librarian);
            return View(await libraryProjectContext.ToListAsync());
        }

        // GET: CheckOuts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut
                .Include(c => c.Book)
                .Include(c => c.Customer)
                .Include(c => c.Librarian)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkOut == null)
            {
                return NotFound();
            }

            return View(checkOut);
        }

        // GET: CheckOuts/Create
        public async Task<IActionResult> Create(int? id, int bookId)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.LibrarianId != null)
                    ViewData["LibrarianId"] = curruser.LibrarianId;
                else
                    return NotFound();
            }
            ViewData["BookId"] = bookId;
            var bookName = _context.Book.Where(b => b.Id == bookId).FirstOrDefault();
            ViewData["BookName"] =  bookName.Title;
            //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Email");
            //ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email");
            return View();
        }

        // POST: CheckOuts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChecksOut,ChecksIn,BookId,CustomerId,LibrarianId")] CheckOut checkOut)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkOut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", checkOut.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Email", checkOut.CustomerId);
            ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", checkOut.LibrarianId);
            return View(checkOut);
        }*/
        [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create(int BookId,int CustomerId,int LibrarianId)
       {
            
           
            if (ModelState.IsValid)
           {
                var checkOut = new CheckOut();
                var customer = _context.Customer.FirstOrDefault(s => s.CardId == CustomerId);
                var customerId = customer.Id;
                var broj = _context.CheckOut.Where(c => c.CustomerId == customerId && c.ChecksIn==null).Count();
                if (broj < 3)
                {
                    var book = _context.Book.FirstOrDefault(s => s.Id == BookId);
                    book.Status = "not available";
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    checkOut.ChecksOut = DateTime.Now;
                    checkOut.ChecksIn = null;
                    checkOut.CustomerId = customerId;
                    checkOut.BookId = BookId;
                    checkOut.LibrarianId = LibrarianId;
                    _context.Add(checkOut);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    return RedirectToAction(nameof(MoreBooks));
            }
           //ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", checkOut.BookId);
           //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Email", checkOut.CustomerId);
           //ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", checkOut.LibrarianId);
           return View();
       }
        public IActionResult MoreBooks()
        {
            return View();
        }

        // GET: CheckOuts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut.FindAsync(id);
            if (checkOut == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = checkOut.BookId;
            ViewData["CustomerId"] = checkOut.CustomerId;
            ViewData["LibrarianId"] =  checkOut.LibrarianId;
            return View(checkOut);
        }

        // POST: CheckOuts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChecksOut,ChecksIn,BookId,CustomerId,LibrarianId")] CheckOut checkOut)
        {
            if (id != checkOut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkOut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckOutExists(checkOut.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", checkOut.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Email", checkOut.CustomerId);
            ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", checkOut.LibrarianId);
            return View(checkOut);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChecksOut,ChecksIn,BookId,CustomerId,LibrarianId")] CheckOut checkOut)
        {
            if (id != checkOut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = _context.Book.FirstOrDefault(s => s.Id == checkOut.BookId);
                    book.Status = "available";
                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    checkOut.ChecksIn = DateTime.Now;
                    _context.Update(checkOut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckOutExists(checkOut.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ShowCustomerBooks","Customers",new{ id=checkOut.CustomerId});
            }
            return View(checkOut);
        }

        // GET: CheckOuts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut
                .Include(c => c.Book)
                .Include(c => c.Customer)
                .Include(c => c.Librarian)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkOut == null)
            {
                return NotFound();
            }

            return View(checkOut);
        }

        // POST: CheckOuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkOut = await _context.CheckOut.FindAsync(id);
            _context.CheckOut.Remove(checkOut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckOutExists(int id)
        {
            return _context.CheckOut.Any(e => e.Id == id);
        }
    }
}

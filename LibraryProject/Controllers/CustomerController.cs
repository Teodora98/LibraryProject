using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Authorize(Roles="Customer")]
    public class CustomerController : Controller
    {
        private readonly LibraryProjectContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CustomerController(LibraryProjectContext context, IWebHostEnvironment hostEnvironment, UserManager<AppUser> userMgr)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            userManager = userMgr;
        }

        public async Task<IActionResult> CustomerCardId(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.CustomerId != null)
                    return RedirectToAction(nameof(CustomerCardId), new { id = curruser.CustomerId });
                else
                    return NotFound();
            }

            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == id);
            IQueryable<CheckOut> checkout = _context.CheckOut.OrderBy(c => c.ChecksOut).Include(c => c.Customer)
                                          .Include(c => c.Book).Where(s => s.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            AppUser user = await userManager.GetUserAsync(User);
            if (customer.Id != user.CustomerId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }

            var vm = new CustomerCheckOut
            {
                CheckOut = checkout.ToList(),
                customer = customer
            };
            return View(vm);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.CustomerId != null)
                    return RedirectToAction(nameof(Edit), new { id = curruser.CustomerId });
                else
                    return NotFound();
            }
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            CustomerViewModel customermodel = new CustomerViewModel
            {
                Id = customer.Id,
                CardId = customer.CardId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Occupation = customer.Occupation,
                SSN = customer.SSN,
                Address = customer.Address,
                BirthDate = customer.BirthDate,
                MembershipDate = customer.MembershipDate,
                LibrarianId = (int)customer.LibrarianId,

            };
            return View(customermodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(model);

                    Customer customer = new Customer
                    {

                        Id = model.Id,
                        CardId = model.CardId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Occupation = model.Occupation,
                        SSN = model.SSN,
                        Address = model.Address,
                        BirthDate = model.BirthDate,
                        MembershipDate = model.MembershipDate,
                        LibrarianId = model.LibrarianId,
                        CustomerProfileImage = uniqueFileName,

                    };
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ShowAllBooks));
            }
            return View();
        }
        private string UploadedFile(CustomerViewModel model)
        {
            string uniqueFileName = null;

            if (model.CustomerProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CustomerProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CustomerProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
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
        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class CustomersController : Controller
    {
        private readonly LibraryProjectContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CustomersController(LibraryProjectContext context, IWebHostEnvironment hostEnvironment, UserManager<AppUser> userMgr)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            userManager = userMgr;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string sCardId,string sSSN, string sFullName)
        {
            //var libraryProjectContext = _context.Customer.Include(c => c.CreateUserBy);
            //return View(await libraryProjectContext.ToListAsync());
            IQueryable<Customer> customers = _context.Customer.AsQueryable();
            if (!string.IsNullOrEmpty(sFullName))
            {
                customers = customers.Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(sFullName.ToLower()));
            }
            if (!string.IsNullOrEmpty(sSSN))
            {
                customers = customers.Where(m => m.SSN.ToString().Contains(sSSN));
            }
            if (!string.IsNullOrEmpty(sCardId))
            {
                customers = customers.Where(m => m.CardId.ToString().Contains(sCardId));
            }
            customers = customers.Include(s => s.CreateUserBy);
            var customerVM = new SearchCustomers
            {
                Customer = await customers.ToListAsync()
            };
            return View(customerVM);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.CreateUserBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.LibrarianId != null)
                    ViewData["LibrarianId"] = curruser.LibrarianId;
                else
                    return NotFound();
            }
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardId,FirstName,LastName,SSN,Address,Occupation,PhoneNumber,BirthDate,MembershipDate,Email,LibrarianId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction ("CustomerProfile", "Admin",new {customerId=customer.Id});
            }
            ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", customer.LibrarianId);
            return View(customer);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                ViewData["CardId"] = model.Id;
                Customer customer = new Customer
                {

                    Id = model.Id,
                    CardId=model.CardId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Occupation=model.Occupation,
                    SSN=model.SSN,
                    Address=model.Address,
                    BirthDate=model.BirthDate,
                    MembershipDate=DateTime.Now,
                    LibrarianId=model.LibrarianId,
                    CustomerProfileImage = uniqueFileName,
                };
                _context.Add(customer);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("CustomerProfile", "Admin", new { customerId = customer.Id });
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

        // GET: Customers/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", customer.LibrarianId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardId,FirstName,LastName,SSN,Address,Occupation,PhoneNumber,BirthDate,MembershipDate,Email,LibrarianId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            ViewData["LibrarianId"] = new SelectList(_context.Set<Librarian>(), "Id", "Email", customer.LibrarianId);
            return View(customer);
        }*/
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
                return RedirectToAction("Details",id);
            }
            return View();
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.CreateUserBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            /*var checkouts =  _context.CheckOut.Where(c => c.CustomerId == id);
            foreach(var item in checkouts)
            {
                _context.CheckOut.Remove(item);
            }*/
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<IActionResult> CustomerCardId(int id)
        {
            var card =await  _context.Customer.FirstOrDefaultAsync(c => c.CardId == id);
            var Id = card.Id;
            IQueryable<CheckOut> checkout = _context.CheckOut.OrderBy(c=>c.ChecksOut).Include(c => c.Customer)
                                          .Include(c => c.Book).Where(s => s.CustomerId == Id);
            var vm = new CustomerCheckOut
            {
                CheckOut = checkout.ToList(),
                customer = card
            };
            return View(vm);
        }*/
        public async Task<IActionResult> ShowCustomerBooks(int? id)
        {

            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == id);
            IQueryable<CheckOut> checkout = _context.CheckOut.OrderBy(c => c.ChecksOut).Include(c => c.Customer)
                                          .Include(c => c.Book).Where(s => s.CustomerId == id);
            IQueryable<CheckOut> noCheckIn = _context.CheckOut.Where(s => s.CustomerId == id);
            var l = noCheckIn.Where(s => s.ChecksIn == null).Count();
            if (customer == null)
            {
                return NotFound();
            }

            var vm = new CustomerCheckOut
            {
                CheckOut = checkout.ToList(),
                customer = customer,
                NoMoreBooks=l
            };
            return View(vm);
        }
        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}

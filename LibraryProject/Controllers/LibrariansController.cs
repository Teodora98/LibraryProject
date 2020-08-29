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
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LibraryProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LibrariansController : Controller
    {
        private readonly LibraryProjectContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public LibrariansController(LibraryProjectContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Librarians
        public async Task<IActionResult> Index()
        {
            return View(await _context.Librarian.ToListAsync());
        }

        // GET: Librarians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var librarian = await _context.Librarian
                .FirstOrDefaultAsync(m => m.Id == id);
            if (librarian == null)
            {
                return NotFound();
            }

            return View(librarian);
        }

        // GET: Librarians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Librarians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,SSN,Address,PhoneNumber,BirthDate,Email")] Librarian librarian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(librarian);
                await _context.SaveChangesAsync();
                return RedirectToAction("LibrarianProfile", "Admin", new { librarianId = librarian.Id });
            }
            return View(librarian);
        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibrarianViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Librarian librarian = new Librarian
                {

                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SSN = model.SSN,
                    Address = model.Address,
                    BirthDate = model.BirthDate,
                    LibrarianProfileImage = uniqueFileName,

                };
                _context.Add(librarian);
                await _context.SaveChangesAsync();
                return RedirectToAction("LibrarianProfile", "Admin", new { librarianId = librarian.Id });
            }
            return View();
        }
        
        // GET: Librarians/Edit/5
        /* public async Task<IActionResult> Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var librarian = await _context.Librarian.FindAsync(id);
             if (librarian == null)
             {
                 return NotFound();
             }
             return View(librarian);
         }

         // POST: Librarians/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,SSN,Address,BirthDate")] Librarian librarian)
         {
             if (id != librarian.Id)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(librarian);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!LibrarianExists(librarian.Id))
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
             return View(librarian);
         }
        */
        public async Task<IActionResult> Edit(int? id)
        {
            var librarian = await _context.Librarian.FindAsync(id);

            if (librarian == null)
            {
                return NotFound();
            }
            LibrarianViewModel libmodel = new LibrarianViewModel
            {
                Id = librarian.Id,
                FirstName = librarian.FirstName,
                LastName = librarian.LastName,
                SSN = librarian.SSN,
                Address = librarian.Address,
                BirthDate = librarian.BirthDate,
            };
            return View(libmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibrarianViewModel model)
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

                    Librarian librarian = new Librarian
                    {

                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        SSN = model.SSN,
                        Address = model.Address,
                        BirthDate = model.BirthDate,
                        LibrarianProfileImage = uniqueFileName,

                    };
                    _context.Update(librarian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrarianExists(model.Id))
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
            return View();
        }
        private string UploadedFile(LibrarianViewModel model)
        {
            string uniqueFileName = null;

            if (model.LibrarianProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.LibrarianProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.LibrarianProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        // GET: Librarians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var librarian = await _context.Librarian
                .FirstOrDefaultAsync(m => m.Id == id);
            if (librarian == null)
            {
                return NotFound();
            }

            return View(librarian);
        }

        // POST: Librarians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var librarian = await _context.Librarian.FindAsync(id);
            _context.Librarian.Remove(librarian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrarianExists(int id)
        {
            return _context.Librarian.Any(e => e.Id == id);
        }
    }
}

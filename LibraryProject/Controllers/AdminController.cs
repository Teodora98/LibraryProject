using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Controllers
{
       
        public class AdminController : Controller
        {
            private UserManager<AppUser> userManager;
            private IPasswordHasher<AppUser> passwordHasher;
            private IPasswordValidator<AppUser> passwordValidator;
            private IUserValidator<AppUser> userValidator;
            private readonly LibraryProjectContext _context;

            public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser>
    userValid, LibraryProjectContext context)
            {
                userManager = usrMgr;
                passwordHasher = passwordHash;
                passwordValidator = passwordVal;
                userValidator = userValid;
                _context = context;
            }
        [Authorize]
        public async Task<IActionResult> IndexCustomers(string email,string sRole)
            {
                IQueryable<AppUser> users = userManager.Users.OrderBy(u => u.Email);
            //return View(users);
                var role = userManager.Users.Select(s => s.Role).Distinct();
                if (!string.IsNullOrEmpty(email))
                {
                    users = users.Where(s => s.Email.ToLower().Contains(email.ToLower()));
                }
                if (!string.IsNullOrEmpty(sRole))
                {
                  users = users.Where(s => s.Role.ToLower().Contains(sRole.ToLower()));
                }
            var customerVM = new FindCustomerAccount
            {
                user = await users.ToListAsync(),
                Role = new SelectList(role.ToList())
            };
                return View(customerVM);
            }

        [Authorize(Roles = "Librarian")]
        public IActionResult CustomerProfile(int customerId)
            {
                
                AppUser user = userManager.Users.FirstOrDefault(u => u.CustomerId == customerId);
                Customer customer = _context.Customer.Where(s => s.Id == customerId).FirstOrDefault();
                if (customer != null)
                {
                    ViewData["FullName"] = customer.FirstName;
                    ViewData["CustomerId"] = customer.Id;
                }
                if (user != null)
                    return View(user);
                else
                    return View(null);
            }
        [Authorize(Roles = "Librarian")]
        [HttpPost]
            public async Task<IActionResult> CustomerProfile(int customerId, string email, string password)
            {
                //AppUser user = await userManager.FindByIdAsync(id);
                AppUser user = userManager.Users.FirstOrDefault(u => u.CustomerId == customerId);
                if (user != null)
                {
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    user.Email = email;
                    user.UserName = email;

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Email cannot be empty");

                    validUser = await userValidator.ValidateAsync(userManager, user);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                        if (validPass.Succeeded)
                            user.PasswordHash = passwordHasher.HashPassword(user, password);
                        else
                            Errors(validPass);
                    }

                    if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                    {
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return RedirectToAction("Index","Customers");
                        else
                            Errors(result);
                    }
                }
                else
                {
                    AppUser newuser = new AppUser();
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    newuser.Email = email;
                    newuser.UserName = email;
                    newuser.CustomerId = customerId;
                    newuser.Role = "Customer";

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Email cannot be empty");

                    validUser = await userValidator.ValidateAsync(userManager, newuser);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                        if (validPass.Succeeded)
                            newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                        else
                            Errors(validPass);
                    }
                    else
                        ModelState.AddModelError("", "Password cannot be empty");

                    if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                    {
                        IdentityResult result = await userManager.CreateAsync(newuser, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newuser, "Customer");
                            return RedirectToAction("Index", "Customers");
                        //return RedirectToAction(nameof(CustomerProfile), new { customerId });
                    }
                        else
                            Errors(result);
                    }
                    user = newuser;
                }
                Customer customer = _context.Customer.Where(s => s.Id == customerId).FirstOrDefault();
                if (customer != null)
                {
                    ViewData["FullName"] = customer.FirstName;
                    ViewData["CustomerId"] = customer.Id;
                }
                return View(user);
            }
        [Authorize(Roles = "Admin")]
        public IActionResult LibrarianProfile(int librarianId)
            {
                //AppUser user = await userManager.FindByIdAsync(id);
                AppUser user = userManager.Users.FirstOrDefault(u => u.LibrarianId == librarianId);
                Librarian librarian = _context.Librarian.Where(s => s.Id == librarianId).FirstOrDefault();
                if (librarian != null)
                {
                    ViewData["FullNameId"] = librarian.FirstName;
                    ViewData["LibrarianId"] = librarian.Id;
                }
                if (user != null)
                    return View(user);
                else
                    return View(null);
            }
        [Authorize(Roles = "Admin")]
        [HttpPost]
            public async Task<IActionResult> LibrarianProfile(int librarianId, string email, string password, string phoneNumber)
            {
                //AppUser user = await userManager.FindByIdAsync(id);
                AppUser user = userManager.Users.FirstOrDefault(u => u.LibrarianId == librarianId);
                if (user != null)
                {
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    user.Email = email;
                    user.UserName = email;
                    user.PhoneNumber = phoneNumber;

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Email cannot be empty");

                    validUser = await userValidator.ValidateAsync(userManager, user);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                        if (validPass.Succeeded)
                            user.PasswordHash = passwordHasher.HashPassword(user, password);
                        else
                            Errors(validPass);
                    }

                    if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                    {
                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return RedirectToAction("Index","Librarians");
                        else
                            Errors(result);
                    }
                }
                else
                {
                    AppUser newuser = new AppUser();
                    IdentityResult validUser = null;
                    IdentityResult validPass = null;

                    newuser.Email = email;
                    newuser.UserName = email;
                    newuser.PhoneNumber = phoneNumber;
                    newuser.LibrarianId = librarianId;
                    newuser.Role = "Librarian";

                    if (string.IsNullOrEmpty(email))
                        ModelState.AddModelError("", "Email cannot be empty");

                    validUser = await userValidator.ValidateAsync(userManager, newuser);
                    if (!validUser.Succeeded)
                        Errors(validUser);

                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                        if (validPass.Succeeded)
                            newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                        else
                            Errors(validPass);
                    }
                    else
                        ModelState.AddModelError("", "Password cannot be empty");

                    if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                    {
                        IdentityResult result = await userManager.CreateAsync(newuser, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newuser, "Librarian");
                        //return RedirectToAction(nameof(LibrarianProfile), new { librarianId });
                        return RedirectToAction("Index", "Librarians");
                    }
                        else
                            Errors(result);
                    }
                    user = newuser;
                }
                Librarian librarian = _context.Librarian.Where(s => s.Id == librarianId).FirstOrDefault();
                if (librarian != null)
                {
                    ViewData["FullName"] = librarian.FirstName;
                    ViewData["LibrarianId"] = librarian.Id;
                }
                return View(user);
            }
        [Authorize(Roles = "Admin")]
        [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {

            //AppUser user = await userManager.FindByIdAsync(id);
            //var ID = Int16.Parse(id);
            AppUser u = userManager.Users.Where(a => a.LibrarianId == id).FirstOrDefault();

            if (u!= null)
                {
                    IdentityResult result = await userManager.DeleteAsync(u);
                    if (result.Succeeded)
                    return RedirectToAction("Delete", "Librarians", new { id =id });
                else
                        Errors(result);
                }
                else
                    ModelState.AddModelError("", "User Not Found");
                return View("IndexCustomers", userManager.Users);
            }
        [Authorize(Roles = "Librarian")]
        [HttpPost]
            public async Task<IActionResult> DeleteCustomer(int id)
        {

            //AppUser user = await userManager.FindByIdAsync(id);
            //var ID = Int16.Parse(id);
            AppUser u = userManager.Users.Where(a => a.CustomerId == id).FirstOrDefault();

            if (u != null)
            {
                IdentityResult result = await userManager.DeleteAsync(u);
                if (result.Succeeded)
                    return RedirectToAction("Delete", "Customers", new { id = id });
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
        }
    }

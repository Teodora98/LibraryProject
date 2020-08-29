using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        private readonly LibraryProjectContext _context;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser>
userValid, LibraryProjectContext context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            _context = context;
        }

        [AllowAnonymous]

        public IActionResult Login(string returnUrl)
        {
            LoginViewModel login = new LoginViewModel();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        if ((await userManager.IsInRoleAsync(appUser, "Admin")))
                        {
                            return RedirectToAction("ShowAllBooks", "Books", null);
                        }
                        if ((await userManager.IsInRoleAsync(appUser, "Librarian")))
                        {
                            return RedirectToAction("ShowAllBooks", "Librarian", new { id = appUser.LibrarianId });
                        }
                        if ((await userManager.IsInRoleAsync(appUser, "Customer")))
                        {
                            return RedirectToAction("ShowAllBooks", "Customer", new { id = appUser.CustomerId });
                        }
                    }
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", null);
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            AppUser curruser = await userManager.GetUserAsync(User);
            string userDetails = curruser.UserName;
            string role = "Admin";
            if (curruser.LibrarianId != null)
            {
                Librarian librarian = await _context.Librarian.FindAsync(curruser.LibrarianId);
                userDetails = librarian.FullName + ", " + librarian.Id;
                role = "Librarian";
            }
            else if (curruser.CustomerId != null)
            {
                Customer customer = await _context.Customer.FindAsync(curruser.CustomerId);
                userDetails = customer.FullName+','+customer.CardId;
                role = "Customer";
            }
            UserInfoViewModel userInfoViewModel = new UserInfoViewModel
            {
                UserDetails = userDetails,
                Role = role,
                Id = curruser.Id,
                PasswordHash = curruser.PasswordHash,
                PhoneNumber = curruser.PhoneNumber,
                Email = curruser.Email
            };
            return View(userInfoViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfo(UserInfoViewModel entry)
        {
            AppUser user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(entry.NewPassword))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, entry.NewPassword);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, entry.NewPassword);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(entry.NewPassword) && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(UserInfo));
                    else
                        Errors(result);
                }
            }
            return View(user);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}

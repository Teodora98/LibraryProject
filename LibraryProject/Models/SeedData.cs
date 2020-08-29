using LibraryProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Customer");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Customer"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Librarian");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Librarian"));
            }

            AppUser user = await UserManager.FindByEmailAsync("admin@yahoo.com");
            if (user == null)
            {
                var User = new AppUser();
                User.Email = "admin@yahoo.com";
                User.UserName = "admin@yahoo.com";
                User.Role = "Admin";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryProjectContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<LibraryProjectContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                
                 if (context.Book.Any() || context.Librarian.Any() || context.Customer.Any() || context.CheckOut.Any())
                 {
                     return;   // DB has been seeded
                 }
                context.Book.AddRange(
                    new Book
                    {
                        Title = "Poteklo",
                        Author = "Den Braun",
                        PageNumber = 500,
                        PublicationYear = 2019,
                        PublishingHouse = "Izdavachka kukja Tri",
                        ISBN = 1,
                        Genre="Triler",
                        ShortContent = "Lorem Ipsum as their default model textand a search for 'lorem ipsum' will uncover many web sites still in their infancy.Various versions have evolved over the years, sometimes by accident, sometimes on purpose(injected humour and the like)."
                    },
                     new Book
                     { 
                         Title = "Digitalna Tvrdina",
                         Author = "Den Braun",
                         PageNumber = 300,
                         PublicationYear = 2017,
                         PublishingHouse = "Kultura",
                         ISBN = 2,
                         Genre = "Triler",
                         ShortContent = "Lorem Ipsum as their default model textand a search for 'lorem ipsum' will uncover many web sites still in their infancy.Various versions have evolved over the years, sometimes by accident, sometimes on purpose(injected humour and the like)."
                     });
                context.SaveChanges();

                context.Librarian.AddRange(
                  new Librarian
                  {
                      FirstName = "Marija",
                      LastName = "Stankovska",
                      SSN = 1214,
                      Address = "nas. Karposh br.32,Strumica",
                      BirthDate = DateTime.Parse(("1992-12-1"))
                  });
                context.SaveChanges();

                context.Customer.AddRange(
                    new Customer
                    {
                        CardId = 1,
                        FirstName = "Tamara",
                        LastName = "Arsikj",
                        SSN = 1212,
                        Occupation = "student",
                        Address = "ul.Oktovrsiska br.32,Kumanovo",
                        MembershipDate = DateTime.Parse(("2020-1-1")),
                        BirthDate = DateTime.Parse(("1996-12-1")),
                        LibrarianId = 1
                    },
                     new Customer
                     {
                         CardId = 2,
                         FirstName = "Teodora",
                         LastName = "Cvetkovikj",
                         SSN = 1213,
                         Occupation = "student",
                         Address = "ul.Partizanska br.22,Skopje",
                         MembershipDate = DateTime.Parse(("2019-3-1")),
                         BirthDate = DateTime.Parse(("1997-5-5")),
                         LibrarianId = 1
                     });
                context.SaveChanges();

                context.CheckOut.AddRange(
                    new CheckOut
                    {
                        ChecksOut = DateTime.Parse(("2020-1-9")),
                        BookId = 1,
                        LibrarianId = 1,
                        CustomerId = 1
                    },
                     new CheckOut
                     {
                         ChecksOut = DateTime.Parse(("2020-8-7")),
                         BookId = 2,
                         LibrarianId = 1,
                         CustomerId = 2
                     });
                context.SaveChanges();

            }
        }
    }
}

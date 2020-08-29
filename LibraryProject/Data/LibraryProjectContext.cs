using Microsoft.EntityFrameworkCore;
using LibraryProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryProject.Data
{
    public class LibraryProjectContext : IdentityDbContext<AppUser>
    { 
        public LibraryProjectContext (DbContextOptions<LibraryProjectContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryProject.Models.Book> Book { get; set; }

        public DbSet<LibraryProject.Models.CheckOut> CheckOut { get; set; }

        public DbSet<LibraryProject.Models.Customer> Customer { get; set; }

        public DbSet<LibraryProject.Models.Librarian> Librarian { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CheckOut>()
                .HasOne<Book>(p => p.Book)
                .WithMany(p => p.ChecksOut)
                .HasForeignKey(p => p.BookId);

            builder.Entity<CheckOut>()
                .HasOne<Customer>(p => p.Customer)
                .WithMany(p => p.ChecksOut)
                .HasForeignKey(p => p.CustomerId);

            builder.Entity<CheckOut>()
                .HasOne<Librarian>(p => p.Librarian)
                .WithMany(p => p.TakeOnLease)
                .HasForeignKey(p => p.LibrarianId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Customer>()
                .HasOne<Librarian>(p => p.CreateUserBy)
                .WithMany(p => p.CreateUsers)
                .HasForeignKey(p => p.LibrarianId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

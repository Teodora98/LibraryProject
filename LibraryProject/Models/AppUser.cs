using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Role")]
        public string Role { get; set; }
        public int? CustomerId { get; set; }
        [Display(Name = "Customer")]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int? LibrarianId { get; set; }
        [Display(Name = "Librarian")]
        [ForeignKey("LibrarianId")]
        public Librarian Librarian { get; set; }
    }
}

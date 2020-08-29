using LibraryProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class LibrarianViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public int SSN { get; set; }
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        public ICollection<Customer> CreateUsers { get; set; }
        public ICollection<CheckOut> TakeOnLease { get; set; }
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public IFormFile LibrarianProfileImage { get; set; }
    }
}

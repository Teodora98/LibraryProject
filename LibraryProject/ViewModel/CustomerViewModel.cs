using LibraryProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required]
        public int CardId { get; set; }
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
        public string Occupation { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Mambership Date")]
        public DateTime MembershipDate { get; set; }
        public ICollection<CheckOut> ChecksOut { get; set; }
        public int LibrarianId { get; set; }
        [ForeignKey("LibrarianId")]
        public Librarian CreateUserBy { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public IFormFile CustomerProfileImage { get; set; }
    }
}

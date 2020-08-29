using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class CheckOut
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Cheched Out")]
        public DateTime ChecksOut { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Cheched In")]
        public DateTime? ChecksIn { get; set; }
        [Display(Name = "Book")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [Display(Name = "Book")]
        public Book Book { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [Display(Name = "Customer")]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [Display(Name = "Librarian")]
        public int? LibrarianId { get; set; }
        [Display(Name = "Librarian")]
        [ForeignKey("LibrarianId")]
        public Librarian Librarian { get; set; }
    }
}

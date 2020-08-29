using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Author { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Display(Name = "Page number")]
        public int? PageNumber { get; set; }
        [Required]
        [StringLength(220, MinimumLength = 30)]
        [Display(Name = "Short Content")]
        public string ShortContent { get; set; }
        [Display(Name = "Publication Year")]
        public int? PublicationYear { get; set; }
        [AllowNull]
        [Display(Name = "Publishing House")]
        public string PublishingHouse { get; set; }
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Genre { get; set; }
        public string Status { get; set; }
        public string BookImage { get; set; }
        public ICollection<CheckOut> ChecksOut { get; set; }
    }
}

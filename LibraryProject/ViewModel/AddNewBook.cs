using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class AddNewBook
    {

        public Book book { get; set; }
        [Required]
        public  int ISNB { get; set; }
    }
}

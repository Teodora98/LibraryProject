using LibraryProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class SearchBooks
    {
        public  string sTitle { get; set; }
        public  string sAuthor { get; set; }
        public SelectList Genre { get; set; }
        public string sGenre { get; set; }
        public  List<Book> Books  { get; set; }
    }
}

using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class SearchCustomers
    {
        public string sCardId { get; set; }
        public string sSSN { get; set; }
        public string sFullName { get; set; }

        public IList<Customer> Customer { get; set; }
    }
}

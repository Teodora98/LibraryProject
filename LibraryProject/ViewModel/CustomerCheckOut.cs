using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class CustomerCheckOut
    {
        public List<CheckOut> CheckOut { get; set; }
        public Customer customer { get; set; }
        public int NoMoreBooks { get; set; }
    }
}

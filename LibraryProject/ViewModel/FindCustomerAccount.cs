using LibraryProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel
{
    public class FindCustomerAccount
    {
        public IList<AppUser> user { get; set; }
        public string email { get; set; }
        public SelectList Role { get; set; }
        public string sRole { get; set; }
    }
}

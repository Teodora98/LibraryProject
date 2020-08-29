using System.ComponentModel.DataAnnotations;

namespace LibraryProject.ViewModel
{
    public class UserInfoViewModel
    {
        [Display(Name = "User")]
        public string UserDetails { get; set; }
        [Display(Name = "Role")]
        public string Role { get; set; }
        [Display(Name = "Id User")]
        public string Id { get; set; }
        [Display(Name = "Password Hash")]
        public string PasswordHash { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat new password")]
        [Compare("NewPassword", ErrorMessage = "Лозинките не се совпаѓаат.")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
    }
}

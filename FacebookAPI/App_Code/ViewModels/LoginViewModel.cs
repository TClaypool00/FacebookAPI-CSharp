using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [MaxLength(255, ErrorMessage = "Email address has a max lengh of 255")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid email address")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(255, ErrorMessage = "Password has a max lengh of 255")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

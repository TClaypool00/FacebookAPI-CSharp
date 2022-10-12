using FacebookAPI.App.Models.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App.Models.PostModels
{
    public class RegisterModel : ApiUser
    {
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20, ErrorMessage = "Password has max limit of 20 characters")]
        [MinLength(8, ErrorMessage = "Password has limit of 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [MaxLength(20, ErrorMessage = "Confirm password has max limit of 20 characters")]
        [MinLength(8, ErrorMessage = "Confirm password has limit of 8 characters")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public bool PasswordsAreTheSame()
        {
            return Password == ConfirmPassword;
        }
    }
}

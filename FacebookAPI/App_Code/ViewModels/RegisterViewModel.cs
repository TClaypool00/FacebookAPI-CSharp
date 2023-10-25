using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class RegisterViewModel : UserViewModel
    {
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not maatch")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of birth is required")]        
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Gender selection is required")]
        public int? GenderId { get; set; }
        public List<SelectListItem> GenderDropDown { get; set; }
    }
}

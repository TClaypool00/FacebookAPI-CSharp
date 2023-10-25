using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class UserViewModel : LoginViewModel
    {
        protected readonly CoreUser _coreUser;

        public UserViewModel()
        {

        }

        public UserViewModel(CoreUser coreUser)
        {
            if (coreUser is null)
            {
                throw new ArgumentNullException(nameof(coreUser));
            }

            _coreUser = coreUser;

            UserId = _coreUser.UserId;
            FirstName = _coreUser.FirstName;
            LastName = _coreUser.LastName;
            Email = _coreUser.Email;
            PhoneNumber = _coreUser.PhoneNumber;
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(255, ErrorMessage = "First name has a max lengh of 255")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(255, ErrorMessage = "Last name has a max lengh of 255")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(14, ErrorMessage = "Phone has a max length of 14")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Not a valid phone number format")]
        public string PhoneNumber { get; set; }        
    }
}

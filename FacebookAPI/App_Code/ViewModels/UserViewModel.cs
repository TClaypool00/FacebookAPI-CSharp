using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class UserViewModel : LoginViewModel
    {
        #region Private fields
        protected CoreUser _coreUser;
        #endregion

        #region Constructors
        public UserViewModel()
        {

        }

        public UserViewModel(CoreUser coreUser)
        {
            Construct(coreUser);
        }

        public UserViewModel(CoreUser coreUser, string token)
        {
            Construct(coreUser);

            Token = token;
        }
        #endregion

        #region Public Properties
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

        public bool IsAdmin { get; set; }

        public string Token { get; set; }
        #endregion

        #region Private methods
        private void Construct(CoreUser coreUser)
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
            IsAdmin = _coreUser.IsAdmin;

            Password = "";
        }
        #endregion
    }
}

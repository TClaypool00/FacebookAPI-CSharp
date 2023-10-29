using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public class UserProfileViewModel : UserViewModel
    {
        private readonly bool _isSameUser;

        public UserProfileViewModel()
        {

        }

        public UserProfileViewModel(CoreUser coreUser) : base(coreUser)
        {
            ProfileId = _coreUser.Profile.ProfileId;
            AboutMe = _coreUser.Profile.AboutMe;
            GenderId = _coreUser.Profile.Gender.GenderId;
            ProfileId = _coreUser.Profile.ProfileId;
            MiddleName = _coreUser.Profile.MiddleName;
            _isSameUser = _coreUser.SameUser;

        }

        public int ProfileId { get; set; }

        [Display(Name = "About me")]
        [MaxLength(255, ErrorMessage = "About me has a max length of 255")]
        [Required(ErrorMessage = "About me is required")]
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        public int GenderId { get; set; }

        [Display(Name = "Middle name")]
        [MaxLength(255, ErrorMessage = "Middle name has a max length of 255")]
        public string MiddleName { get; set; }

    }
}

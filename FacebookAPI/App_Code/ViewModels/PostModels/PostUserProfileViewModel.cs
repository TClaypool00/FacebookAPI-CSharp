using FacebookAPI.App_Code.CoreModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostUserProfileViewModel : UserProfileViewModel
    {
        public PostUserProfileViewModel()
        {

        }

        public PostUserProfileViewModel(CoreUser coreUser, CoreFriend coreFriend) : base(coreUser, coreFriend)
        {

        }

        public List<SelectListItem> GenderDropDown { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDateDate { get; set; }


        public void SetProperties(UserProfileViewModel userProfileViewModel)
        {
            if (userProfileViewModel is null)
            {
                throw new ArgumentNullException(nameof(userProfileViewModel));
            }

            UserId = userProfileViewModel.UserId;
            FirstName = userProfileViewModel.FirstName;
            MiddleName = userProfileViewModel.MiddleName;
            LastName = userProfileViewModel.LastName;
            Email = userProfileViewModel.Email;
            PhoneNumber = userProfileViewModel.PhoneNumber;
            BirthDate = userProfileViewModel.BirthDate;
            GenderId = userProfileViewModel.GenderId;
            AboutMe = userProfileViewModel.AboutMe;
        }
    }
}

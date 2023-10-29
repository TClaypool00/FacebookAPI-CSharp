using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostUserProfileViewModel : UserProfileViewModel
    {
        public PostUserProfileViewModel()
        {

        }

        public PostUserProfileViewModel(CoreUser coreUser) : base(coreUser)
        {

        }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDateDate { get; set; }
    }
}

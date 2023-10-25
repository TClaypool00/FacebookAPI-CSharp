using FacebookAPI.App_Code.CoreModels;
using System.ComponentModel.DataAnnotations;
using static FacebookAPI.App_Code.BOL.Friend;

namespace FacebookAPI.App_Code.ViewModels
{
    public class UserProfileViewModel : UserViewModel
    {
        private readonly CoreFriend _friend;

        public UserProfileViewModel()
        {

        }

        public UserProfileViewModel(CoreUser coreUser, CoreFriend friend) : base(coreUser)
        {
            ProfileId = _coreUser.Profile.ProfileId;
            AboutMe = _coreUser.Profile.AboutMe;
            GenderId = _coreUser.Profile.Gender.GenderId;
            GenderName = _coreUser.Profile.Gender.GenderName;
            ProfileId = _coreUser.Profile.ProfileId;
            PicturePath = _coreUser.Picture.PictureFileName;
            CaptionText = _coreUser.Picture.CaptionText;
            SameUser = _coreUser.SameUser;
            BirthDate = _coreUser.Profile.BirthDateString;

            if (friend != null)
            {
                _friend = friend;

                UserFriendType = _friend.UserFriendType;
                FriendType = _friend.FriendTypes;
            }
        }

        public int ProfileId { get; set; }

        [Display(Name = "About me")]
        [MaxLength(255, ErrorMessage = "About me has a max length of 255")]
        [Required(ErrorMessage = "About me is required")]
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        [Display(Name = "Date of birth")]
        public string BirthDate { get; set; }

        public int GenderId { get; set; }

        [Display(Name = "Gender")]
        public string GenderName { get; set; }

        [Display(Name = "Middle name")]
        [MaxLength(255, ErrorMessage = "Middle name has a max length of 255")]
        public string MiddleName { get; set; }

        public int ProfilePictureId { get; set; }

        public string PicturePath { get; set; }

        public string CaptionText { get; set; }

        public bool SameUser { get; }

        public FriendTypes FriendType { get; set; }

        public UserFriendTypes UserFriendType { get; set; }

        public void SetProperties(CoreUser user)
        {
            FirstName = user.FirstName;
            MiddleName = user.Profile.MiddleName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.Profile.BirthDateString;
            GenderId = user.Profile.Gender.GenderId;
            GenderName = user.Profile.Gender.GenderName;
            AboutMe = user.Profile.AboutMe;
        }

        public void SetProperties(CorePicture picture)
        {
            PicturePath = picture.UserPicturePath;
            ProfilePictureId = picture.PictureId;
        }
    }
}

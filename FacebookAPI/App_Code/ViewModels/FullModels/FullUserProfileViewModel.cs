using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels.FullModels
{
    public class FullUserProfileViewModel : PostUserProfileViewModel
    {
        #region Constructors
        public FullUserProfileViewModel()
        {
            
        }

        public FullUserProfileViewModel(CoreUser coreUser) : base(coreUser)
        {
            SetBirthDate();
        }

        public FullUserProfileViewModel(CoreUser coreUser, string message) : base(coreUser)
        {
            SetBirthDate();
            Message = message;

        }
        #endregion

        #region Public Properties
        public string BirthDateString { get; set; }

        public string Message { get; set; }
        #endregion

        #region Private Methods
        private void SetBirthDate()
        {
            BirthDateString = _coreUser.Profile.BirthDateString;
        }
        #endregion
    }
}

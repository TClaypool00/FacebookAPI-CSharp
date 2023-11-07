using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.FullModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacebookAPI.App_Code.ViewModels.DropdownModels
{
    public class UserProfileWithDropDown : FullUserProfileViewModel
    {
        #region Private Fields
        private readonly List<SelectListItem> _genderDropDown;
        #endregion

        #region Constructors
        public UserProfileWithDropDown()
        {
            
        }

        public UserProfileWithDropDown(CoreUser coreUser, List<SelectListItem> genderDropDown) : base(coreUser)
        {
            _genderDropDown = genderDropDown;
        }
        #endregion

        #region Public Properties
        public List<SelectListItem> GenderDropDown
        {
            get
            {
                return _genderDropDown;
            }
        }
        #endregion
    }
}

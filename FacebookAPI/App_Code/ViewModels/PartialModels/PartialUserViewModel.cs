using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.ViewModels.PartialModels
{
    public class PartialUserViewModel
    {
        private readonly CoreUser _coreUser;

        public PartialUserViewModel(CoreUser coreUser)
        {
            if (coreUser is null)
            {
                throw new ArgumentNullException(nameof(coreUser));
            }

            _coreUser = coreUser;

            UserId = _coreUser.UserId;
            FirstName = _coreUser.FirstName;
            LastName = _coreUser.LastName;
            PicturePath = _coreUser.Picture.FullPath;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PicturePath { get; set; }
    }
}

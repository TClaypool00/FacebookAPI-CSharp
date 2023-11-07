using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IUserService
    {
        #region Public Methods
        public Task CreateUserAsync(CoreUser user);

        public Task<bool> PhoneNumberExistsAsync(string phoneNumber, int? id = null);

        public Task<bool> EmailExistsAsync(string email, int? id = null);

        public Task<bool> UserExistsAsync(int id);

        public Task UpdatePasswordAsync(int id, string newPassword);

        public Task<CoreUser> GetUserAsync(string email);

        public Task<CoreUser> GetUserAsync(int id, bool includePassword = false);

        public Task<CoreUser> GetFullNameAsync(int id);

        public Task<List<CoreUser>> GetFriendsAsync(string search, int userId);
        #endregion


        #region Public Properties
        public string UserCreatedMessage { get; }

        public string PhoneNumberExistsMessage { get; }

        public string EmailExistsMessage { get; }

        public string UserDoesNotExistsMessage { get; }

        public string EmailDoesNotExistMessage { get; }

        public string UpdatePasswordSuccessMessage { get; }
        #endregion
    }
}

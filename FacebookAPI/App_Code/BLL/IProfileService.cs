using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IProfileService
    {
        public Task<CoreUser> GetUserProfileAsync(int id, int userId);

        public Task<CoreUser> UpdateUserAsync(int id, CoreUser user);

        public string UpdateProfileOKMessage { get; }
    }
}

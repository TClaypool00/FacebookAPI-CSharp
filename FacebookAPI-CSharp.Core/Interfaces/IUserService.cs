using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Interfaces
{
    public interface IUserService
    {
        public Task<List<CoreUser>> GetUsersAsync();

        public Task<CoreUser> GetUserAsync(int id, bool includePassword = false);

        public Task<bool> AddUserAsync(CoreUser user);

        public Task<bool> UpdateUserAsync(int id, CoreUser user);

        public Task<bool> UserExistsAsync(int id);

        public Task<bool> UserExistsByEmailAsync(string email);

        public Task<bool> UserExistsByPhonNumAsync(string phonNum);

        public bool PasswordMeetsRequirements(string password);

        public Task<bool> DeleteUserAsync(int id);
    }
}

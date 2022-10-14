using FacebookAPI.Core.Interfaces;
using FacebookAPI.Core.Models.CoreModels;
using FacebookAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FacebookAPI.DataAccess.Services
{
    public class UserService : IUserService, IGeneralService
    {
        private readonly FacebookDBContext _context;

        public UserService(FacebookDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(CoreUser user)
        {
            try
            {
                user.IsAdmin = false;
                await _context.Users.AddAsync(Mapper.MapUser(user));
                await SaveAsync();

                return true;
            } catch(Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await GetDataUser(id);
                _context.Users.Remove(user);

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<CoreUser> GetUserAsync(int id, bool includePassword = false)
        {
            return Mapper.MapUser(await GetDataUser(id), includePassword);
        }

        public async Task<List<CoreUser>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            User user;
            var coreUsers = new List<CoreUser>();

            for (int i = 0; i < users.Count; i++)
            {
                user = users[i];
                coreUsers.Add(Mapper.MapUser(user, false));
            }

            return coreUsers;
        }

        public bool PasswordMeetsRequirements(string password)
        {
            var checker = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$");

            return checker.IsMatch(password);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserAsync(int id, CoreUser user)
        {
            try
            {
                var dataUser = await GetDataUser(id);
                user.Password = dataUser.Password;

                _context.Entry(dataUser).CurrentValues.SetValues(Mapper.MapUser(user));

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> UserExistsAsync(int id)
        {
            return _context.Users.AnyAsync(u => u.UserId == id);
        }

        public Task<bool> UserExistsByPhonNumAsync(string phoneNum)
        {
            return _context.Users.AnyAsync(u => u.PhoneNum == phoneNum);
        }

        public Task<bool> UserExistsByEmailAsync(string email)
        {
            return _context.Users.AnyAsync(u => u.Email == email);
        }

        private async Task<User> GetDataUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }
    }
}

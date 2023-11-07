using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class UserService : ServiceHelper, IUserService
    {
        #region Constructors
        public UserService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:User"];
        }
        #endregion

        #region Public Properties
        public string UserCreatedMessage => "Account has been created";

        public string PhoneNumberExistsMessage => "Phone number already exists";

        public string EmailExistsMessage => "Email address already exists";

        public string UserDoesNotExistsMessage => "User does not exists";

        public string EmailDoesNotExistMessage => "No user exists with the email address entered.";

        public string UpdatePasswordSuccessMessage => "Password has been updated";
        #endregion

        #region Public Methods
        public async Task CreateUserAsync(CoreUser user)
        {
            try
            {
                var dataProfile = new Profile(user.Profile);
                await _context.Profiles.AddAsync(dataProfile);
                await SaveAsync();

                if (dataProfile.ProfileId == 0)
                {
                    throw new ApplicationException($"{_couldNotAddedMessage} {_tableName}");
                }

                user.ProfileId = dataProfile.ProfileId;

                try
                {
                    var dataUser = new User(user);

                    await _context.Users.AddAsync(dataUser);

                    await SaveAsync();
                }
                catch (Exception)
                {
                    _context.Profiles.Remove(dataProfile);
                    await SaveAsync();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> EmailExistsAsync(string email, int? id = null)
        {
            if (id is null)
            {
                return _context.Users.AnyAsync(u => u.Email == email);
            }

            return _context.Users.AnyAsync(u => u.Email == email && u.UserId != id);
        }

        public async Task<List<CoreUser>> GetFriendsAsync(string search, int userId)
        {
            var coreUsers = new List<CoreUser>();

            //TODO: Make Picture query more effectient. Only need the file name
            var users = await _context.Users
                .Where(a => a.UserId != userId && (a.FirstName.ToLower().Contains(search.ToLower()) || a.LastName.ToLower().Contains(search.ToLower())))
                .Select(u => new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePicture = u.Pictures.FirstOrDefault(p => p.UserId == u.UserId)
                })
                .Take(_longerTakeValue)
                .ToListAsync();

            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    coreUsers.Add(new CoreUser(users[i], _configuration));
                }
            }

            return coreUsers;
        }

        public async Task<CoreUser> GetUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return new CoreUser(user);
        }

        public async Task<CoreUser> GetUserAsync(int id, bool includePassword = false)
        {
            User user;

            if (includePassword)
            {
                user = await FindFullUser(id);
            }
            else
            {
                user = await FindUserAsync(id);
            }

            DetachEntity(user);

            return new CoreUser(user);
        }

        public Task<bool> PhoneNumberExistsAsync(string phoneNumber, int? id = null)
        {
            if (id is null)
            {
                return _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
            }

            return _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && u.UserId != id);
        }

        public async Task UpdatePasswordAsync(int id, string newPassword)
        {
            try
            {
                var dataUser = await FindFullUser(id);

                dataUser.Password = newPassword;
                _context.Users.Update(dataUser);

                await SaveAsync();

                DetachEntity(dataUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> UserExistsAsync(int id)
        {
            return _context.Users.AnyAsync(u => u.UserId == id);
        }

        public async Task<CoreUser> GetFullNameAsync(int id)
        {
            var dataUser = await _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).FirstOrDefaultAsync(a => a.UserId == id);

            return new CoreUser(dataUser);
        }
        #endregion

        #region Private Methods
        private Task<User> FindUserAsync(int id, bool includePicture = false)
        {
            if (!includePicture)
            {
                return _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    PhoneNumber = u.PhoneNumber,
                    ProfileId = u.ProfileId
                })
                .FirstOrDefaultAsync(a => a.UserId == id);
            }
            else
            {
                return _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    PhoneNumber = u.PhoneNumber,
                    ProfileId = u.ProfileId,
                    ProfilePicture = u.Pictures.FirstOrDefault(p => p.UserId == id)
                })
                .FirstOrDefaultAsync(a => a.UserId == id);
            }
        }

        private Task<User> FindFullUser(int id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }
        #endregion
    }
}

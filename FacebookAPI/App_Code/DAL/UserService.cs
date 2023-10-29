using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class UserService : ServiceHelper, IUserService
    {
        private readonly string _errorMessage;

        public UserService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {
            _errorMessage = "Could not add a user";
        }

        public string UserCreatedMessage => "Account has been created";

        public string PhoneNumberExistsMessage => "Phone number already exists";

        public string EmailExistsMessage => "Email address already exists";

        public string UserDoesNotExistsMessage => "User does not exists";

        public string EmailDoesNotExistMessage => "No user exists with the email address entered.";

        public string UserUpdatedMessage => "Profile has been updated";

        public string UpdatePasswordSuccessMessage => "Password has been updated";

        public async Task CreateUserAsync(CoreUser user)
        {
            try
            {
                var dataProfile = new Profile(user.Profile);
                await _context.Profiles.AddAsync(dataProfile);
                await SaveAsync();

                if (dataProfile.ProfileId == 0)
                {
                    throw new ApplicationException(_errorMessage);
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
                .Where(a => a.UserId != userId  && (a.FirstName.ToLower().Contains(search.ToLower()) || a.LastName.ToLower().Contains(search.ToLower())))
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

        public async Task<CoreUser> GetUserProfileAsync(int id, int userId)
        {
            var dataUser = await FindUserAsync(id, true);         

            dataUser.Profile = await _context.Profiles
                .Select(p => new Profile
                {
                    ProfileId = p.ProfileId,
                    AboutMe = p.AboutMe,
                    BirthDate = p.BirthDate,
                    Gender = new Gender
                    {
                        GenderId = p.Gender.GenderId,
                        GenderName = p.Gender.GenderName
                    }
                })
                .FirstOrDefaultAsync(a => a.ProfileId == dataUser.ProfileId);


            var coreUser = new CoreUser(dataUser, userId, _configuration);

            return coreUser;
        }

        public Task<bool> PhoneNumberExistsAsync(string phoneNumber, int? id = null)
        {
            if (id is null)
            {
                return _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
            }

            return _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && u.UserId != id);
        }

        public async Task<bool> UpdatePassword(CoreUser user)
        {
            try
            {
                var dataUser = new User(user);
                _context.Users.Update(dataUser);

                await SaveAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CoreUser> UpdateUserAsync(int id, CoreUser user)
        {
            try
            {
                var dataUser = await FindUserAndProfile(id);
                dataUser.FirstName = user.FirstName;
                dataUser.LastName = user.LastName;
                dataUser.Email = user.Email;
                dataUser.PhoneNumber = user.PhoneNumber;


                _context.Users.Update(dataUser);
                await SaveAsync();

                try
                {
                    dataUser.Profile.AboutMe = user.Profile.AboutMe;
                    dataUser.Profile.BirthDate = user.Profile.BirthDate;
                    dataUser.Profile.MiddleName = user.Profile.MiddleName;

                    if (dataUser.Profile.GenderId != user.Profile.GenderId)
                    {
                        dataUser.Profile.GenderId = user.Profile.GenderId;

                        dataUser.Profile.Gender = await _context.Genders.FirstOrDefaultAsync(x => x.GenderId == user.Profile.GenderId);
                    }

                    _context.Profiles.Update(dataUser.Profile);
                    await SaveAsync();

                    user.Profile.Gender = new CoreGender(dataUser.Profile.Gender);

                    return user;
                }
                catch (Exception)
                {
                    throw;
                }
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

        private Task<User> FindUserAndProfile(int id)
        {
            return _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = u.Password,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    PhoneNumber = u.PhoneNumber,
                    Profile = new Profile
                    {
                        AboutMe = u.Profile.AboutMe,
                        BirthDate = u.Profile.BirthDate,
                        MiddleName = u.Profile.MiddleName,
                        ProfileId = u.ProfileId,
                        Gender = new Gender
                        {
                            GenderId = u.Profile.Gender.GenderId,
                            GenderName = u.Profile.Gender.GenderName
                        }
                    }

                })
                .FirstOrDefaultAsync (a => a.UserId == id);
        }
    }
}

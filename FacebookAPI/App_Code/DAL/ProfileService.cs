using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class ProfileService : ServiceHelper, IProfileService
    {
        #region Constructors
        public ProfileService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Profile"];
        }
        #endregion

        #region Public Properties
        public string UpdateProfileOKMessage => $"{_tableName} {_updatedOKMessage}";
        #endregion

        #region Public Methods
        public async Task<CoreUser> GetUserProfileAsync(int id, int userId)
        {
            var dataUser = await FindUserAndProfile(id);


            var coreUser = new CoreUser(dataUser, userId, _configuration);

            return coreUser;
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

                    DetachEntity(dataUser);

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
        #endregion

        #region Private methods
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
                .FirstOrDefaultAsync(a => a.UserId == id);
        }
        #endregion
    }
}

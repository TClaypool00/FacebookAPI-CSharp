using FacebookAPI.Core.CoreModels;
using FacebookAPI.DataAccess.Models;

namespace FacebookAPI.DataAccess
{
    public class Mapper
    {
        public static CoreUser MapUser(User user, bool includePassword)
        {
            var coreUser =  new CoreUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum
            };

            coreUser.Password = includePassword ? user.Password : "";

            return coreUser;
        }

        public static User MapUser(CoreUser user, int? userId = null)
        {
            var dataUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum,
                Password = user.Password
            };

            if (userId is not null)
            {
                dataUser.UserId = (int)userId;
            }

            return dataUser;
        }
    }
}

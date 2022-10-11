using FacebookAPI.App.ApiModels;
using FacebookAPI.App.PostModels;
using FacebookAPI_CSharp.Core.CoreModels;

namespace FacebookAPI.App
{
    public class ApiMapper
    {
        public static CoreUser MapUser(ApiUser user)
        {
            return new CoreUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum,
                Password = user.Password
            };
        }

        public static CoreUser MapUser(PostUser user)
        {
            return new CoreUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNum = user.PhoneNum,
                Password=user.Password
            };
        }

        public static ApiUser MapUser(CoreUser user)
        {
            return new ApiUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum,
                Password = user.Password
            };
        }
    }
}

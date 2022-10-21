using FacebookAPI.App.Models.ApiModels;
using FacebookAPI.App.Models.PostModels;
using FacebookAPI.Core.Interfaces;
using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.App
{
    public class ApiMapper
    {
        public static async Task<CoreFriend> MapFriend(ApiPostFriend friend, IUserService userService)
        {
            return new CoreFriend
            {
                Sender = await userService.GetUserAsync(friend.SenderId),
                Receiver = await userService.GetUserAsync(friend.ReceiverId)
            };
        }

        public static ApiFriend MapFriend(CoreFriend friend, int userId)
        {
            var apiFriend = new ApiFriend
            {
                DateAccepted = friend.DateAccepted,
                IsAccepted = friend.IsAccepted
            };

            if (userId != friend.Sender.UserId)
            {
                apiFriend.UserId = friend.Sender.UserId;
                apiFriend.FirstName = friend.Sender.FirstName;
                apiFriend.LastName = friend.Sender.LastName;
            } else
            {
                apiFriend.UserId = friend.Receiver.UserId;
                apiFriend.FirstName = friend.Receiver.FirstName;
                apiFriend.LastName = friend.Receiver.LastName;
            }

            return apiFriend;
        }

        public static async Task<CorePost> MapPost(ApiPostModel post, IUserService userService)
        {
            return new CorePost
            {
                PostId = post.PostId,
                Body = post.Body,
                DatePosted = post.DatePosted,
                User = await userService.GetUserAsync(post.UserId)
            };
        }

        public static async Task<CorePost> MapPost(ApiPostTextModel model, IUserService userService)
        {
            return new CorePost
            {
                Body = model.Body,
                User = await userService.GetUserAsync(model.UserId)
            };
        }

        public static ApiPostModel MapPost(CorePost post)
        {
            return new ApiPostModel
            {
                PostId = post.PostId,
                Body = post.Body,
                DatePosted = post.DatePosted,
                UserId = post.User.UserId,
                FirstName = post.User.FirstName,
                LastName = post.User.LastName
            };
        }

        public static CoreUser MapUser(ApiUser user)
        {
            return new CoreUser
            {
                UserId = (int)user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum
            };
        }

        public static CoreUser MapUser(RegisterModel user)
        {
            return new CoreUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNum = user.PhoneNum,
                Password = user.Password
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
                PhoneNum = user.PhoneNum
            };
        }
    }
}

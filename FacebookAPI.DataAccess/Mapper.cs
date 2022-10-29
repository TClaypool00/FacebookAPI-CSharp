using FacebookAPI.Core.Models.CoreModels;
using FacebookAPI.DataAccess.Models;

namespace FacebookAPI.DataAccess
{
    public class Mapper
    {
        public static CoreFriend MapFriend(Friend friend)
        {
            return new CoreFriend
            {
                Sender = MapUser(friend.Sender),
                Receiver = MapUser(friend.Receiver),
                DateAccepted = friend.DateAccepted,
                IsAccepted = friend.IsAccepted
            };
        }

        public static CoreFriend MapFriend(Friend friend, CoreUser user)
        {
            return new CoreFriend
            {
                Sender = user.UserId == friend.SenderId ? user : MapUser(friend.Sender),
                Receiver = user.UserId == friend.ReceiverId ? user : MapUser(friend.Receiver),
                DateAccepted = friend.DateAccepted,
                IsAccepted = friend.IsAccepted
            };
        }

        public static Friend MapFriend(CoreFriend frined)
        {
            return new Friend
            {
                SenderId = frined.Sender.UserId,
                ReceiverId = frined.Receiver.UserId,
                DateAccepted = frined.DateAccepted,
                IsAccepted = frined.IsAccepted
            };
        }

        public static CoreParentType MapParentType(ParentType parentType)
        {
            return new CoreParentType
            {
                ParenTypeId = parentType.ParentTypeId,
                Name = parentType.Name
            };
        }

        public static ParentType MapParentType(CoreParentType parentType)
        {
            var dataParentType = new ParentType
            {
                Name = parentType.Name
            };

            if (parentType.ParenTypeId != 0)
            {
                dataParentType.ParentTypeId = parentType.ParenTypeId;
            }

            return dataParentType;
        }

        public static CorePost MapPost(Post post)
        {
            return new CorePost
            {
                PostId = post.PostId,
                Body = post.Body,
                DatePosted = post.DatePosted,
                User = MapUser(post.User)
            };
        }

        public static Post MapPost(CorePost post)
        {
            var dataPost = new Post
            {
                Body = post.Body,
                DatePosted = post.DatePosted,
                UserId = post.User.UserId,
            };

            if (post.PostId != 0)
            {
                dataPost.PostId = post.PostId;
            }

            return dataPost;
        }

        public static CoreUser MapUser(User user, bool includePassword = false)
        {
            return new CoreUser
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                PhoneNum = user.PhoneNum,
                Password = includePassword ? user.Password : ""
            };
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

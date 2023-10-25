using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class FriendService : ServiceHelper, IFriendService
    {
        public FriendService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {

        }

        public string FriendAddedMessage => "Friend Request sent.";

        public string FriendCouldNotBeAddedMessage => "Friend Request could not be sent.";

        public string FriendDeletedMessage => "Friend Request removed.";

        public string FriendRequestAcceptedMessage => "Friend Request has been accepted.";

        public async Task<CoreFriend> AcceptFriendAsync(CoreFriend friend)
        {
            var dataFriend = new Friend(friend)
            {
                DateAccepted = DateTime.Now
            };

            _context.Friends.Update(dataFriend);

            await SaveAsync();

            friend.DateAccepted = dataFriend.DateAccepted;

            return friend;
        }

        public async Task<CoreFriend> CreateFriendAsync(int senderId, int receiverId)
        {
            try
            {
                var dataFriend = new Friend(senderId, receiverId);

                await _context.Friends.AddAsync(dataFriend);

                await SaveAsync();

                return new CoreFriend(dataFriend);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFriendAsync(CoreFriend friend)
        {
            try
            {
                var dataFriend = new Friend(friend);

                _context.Friends.Remove(dataFriend);

                await SaveAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CoreFriend> GetFriendAsync(int senderId, int receiverId, int userId)
        {
            var friend = await FindFriendAsync(senderId, receiverId);

            return new CoreFriend(friend, userId);
        }

        private Task<Friend> FindFriendAsync(int senderId, int receiverId)
        {
            return _context.Friends.FirstOrDefaultAsync(f => (f.SenderId == senderId && f.ReceiverId == receiverId) || (f.ReceiverId == senderId && f.SenderId == receiverId));
        }
    }
}

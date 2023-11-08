using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class FriendService : ServiceHelper, IFriendService
    {
        #region Constructors
        public FriendService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {

        }
        #endregion

        #region Public Properties
        public string FriendAddedMessage => _configuration["Friend:AddedMessage"];

        public string FriendDeletedMessage => _configuration["Friend:RemovedMessage"];

        public string FriendRequestAcceptedMessage => _configuration["Friend:AcceptedMessage"];

        public string FriendRequestExistsMessage => _configuration["Friend:ExistsMessage"];

        public string FriendRequestDoesNotExistsMessage => _configuration["Friend:DoesNotExistsMessage"];
        #endregion

        #region Public Methods
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

        public async Task CreateFriendAsync(int senderId, int receiverId)
        {
            try
            {
                var dataFriend = new Friend(senderId, receiverId);

                await _context.Friends.AddAsync(dataFriend);

                await SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFriendAsync(CoreFriend friend)
        {
            try
            {
                var dataFriend = new Friend(friend);

                _context.Friends.Remove(dataFriend);

                await SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> FriendExistsAsync(int senderId, int receiverId)
        {
            return _context.Friends.AnyAsync(f => (f.ReceiverId == receiverId && f.SenderId == senderId) || (f.ReceiverId == senderId && f.SenderId == receiverId));
        }
        #endregion
    }
}

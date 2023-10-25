using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IFriendService
    {
        public Task<CoreFriend> CreateFriendAsync(int senderId, int receiverId);

        public Task<bool> DeleteFriendAsync(CoreFriend friend);

        public Task<CoreFriend> AcceptFriendAsync(CoreFriend friend);

        public Task<CoreFriend> GetFriendAsync(int senderId, int receiverId, int userId);

        public string FriendAddedMessage { get; }

        public string FriendCouldNotBeAddedMessage { get; }

        public string FriendDeletedMessage { get; }

        public string FriendRequestAcceptedMessage { get; }
    }
}

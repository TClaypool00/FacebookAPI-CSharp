using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IFriendService
    {
        #region Public Methods
        public Task<CoreFriend> CreateFriendAsync(int senderId, int receiverId);

        public Task DeleteFriendAsync(CoreFriend friend);

        public Task<CoreFriend> AcceptFriendAsync(CoreFriend friend);

        public Task<bool> FriendExistsAsync(int  senderId, int receiverId);
        #endregion

        #region Public Properties
        public string FriendAddedMessage { get; }

        public string FriendDeletedMessage { get; }

        public string FriendRequestAcceptedMessage { get; }

        public string FriendRequestExistsMessage { get; }

        public string FriendRequestDoesNotExistsMessage { get; }
        #endregion
    }
}

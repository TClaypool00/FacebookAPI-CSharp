using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IFriendService
    {
        #region Public Methods
        public Task CreateFriendAsync(int senderId, int receiverId);

        public Task DeleteFriendAsync(int senderId, int receiverId);

        public Task AcceptFriendAsync(int senderId, int receiverId);

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

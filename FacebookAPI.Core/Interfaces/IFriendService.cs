using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Interfaces
{
    public interface IFriendService
    {
        Task<List<CoreFriend>> GetFriendsAsync(int userId, bool? isAccepted, CoreUser user);
        Task AcceptFriendAsync(int senderId, int receiverId);
        Task AddFriendAsync(CoreFriend friend);
        Task<bool> FriendExists(int senderId, int receiverId);
        Task DeleteFriendAsync(int senderId, int receiverId);
    }
}

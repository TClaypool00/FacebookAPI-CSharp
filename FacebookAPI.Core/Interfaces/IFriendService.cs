using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Interfaces
{
    public interface IFriendService
    {
        Task<List<CoreFriend>> GetFriendsAsync(int userId, CoreUser user);
        Task<CoreFriend> GetFrinedAsync(int senderId, int receiverId);
        Task AcceptFriendAsync(CoreFriend friend);
        Task AddFriendAsync(CoreFriend friend);
        Task<bool> FriendExists(int senderId, int receiverId);
        Task DeleteFriendAsync(int senderId, int receiverId);
    }
}

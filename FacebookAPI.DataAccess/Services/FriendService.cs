using FacebookAPI.Core.Interfaces;
using FacebookAPI.Core.Models.CoreModels;
using FacebookAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.DataAccess.Services
{
    public class FriendService : IFriendService, IGeneralService
    {
        private readonly FacebookDBContext _context;

        public FriendService(FacebookDBContext context)
        {
            _context = context;
        }

        public async Task AddFriendAsync(CoreFriend friend)
        {
            await _context.Friends.AddAsync(Mapper.MapFriend(friend));
            await SaveAsync();
        }

        public async Task DeleteFriendAsync(int senderId, int receiverId)
        {
            _context.Friends.Remove(await GetDataFriendAsync(senderId, receiverId));
            await SaveAsync();
        }

        public async Task<bool> FriendExists(int senderId, int receiverId)
        {
            return await _context.Friends.AnyAsync(f => (f.ReceiverId == receiverId && f.SendId == senderId) || (f.ReceiverId == senderId && f.SendId == receiverId));
        }

        public async Task<CoreFriend> GetFrinedAsync(int senderId, int receiverId)
        {
            return Mapper.MapFriend(await GetDataFriendAsync(senderId, receiverId));
        }

        public async Task<List<CoreFriend>> GetFriendsAsync(int userId, CoreUser user)
        {
            var friends = await _context.Friends
                .Include(s => s.Sender)
                .Include(s => s.Receiver)
                .Where(f => f.SendId == userId || f.ReceiverId == userId)
                .ToListAsync();

            var coreFriends = new List<CoreFriend>();
            Friend frined;

            for (int i = 0; i < friends.Count; i++)
            {
                frined = friends[i];
                coreFriends.Add(Mapper.MapFriend(frined, user));
            }

            return coreFriends;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task<Friend> GetDataFriendAsync(int senderId, int receiverId, bool includeUserData = true)
        {
            if (includeUserData)
            {
                return await _context.Friends
                    .Include(s => s.Sender)
                    .Include(s => s.Receiver)
                    .FirstOrDefaultAsync(f => (f.ReceiverId == receiverId && f.SendId == senderId) || (f.ReceiverId == senderId && f.SendId == receiverId));
            } else
            {
                return await _context.Friends.FirstOrDefaultAsync(f => (f.ReceiverId == receiverId && f.SendId == senderId) || (f.ReceiverId == senderId && f.SendId == receiverId));
            }
        }

        public async Task AcceptFriendAsync(CoreFriend friend)
        {
            var dataFriend = Mapper.MapFriend(friend);
            var oldFriend = await GetDataFriendAsync(friend.SendId, friend.ReceiverId);
            dataFriend.IsAccepted = true;
            dataFriend.DateAccepted = DateTime.Now;

            _context.Entry(oldFriend).CurrentValues.SetValues(dataFriend);

            await SaveAsync();
        }
    }
}

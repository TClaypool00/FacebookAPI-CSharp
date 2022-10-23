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
            return await _context.Friends.AnyAsync(f => (f.ReceiverId == receiverId && f.SenderId == senderId) || (f.ReceiverId == senderId && f.SenderId == receiverId));
        }

        public async Task<List<CoreFriend>> GetFriendsAsync(int userId, bool? isAccepted, CoreUser user)
        {
            var friends = await _context.Friends
                .Include(s => s.Sender)
                .Include(s => s.Receiver)
                .Where(f => f.SenderId == userId || f.ReceiverId == userId)
                .ToListAsync();

            if (isAccepted is not null)
            {
                friends = friends.Where(f => f.IsAccepted = (bool)isAccepted).ToList();
            }

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
                    .FirstOrDefaultAsync(f => (f.ReceiverId == receiverId && f.SenderId == senderId) || (f.ReceiverId == senderId && f.SenderId == receiverId));
            } else
            {
                return await _context.Friends.FirstOrDefaultAsync(f => (f.ReceiverId == receiverId && f.SenderId == senderId) || (f.ReceiverId == senderId && f.SenderId == receiverId));
            }
        }

        public async Task AcceptFriendAsync(int senderId, int receiverId)
        {            
            var oldFriend = await GetDataFriendAsync(senderId, receiverId, false);
            var dataFriend = oldFriend;
            dataFriend.IsAccepted = true;
            dataFriend.DateAccepted = DateTime.Now;

            _context.Entry(oldFriend).CurrentValues.SetValues(dataFriend);

            await SaveAsync();
        }
    }
}

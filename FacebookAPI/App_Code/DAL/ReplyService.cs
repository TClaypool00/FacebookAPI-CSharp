using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class ReplyService : ServiceHelper, IReplyService
    {
        #region Constructors
        public ReplyService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Reply"];
        }
        #endregion

        #region Publci Properties
        public string ReplyAddedOKMessage => $"{_tableName} {_addedOKMessage}";

        public string ReplyUpdatedOKMessage => $"{_tableName} {_updatedOKMessage}";

        public string ReplyNotFoundMessage => $"{_tableName} {_doesNotExistMessage}";
        #endregion

        #region Public Methods
        public async Task<CoreReply> AddReplyAsync(CoreReply reply)
        {
            var dataReply = new Reply(reply);

            await _context.Replies.AddAsync(dataReply);
            await _context.SaveChangesAsync();

            if (dataReply.ReplyId == 0)
            {
                throw new ApplicationException($"{_tableName} {_couldNotAddedMessage}");
            }

            reply.ReplyId = dataReply.ReplyId;
            reply.DatePosted = dataReply.DatePosted;
            reply.DateUpdated = dataReply.DateUpdated;

            return reply;
        }

        public async Task<CoreReply> GetReplyAsync(int id)
        {
            var reply = await _context.Replies
                .Select(r => new Reply
                {
                    ReplyId = id,
                    ReplyBody  = r.ReplyBody,
                    DatePosted = r.DatePosted,
                    DateUpdated = r.DateUpdated,
                    CommentId = r.CommentId,
                    User = new User
                    {
                        UserId = r.User.UserId,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                    }
                }).FirstOrDefaultAsync(a => a.ReplyId == id);

            return new CoreReply(reply);
        }

        public Task<bool> ReplyExistsAsync(int id)
        {
            return _context.Replies.AnyAsync(r => r.ReplyId == id);
        }

        public async Task<CoreReply> UpdateReplyAsync(CoreReply reply)
        {
            try
            {
                var dataReply = await FindReplyByIdAsync(reply.ReplyId);
                dataReply.ReplyBody = reply.ReplyBody;
                dataReply.UserId = reply.UserId;

                _context.Replies.Update(dataReply);
                await SaveAsync();

                reply.DateUpdated = dataReply.DateUpdated;

                DetachEntity(dataReply);

                return reply;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> UserHasAccessToReplyAsync(int id, int userId)
        {
            return _context.Replies.AnyAsync(r => r.ReplyId == id && r.UserId == userId);
        }
        #endregion

        #region Private methods
        private Task<Reply> FindReplyByIdAsync(int id)
        {
            return _context.Replies.FirstOrDefaultAsync(r => r.ReplyId == id);
        }
        #endregion
    }
}

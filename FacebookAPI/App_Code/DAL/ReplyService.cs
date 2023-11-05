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

        public string NoRepliesFoundMessage => _configuration["NotFoundMessages:Replies"];

        public string ReplyDeletedOKMessage => $"{_tableName} {_deletedMessage}";
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

        public async Task DeleteReplyByIdAsync(int id)
        {
            var dataReply = await FindReplyByIdAsync(id);

            dataReply.Likes = await _context.Likes.Where(x => x.ReplyId == id).ToListAsync();

            if (dataReply.Likes.Count > 0)
            {
                _context.Likes.RemoveRange(dataReply.Likes);
                await SaveAsync();
            }

            _context.Replies.Remove(dataReply);
            await SaveAsync();
        }

        public async Task<List<CoreReply>> GetRepliesAsync(int? index = null, int? userId = null, int? commentId = null, int? postId = null)
        {
            ConfigureIndex(index);
            List<Reply> replies = null;
            var coreReplies = new List<CoreReply>();

            if (userId is not null)
            {
                replies = await _context.Replies
                    .Where(a => a.UserId == userId)
                    .Select(r => new Reply
                    {
                        ReplyId = r.ReplyId,
                        ReplyBody = r.ReplyBody,
                        DatePosted = r.DatePosted,
                        DateUpdated = r.DateUpdated,
                        CommentId = r.CommentId,
                        User = new User
                        {
                            UserId = r.User.UserId,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName
                        }
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();
            }

            if (commentId is not null)
            {
                if (replies is null)
                {
                    replies = await _context.Replies
                    .Where(a => a.CommentId == commentId)
                    .Select(r => new Reply
                    {
                        ReplyId = r.ReplyId,
                        ReplyBody = r.ReplyBody,
                        DatePosted = r.DatePosted,
                        DateUpdated = r.DateUpdated,
                        CommentId = r.CommentId,
                        User = new User
                        {
                            UserId = r.User.UserId,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName
                        }
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();
                }
                else
                {
                    replies = replies.Where(u => u.CommentId == commentId).ToList();
                }
            }

            if (postId is not null)
            {
                var commentIds = await _context.Comments
                    .Where(a => a.PostId == postId)
                    .Select(c => c.CommentId)
                    .ToListAsync();

                if (replies is null)
                {
                    replies = await _context.Replies
                    .Where(a => commentIds.Contains(a.CommentId))
                    .Select(r => new Reply
                    {
                        ReplyId = r.ReplyId,
                        ReplyBody = r.ReplyBody,
                        DatePosted = r.DatePosted,
                        DateUpdated = r.DateUpdated,
                        CommentId = r.CommentId,
                        User = new User
                        {
                            UserId = r.User.UserId,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName
                        }
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();
                }
                else
                {
                    replies = replies.Where(r => commentIds.Contains(r.CommentId)).ToList();
                }
            }

            if (replies is not null && replies.Count > 0)
            {
                for (int i = 0; i < replies.Count; i++)
                {
                    coreReplies.Add(new CoreReply(replies[i]));
                }
            }

            return coreReplies;
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

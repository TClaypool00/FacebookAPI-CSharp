using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class CommentService : ServiceHelper, ICommentService
    {
        public CommentService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Comment"];
        }

        public string CommentAddedOKMessag => $"{_tableName} {_addedOKMessage}";

        public string CommentUpdatedOKMessage => $"{_tableName} {_updatedOKMessage}";

        public string CommentNotFoundMessage => $"{_tableName} {_doesNotExistMessage}";

        public string NoCommentsFound => _configuration["NotFoundMessages:Comments"];

        public string CommentDeletedOKMessage => $"{_tableName} {_deletedMessage}";

        public async Task<CoreComment> AddCommentAsync(CoreComment comment)
        {
            var dataComment = new Comment(comment);

            await _context.Comments.AddAsync(dataComment);
            await SaveAsync();

            if (dataComment.CommentId == 0)
            {
                throw new ApplicationException($"{_tableName} {_couldNotAddedMessage}");
            }

            comment.CommentId = dataComment.CommentId;
            comment.DatePosted = dataComment.DatePosted;

            return comment;
        }

        public Task<bool> CommentExistsAsync(int id)
        {
            return _context.Comments.AnyAsync(c => c.CommentId == id);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await FindCommentByIdAsync(id);

            _context.Comments.Remove(comment);
            await SaveAsync();
        }

        public async Task<CoreComment> GetCommentAsync(int id)
        {
            var comment = await FindCommentByIdAsync(id, true);

            return new CoreComment(comment);
        }

        public async Task<List<CoreComment>> GetCommentsAsync(int userId, int? index = null, int? postId = null)
        {
            ConfigureIndex(index);

            var coreComments = new List<CoreComment>();
            List<Comment> comments;

            if (postId is null)
            {
                comments = await _context.Comments
                    .Where(u => u.UserId == userId)
                    .Take(_takeValue)
                    .Skip(_index)
                    .Select(c => new Comment
                    {
                        CommentId = c.CommentId,
                        CommentBody = c.CommentBody,
                        DatePosted = c.DatePosted,
                        DateUpdated = c.DateUpdated,
                        PostId = c.PostId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                        }
                    })
                    .ToListAsync();
            }
            else
            {
                comments = await _context.Comments
                    .Where(u => u.UserId == userId && u.PostId == postId)
                    .Take(_takeValue)
                    .Skip(_index)
                    .Select(c => new Comment
                    {
                        CommentId = c.CommentId,
                        CommentBody = c.CommentBody,
                        DatePosted = c.DatePosted,
                        DateUpdated = c.DateUpdated,
                        PostId = postId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                        }
                    })
                    .ToListAsync();
            }

            if (comments.Count > 0)
            {
                for (int i = 0; i < comments.Count; i++)
                {
                    coreComments.Add(new CoreComment(comments[i]));
                }
            }

            return coreComments;
        }

        public async Task<CoreComment> UpdateCommentAsync(CoreComment comment)
        {
            var dataComment = await FindCommentByIdAsync(comment.CommentId);
            dataComment.CommentBody = comment.CommentBody;
            dataComment.PostId = comment.PostId;
            dataComment.UserId = comment.UserId;

            _context.Comments.Update(dataComment);

            await SaveAsync();

            comment.DateUpdated = dataComment.DateUpdated;

            return comment;
        }

        public Task<bool> UserHasAccessToCommentAsync(int id, int userId)
        {
            return _context.Comments.AnyAsync(c => c.CommentId == id  && c.UserId == userId);
        }

        private Task<Comment> FindCommentByIdAsync(int id, bool includeUser = false)
        {
            if (includeUser)
            {
                return _context.Comments
                    .Select(c => new Comment
                    {
                        CommentId = c.CommentId,
                        CommentBody = c.CommentBody,
                        DatePosted = c.DatePosted,
                        DateUpdated = c.DateUpdated,
                        PostId = c.PostId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName
                        }
                    }).FirstOrDefaultAsync(a => a.CommentId == id);
            }
            else
            {
                return _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
            }
        }
    }
}

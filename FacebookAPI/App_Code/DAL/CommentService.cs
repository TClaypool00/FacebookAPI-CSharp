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

        private Task<Comment> FindCommentByIdAsync(int id)
        {
            return _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
        }
    }
}

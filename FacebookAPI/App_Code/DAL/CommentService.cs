using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.DAL
{
    public class CommentService : ServiceHelper, ICommentService
    {
        public CommentService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Comments"];
        }

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
    }
}

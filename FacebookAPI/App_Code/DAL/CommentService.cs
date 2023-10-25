using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.DAL
{
    public class CommentService : ICommentService
    {
        private readonly FacebookDbContext _context;

        public CommentService(FacebookDbContext context)
        {
            _context = context;
        }

        public async Task<CoreComment> AddCommentAsync(CoreComment comment)
        {
            var dataComment = new Comment(comment);

            await _context.Comments.AddAsync(dataComment);
            await SaveAsync();

            if (dataComment.CommentId == 0)
            {
                throw new ApplicationException("Could not add post");
            }

            comment.CommentId = dataComment.CommentId;
            comment.DatePosted = dataComment.DatePosted;

            return comment;
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

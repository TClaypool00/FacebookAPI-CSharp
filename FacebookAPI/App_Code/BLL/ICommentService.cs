using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface ICommentService
    {
        #region Public Properties
        public Task<List<CoreComment>> GetCommentsAsync(int userId, int? index = null, int? postId = null, bool? includeReplies = null);

        public Task<CoreComment> AddCommentAsync(CoreComment comment);

        public Task<CoreComment> UpdateCommentAsync(CoreComment comment);

        public Task<CoreComment> GetCommentAsync(int id, int userId, bool? includeReplies = null);

        public Task<bool> CommentExistsAsync(int id);

        public Task<bool> UserHasAccessToCommentAsync(int id, int userId);

        public Task DeleteCommentAsync(int id);
        #endregion

        #region Public Properties
        public string CommentAddedOKMessag { get; }

        public string CommentUpdatedOKMessage { get; }

        public string CommentNotFoundMessage { get; }

        public string NoCommentsFound { get; }

        public string CommentDeletedOKMessage { get; }
        #endregion
    }
}

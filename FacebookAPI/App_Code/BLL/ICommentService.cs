using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface ICommentService
    {
        #region Public Properties
        public Task<CoreComment> AddCommentAsync(CoreComment comment);

        public Task<CoreComment> UpdateCommentAsync(CoreComment comment);

        public Task<CoreComment> GetCommentAsync(int id);

        public Task<bool> CommentExistsAsync(int id);

        public Task<bool> UserHasAccessToCommentAsync(int id, int userId);
        #endregion

        #region Public Properties
        public string CommentAddedOKMessag { get; }

        public string CommentUpdatedOKMessage { get; }

        public string CommentNotFoundMessage { get; }
        #endregion
    }
}

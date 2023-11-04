using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IPostService
    {
        #region Public Methods
        public Task<CorePost> AddPostAsync(CorePost post);

        public Task<CorePost> UpdatePostAsync(CorePost post);

        public Task<CorePost> GetPostByIdAsync(int id, int userId, bool? includeComments = null);

        public Task<List<CorePost>> GetAllPostsAsync(int userId, int? index = null, bool? includeComments = null);

        public Task<List<CorePost>> GetFriendsPostsAsync(int userId, int? index = null);

        public Task<bool> PostExistsAsync(int id);

        public Task<bool> UserHasAccessToPostAsync(int id, int userId);

        public Task DeletePostAsync(int id);
        #endregion

        #region Public Properties
        public string PostDoesNotExistMessage { get; }
        public string UserDoesNotHaveAccessMessage { get; }
        public string NoPostsFoundMessage { get; }
        public string PostDeletedMessage { get; }
        #endregion
    }
}

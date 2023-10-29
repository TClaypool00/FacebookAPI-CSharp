using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IPostService
    {
        #region Public Methods
        public Task<CorePost> AddPostAsync(CorePost post);

        public Task<CorePost> UpdatePostAsync(CorePost post);

        public Task<CorePost> GetPostByIdAsync(int id);

        public Task<List<CorePost>> GetAllPostsAsync(int? index = null, int? userId = null, bool includeComments = true);

        public Task<List<CorePost>> GetFriendsPostsAsync(int userId, int? index = null);

        public Task<bool> PostExistsAsync(int id);

        public Task<bool> UserHasAccessToPostAsync(int id, int userId);
        #endregion

        #region Public Properties
        public string PostDoesNotExistMessage { get; }
        public string UserDoesNotHaveAccessMessage { get; }
        #endregion
    }
}

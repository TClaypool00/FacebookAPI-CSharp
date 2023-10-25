using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IPostService
    {
        public Task<CorePost> AddPostAsync(CorePost post);

        public Task<List<CorePost>> GetAllPostsAsync(int? index = null, int? userId = null, bool includeComments = true);

        public Task<List<CorePost>> GetFriendsPostsAsync(int userId, int? index = null);
    }
}

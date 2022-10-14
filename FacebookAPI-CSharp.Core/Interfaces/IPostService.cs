using FacebookAPI.Core.Models.CoreModels;

namespace FacebookAPI.Core.Interfaces
{
    public interface IPostService
    {
        public Task<List<CorePost>> GetPostsAsync(int? userId = null);
        public Task<CorePost> GetPostByIdAsync(int id);
        public Task<CorePost> AddPostAsync(CorePost post);
        public Task<CorePost> UpdatePostAsync(CorePost post, int id);
        public Task<bool> PostExistsAsync(int id);
        public Task<bool> DeletePostAsync(int id);
    }
}

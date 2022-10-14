using FacebookAPI.Core.Interfaces;
using FacebookAPI.Core.Models.CoreModels;
using FacebookAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.DataAccess.Services
{
    public class PostService : IPostService, IGeneralService
    {
        private readonly FacebookDBContext _context;

        public PostService(FacebookDBContext context)
        {
            _context = context;
        }

        public async Task<CorePost> AddPostAsync(CorePost post)
        {
            var dataPost = Mapper.MapPost(post);

            await _context.Posts.AddAsync(dataPost);
            await SaveAsync();

            post.PostId = dataPost.PostId;
            post.DatePosted = dataPost.DatePosted;

            return post;

        }

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var post = await GetPost(id);
                _context.Posts.Remove(post);
                await SaveAsync();

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<CorePost> GetPostByIdAsync(int id)
        {
            return Mapper.MapPost(await GetPost(id));
        }

        public async Task<List<CorePost>> GetPostsAsync(int? userId = null)
        {
            List<Post> posts;

            if (userId is null)
            {
                posts = await _context.Posts
                    .Include(u => u.User)
                    .ToListAsync();
            } else
            {
                posts = await _context.Posts
                    .Include(u => u.User)
                    .Where(p => p.User.UserId == userId).ToListAsync();
            }

            return posts.Select(Mapper.MapPost).ToList();
        }

        public async Task<bool> PostExistsAsync(int id)
        {
            return await _context.Posts.AnyAsync(p => p.PostId == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CorePost> UpdatePostAsync(CorePost post, int id)
        {
            var oldPost = await GetPost(id);

            post.DatePosted = oldPost.DatePosted;
            post.PostId = id;

            _context.Entry(oldPost).CurrentValues.SetValues(Mapper.MapPost(post));

            return post;
        }

        private async Task<Post> GetPost(int id)
        {
            return await _context.Posts.Include(u => u.User).FirstOrDefaultAsync(p => p.PostId == id);
        }
    }
}

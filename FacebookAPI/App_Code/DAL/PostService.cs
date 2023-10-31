using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class PostService : ServiceHelper, IPostService
    {
        #region Public Properties
        public string PostDoesNotExistMessage => $"{_tableName} {_doesNotExistMessage}";

        public string UserDoesNotHaveAccessMessage => $"{_doesNotHaveAccessMessage} {_tableName}";

        public string NoPostsFoundMessage => _configuration["NotFoundMessages:Posts"];

        public string PostDeletedMessage => $"{_tableName} {_deletedMessage}";
        #endregion

        #region Constructors
        public PostService(FacebookDbContext context, IConfiguration configuration) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Post"];
        }
        #endregion

        #region Public Methods
        public async Task<CorePost> AddPostAsync(CorePost post)
        {
            try
            {
                var dataPost = new Post(post);

                await _context.Posts.AddAsync(dataPost);

                await SaveAsync();

                if (dataPost.PostId == 0)
                {
                    throw new ApplicationException($"{_tableName} ");
                }

                //post.SetNewValues(dataPost);

                return post;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CorePost>> GetAllPostsAsync(int userId, int? index = null, bool? includeComments = null)
        {
            ConfigureIndex(index);

            var corPosts = new List<CorePost>();
            List<Comment> comments = null;
            List<Post> posts;

            posts = await _context.Posts
                    .Where(a => a.User.UserId == userId)
                    .Select(p => new Post
                    {
                        PostId = p.PostId,
                        PostBody = p.PostBody,
                        DatePosted = p.DatePosted,
                        DateUpdated = p.DateUpdated,
                        User = new User
                        {
                            UserId = p.User.UserId,
                            FirstName = p.User.FirstName,
                            LastName = p.User.LastName
                        },
                        LikeCount = p.Likes.Where(a => a.UserId == userId && a.PostId == p.PostId).Count(),
                        Liked = p.Likes.Any(a => a.UserId == userId && a.PostId == p.PostId)
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();

            var postIds = PostIdsList(posts);

            if (includeComments == true)
            {
                comments = await FindComments(postIds);
            }

            for (int i = 0; i < posts.Count; i++)
            {
                var post = posts[i];

                if (includeComments == true)
                {
                    post.Comments = comments.Where(a => a.PostId == post.PostId).ToList();
                }

                corPosts.Add(new CorePost(posts[i]));
            }

            return corPosts;
        }

        public async Task<List<CorePost>> GetFriendsPostsAsync(int userId, int? index = null)
        {
            var corePosts = new List<CorePost>();

            var friends = await _context.Friends
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.DateAccepted != null)
                .Select(a => new Friend
                {
                    SenderId = a.SenderId,
                    ReceiverId = a.ReceiverId
                })
                .Take(_longerTakeValue)
                .ToListAsync();

            var friendsUserIds = FriendIds(friends, userId);

            var posts = await _context.Posts
                .Where(p => friendsUserIds.Contains(p.User.UserId))
                .Select(c => new Post
                {
                    DatePosted = c.DatePosted,
                    PostId = c.PostId,
                    PostBody = c.PostBody,
                    LikeCount = c.Likes.Select(l => l.LikeId).Count(),
                    Liked = c.Likes.Any(c => c.UserId == userId),
                    User = new User
                    {
                        UserId = c.User.UserId,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName
                    }
                })
                .Take(_takeValue)
                .ToListAsync();

            var postIds = PostIdsList(posts);

            var comments = await FindComments(postIds);

            for (int i = 0; i < posts.Count; i++)
            {
                var post = posts[i];

                post.Comments = comments.Where(a => a.PostId == post.PostId).ToList();

                corePosts.Add(new CorePost(posts[i]));
            }

            return corePosts;

        }

        public Task<bool> PostExistsAsync(int id)
        {
            return _context.Posts.AnyAsync(p => p.PostId == id);
        }

        public Task<bool> UserHasAccessToPostAsync(int id, int userId)
        {
            return _context.Posts.AnyAsync(p => p.PostId == id && p.UserId == userId);
        }

        public async Task<CorePost> UpdatePostAsync(CorePost post)
        {
            try
            {
                var dataPost = await FindPostByIdAsync(post.PostId, false);
                dataPost.PostBody = post.PostBody;

                _context.Posts.Update(dataPost);
                await SaveAsync();

                post.DateUpdated = dataPost.DateUpdated;

                return post;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CorePost> GetPostByIdAsync(int id, bool? includeComments = null)
        {
            var dataPost = await FindPostByIdAsync(id);

            if (includeComments == true)
            {
                dataPost.Comments = await FindCommentsByPostId(id);
            }

            return new CorePost(dataPost);
        }

        public async Task DeletePostAsync(int id)
        {
            var dataPost = await FindPostByIdAsync(id, false);

            dataPost.Comments = await FindCommentsByPostId(id, false);

            if (dataPost.Comments.Count > 0)
            {
                _context.Comments.RemoveRange(dataPost.Comments);

                await SaveAsync();
            }

            _context.Posts.Remove(dataPost);

            await SaveAsync();
        }
        #endregion

        #region Private Methods
        private static List<int> PostIdsList(List<Post> posts)
        {
            var postIds = new List<int>();
            int postId;

            for (int i = 0; i < posts.Count; i++)
            {
                postId = posts[i].PostId;

                if (!postIds.Any(p => p == postId))
                {
                    postIds.Add(postId);
                }
            }

            return postIds;
        }

        private static List<int> FriendIds(List<Friend> friends, int userId)
        {
            var userIds = new List<int>();
            Friend friend;

            for (int i = 0; i < friends.Count; i++)
            {
                friend = friends[i];

                if (friend.SenderId == userId)
                {
                    userIds.Add(friend.ReceiverId);
                }
                else if (friend.ReceiverId == userId)
                {
                    userIds.Add(friend.SenderId);
                }
            }

            return userIds;
        }

        private Task<List<Comment>> FindComments(List<int> ids)
        {
            return _context.Comments
                .Where(c => ids.Contains((int)c.PostId))
                .Select(c => new Comment
                {
                    CommentId = c.CommentId,
                    CommentBody = c.CommentBody,
                    DatePosted = c.DatePosted,
                    PostId = c.PostId,
                    User = new User
                    {
                        UserId = c.User.UserId,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName
                    }
                })
                .Take(_takeValue)
                .ToListAsync();
        }

        private Task<Post> FindPostByIdAsync(int id, bool includeUser = true)
        {
            if (!includeUser)
            {
                return _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            }
            else
            {
                return _context.Posts.Select(p => new Post
                {
                    PostId = p.PostId,
                    PostBody = p.PostBody,
                    DatePosted = p.DatePosted,
                    User = new User
                    {
                        UserId = p.User.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                    }
                }).FirstOrDefaultAsync(a => a.PostId == id);
            }
        }

        private Task<List<Comment>> FindCommentsByPostId(int id, bool includeUser = true)
        {
            if (includeUser)
            {
                return _context.Comments
                .Select(c => new Comment
                {
                    CommentId = c.CommentId,
                    CommentBody = c.CommentBody,
                    DatePosted = c.DatePosted,
                    DateUpdated = c.DateUpdated,
                    PostId = id,
                    User = new User
                    {
                        UserId = c.User.UserId,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName
                    }
                })
                .Take(_takeValue)
                .Skip(_index)
                .ToListAsync();
            }
            else
            {
                return _context.Comments
                    .Where(c => c.PostId == id)
                    .ToListAsync();
            }
        }
        #endregion
    }
}

using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.Helpers;
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
                    throw new ApplicationException($"{_couldNotAddedMessage} {_tableName} ");
                }

                post.PostId = dataPost.PostId;

                if (post.Pictures is not null && post.Pictures.Count > 0)
                {
                    var fileHelepr = new FileHelper(_configuration);

                    for (int i = 0; i < post.Pictures.Count; i++)
                    {
                        var picture = post.Pictures[i];

                        try
                        {
                            dataPost.Pictures.Add(new Picture(picture, post.PostId));
                            await fileHelepr.AddPicture(picture);

                            await _context.Pictures.AddAsync(dataPost.Pictures[i]);
                            await SaveAsync();
                        }
                        catch (Exception)
                        {
                            fileHelepr.DeletePicture(picture);

                            _context.Posts.Remove(dataPost);
                            await SaveAsync();

                            throw;
                        }
                    }
                }

                return post;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CorePost>> GetAllPostsAsync(int userId, int? index = null, bool? includeComments = null, bool? includeReplies = null)
        {
            ConfigureIndex(index);

            var corPosts = new List<CorePost>();
            List<Comment> comments = null;
            List<Post> posts;
            List<Reply> replies = null;
            Comment comment = null;

            if (includeReplies == true)
            {
                includeComments = true;
            } else if (includeComments != true)
            {
                includeReplies = null;
            }

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
                        LikeCount = p.Likes.Count(a => a.PostId == p.PostId),
                        Liked = p.Likes.Any(a => a.UserId == userId && a.PostId == p.PostId)
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();

            if (posts is not null && posts.Count > 0)
            {
                var helper = new DataEnttiyHelper();

                if (includeComments == true)
                {
                    var postIds = helper.GetIds(posts);
                    comments = await FindComments(postIds);
                }

                if (includeReplies == true)
                {
                    var commentIds = helper.GetIds(comments);
                    replies = await _context.Replies
                        .Where(a => commentIds.Contains(a.CommentId))
                        .Select(b => new Reply
                        {
                            ReplyId = b.ReplyId,
                            ReplyBody = b.ReplyBody,
                            DatePosted = b.DatePosted,
                            DateUpdated = b.DateUpdated,
                            CommentId = b.CommentId,
                            LikeCount = b.Likes.Count,
                            Liked = b.Likes.Any(l => l.UserId == userId),
                            User = new User
                            {
                                UserId = b.User.UserId,
                                FirstName = b.User.FirstName,
                                LastName = b.User.LastName
                            }
                        })
                        .Take(_subTakeValue)
                        .ToListAsync();
                }

                for (int i = 0; i < posts.Count; i++)
                {
                    var post = posts[i];

                    if (includeComments == true)
                    {
                        post.Comments = comments.Where(a => a.PostId == post.PostId).ToList();

                        if (includeReplies == true && post.Comments.Count > 0)
                        {
                            for (int j = 0; j < post.Comments.Count; j++)
                            {
                                comment = post.Comments[j];

                                comment.Replies = replies.Where(z => z.CommentId == comment.CommentId).ToList();
                            }
                        }
                    }

                    corPosts.Add(new CorePost(posts[i]));
                }
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
                    LikeCount = c.Likes.Count(l => l.PostId == c.PostId),
                    Liked = c.Likes.Any(l => l.UserId == userId && l.PostId == c.PostId),
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
                var dataPost = await FindPostByIdAsync(post.PostId);
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

        public async Task<CorePost> GetPostByIdAsync(int id, int userId, bool? includeComments = null)
        {
            var dataPost = await FindPostByIdAsync(id, userId);

            if (includeComments == true)
            {
                dataPost.Comments = await FindCommentsByPostId(id);
            }

            return new CorePost(dataPost);
        }

        public async Task DeletePostAsync(int id)
        {
            var dataPost = await FindPostByIdAsync(id);

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

        private Task<Post> FindPostByIdAsync(int id)
        {
            return _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
        }
        
        private Task<Post> FindPostByIdAsync(int id, int userId)
        {
            return _context.Posts.Select(p => new Post
            {
                PostId = p.PostId,
                PostBody = p.PostBody,
                DatePosted = p.DatePosted,
                LikeCount = p.Likes.Count(l => l.PostId == p.PostId),
                Liked = p.Likes.Any(a => a.PostId == p.PostId && a.UserId == userId),
                User = new User
                {
                    UserId = p.User.UserId,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                }
            }).FirstOrDefaultAsync(a => a.PostId == id);
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

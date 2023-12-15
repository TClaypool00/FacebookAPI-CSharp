using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class CommentService : ServiceHelper, ICommentService
    {
        public CommentService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration["tableNames:Comment"];
        }

        public string CommentAddedOKMessag => $"{_tableName} {_addedOKMessage}";

        public string CommentUpdatedOKMessage => $"{_tableName} {_updatedOKMessage}";

        public string CommentNotFoundMessage => $"{_tableName} {_doesNotExistMessage}";

        public string NoCommentsFound => _configuration["NotFoundMessages:Comments"];

        public string CommentDeletedOKMessage => $"{_tableName} {_deletedMessage}";

        public async Task<CoreComment> AddCommentAsync(CoreComment comment)
        {
            var dataComment = new Comment(comment);

            await _context.Comments.AddAsync(dataComment);
            await SaveAsync();

            if (dataComment.CommentId == 0)
            {
                throw new ApplicationException($"{_tableName} {_couldNotAddedMessage}");
            }

            comment.CommentId = dataComment.CommentId;
            comment.DatePosted = dataComment.DatePosted;

            return comment;
        }

        public Task<bool> CommentExistsAsync(int id)
        {
            return _context.Comments.AnyAsync(c => c.CommentId == id);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await FindCommentByIdAsync(id);

            comment.Replies = await _context.Replies.Where(x => x.CommentId == id).ToListAsync();

            var helper = new DataEnttiyHelper();

            var replyIds = helper.GetIds(comment.Replies);

            comment.Likes = await _context.Likes.Where(x => replyIds.Contains((int)x.ReplyId)).ToListAsync();

            if (comment.Likes.Count > 0)
            {
                _context.Likes.RemoveRange(comment.Likes);
                await SaveAsync();
            }

            _context.Replies.RemoveRange(comment.Replies);
            await SaveAsync();

            comment.Likes = await _context.Likes.Where(x => x.CommentId == id).ToListAsync();

            if (comment.Likes.Count > 0)
            {
                _context.Likes.RemoveRange(comment.Likes);
                await SaveAsync();
            }

            _context.Comments.Remove(comment);
            await SaveAsync();
        }

        public async Task<CoreComment> GetCommentAsync(int id, int userId, bool? includeReplies = null)
        {
            var comment = await FindCommentByIdAsync(id, true);

            if (includeReplies == true)
            {
                comment.Replies = await _context.Replies.Select(x => new Reply
                {
                    ReplyId = x.ReplyId,
                    ReplyBody = x.ReplyBody,
                    DatePosted = x.DatePosted,
                    DateUpdated = x.DateUpdated,
                    CommentId = x.CommentId,
                    LikeCount = x.Likes.Count(a => a.ReplyId == x.ReplyId),
                    Liked = x.Likes.Any(a => a.ReplyId == x.ReplyId && a.UserId == userId),
                    User = new User
                    {
                        UserId = x.User.UserId,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName
                    }
                })
                .Take(_subTakeValue)
                .ToListAsync();
            }

            return new CoreComment(comment, _configuration);
        }

        public async Task<List<CoreComment>> GetCommentsAsync(int currentUserId, int? userId = null, int? index = null, int? postId = null, bool? includeReplies = null)
        {
            ConfigureIndex(index);

            userId ??= currentUserId;

            var coreComments = new List<CoreComment>();
            List<Comment> comments = null;
            List<Reply> replies = null;
            Comment comment;

            if (postId is not null)
            {
                comments = await _context.Comments
                    .Where(u => u.PostId == postId)
                    .Select(c => new Comment
                    {
                        CommentId = c.CommentId,
                        CommentBody = c.CommentBody,
                        DatePosted = c.DatePosted,
                        DateUpdated = c.DateUpdated,
                        PostId = c.PostId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                        }
                    })
                    .Skip(_index)
                    .Take(_takeValue)
                    .ToListAsync();
            }

            if (userId is not null)
            {
                if (comments is null)
                {
                    comments = await _context.Comments
                    .Where(u => u.UserId == userId)
                    .Select(c => new Comment
                    {
                        CommentId = c.CommentId,
                        CommentBody = c.CommentBody,
                        DatePosted = c.DatePosted,
                        DateUpdated = c.DateUpdated,
                        PostId = postId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                        }
                    })
                    .Take(_takeValue)
                    .Skip(_index)
                    .ToListAsync();
                }
                else
                {
                    comments = comments.Where(u => u.UserId == userId).ToList();
                }
            }

            if (comments is not null && comments.Count > 0)
            {
                if (includeReplies == true)
                {
                    var helper = new DataEnttiyHelper();
                    var commentIds = helper.GetIds(comments);

                    replies = await _context.Replies
                            .Where(f => commentIds.Contains(f.CommentId))
                            .Select(x => new Reply
                            {
                                ReplyId = x.ReplyId,
                                ReplyBody = x.ReplyBody,
                                DatePosted = x.DatePosted,
                                DateUpdated = x.DateUpdated,
                                CommentId = x.CommentId,
                                LikeCount = x.Likes.Count,
                                Liked = x.Likes.Any(b => b.UserId == userId),
                                User = new User
                                {
                                    UserId = x.User.UserId,
                                    FirstName = x.User.FirstName,
                                    LastName = x.User.LastName
                                }
                            })
                            .Take(_subTakeValue)
                            .ToListAsync();
                }

                for (int i = 0; i < comments.Count; i++)
                {
                    comment = comments[i];
                    if (includeReplies == true)
                    {
                        comment.Replies = replies.Where(a => a.CommentId == comment.CommentId).ToList();
                    }
                    coreComments.Add(new CoreComment(comment, _configuration));
                }
            }

            return coreComments;
        }

        public async Task<CoreComment> UpdateCommentAsync(CoreComment comment)
        {
            var dataComment = await FindCommentByIdAsync(comment.CommentId);
            dataComment.CommentBody = comment.CommentBody;
            dataComment.PostId = comment.PostId;
            dataComment.UserId = comment.UserId;

            _context.Comments.Update(dataComment);

            await SaveAsync();

            comment.DateUpdated = dataComment.DateUpdated;

            return comment;
        }

        public Task<bool> UserHasAccessToCommentAsync(int id, int userId)
        {
            return _context.Comments.AnyAsync(c => c.CommentId == id  && c.UserId == userId);
        }

        private Task<Comment> FindCommentByIdAsync(int id, bool includeUser = false)
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
                        PostId = c.PostId,
                        User = new User
                        {
                            UserId = c.User.UserId,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName
                        }
                    }).FirstOrDefaultAsync(a => a.CommentId == id);
            }
            else
            {
                return _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
            }
        }
    }
}

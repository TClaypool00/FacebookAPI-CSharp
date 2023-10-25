using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using Microsoft.EntityFrameworkCore;
using static FacebookAPI.App_Code.BOL.Like;

namespace FacebookAPI.App_Code.DAL
{
    public class LikeService : ServiceHelper, ILikeService
    {
        private LikeOptions _likeOption;
        private int _id;
        private readonly string _errorMessage;

        public string LikeDoesNotExistMessage => "Like does not exists";

        public string LikeExistMessage => "Like already exists";

        public string LikeIdMessage => "Like Id must be greater than 0";

        public LikeService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _errorMessage = "Not valid option";
        }

        public async Task<Tuple<int, bool>> AddLikeAsync(int id, int userId, Like.LikeOptions likeOptions)
        {
            try
            {
                SetVariables(likeOptions, id);
                var dataLike = new Like(id, userId, likeOptions);
                await _context.Likes.AddAsync(dataLike);
                await SaveAsync();

                return new Tuple<int, bool>(await NewLikeCount(), true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, bool>> DeleteLikeAsync(int id, int userId, LikeOptions likeOptions)
        {
            try
            {
                SetVariables(likeOptions, id);
                var like = _likeOption switch
                {
                    LikeOptions.Post => await _context.Likes.FirstOrDefaultAsync(l => l.PostId == id && l.UserId == userId),
                    LikeOptions.Comment => await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == id && l.UserId == userId),
                    LikeOptions.Reply => await _context.Likes.FirstOrDefaultAsync(l => l.ReplyId == id && l.UserId == userId),
                    LikeOptions.Picture => await _context.Likes.FirstOrDefaultAsync(l => l.PictureId == id && l.UserId == userId),
                    _ => throw new ApplicationException(_errorMessage),
                };

                _context.Likes.Remove(like);
                await SaveAsync();

                return new Tuple<int, bool>(await NewLikeCount(), false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> LikeExistsAsync(int id, int userId, LikeOptions likeOptions)
        {
            SetVariables(likeOptions, id);

            return _likeOption switch
            {
                LikeOptions.Post => _context.Likes.AnyAsync(l => l.PostId == _id && l.UserId == userId),
                LikeOptions.Comment => _context.Likes.AnyAsync(l => l.CommentId == _id && l.UserId == userId),
                LikeOptions.Reply => _context.Likes.AnyAsync(l => l.ReplyId == _id && l.UserId == userId),
                LikeOptions.Picture => _context.Likes.AnyAsync(l => l.PictureId == _id && l.UserId == userId),
                _ => throw new ApplicationException(_errorMessage),
            };
        }

        private Task<int> NewLikeCount()
        {
            switch (_likeOption)
            {
                case LikeOptions.Post:
                    return _context.Likes.Where(l => l.PostId == _id).CountAsync();
                case LikeOptions.Comment:
                    return _context.Likes.Where(l => l.CommentId == _id).CountAsync();
                case LikeOptions.Reply:
                    return _context.Likes.Where(l => l.ReplyId == _id).CountAsync();
                case LikeOptions.Picture:
                    return _context.Likes.Where(l => l.PictureId == _id).CountAsync(); ;
                default:
                    throw new ApplicationException(_errorMessage);
            }
        }

        private void SetVariables(LikeOptions likeOptions, int id)
        {
            _likeOption = likeOptions;
            _id = id;
        }
    }
}

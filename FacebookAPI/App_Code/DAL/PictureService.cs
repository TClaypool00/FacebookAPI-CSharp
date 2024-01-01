using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.DAL
{
    public class PictureService : ServiceHelper, IPictureService
    {
        #region Private fields
        private readonly string _multiTableName;
        #endregion

        #region Constructors
        public PictureService(IConfiguration configuration, FacebookDbContext context) : base(configuration, context)
        {
            _tableName = _configuration.GetSection("tableNames").GetSection("Picture").Value;
            _multiTableName = _configuration.GetSection("tableName").GetSection("Pictures").Value;
        }
        #endregion

        #region Public Properites
        public string PictureAddedOKMessage => $"{_tableName} {_addedOKMessage}";

        public string PictureCouldNotBeAddedMessage => $"{_tableName} {_couldNotAddedMessage}";

        public string PicturesAddedOKMessage => $"{_multiTableName} {_addedOKMessage}";

        public string PictureDoesNotExistMessage => $"{_tableName} {_doesNotExistMessage}";

        public string PictureUpdatedOKMessage => $"{_tableName} {_updatedOKMessage}";

        public string PictureNotDeletedMessage => $"{_tableName} {_configuration["messages:DeletedError"]}";

        public string PictureDeletedOKMessage => $"{_tableName} {_deletedMessage}";

        public string PicturesNotFoundMessage => $"{_multiTableName} {_doesNotExistMessage}";
        #endregion

        #region Public Methods
        public async Task<CorePicture> AddPictureAsync(CorePicture picture)
        {
            try
            {
                if (picture.ProfilePicture)
                {
                    var currentProfilePicture = await _context.Pictures.FirstOrDefaultAsync(p => p.UserId == picture.UserId && p.ProfilePicture == true);
                    currentProfilePicture.ProfilePicture = false;

                    _context.Pictures.Update(currentProfilePicture);
                    await SaveAsync();
                }

                var dataPicture = new Picture(picture);
                await _context.Pictures.AddAsync(dataPicture);
                await SaveAsync();

                try
                {
                    var fileHelper = new FileHelper(_configuration);
                    await fileHelper.AddPicture(picture);

                    picture.PictureId = dataPicture.PictureId;

                    return picture;
                }
                catch (Exception)
                {
                    _context.Pictures.Remove(dataPicture);
                    await SaveAsync();

                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<int>> AddPicturesAsync(List<CorePicture> pictures)
        {
            var ids = new List<int>();

            for (int i = 0; i < pictures.Count; i++)
            {
                try
                {
                    var picture = pictures[i];
                    await AddPictureAsync(picture);
                }
                catch (Exception)
                {
                    ids.Add(i);
                }
            }

            return ids;
        }

        public async Task DeletePictureAsync(int id)
        {
            var picture = await FindPictureById(id);
            var fileHelper = new FileHelper(_configuration);
            List<Like> likes;

            try
            {
                fileHelper.DeletePicture(picture);
            }
            catch (Exception)
            {
                throw;
            }

            picture.Comments = await _context.Comments
                .Where(c => c.PictureId == id)
                .ToListAsync();

            if (picture.Comments != null)
            {
                var dataHelper = new DataEnttiyHelper();
                var commentIds = dataHelper.GetIds(picture.Comments);

                var replies = await _context.Replies
                    .Where(r => commentIds.Contains(r.CommentId))
                    .ToListAsync();

                if (replies is not null)
                {
                    var replyIds = dataHelper.GetIds(replies);

                    likes = await _context.Likes
                        .Where(r => replyIds.Contains((int)r.ReplyId))
                        .ToListAsync();
                    
                    if (likes is not null)
                    {
                        _context.Likes.RemoveRange(likes);
                    }

                    _context.Replies.RemoveRange(replies);
                    await SaveAsync();
                }

                likes = null;
                likes = await _context.Likes
                    .Where(l => commentIds.Contains((int)l.CommentId))
                    .ToListAsync();

                if (likes is not null)
                {
                    _context.Likes.RemoveRange(likes);
                    await SaveAsync();
                }

                likes = null;
                _context.Comments.RemoveRange(picture.Comments);
                await SaveAsync();
            }

            likes = await _context.Likes
                .Where(l => l.PictureId == id)
                .ToListAsync();

            if (likes is not null)
            {
                _context.Likes.RemoveRange(likes);

                await SaveAsync();
            }

            _context.Pictures.Remove(picture);

            await SaveAsync();
        }

        public async Task<List<CorePicture>> GetPictureAsync(int userId, int? postId = null, int? index = null)
        {
            ConfigureIndex(index);
            var corePictures = new List<CorePicture>();
            List<Picture> pictures;
            

            if (postId is not null)
            {
                pictures = await _context.Pictures
                .Take(_takeValue)
                .Skip(_index)
                .Select(p => new Picture
                {
                    PictureId = p.PictureId,
                    PictureFileName = p.PictureFileName,
                    CaptionText = p.CaptionText,
                    PostId = p.PostId,
                    LikeCount = p.Likes.Count,
                    Liked = p.Likes.Any(u => u.UserId == userId),
                    User = new User
                    {
                        UserId = p.User.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,

                    }
                })
                .ToListAsync();
            }
            else
            {
                pictures = await _context.Pictures
                .Where(b => b.PostId == postId)
                .Take(_takeValue)
                .Skip(_index)
                .Select(p => new Picture
                {
                    PictureId = p.PictureId,
                    PictureFileName = p.PictureFileName,
                    CaptionText = p.CaptionText,
                    PostId = p.PostId,
                    LikeCount = p.Likes.Count,
                    Liked = p.Likes.Any(u => u.UserId == userId),
                    User = new User
                    {
                        UserId = p.User.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,

                    }
                })
                .ToListAsync();
            }

            if (pictures.Count > 0)
            {
                for (int i = 0; i < pictures.Count; i++)
                {
                    corePictures.Add(new CorePicture(pictures[i], _configuration));
                }
            }

            return corePictures;
        }

        public async Task<CorePicture> GetPictureByIdAsync(int id, int userId)
        {
            var picture = await _context.Pictures
                .Select(p => new Picture
                {
                    PictureId = p.PictureId,
                    CaptionText = p.CaptionText,
                    PictureFileName = p.PictureFileName,
                    ProfilePicture = p.ProfilePicture,
                    PostId = p.PostId,
                    LikeCount = p.Likes.Count,
                    Liked = p.Likes.Any(d => d.UserId == userId),
                    User = new User
                    {
                        UserId = p.User.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName
                    }
                })
                .FirstOrDefaultAsync(a => a.PictureId == id);

            return new CorePicture(picture, _configuration);
        }

        public Task<bool> PictureExistsAsync(int id)
        {
            return _context.Pictures.AnyAsync(p => p.PictureId == id);
        }

        public string PicturesCouldNotBeAddedMessage(List<int> ids)
        {
            string output = _multiTableName;

            for (int i = 0; i < ids.Count; i++)
            {
                int id = ids[i];

                output += id;

                if (i != ids.Count)
                {
                    output += ", ";
                }
            }

            output += _couldNotAddedMessage;

            return output;
        }

        public async Task<CorePicture> UpdatePictureByIdAsync(CorePicture picture)
        {
            var dataPicture = await FindPictureById(picture.PictureId);
            dataPicture.ProfilePicture = picture.ProfilePicture;
            dataPicture.UserId = picture.UserId;
            dataPicture.PostId = picture.PostId;
            dataPicture.CaptionText = picture.CaptionText;

            await SaveAsync();

            return new CorePicture(dataPicture, _configuration);
        }

        public async Task UpdateProfilePictureAsync(int id, bool profilePicture, int userId)
        {
            Picture picture;

            if (profilePicture)
            {
                picture = await _context.Pictures.FirstOrDefaultAsync(p => p.ProfilePicture == true && p.UserId == userId);
                
                if (picture is not null)
                {
                    picture.ProfilePicture = false;

                    await SaveAsync();
                }
            }

            picture = await FindPictureById(id);
            picture.ProfilePicture = profilePicture;


            await SaveAsync();
        }

        public Task<bool> UserOwnsPictureAsync(int id, int userId)
        {
            return _context.Pictures.AnyAsync(p => p.PictureId == id && p.UserId == userId);
        }
        #endregion

        #region Private Methods
        private Task<Picture> FindPictureById(int id)
        {
            return _context.Pictures.FirstOrDefaultAsync(p => p.PictureId == id);
        }
        #endregion
    }
}

using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.Interfaces;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class PostViewModel : PostPostViewModel, IBaseViewModel
    {
        #region Private fields
        private CorePost _corePost;
        #endregion

        public PostViewModel()
        {

        }

        public PostViewModel(CorePost corePost, int userId, string userDisplayName)
        {

            Construct(corePost);
            
            _userId = userId;
            _userDisplayName = userDisplayName;

        }

        public PostViewModel(CorePost corePost)
        {
            Construct(corePost);
        }

        #region Public Properties
        public int PostId { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        public List<string> PictureUrls { get; set; }

        #region Implemented methods
        public string UserDisplayName { get => _userDisplayName; set => _userDisplayName = value; }
        public string DatePosted { get => _datePosted; set => _datePosted = value; }
        public bool IsEdited { get => _isEdited; set => _isEdited = value; }
        public int LikeCount { get => _likeCount; set => _likeCount = value; }
        public bool Liked { get => _liked; set => _liked = value; }
        public string Message { get => _message; set => _message = value; }
        #endregion
        #endregion

        private void Construct(CorePost corePost)
        {
            if (corePost is null)
            {
                throw new ArgumentNullException(nameof(corePost));
            }

            _corePost = corePost;

            PostId = _corePost.PostId;
            PostBody = _corePost.PostBody;
            DatePosted = _corePost.DatePostedString;
            LikeCount = _corePost.LikeCount;
            Liked = _corePost.Liked;
            IsEdited = _corePost.IsEdited;

            if (_corePost.Comments is not null)
            {
                Comments = new List<CommentViewModel>();

                for (int i = 0; i < _corePost.Comments.Count; i++)
                {
                    Comments.Add(new CommentViewModel(_corePost.Comments[i]));
                }
            }

            if (_corePost.Pictures is not null && _corePost.Pictures.Count > 0)
            {
                PictureUrls = new List<string>();

                for (int i = 0; i < _corePost.Pictures.Count; i++)
                {
                    PictureUrls.Add(_corePost.Pictures[i].FullPath);
                }
            }

            if (_corePost.User is not null)
            {
                UserId = _corePost.User.UserId;
                _userDisplayName = _corePost.User.ProtectedName;
            }
        }
    }
}

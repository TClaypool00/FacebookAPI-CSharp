using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.Interfaces;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class CommentViewModel : PostCommentViewModel, IBaseViewModel
    {
        private CoreComment _coreComment;

        public CommentViewModel(CoreComment coreComment)
        {
            Construct(coreComment);
        }

        public CommentViewModel(CoreComment coreComment, string message)
        {
            Construct(coreComment);
            Message = message;

        }


        public int CommentId { get; set; }
        public string UserDisplayName { get => _userDisplayName; set => _userDisplayName = value; }
        public string DatePosted { get => _datePosted; set => _datePosted = value; }
        public bool IsEdited { get => _isEdited; set => _isEdited = value; }
        public int LikeCount { get => _likeCount; set => _likeCount = value; }
        public bool Liked { get => _liked; set => _liked = value; }
        public string Message { get => _message; set => _message = value; }

        private void Construct(CoreComment coreComment)
        {
            if (coreComment is null)
            {
                throw new ArgumentNullException(nameof(coreComment));
            }

            _coreComment = coreComment;

            CommentId = _coreComment.CommentId;
            CommentBody = _coreComment.CommentBody;
            UserId = _coreComment.UserId;            
            DatePosted = _coreComment.DatePostedString;
            PostId = _coreComment.PostId;

            if (_coreComment.User is not null)
            {
                UserId = _coreComment.User.UserId;
                UserDisplayName = _coreComment.User.ProtectedName;
            }
        }
    }
}

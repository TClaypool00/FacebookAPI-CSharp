using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class CommentViewModel : PostCommentViewModel
    {
        private CoreComment _coreComment;

        public CommentViewModel(CoreComment coreComment)
        {
            Construct(coreComment);

            if (_coreComment.User is not null)
            {
                UserId = _coreComment.User.UserId;
                UserDisplayName = _coreComment.User.ProtectedName;
            }
        }

        private string _userDisplayName;

        public CommentViewModel(CoreComment coreComment, string userDisplayName)
        {
            Construct(coreComment);

            _userDisplayName = userDisplayName;
        }

        public int CommentId { get; set; }

        public string UserDisplayName
        {
            get
            {
                return _userDisplayName;
            }
            set
            {
                _userDisplayName = value;
            }
        }

        public string DatePosted { get; set; }

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
        }
    }
}

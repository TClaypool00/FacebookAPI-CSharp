using FacebookAPI.App_Code.ViewModels.Interfaces;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class ReplyViewModel : PostReplyViewModel, IBaseViewModel
    {
        #region Private Fields
        private CoreReply _coreReply;
        #endregion

        #region Constructors
        public ReplyViewModel() : base()
        {
            
        }

        public ReplyViewModel(CoreReply coreReply) : base()
        {
            Construct(coreReply);
        }

        public ReplyViewModel(CoreReply coreReply, string message) : base()
        {
            Construct(coreReply);
            Message = message;
        }
        #endregion

        #region Public Properties
        public int ReplyId { get; set; }
        

        #region Implemented Properties
        public string UserDisplayName { get => _userDisplayName; set => _userDisplayName = value; }
        public string DatePosted { get => _datePosted; set => _datePosted = value; }
        public bool IsEdited { get => _isEdited; set => _isEdited = value; }
        public int LikeCount { get => _likeCount; set => _likeCount = value; }
        public bool Liked { get => _liked; set => _liked = value; }
        public string Message { get => _message; set => _message = value; }
        #endregion
        #endregion

        #region Private methods
        private void Construct(CoreReply coreReply)
        {
            _coreReply = coreReply ?? throw new ArgumentNullException(nameof(coreReply));

            ReplyId = _coreReply.ReplyId;
            ReplyBody = _coreReply.ReplyBody;
            DatePosted = _coreReply.DatePostedString;
            LikeCount = _coreReply.LikeCount;
            Liked = _coreReply.Liked;
            CommentId = _coreReply.CommentId;

            if (_coreReply.User is not null)
            {
                _userId = _coreReply.User.UserId;
                _userDisplayName = _coreReply.User.ProtectedName;
            }
        }
        #endregion
    }
}

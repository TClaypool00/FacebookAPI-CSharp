using FacebookAPI.App_Code.CoreModels.BaseCoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreReply : BaseCoreModel
    {
        #region Private fields
        private PostReplyViewModel _postReplyViewModel;
        #endregion

        #region Constructors
        public CoreReply()
        {
            
        }

        public CoreReply(PostReplyViewModel postReplyViewModel)
        {
            Construct(postReplyViewModel);
        }

        public CoreReply(PostReplyViewModel postReplyViewModel, int id)
        {
            Construct(postReplyViewModel);
            ReplyId = id;
        }
        #endregion

        #region Public Properties
        public int ReplyId { get; set; }

        public string ReplyBody { get; set; }

        public int CommentId { get; set; }
        public CoreComment Comment { get; set; }
        #endregion

        #region Private methods
        private void Construct(PostReplyViewModel postReplyViewModel)
        {
            _postReplyViewModel = postReplyViewModel ?? throw new ArgumentNullException(nameof(postReplyViewModel));

            ReplyBody = _postReplyViewModel.ReplyBody;
            CommentId = _postReplyViewModel.CommentId;
            UserId = _postReplyViewModel.UserId;
        }
        #endregion
    }
}

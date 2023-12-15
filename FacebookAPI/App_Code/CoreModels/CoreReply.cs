using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels.BaseCoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreReply : BaseCoreModel
    {
        #region Private fields
        private PostReplyViewModel _postReplyViewModel;
        private readonly Reply _reply;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public CoreReply()
        {
            
        }

        public CoreReply(PostReplyViewModel postReplyViewModel)
        {
            Construct(postReplyViewModel);
        }

        public CoreReply(PostReplyViewModel postReplyViewModel, int id, IConfiguration configuration)
        {
            Construct(postReplyViewModel);
            ReplyId = id;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

        public CoreReply(Reply reply)
        {
            _reply = reply ?? throw new ArgumentNullException(nameof(reply));

            ReplyId = _reply.ReplyId;
            ReplyBody = _reply.ReplyBody;
            DatePosted = _reply.DatePosted;
            DateUpdated = _reply.DateUpdated;
            CommentId = _reply.CommentId;
            Liked = _reply.Liked;
            LikeCount = _reply.LikeCount;

            if (_reply.Comment is not null)
            {
                Comment = new CoreComment(_reply.Comment, _configuration);
            }

            if (_reply.User is not null)
            {
                User = new CoreUser(_reply.User);
            }
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

using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreComment
    {
        private readonly PostCommentViewModel _postComentModel;
        private readonly Comment _comment;
        private int _userId;

        public int CommentId { get; set; }

        public string CommentBody { get; set; }

        public DateTime DatePosted { get; set; }

        public string DatePostedString
        {
            get
            {
                return DatePosted.ToString("f");
            }
        }

        public DateTime DateUpdated { get; set; }

        public string DateUpdatedString
        {
            get
            {
                return DateUpdated.ToString("f");
            }
        }

        public int PostId { get; set; }
        public CorePost Post { get; set; }

        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }
        public CoreUser User { get; set; }

        public CoreComment()
        {

        }

        public CoreComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            _comment = comment;

            CommentId = _comment.CommentId;
            CommentBody = _comment.CommentBody;
            DatePosted = _comment.DatePosted;

            if (_comment.User is not null)
            {
                User = new CoreUser(_comment.User);
            }
        }

        public CoreComment(PostCommentViewModel postComentModel, int userId)
        {
            if (postComentModel is null)
            {
                throw new ArgumentNullException(nameof(postComentModel));
            }

            _postComentModel = postComentModel;

            CommentBody = _postComentModel.CommentBody;
            PostId = _postComentModel.PostId;
            _userId = userId;
        }
    }
}

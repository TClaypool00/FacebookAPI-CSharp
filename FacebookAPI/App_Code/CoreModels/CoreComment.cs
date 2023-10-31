using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels.BaseCoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CoreComment : BaseCoreModel
    {
        #region Private fields
        private PostCommentViewModel _postComentModel;

        #region Read-Only
        private readonly Comment _comment;
        #endregion
        #endregion


        #region Constructors
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
            DateUpdated = _comment.DateUpdated;
            if (_comment.PostId is not null)
            {
                PostId = (int)_comment.PostId;
            }

            if (_comment.User is not null)
            {
                User = new CoreUser(_comment.User);
            }

            if (_comment.Post is not null)
            {
                Post = new CorePost(_comment.Post);
            }
        }

        public CoreComment(PostCommentViewModel postComentModel)
        {
            Construct(postComentModel);
        }

        public CoreComment(PostCommentViewModel postComentModel, int id)
        {
            Construct(postComentModel);

            CommentId = id;
        }
        #endregion

        #region Public Properties
        public int CommentId { get; set; }

        public string CommentBody { get; set; }

        public int PostId { get; set; }
        public CorePost Post { get; set; }
        #endregion

        #region Private methods
        private void Construct(PostCommentViewModel postComentModel)
        {
            if (postComentModel is null)
            {
                throw new ArgumentNullException(nameof(postComentModel));
            }

            _postComentModel = postComentModel;

            CommentBody = _postComentModel.CommentBody;
            PostId = _postComentModel.PostId;
            _userId = _postComentModel.UserId;
        }
        #endregion
    }
}

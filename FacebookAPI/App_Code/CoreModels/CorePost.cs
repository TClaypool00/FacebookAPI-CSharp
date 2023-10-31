using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels.BaseCoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CorePost : BaseCoreModel
    {
        #region Private fields
        private Post _post;
        private PostPostViewModel _postViewModel;
        #endregion

        #region Constructors
        public CorePost()
        {

        }

        public CorePost(PostPostViewModel model)
        {
            Construct(model);
        }

        public CorePost(PostPostViewModel model, int id)
        {
            Construct(model);
            PostId = id;
        }

        public CorePost(Post post)
        {
            Construct(post);
        }
        #endregion

        #region Public Properties
        public int PostId { get; set; }

        public string PostBody { get; set; }

        public List<CoreComment> Comments { get; set; }
        #endregion

        #region Private methods
        private void Construct(Post post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));

            PostId = _post.PostId;
            PostBody = _post.PostBody;
            DatePosted = _post.DatePosted;
            DateUpdated = _post.DateUpdated;
            LikeCount = _post.LikeCount;
            Liked = _post.Liked;

            if (_post.Comments is not null)
            {
                Comments = new List<CoreComment>();

                for (int i = 0; i < _post.Comments.Count; i++)
                {
                    Comments.Add(new CoreComment(_post.Comments[i]));
                }
            }

            if (_post.User is not null)
            {
                User = new CoreUser(_post.User);
            }
        }

        private void Construct(PostPostViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _postViewModel = model;
            _userId = _postViewModel.UserId;

            UserId = _userId;
            PostBody = _postViewModel.PostBody;
        }
        #endregion
    }
}

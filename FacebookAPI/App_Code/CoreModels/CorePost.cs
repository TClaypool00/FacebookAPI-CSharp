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
        private IConfiguration _configuration;
        #endregion

        #region Constructors
        public CorePost()
        {

        }

        public CorePost(PostPostViewModel model, IConfiguration configuration)
        {
            Construct(configuration);
            Construct(model);
        }

        public CorePost(PostPostViewModel model, int id, IConfiguration configuration)
        {
            Construct(model, id);
            Construct(configuration);
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

        public List<CorePicture> Pictures { get; set; }
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

            if (_postViewModel.Pictures is not null && _postViewModel.Pictures.Count > 0)
            {
                Pictures = new List<CorePicture>();

                for (int i = 0; i < _postViewModel.Pictures.Count; i++)
                {
                    var picture = _postViewModel.Pictures[i];

                    Pictures.Add(new CorePicture(picture, _configuration, _postViewModel.Files[i]));
                }
            }
        }

        private void Construct(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private void Construct(PostPostViewModel model, int id)
        {
            Construct(model);
            PostId = id;
        }
        #endregion
    }
}

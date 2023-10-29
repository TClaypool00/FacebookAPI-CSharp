using FacebookAPI.App_Code.CoreModels;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class PostViewModel : PostPostViewModel
    {
        private CorePost _corePost;
        private string _userDisplayName;

        public int PostId { get; set; }

        public string DatePosted { get; set; }

        public string UserDisplayName
        {
            get { return _userDisplayName; }
            set
            {
                _userDisplayName = value;
            }
        }

        public int LikeCount { get; set; }
        public bool Liked { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public PostViewModel()
        {

        }

        public PostViewModel(CorePost corePost, int userId, string userDisplayName)
        {

            SetCorePostValues(corePost);
            
            _userId = userId;
            _userDisplayName = userDisplayName;

        }

        public PostViewModel(CorePost corePost)
        {
            SetCorePostValues(corePost);

            if (_corePost.User is not null)
            {
                UserId = _corePost.User.UserId;
                _userDisplayName = _corePost.User.ProtectedName;
            }
        }

        public void SetProperties(Tuple<int, bool> tuple)
        {
            LikeCount = tuple.Item1;
            Liked = tuple.Item2;
        }

        private void SetCorePostValues(CorePost corePost)
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

            if (_corePost.Comments is not null)
            {
                Comments = new List<CommentViewModel>();

                for (int i = 0; i < _corePost.Comments.Count; i++)
                {
                    Comments.Add(new CommentViewModel(_corePost.Comments[i]));
                }
            }
        }
    }
}

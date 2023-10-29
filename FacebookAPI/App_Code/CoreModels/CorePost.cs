using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CorePost
    {
        private Post _post;
        private readonly int _userId;
        private readonly PostPostViewModel _postViewModel;
        private DateTime _datePosted;
        private DateTime _dateUpdated;

        public int PostId { get; set; }

        public string PostBody { get; set; }

        public DateTime DatePosted
        {
            get
            {
                return _datePosted;
            }
            set
            {
                _datePosted = value;
            }
        }

        public string DatePostedString
        {
            get
            {
                return _datePosted.ToString("f");
            }
        }

        public DateTime DateUpdated
        {
            get
            {
                return _dateUpdated;
            }
            set
            {
                _dateUpdated = value;
            }
        }

        public string DateUpdatedString
        {
            get
            {
                return DateUpdated.ToString("f");
            }
        }

        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public int UserId { get; set; }
        public CoreUser User { get; set; }

        public List<CoreComment> Comments { get; set; }

        public CorePost()
        {

        }

        public CorePost(PostPostViewModel model)
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

        public CorePost(Post post)
        {
            SetPostValues(post);
        }

        public void SetNewValues(Post post)
        {
            SetPostValues(post);
        }

        private void SetPostValues(Post post)
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
    }
}

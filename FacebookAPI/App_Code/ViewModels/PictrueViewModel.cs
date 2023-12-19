using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.ViewModels
{
    public class PictrueViewModel
    {
        #region Private Fields
        private readonly CorePicture _corePicture;
        #endregion

        #region Constructors
        public PictrueViewModel()
        {
            
        }

        public PictrueViewModel(CorePicture corePicture)
        {
            _corePicture = corePicture ?? throw new ArgumentNullException(nameof(corePicture));

            PictureId = _corePicture.PictureId;
            CaptionText = _corePicture.CaptionText;
            FullPath = _corePicture.FullPath;

            UserId = _corePicture.User.UserId;
            ProtectedName = _corePicture.User.ProtectedName;
            LikeCount = _corePicture.LikeCount;
            Liked = _corePicture.Liked;
            PostId = _corePicture.PostId;
            ProfilePicture = _corePicture.ProfilePicture;
            
            if (_corePicture.Comments is not null && _corePicture.Comments.Count > 0)
            {
                Comments = new List<CommentViewModel>();

                for (int i = 0; i < _corePicture.Comments.Count; i++)
                {
                    Comments.Add(new CommentViewModel(_corePicture.Comments[i]));
                }
            }
        }
        #endregion

        #region Public Properties
        public int PictureId { get; set; }

        public string CaptionText { get; set; }

        public string FullPath { get; set; }

        public int? PostId { get; set; }

        public int UserId { get; set; }

        public string ProtectedName { get; set; }

        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public bool ProfilePicture { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        #endregion
    }
}

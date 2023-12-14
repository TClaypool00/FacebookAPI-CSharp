using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CorePicture
    {
        #region Private fields
        private readonly string _fileNameWithoutExtension;
        private readonly PostPictureViewModel _postPictureViewModel;
        private readonly string _fileNameExtension;
        private readonly IFormFile _picture;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public CorePicture()
        {
            
        }

        public CorePicture(PostPictureViewModel postPictureViewModel, IConfiguration configuration, IFormFile picture)
        {
            _postPictureViewModel = postPictureViewModel ?? throw new ArgumentNullException(nameof(postPictureViewModel));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _picture = picture ?? throw new ArgumentNullException(nameof(picture));

            UserId = _postPictureViewModel.UserId;
            UserFolderPath = $"{_configuration.GetSection("tableNames:User").Value}{UserId}";
            _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_picture.FileName);
            _fileNameExtension = Path.GetExtension(_picture.FileName);
            NewFileName = $"{_fileNameWithoutExtension}-{Guid.NewGuid()}{_fileNameExtension}";
            FullPath = $@"{SecretConfig.DirectoryPath}\{UserFolderPath}\{NewFileName}";
            CaptionText = _postPictureViewModel.CaptionText;
            ProfilePicture = _postPictureViewModel.ProfilePicture;
        }
        #endregion

        #region Public Properites
        public int PictureId { get; set; }

        public string CaptionText { get; set; }

        public string FullPath { get; set; }

        public string UserFolderPath { get; set; }

        public string NewFileName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool ProfilePicture { get; set; }

        public int? PostId { get; set; }

        public IFormFile Picture
        {
            get
            {
                return _picture;
            }
        }
#nullable enable
        public Post? Post { get; set; }
#nullable disable
        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public List<CoreComment> Comments { get; set; }
        #endregion
    }
}

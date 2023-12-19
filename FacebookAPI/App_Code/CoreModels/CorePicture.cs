using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CorePicture
    {
        #region Private fields
        private readonly PostPictureViewModel _postPictureViewModel;
        private readonly Picture _picture;
        private readonly SinglePostPictureViewModel _singlePostPictureViewModel;
        private string _fileNameWithoutExtension;
        private string _fileNameExtension;
        private IFormFile _pictureFile;
        private IConfiguration _configuration;
        private int _userId;
        #endregion

        #region Constructors
        public CorePicture()
        {
            
        }

        public CorePicture(PostPictureViewModel postPictureViewModel, IConfiguration configuration, IFormFile pictureFile)
        {
            _postPictureViewModel = postPictureViewModel ?? throw new ArgumentNullException(nameof(postPictureViewModel));

            Construct(configuration, pictureFile);

            CaptionText = _postPictureViewModel.CaptionText;
            ProfilePicture = _postPictureViewModel.ProfilePicture;
            PostId = _postPictureViewModel.PostId;
        }

        public CorePicture(SinglePostPictureViewModel singlePostPictureViewModel, IConfiguration configuration, IFormFile pictureFile)
        {
            _singlePostPictureViewModel = singlePostPictureViewModel ?? throw new ArgumentNullException(nameof(singlePostPictureViewModel));

            Construct(configuration, pictureFile);

            CaptionText = _singlePostPictureViewModel.CaptionText;
            ProfilePicture = _singlePostPictureViewModel.ProfilePicture;
            PostId = _singlePostPictureViewModel.PostId;

        }

        public CorePicture(Picture picture, IConfiguration configuration)
        {
            _picture = picture ?? throw new ArgumentNullException(nameof(picture));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            NewFileName = _picture.PictureFileName;

            PictureId = _picture.PictureId;
            
            if (_picture.User is not null)
            {
                User = new CoreUser(_picture.User);
            }

            PostId = _picture.PostId;
            CaptionText = _picture.CaptionText;
            ProfilePicture = _picture.ProfilePicture;
            PostId = _picture.PostId;
            

            SetPictureProperties();
        }
        #endregion

        #region Public Properites
        public int PictureId { get; set; }

        public string CaptionText { get; set; }

        public string FullPath { get; set; }

        public string UserFolderPath { get; set; }

        public string NewFileName { get; set; }

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

        public bool ProfilePicture { get; set; }

        public int? PostId { get; set; }

        public IFormFile Picture
        {
            get
            {
                return _pictureFile;
            }
        }
#nullable enable
        public Post? Post { get; set; }
#nullable disable
        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public List<CoreComment> Comments { get; set; }
        #endregion

        #region Private Methods
        private void SetPictureProperties()
        {
            if (UserId == 0)
            {
                UserId = _picture.User.UserId;
            }

            UserFolderPath = $"{_configuration.GetSection("tableNames:User").Value}{UserId}";
            FullPath = $@"{SecretConfig.DirectoryPath}\{UserFolderPath}\{NewFileName}";
        }

        private void Construct(IConfiguration configuration, IFormFile formFile)
        {
            
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pictureFile = formFile ?? throw new ArgumentNullException(nameof(formFile));

            UserId = _postPictureViewModel.UserId;
            _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_pictureFile.FileName);
            _fileNameExtension = Path.GetExtension(_pictureFile.FileName);
            NewFileName = $"{_fileNameWithoutExtension}-{Guid.NewGuid()}{_fileNameExtension}";

            SetPictureProperties();
        }
        #endregion
    }
}

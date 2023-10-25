using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.ViewModels.PostModels;

namespace FacebookAPI.App_Code.CoreModels
{
    public class CorePicture
    {
        private readonly Picture _picture;
        private readonly PostPictureViewModel _postPictureViewModel;
        private int _userId;
        private readonly string _userPicturePath;
        private readonly IFormFile _pictureFile;
        private readonly string _newFileName;
        private readonly IConfiguration _configuration;
        private readonly string _newFilePath;
        private readonly IWebHostEnvironment _environment;
        private readonly string _userFolderPath;

        public CorePicture(PostPictureViewModel postPictureViewModel, int userId, IWebHostEnvironment environment)
        {
            _postPictureViewModel = postPictureViewModel ?? throw new ArgumentNullException(nameof(postPictureViewModel));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));

            _userId = userId;
            _pictureFile = _postPictureViewModel.Picture;
            _newFileName = FileRepository.NewFileName(_postPictureViewModel.Picture);
            CaptionText = _postPictureViewModel.CaptionText;
            _userPicturePath = FileRepository.GetUserPath(_userId, _newFileName);
            _newFilePath = FileRepository.GetFullUserPath(_userId, _newFileName, _environment);
            _userFolderPath = FileRepository.GetUserFolderPath(_userId, _environment);
        }

        public CorePicture(Picture picture, int userId)
        {
            _picture = picture ?? throw new ArgumentNullException(nameof(picture));
            _userId= userId;

            PictureId = _picture.PictureId;
            CaptionText = _picture.CaptionText;

            PictureFileName = FileRepository.GetUserPath(userId, _picture.PictureFileName);
            
        }

        public CorePicture(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            PictureId = 0;
            PictureFileName = _configuration.GetSection("app").GetSection("defaultProfilePicture").Value;
            CaptionText = _configuration.GetSection("app").GetSection("defaultPictureText").Value;
        }

        public CorePicture()
        {
            if (string.IsNullOrEmpty(CaptionText))
            {
                CaptionText = "";
            }
        }

        public int PictureId { get; set; }

        public string CaptionText { get; set; }

        public string PictureFileName { get; set; }

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

        public string UserPicturePath
        {
            get
            {
                return _userPicturePath;
            }
        }

        public IFormFile PictureFile
        {
            get
            {
                return _pictureFile;
            }
        }

        public string NewFileName
        {
            get
            {
                return _newFileName;
            }
        }

        public string NewFilePath
        {
            get
            {
                return _newFilePath;
            }
        }

        public string UserFolderPath
        {
            get
            {
                return _userFolderPath;
            }
        }
    }
}

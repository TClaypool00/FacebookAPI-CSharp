using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.Extensions.Configuration;

namespace FacebookAPI.App_Code.Helpers
{
    public class FileHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _notFoundMessage;

        public FileHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _notFoundMessage = _configuration.GetSection("messages").GetSection("PictureNotFound").Value;
        }

        public async Task AddPicture(CorePicture corePicture)
        {
            string userPath = GetUserPath(corePicture.UserId);

            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }

            using Stream fileStream = new FileStream(corePicture.FullPath, FileMode.Create);
            await corePicture.Picture.CopyToAsync(fileStream);
        }

        public void DeletePicture(CorePicture corePicture)
        {
            if (File.Exists(corePicture.FullPath))
            {
                File.Delete(corePicture.FullPath);
            }
            else
            {
                throw new ApplicationException(_notFoundMessage);
            }
        }

        public void DeletePicture(Picture picture)
        {
            string picturePath = GetPicturePath(picture);

            if (File.Exists(picturePath))
            {
                File.Delete(picturePath);
            }
            else
            {
                throw new ApplicationException(_notFoundMessage);
            }
        }

        public string GetPicturePath(Picture picture)
        {
            return $@"{GetUserPath(picture.UserId)}\{picture.PictureFileName}";
        }

        private string GetUserPath(int userId)
        {
            return $@"{SecretConfig.DirectoryPath}\{_configuration.GetSection("tableNames").GetSection("User").Value}{userId}";
        }
    }
}

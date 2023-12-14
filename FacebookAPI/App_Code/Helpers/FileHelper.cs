using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;
using Microsoft.Extensions.Configuration;

namespace FacebookAPI.App_Code.Helpers
{
    public class FileHelper
    {
        private readonly IConfiguration _configuration;

        public FileHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddPicture(CorePicture corePicture)
        {
            string userPath = $"{SecretConfig.DirectoryPath}\\{corePicture.UserFolderPath}";

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
                throw new ApplicationException(_configuration.GetSection("messages").GetSection("PictureNotFound").Value);
            }
        }
    }
}

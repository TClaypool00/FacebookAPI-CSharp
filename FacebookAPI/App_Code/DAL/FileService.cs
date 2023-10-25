using FacebookAPI.App_Code.BLL;
using FacebookAPI.App_Code.BOL;
using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.DAL
{
    public class FileService : ServiceHelper, IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IConfiguration configuration, IWebHostEnvironment environment, FacebookDbContext context) : base(configuration, context)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<CorePicture> ChangeProfilePicture(CorePicture picture)
        {
            try
            {
                if (!Directory.Exists(picture.UserFolderPath))
                {
                    Directory.CreateDirectory(picture.UserFolderPath);
                }

                await FileRepository.CreateNewFileAsync(picture.NewFileName, _environment, picture.UserId, picture.PictureFile);
            }
            catch (Exception)
            {
                throw;
            }

            var dataPicture = new Picture(picture);
            try
            {
                await _context.Pictures.AddAsync(dataPicture);
                await SaveAsync();

                return picture;
            }
            catch (Exception)
            {
                if (File.Exists(picture.NewFilePath))
                {
                    File.Delete(picture.NewFilePath);
                }

                throw;
            }
        }
    }
}

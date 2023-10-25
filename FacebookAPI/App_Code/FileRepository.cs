namespace FacebookAPI.App_Code
{
    public class FileRepository
    {
        public static string NewFileName(IFormFile file)
        {
            string justFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExt = Path.GetExtension(file.FileName);
            return $"{justFileName}-{Guid.NewGuid()}{fileExt}";
        }

        public static async Task CreateNewFileAsync(string fileName, IWebHostEnvironment environment, int userId, IFormFile file)
        {
            try
            {
                string userFolder = $@"{environment.WebRootPath}\Images\user{userId}\";
                string filePath;

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                filePath = $"{userFolder}{fileName}";

                var fileStream = new FileStream(filePath, FileMode.Create);
                if (file.Length > 0)
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetFullUserPath(int userId, string fileName, IWebHostEnvironment environment)
        {
            return $@"{GetUserFolderPath(userId, environment)}\{fileName}";
        }

        public static string GetUserFolderPath(int userId, IWebHostEnvironment environment)
        {
            return $@"{environment.WebRootPath}\Images\user{userId}";
        }

        public static string GetUserPath(int userId, string fileName)
        {
            return $"user{userId}/{fileName}";
        }
    }
}

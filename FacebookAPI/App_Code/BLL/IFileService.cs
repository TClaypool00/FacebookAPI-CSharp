using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IFileService
    {
        public Task<CorePicture> ChangeProfilePicture(CorePicture picture);
    }
}

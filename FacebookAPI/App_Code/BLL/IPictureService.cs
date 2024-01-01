using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IPictureService
    {
        #region Public Methods
        public Task<List<CorePicture>> GetPictureAsync(int userId, int? postId = null, int ? index = null);

        public Task<CorePicture> AddPictureAsync(CorePicture picture);

        public Task<CorePicture> GetPictureByIdAsync(int id, int userId);

        public Task<CorePicture> UpdatePictureByIdAsync(CorePicture picture);

        public Task<List<int>> AddPicturesAsync(List<CorePicture> pictures);

        public Task<bool> PictureExistsAsync(int id);

        public Task<bool> UserOwnsPictureAsync(int id, int userId);

        public Task UpdateProfilePictureAsync(int id, bool profilePicture, int userId);

        public Task DeletePictureAsync(int id);

        public string PicturesCouldNotBeAddedMessage(List<int> ids);
        #endregion

        #region Public Properites
        public string PictureAddedOKMessage { get; }

        public string PictureCouldNotBeAddedMessage { get; }

        public string PicturesAddedOKMessage { get; }

        public string PictureDoesNotExistMessage { get; }

        public string PictureUpdatedOKMessage { get; }

        public string PictureNotDeletedMessage { get; }

        public string PictureDeletedOKMessage { get; }

        public string PicturesNotFoundMessage { get; }
        #endregion
    }
}

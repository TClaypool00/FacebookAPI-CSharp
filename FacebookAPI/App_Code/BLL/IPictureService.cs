using FacebookAPI.App_Code.CoreModels;

namespace FacebookAPI.App_Code.BLL
{
    public interface IPictureService
    {
        #region Public Methods
        public Task<CorePicture> AddPictureAsync(CorePicture picture);

        public Task<List<int>> AddPicturesAsync(List<CorePicture> pictures);

        public string PicturesCouldNotBeAddedMessage(List<int> ids);
        #endregion

        #region Public Properites
        public string PictureAddedOKMessage { get; }

        public string PictureCouldNotBeAddedMessage { get; }

        public string PicturesAddedOKMessage { get; }
        #endregion
    }
}

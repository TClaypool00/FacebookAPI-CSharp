namespace FacebookAPI.App_Code.ViewModels.Interfaces
{
    public interface IBaseViewModel
    {
        #region Public Properties
        public string UserDisplayName { get; set; }

        public string DatePosted { get; set; }

        public bool IsEdited { get; set; }

        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public string Message { get; set; }

        #endregion
    }
}

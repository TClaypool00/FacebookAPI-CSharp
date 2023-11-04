namespace FacebookAPI.App_Code.ViewModels
{
    public class LikeViewModel
    {
        #region Private fields
        private readonly Tuple<int, bool> _likeTurple;
        #endregion

        #region Constructors
        public LikeViewModel(Tuple<int, bool> tuple)
        {
            LikeCount = _likeTurple.Item1;
            Liked = _likeTurple.Item2;
        }
        #endregion

        #region Public Properties
        public int LikeCount { get; set; }

        public bool Liked { get; set; }
        #endregion
    }
}

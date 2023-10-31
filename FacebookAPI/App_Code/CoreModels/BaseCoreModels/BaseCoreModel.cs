namespace FacebookAPI.App_Code.CoreModels.BaseCoreModels
{
    public abstract class BaseCoreModel
    {
        #region Protected fields
        protected int _userId;
        protected DateTime _datePosted;
        protected DateTime _dateUpdated;
        #endregion

        #region Public Properties
        public DateTime DatePosted
        {
            get
            {
                return _datePosted;
            }
            set
            {
                _datePosted = value;
            }
        }

        public string DatePostedString
        {
            get
            {
                return _datePosted.ToString("f");
            }
        }

        public DateTime DateUpdated
        {
            get
            {
                return _dateUpdated;
            }
            set
            {
                _dateUpdated = value;
            }
        }

        public string DateUpdatedString
        {
            get
            {
                return DateUpdated.ToString("f");
            }
        }

        public int LikeCount { get; set; }

        public bool Liked { get; set; }

        public bool IsEdited
        {
            get
            {
                return _datePosted < _dateUpdated;
            }
        }

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
        public CoreUser User { get; set; }
        #endregion
    }
}

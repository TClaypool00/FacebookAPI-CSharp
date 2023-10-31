using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels
{
    public abstract class BaseViewModel
    {
        #region Protected fields
        protected string _userDisplayName;
        protected string _datePosted;
        protected int _userId;
        protected int _likeCount;
        protected bool _liked;
        protected bool _isEdited;
        protected string _message;
        #endregion

        [Required(ErrorMessage = "User Id is required")]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostPostViewModel
    {
        protected int _userId;

        [Required(ErrorMessage = "Body is required")]
        [MaxLength(255, ErrorMessage = "Body has max length of 255")]
        public string PostBody { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
            }
        }
    }
}

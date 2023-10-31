using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostReplyViewModel : BaseViewModel
    {
        #region Constructors
        public PostReplyViewModel() : base()
        {

        }
        #endregion

        #region Public Properties
        [Required(ErrorMessage = "Reply Body is required")]
        [MaxLength(255, ErrorMessage = "Reply Body has max length of 255")]
        public string ReplyBody { get; set; }

        [Required(ErrorMessage = "Comment Id is required")]
        public int CommentId { get; set; }
        #endregion
    }
}

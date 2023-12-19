using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostCommentViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Comment body is required")]
        [MaxLength(255, ErrorMessage = "Comment body has max length of 255")]
        public string CommentBody { get; set; }

        public int? PostId { get; set; }

        public int? PictureId { get; set; }
    }
}

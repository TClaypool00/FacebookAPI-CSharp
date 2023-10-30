using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostCommentViewModel
    {
        [Required(ErrorMessage = "Comment body is required")]
        [MaxLength(255, ErrorMessage = "Comment body has max length of 255")]
        public string CommentBody { get; set; }

        [Required(ErrorMessage = "Post Id is required")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
    }
}

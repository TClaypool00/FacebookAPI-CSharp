using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostCommentViewModel
    {
        [Required(ErrorMessage = "Comment body is required")]
        [MaxLength(255, ErrorMessage = "Comment body has max length of 255")]
        [Display(Name = "Write a comment")]
        public string CommentBody { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}

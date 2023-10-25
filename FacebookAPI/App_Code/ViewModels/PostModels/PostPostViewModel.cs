using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostPostViewModel
    {
        [Required(ErrorMessage = "Body is required")]
        [MaxLength(255, ErrorMessage = "Body has max length of 255")]
        [Display(Name = "What's on your mind today?")]
        [DataType(DataType.MultilineText)]
        public string PostBody { get; set; }
    }
}

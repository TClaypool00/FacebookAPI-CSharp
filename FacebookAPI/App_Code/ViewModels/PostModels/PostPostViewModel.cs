using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostPostViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Body is required")]
        [MaxLength(255, ErrorMessage = "Body has max length of 255")]
        public string PostBody { get; set; }
    }
}

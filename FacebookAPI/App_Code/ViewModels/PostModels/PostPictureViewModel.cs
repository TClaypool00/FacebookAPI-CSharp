using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostPictureViewModel
    {
        [Required(ErrorMessage = "Picture is required")]
        [Display(Name = "Upload a picture")]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = "Caption text is requirement")]
        [MaxLength(255, ErrorMessage = "Caption text has a max length of 255")]
        [Display(Name = "Caption Text")]
        public string CaptionText { get; set; }
    }
}

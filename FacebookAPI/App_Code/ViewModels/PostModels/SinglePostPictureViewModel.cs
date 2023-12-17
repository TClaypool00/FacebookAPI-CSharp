using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class SinglePostPictureViewModel
    {
        [Required(ErrorMessage = "Caption text is requirement")]
        [MaxLength(255, ErrorMessage = "Caption text has a max length of 255")]
        public string CaptionText { get; set; }

        public bool ProfilePicture { get; set; }

        public int UserId { get; set; }

        public int? PostId { get; set; }
    }
}
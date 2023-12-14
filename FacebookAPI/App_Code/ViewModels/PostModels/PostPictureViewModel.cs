using FacebookAPI.App_Code.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    [ModelBinder(BinderType = typeof(MetadataValueModelBinder))]
    public class PostPictureViewModel
    {
        [Required(ErrorMessage = "Caption text is requirement")]
        [MaxLength(255, ErrorMessage = "Caption text has a max length of 255")]
        [Display(Name = "Caption Text")]
        public string CaptionText { get; set; }

        public bool ProfilePicture { get; set; }

        public int UserId { get; set; }
    }
}

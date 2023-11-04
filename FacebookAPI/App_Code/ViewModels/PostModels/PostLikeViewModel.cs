using FacebookAPI.App_Code.CustomAnnotation;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostLikeViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        [IdMustBeGreaterThanZero]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
    }
}

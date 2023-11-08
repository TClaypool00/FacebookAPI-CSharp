using FacebookAPI.App_Code.CustomAnnotation;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostFriendViewModel
    {
        [Required(ErrorMessage = "Receiver id is required")]
        [IdMustBeGreaterThanZero]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage = "Sender Id is required")]
        [IdMustBeGreaterThanZero]
        public int SenderId { get; set; }
    }
}

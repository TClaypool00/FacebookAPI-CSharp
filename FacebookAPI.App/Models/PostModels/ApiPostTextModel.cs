using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App.Models.PostModels
{
    public class ApiPostTextModel
    {
        [Required(ErrorMessage = "Body is required")]
        [MaxLength(255, ErrorMessage = "Body has a limit of 255 characters")]
        public string Body { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
    }
}

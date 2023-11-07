using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.ViewModels.PostModels
{
    public class PostGenderViewModel
    {
        [Required(ErrorMessage = "Gender Name is required")]
        [MaxLength(255, ErrorMessage = "Gender Name has max length of 255")]
        public string GenderName { get; set; }

        [Required(ErrorMessage = "Pronoun is required")]
        [MaxLength(255, ErrorMessage = "Pronoun has max length of 255")]
        public string ProNouns { get; set; }
    }
}

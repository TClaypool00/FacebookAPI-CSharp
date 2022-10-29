using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App.Models.PostModels
{
    public class ApiPostParentType
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Name is a max limit of 255 characters")]
        public string Name { get; set; }
    }
}

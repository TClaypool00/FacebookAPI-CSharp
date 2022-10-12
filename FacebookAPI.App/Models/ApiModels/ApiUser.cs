using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App.Models.ApiModels
{
    public class ApiUser
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(255, ErrorMessage = "First name has a max limt of 255 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(255, ErrorMessage = "Last name has a max limit of 255 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Is Admin is required")]
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(255, ErrorMessage = "Email has max limit of 255 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email must be valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(13, ErrorMessage = "Phone number has max limit of 13 characters")]
        [MinLength(10, ErrorMessage = "Phone number has minumum limit of 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNum { get; set; }
    }
}
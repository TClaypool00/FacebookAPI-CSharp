using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Add service for Gender
    //TODO: Add to Registeration page
    //TODO: Add to profile page
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        [Required]
        [MaxLength(255)]
        public string GenderName { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProNouns { get; set; }

        public List<Profile> Profiles { get; set; }
    }
}

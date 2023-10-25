using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Add service for Locations table
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string LocationName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Job> Jobs { get; set; }
    }
}

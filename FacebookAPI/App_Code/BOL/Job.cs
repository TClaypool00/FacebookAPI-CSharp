using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Add service for Job table
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        [MaxLength(255)]
        public string JobTitle { get; set; }

        [MaxLength(255)]
        public string JobDescription { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}

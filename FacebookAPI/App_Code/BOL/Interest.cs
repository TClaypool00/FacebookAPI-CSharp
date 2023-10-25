using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class Interest
    {
        [Key]
        public int InterestId { get; set; }

        [Required]
        [MaxLength(255)]
        public string InterestName { get; set; }


        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}

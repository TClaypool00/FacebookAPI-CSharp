using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Add Group user tables
    public class GroupUser
    {
        public int GroupUserId { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateAdded { get; set; }

        public List<GroupMessage> Messages { get; set; }
    }
}

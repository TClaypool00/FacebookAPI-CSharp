using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.BOL
{
    public class GroupMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Message { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DatePosted { get; set; }

        public int GroupUserId { get; set; }
        public GroupUser GroupUser { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookAPI.App_Code.BOL
{
    //TODO: Added service for groups
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }

        public List<GroupUser> GroupUsers { get; set; }

    }
}